using Newtonsoft.Json;
using SmartStore.Core;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Forums;
using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Domain.Media;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Tax;
using SmartStore.Core.Localization;
using SmartStore.Core.Logging;

using SmartStore.Services;
using SmartStore.Services.Localization;
using SmartStore.Services.Messages;
using SmartStore.Services.Tax;
using SmartStore.Web.Framework.Plugins;
using SmartStore.Web.Framework.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using SmartStore.Core.Domain.Messages;
using SmartStore.Web.Framework.Filters;
using SmartStore.Services.Customers;

namespace SmartStore.WebApi.Controllers.Api
{
	[CustomWebApiAuthenticate]
	public class NewsletterController : ApiController
    {
        #region Fields
        public Localizer T { get; set; }//Added by Yagnesh 
        public ICommonServices Services { get; set; }//Added by Yagnesh 

		private readonly IWorkContext _workContext;
		private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
		private readonly IStoreContext _storeContext;
		private readonly CustomerSettings _customerSettings;
		private readonly ILocalizationService _localizationService;
		private readonly ICommonServices _services;
		private readonly ICustomerService _customerServices;
		#endregion

		#region Ctor

		public NewsletterController(
            ICommonServices services,
            ILocalizationService localizationService,            
			IWorkContext workContext,
			INewsLetterSubscriptionService newsLetterSubscriptionService,
			CustomerSettings customerSettings,
			IStoreContext storeContext,
			ICustomerService customerService)
        {
            _services = services;
            _localizationService = localizationService;
			_workContext = workContext;
			_newsLetterSubscriptionService = newsLetterSubscriptionService;
			_customerSettings = customerSettings;
			_storeContext = storeContext;
			_customerServices = customerService;
		}

        #endregion
		
        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("Subscribe")]		
		public HttpResponseMessage Subscribe(int customerid,bool subscribe, string email,int id=0)
        {
            var response = this.Request.CreateResponse();
			var customer = _customerServices.GetCustomerById(customerid);
			if (customerid != 0)
			{
				_services.WorkContext.CurrentCustomer = customer;
			}
			string jsonString = "";
            try
            {
				string result="";
				var success = false;
				if (!email.IsEmail())
				{
					result = T("Newsletter.Email.Wrong");
				}
				else
				{
					// subscribe/unsubscribe
					email = email.Trim();
					_workContext.CurrentCustomer = (customer != null) ? customer : _customerServices.GetCustomerById(id);
					var subscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmail(email, _storeContext.CurrentStore.Id);
					if (subscription != null)
					{
						if (subscribe)
						{
							if (!subscription.Active)
							{
								Services.MessageFactory.SendNewsLetterSubscriptionActivationMessage(subscription, _workContext.WorkingLanguage.Id);
								result = T("Newsletter.SubscribeEmailSent");
							}
							else
							{
								result = T("Newsletter.AllReadysubscribed");
							}
						}
						else
						{
							if (subscription.Active)
							{
								Services.MessageFactory.SendNewsLetterSubscriptionDeactivationMessage(subscription, _workContext.WorkingLanguage.Id);
								result = T("Newsletter.UnsubscribeEmailSent");
							}
							else
							{
								result = T("Newsletter.AllReadyunsubscribed");
							}
						}
					}
					else 
					{
						if (subscribe)
						{
							subscription = new NewsLetterSubscription
							{
								NewsLetterSubscriptionGuid = Guid.NewGuid(),
								Email = email,
								Active = false,
								CreatedOnUtc = DateTime.UtcNow,
								StoreId = _storeContext.CurrentStore.Id
							};

							_newsLetterSubscriptionService.InsertNewsLetterSubscription(subscription);
							Services.MessageFactory.SendNewsLetterSubscriptionActivationMessage(subscription, _workContext.WorkingLanguage.Id);
							result = T("Newsletter.SubscribeEmailSent");
						}
						else
						{
							result = T("Newsletter.SubscribscribtionNotFound");
						}
					}
				}

				jsonString = JsonConvert.SerializeObject(result, Formatting.None);
				jsonString = "{\"code\":1,\"message\": \"success\",\"data\":" + jsonString + "}";
				response.StatusCode = HttpStatusCode.OK;
				response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
				return response;
			}
			catch (Exception ex)
			{
				jsonString = "{\"code\": 0,\"message\": \"" + _localizationService.GetResource("Common.SomethingWentWrong") + "\"}";
				response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
				return response;
			}
		}
		
    }

}