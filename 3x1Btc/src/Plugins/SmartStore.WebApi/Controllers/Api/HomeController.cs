using Newtonsoft.Json;
using SmartStore.Core;
using SmartStore.Core.Domain.Advertisments;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Domain.Messages;
using SmartStore.Core.Email;
using SmartStore.Services;
using SmartStore.Services.Customers;
using SmartStore.Services.Hyip;
using SmartStore.Services.Messages;
using SmartStore.Web.Framework.WebApi.Security;
using SmartStore.Web.Models.Common;
using SmartStore.Web.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Http;

namespace SmartStore.WebApi.Controllers.Api
{
	public class HomeController : ApiController
	{
		private readonly ICustomerService _customerService;
		private readonly ITransactionService _transactionService;
		private readonly IPlanService _planService;
		private readonly IWorkContext _workContext;
		private readonly ICampaignService _campaignService;
		private readonly ICommonServices _commonServices;
		public HomeController(ICustomerService customerService,
			ITransactionService transactionService,
			IPlanService planService,
			IWorkContext workContext,
			ICampaignService campaignService,
			ICommonServices commonServices)
		{
			_customerService = customerService;
			_commonServices = commonServices;
			_transactionService = transactionService;
			_planService = planService;
			_workContext = workContext;
			_campaignService = campaignService;
		}
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Index()
        {
            try
            {
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                string jsonString = JsonConvert.SerializeObject("OK GET", Formatting.Indented);
                response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                return response;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("ContactUsNew")]
		public HttpResponseMessage ContactUsNew(ContactUsNewModel model)
		{
			// Validate CAPTCHA
			if (ModelState.IsValid)
			{
				var email = model.Email.Trim();
				var fullName = model.Fname + " " + model.Lname;
				var subject = model.Subject; //"MagicBooster Contact Request";
				var body = Core.Html.HtmlUtils.FormatText(model.Enquiry, false, true, false, false, false, false);

				// Required for some SMTP servers
				EmailAddress sender = null;

				// email
				var msg = _commonServices.MessageFactory.SendContactUsMessage(_workContext.CurrentCustomer, email, fullName, subject, body, sender);
			}

			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = "" });
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetSiteStats")]
		public HttpResponseMessage GetSiteStats()
		{
			HomeModel homemodel = new HomeModel();
			homemodel.LaunchDate = DateTime.Parse("01/05/2020").ToShortDateString(); //DateTime.Today.AddDays(-95).ToShortDateString();
			TimeSpan dt = DateTime.Now - DateTime.Parse(homemodel.LaunchDate);
			if (dt.Days > 0)
			{
				homemodel.RunningDays = dt.Days.ToString();
			}
			else
			{
				homemodel.RunningDays = "0";
			}

			int[] StatusIds = { (int)Status.Completed };
			int[] TranscationTypeIds = { (int)TransactionType.Purchase };
			var totalfundinglist = _transactionService.GetAllTransactions(0, 0, null, null, StatusIds, TranscationTypeIds, 0, int.MaxValue).OrderByDescending(x => x.TransactionDate).Take(10);
			homemodel.TotalSiteDeposit = _transactionService.GetAllTransactions(0, 0, null, null, StatusIds, TranscationTypeIds, 0, int.MaxValue).Select(x => x.Amount).Sum().ToString();
			int[] WithTranscationTypeIds = { (int)TransactionType.Withdrawal };
			var totalWithdrawallist = _transactionService.GetAllTransactions(0, 0, null, null, StatusIds, WithTranscationTypeIds, 0, int.MaxValue).OrderByDescending(x => x.TransactionDate).Take(10);
			homemodel.TotalSiteWithdrawal = _transactionService.GetAllTransactions(0, 0, null, null, StatusIds, WithTranscationTypeIds, 0, int.MaxValue).Select(x => x.Amount).Sum().ToString();
			homemodel.TotalCustomer = _customerService.GetAllCustomers(0, 0, int.MaxValue).Count().ToString();
			var yestarday = DateTime.Now.AddHours(-24);
			homemodel.OnlineVistors = _customerService.GetAllCustomers(0, 0, int.MaxValue).Where(x => x.CreatedOnUtc > yestarday).Count().ToString();
			var withdrawals = totalWithdrawallist.OrderByDescending(x => x.TransactionDate).ToList();
			foreach (var d in withdrawals)
			{
				WithdrawalList w = new WithdrawalList();
				w.Email = d.Customer.Email;
				w.AmountRaw = d.Amount.ToString() + _workContext.WorkingCurrency.CurrencyCode;
				w.TransDate = d.TransactionDate.ToShortDateString();
				w.PaymentMethodIcon = d.ProcessorId + ".svg";
				homemodel.Last10Withdrawal.Add(w);
			}

			var deposits = totalfundinglist.OrderByDescending(x => x.TransactionDate).ToList();
			foreach (var d in deposits)
			{
				DepositList w = new DepositList();
				w.Email = d.Customer.Email;
				w.AmountRaw = d.Amount.ToString() + _workContext.WorkingCurrency.CurrencyCode;
				w.TransDate = d.TransactionDate.ToShortDateString();
				w.PaymentMethodIcon = d.ProcessorId + ".svg";
				homemodel.Last10Deposit.Add(w);
			}
			homemodel.plans = _planService.GetAllPackage().ToList();

			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = homemodel });
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetNewsletter")]
		public HttpResponseMessage GetNewsletter()
		{
			var campaign = _campaignService.GetAllCampaigns().Select(x => new
			{
				Id = x.Id,
				Name = x.Name,
				Subject = x.Subject,
				CreatedOnUtc = x.CreatedOnUtc,
				Body = x.Body
			}).OrderByDescending(x=>x.Id).ToList();
			var c = JsonConvert.SerializeObject(campaign, new JsonSerializerSettings
			{
				Formatting = Newtonsoft.Json.Formatting.Indented,
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			});
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = c });
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetNewsletterById")]
		public HttpResponseMessage GetNewsletterById(int campaignId)
		{
			var campaign = _campaignService.GetCampaignById(campaignId);
			var c = JsonConvert.SerializeObject(campaign, new JsonSerializerSettings
			{
				Formatting = Newtonsoft.Json.Formatting.Indented,
				ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			});
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = c });
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("AddCustomerTraffic")]
		public HttpResponseMessage AddCustomerTraffic(int CustomerId)
		{
			CustomerTraffic customerTraffic = new CustomerTraffic();
			//var guid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
			customerTraffic.CustomerId = CustomerId;
			customerTraffic.IpAddress = GetClientIp();
			customerTraffic.CreatedOnUtc = DateTime.UtcNow;
			customerTraffic.UpdatedOnUtc = DateTime.UtcNow;
			var CustomerTraffic = _customerService.InsertCustomerTraffic(customerTraffic);
			//if(CustomerTraffic != null)
			//{
			//	CustomerToken customerToken = new CustomerToken();
			//	customerToken.CustomerId = CustomerId;
			//	customerToken.EarningSource = "traffic";
			//	customerToken.Deleted = false;
			//	customerToken.CreatedDate = DateTime.UtcNow;
			//	customerToken.NoOfToken = 1;
			//	_customerService.InsertCustomerToken(customerToken);
			//}
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = customerTraffic });
		}

		private string GetClientIp(HttpRequestMessage request = null)
		{
			request = request ?? Request;
			if (request.Properties.ContainsKey("MS_HttpContext"))
			{
				return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
			}
			else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
			{
				RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
				return prop.Address;
			}
			else if (HttpContext.Current != null)
			{
				return HttpContext.Current.Request.UserHostAddress;
			}
			else
			{
				return null;
			}
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("AddCustomerToken")]
		public HttpResponseMessage AddCustomerToken(int CustomerId,string source)
		{
			//CustomerToken customerToken = new CustomerToken();
			//customerToken.CustomerId = CustomerId;
			//customerToken.EarningSource = source;
			//customerToken.Deleted = false;
			//customerToken.CreatedDate = DateTime.UtcNow;
			//if(source == "facebookshare")
			//{
			//	customerToken.NoOfToken = 5;
			//}
			//_customerService.InsertCustomerToken(customerToken);
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success" });
		}
	}
}
