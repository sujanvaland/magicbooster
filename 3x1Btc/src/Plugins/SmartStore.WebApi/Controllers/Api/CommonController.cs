using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SmartStore.Services;
using SmartStore.Services.Authentication;
using SmartStore.Services.Authentication.External;
using SmartStore.Services.Catalog;
using SmartStore.Services.Catalog.Extensions;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Services.Directory;
using SmartStore.Services.Forums;
using SmartStore.Services.Helpers;
using SmartStore.Services.Localization;
using SmartStore.Services.Media;
using SmartStore.Services.Messages;
using SmartStore.Services.Orders;
using SmartStore.Services.Payments;
using SmartStore.Services.Security;
using SmartStore.Services.Tax;
using SmartStore.Web.Framework.Plugins;
using SmartStore.WebApi.Models.Api.Customer;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using SmartStore.Core.Domain.Hyip;
using System.Web.Mvc;
using System.Collections.Generic;
using SmartStore.Services.Boards;
using SmartStore.Services.Hyip;
using SmartStore.Admin.Models.Investment;
using SmartStore.Admin.Models.PaymentMethods;
using SmartStore.Admin;
using SmartStore.Web.Models.Home;
using SmartStore.Services.Advertisments;
using SmartStore.Web.Framework.WebApi.Security;
using RestSharp;
using System.Web;

namespace SmartStore.WebApi.Controllers.Api
{
	[CustomWebApiAuthenticate]
	public class CommonController : ApiController
    {
        #region Fields
        public Localizer T { get; set; }//Added by Yagnesh 
        public ICommonServices Services { get; set; }//Added by Yagnesh 
		private readonly ICommonServices _services;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IPlanService _planService;
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ITransactionService _transactionService;
		private readonly ICountryService _countryService;
		private readonly IAdCampaignService _adCampaignService;
		private readonly LocalizationSettings _localizationSettings;
		#endregion

		#region Ctor

		public CommonController(
			ITransactionService transactionService,
			LocalizationSettings localizationSettings,
			ICustomerPlanService customerPlanService,
			IPlanService planService,
			ICommonServices services,
            IAuthenticationService authenticationService,
            IDateTimeHelper dateTimeHelper,
            DateTimeSettings dateTimeSettings, TaxSettings taxSettings,
            ILocalizationService localizationService,
            IWorkContext workContext, IStoreContext storeContext,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
			IBoardService boardService,
			ICountryService countryService,
			IAdCampaignService adCampaignService)
        {
			_planService = planService;
			_customerPlanService = customerPlanService;
			_localizationSettings = localizationSettings;
			_transactionService = transactionService;
			_services = services;
            _authenticationService = authenticationService;
            _dateTimeHelper = dateTimeHelper;
            _dateTimeSettings = dateTimeSettings;
            _localizationService = localizationService;
            _workContext = workContext;
            _storeContext = storeContext;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
			_countryService = countryService;
			_adCampaignService = adCampaignService;

		}

		#endregion

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("PaymentProcessor")]
		public HttpResponseMessage GetPaymentProcessor()
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				List<SelectListItem> AvailableProcessor = new List<SelectListItem>();
				var storeScope = _storeContext.CurrentStore.Id;
				var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(storeScope);
				var SolitTrustPaySettings = _services.Settings.LoadSetting<SolidTrustPaySettings>(storeScope);
				var PayzaSettings = _services.Settings.LoadSetting<PayzaSettings>(storeScope);
				var PMSettings = _services.Settings.LoadSetting<PMSettings>(storeScope);
				var PayeerSettings = _services.Settings.LoadSetting<PayeerSettings>(storeScope);
				if (coinpaymentSettings.CP_IsActivePaymentMethod)
				{
					AvailableProcessor.Add(new SelectListItem()
					{
						Text = "Bitcoin",
						Value = "0"
					});
				}
				if (coinpaymentSettings.CP_IsActivePaymentMethod)
				{
					AvailableProcessor.Add(new SelectListItem()
					{
						Text = "Bank",
						Value = "5"
					});
				}
				if (PayzaSettings.PZ_IsActivePaymentMethod)
				{
					AvailableProcessor.Add(new SelectListItem()
					{
						Text = "Payza",
						Value = "1"
					});
				}
				if (PMSettings.PM_IsActivePaymentMethod)
				{
					AvailableProcessor.Add(new SelectListItem()
					{
						Text = "PM",
						Value = "2"
					});
				}
				if (PayeerSettings.PY_IsActivePaymentMethod)
				{
					AvailableProcessor.Add(new SelectListItem()
					{
						Text = "Payeer",
						Value = "3"
					});
				}
				if (SolitTrustPaySettings.STP_IsActivePaymentMethod)
				{
					AvailableProcessor.Add(new SelectListItem()
					{
						Text = "SolidTrustPay",
						Value = "4"
					});
				}
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = AvailableProcessor });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = _localizationService.GetResource("Common.SomethingWentWrong") });
			}
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("Withdrawfund")]
		public HttpResponseMessage Withdrawal(TransactionModel model)
		{
			try
			{
				var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
				var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
				if (customerguid != null)
				{
					if (model.CustomerId != cust.Id)
					{
						return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
					}
				}
				var storeScope = _storeContext.CurrentStore.Id;
				var StatusIds = new int[] { (int)Status.Pending, (int)Status.Inprogress, (int)Status.Completed };
				var TranscationTypeIds = new int[] { (int)TransactionType.Withdrawal };
				var withdrawalSettings = _services.Settings.LoadSetting<WithdrawalSettings>(storeScope);
				var transcations = _transactionService.GetTodaysWithdrawal(model.CustomerId);
				model.AvailableBalance = _customerService.GetAvailableBalance(model.CustomerId);
				if (model.Amount <= 0)
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Enter correct amount" });
				}
				if (model.AvailableBalance < model.Amount)
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = T("Investment.Withdrawal.InsufficentBalance").Text });
				}
				if (transcations.Count >= withdrawalSettings.MaxRequestPerDay)
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = T("Investment.Withdrawal.MaxWithdrawalPerDayError", withdrawalSettings.MaxRequestPerDay).Text });
				}
				if (!(model.Amount >= withdrawalSettings.MinWithdrawal && model.Amount <= withdrawalSettings.MaxWithdrawal))
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Minimum withdrawal limit " + _workContext.WorkingCurrency.CurrencyCode + " " + withdrawalSettings.MinWithdrawal + ", Maximum withdrawal limit " + _workContext.WorkingCurrency.CurrencyCode + " " + withdrawalSettings.MaxWithdrawal });
				}

				//if(cust.AffiliateId != 129)
				//{
				//	if (cust.Transaction.Where(x => x.TranscationTypeId == 5).Count() >= 1)
				//	{
				//		var directcount = _customerService.GetCustomerDirectReferral(model.CustomerId).ToList();
				//		int activecount = 0;
				//		foreach (var c in directcount)
				//		{
				//			var isactive = (c.CustomerPosition.Count() > 0 || c.CustomerPlan.Count() > 0) ? true : false;
				//			if (isactive)
				//			{
				//				activecount = activecount + 1;
				//			}
				//			if (activecount >= 2)
				//			{
				//				break;
				//			}
				//		}
				//		if (activecount < 2)
				//		{
				//			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "You need 2 direct paid referral to request withdrawal" });
				//		}
				//	}
				//}

				if (ModelState.IsValid)
				{
					if (withdrawalSettings.AllowAutoWithdrawal)
					{
						try
						{
							model.FinalAmount = model.Amount;
							model.TransactionDate = DateTime.Now;

							model.RefId = 0;
							model.TranscationTypeId = (int)TransactionType.Withdrawal;

							var customer = _customerService.GetCustomerById(model.CustomerId);
							model.BitcoinAddress = customer.GetAttribute<string>(SystemCustomerAttributeNames.BitcoinAddressAcc);

							model.BankName = customer.GetAttribute<string>(SystemCustomerAttributeNames.BankName);
							model.AccountHolderName = customer.GetAttribute<string>(SystemCustomerAttributeNames.AccountHolderName);
							model.AccountNumber = customer.GetAttribute<string>(SystemCustomerAttributeNames.AccountNumber);
							model.NICR = customer.GetAttribute<string>(SystemCustomerAttributeNames.NICR);

							model.PayzaAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.PayzaAcc);
							model.SolidTrustPayAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.SolidTrustPayAcc);
							model.PayeerAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.PayeerAcc);
							model.PMAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.PMAcc);
							model.AdvanceCashAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.AdvanceCashAcc);
							if (model.ProcessorId == (int)PaymentMethod.CoinPayment)
							{
								model.WithdrawalAccount = model.BitcoinAddress;
							}
							if (model.ProcessorId == (int)PaymentMethod.Payza)
							{
								model.WithdrawalAccount = model.PayzaAcc;
							}
							if (model.ProcessorId == (int)PaymentMethod.SolidTrustPay)
							{
								model.WithdrawalAccount = model.SolidTrustPayAcc;
							}
							if (model.ProcessorId == (int)PaymentMethod.Payeer)
							{
								model.WithdrawalAccount = model.PayeerAcc;
							}
							if (model.ProcessorId == (int)PaymentMethod.PM)
							{
								model.WithdrawalAccount = model.PMAcc;
							}

							if (model.WithdrawalAccount.IsEmpty())
							{
								return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Please update your Withdrawal account in Profile page" });
							}
							var transcation = model.ToEntity();
							transcation.ProcessorId = model.ProcessorId;
							transcation.TranscationTypeId = model.TranscationTypeId;
							transcation.StatusId = (int)Status.Pending;
							transcation.WithdrawalAddress = model.WithdrawalAccount;
							_transactionService.InsertTransaction(transcation);

							// Notifications
							if (withdrawalSettings.NotifyWithdrawalRequestToUser)
								Services.MessageFactory.SendWithdrawalNotificationMessageToUser(transcation, "", "", _localizationSettings.DefaultAdminLanguageId);
							if (withdrawalSettings.NotifyWithdrawalRequestToAdmin)
								Services.MessageFactory.SendWithdrawalNotificationMessageToAdmin(transcation, "", "", _localizationSettings.DefaultAdminLanguageId);

							return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success" });
						}
						catch (Exception ex)
						{

						}
						
					}
					else
					{
						return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Withdrawal temporary disabled" });
					}
				}
			}
			catch (Exception exception)
			{
			}
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "something went wrong" });
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetAllCountry")]
		public HttpResponseMessage GetAllCountry()
		{
			var countries = _countryService.GetAllCountries();
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success",data = countries });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("TransferFund")]
		public HttpResponseMessage TransferFund(TransactionModel model)
		{
			var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
			var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
			if (customerguid != null)
			{
				if (model.CustomerId != cust.Id)
				{
					return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
				}
			}
			var availableBalance = _customerService.GetAvailableBalance(cust.Id);
			if (model.Amount <= 0)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Enter correct amount" });
			}
			if (availableBalance < model.Amount * 4)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "You do not have enough fund" });
			}
			var exists = _customerService.GetCustomerByUsername(model.CustomerEmail);

			var InTeam = _customerService.GetIsReceiverInTeam(model.CustomerId, exists.Id);
			if(InTeam == 0)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "You Can Send only in your Team" });
			}
			if (exists != null)
			{
				if (exists.Id > 0)
				{
					TransactionModel transactionModel = new TransactionModel();
					transactionModel.Amount = Convert.ToInt64(model.Amount) * 4;
					transactionModel.CustomerId = exists.Id;
					transactionModel.FinalAmount = transactionModel.Amount;
					transactionModel.NoOfPosition = 0;
					transactionModel.TransactionDate = DateTime.Now;
					transactionModel.ProcessorId = 5;
					transactionModel.TranscationTypeId = (int)TransactionType.Funding;
					var transcation = transactionModel.ToEntity();
					transcation.StatusId = (int)Status.Completed;
					transcation.TranscationTypeId = (int)TransactionType.Funding;
					transcation.TranscationNote = "Received USD";
					_transactionService.InsertTransaction(transcation);

					transactionModel = new TransactionModel();
					transactionModel.Amount = Convert.ToInt64(model.Amount) * 4;
					transactionModel.CustomerId = cust.Id;
					transactionModel.FinalAmount = transactionModel.Amount;
					transactionModel.NoOfPosition = 0;
					transactionModel.TransactionDate = DateTime.Now;
					transactionModel.ProcessorId = 5;
					transactionModel.TranscationTypeId = (int)TransactionType.Transfer;
					transcation = transactionModel.ToEntity();
					transcation.StatusId = (int)Status.Completed;
					transcation.TranscationTypeId = (int)TransactionType.Transfer;
					transcation.TranscationNote = "Transfer USD";
					_transactionService.InsertTransaction(transcation);

					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success" });
				}
			}
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "something went wrong" });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("TransferHistory")]
		public HttpResponseMessage TransferHistory(TransactionModel model)
		{
			var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
			var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
			if (customerguid != null)
			{
				
				if (model.CustomerId != cust.Id)
				{
					return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
				}
			}
			var availableBalance = _customerService.GetAvailableBalance(cust.Id);
			if (model.Amount <= 0)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Enter correct amount" });
			}
			if (availableBalance < model.Amount * 4)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "You do not have enough fund" });
			}
			var exists = _customerService.GetCustomerByUsername(model.CustomerEmail);
			if (exists != null)
			{
				if (exists.Id > 0)
				{
					TransactionModel transactionModel = new TransactionModel();
					transactionModel.Amount = Convert.ToInt64(model.Amount) * 4;
					transactionModel.CustomerId = exists.Id;
					transactionModel.FinalAmount = transactionModel.Amount;
					transactionModel.NoOfPosition = 0;
					transactionModel.TransactionDate = DateTime.Now;
					transactionModel.ProcessorId = 5;
					transactionModel.TranscationTypeId = (int)TransactionType.Funding;
					var transcation = transactionModel.ToEntity();
					transcation.StatusId = (int)Status.Completed;
					transcation.TranscationTypeId = (int)TransactionType.Funding;
					_transactionService.InsertTransaction(transcation);

					transactionModel = new TransactionModel();
					transactionModel.Amount = Convert.ToInt64(model.Amount) * 4;
					transactionModel.CustomerId = cust.Id;
					transactionModel.FinalAmount = transactionModel.Amount;
					transactionModel.NoOfPosition = 0;
					transactionModel.TransactionDate = DateTime.Now;
					transactionModel.ProcessorId = 5;
					transactionModel.TranscationTypeId = (int)TransactionType.Transfer;
					transcation = transactionModel.ToEntity();
					transcation.StatusId = (int)Status.Completed;
					transcation.TranscationTypeId = (int)TransactionType.Transfer;
					_transactionService.InsertTransaction(transcation);

					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success" });
				}
			}
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "something went wrong" });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("TransferCoin")]
		public HttpResponseMessage TransferCoin(TransactionModel model)
		{
			var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
			if (customerguid != null)
			{
				var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
				if (model.CustomerId != cust.Id)
				{
					return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
				}
			}
			var customer = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
			
			var availableCoin = _customerService.GetAvailableCoin(customer.Id);
			if (model.Amount <= 0)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Enter correct no of coin" });
			}
			if (!customer.IsAdmin())
			{
				if (availableCoin < model.Amount)
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "You do not have enough fund" });
				}
			}			
			var exists = _customerService.GetCustomerByUsername(model.CustomerEmail);
			var InTeam = _customerService.GetIsReceiverInTeam(model.CustomerId, exists.Id);
			if (InTeam == 0)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "You Can Send only in your Team" });
			}
			if (exists != null)
			{
				if (exists.Id > 0)
				{
					var transcation = new Transaction();
					transcation.Amount = Convert.ToInt64(model.Amount);
					transcation.CustomerId = exists.Id;
					transcation.FinalAmount = transcation.Amount;
					transcation.NoOfPosition = 0;
					transcation.TransactionDate = DateTime.Now;
					transcation.ProcessorId = 5;
					transcation.TranscationTypeId = (int)TransactionType.EarnedCoin;
					transcation.StatusId = (int)Status.Completed;
					transcation.TranscationTypeId = (int)TransactionType.EarnedCoin;
					transcation.TranscationNote = "Received Coin";
					_transactionService.InsertTransaction(transcation);

					transcation = new Transaction();
					transcation.Amount = Convert.ToInt64(model.Amount);
					transcation.CustomerId = customer.Id;
					transcation.FinalAmount = transcation.Amount;
					transcation.NoOfPosition = 0;
					transcation.TransactionDate = DateTime.Now;
					transcation.ProcessorId = 5;
					transcation.TranscationTypeId = (int)TransactionType.TransferCoin;
					transcation.StatusId = (int)Status.Completed;
					transcation.TranscationTypeId = (int)TransactionType.TransferCoin;
					transcation.TranscationNote = "Transfer Coin";
					_transactionService.InsertTransaction(transcation);

					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success" });
				}
			}
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "something went wrong" });
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GenerateAddress")]
		public HttpResponseMessage GenerateAddress(int transId,int CustomerId)
		{
			var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
			if (customerguid != null)
			{
				var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
				if (CustomerId != cust.Id)
				{
					return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
				}
			}
			var secret = "e57b3908-874a-42e7-be52-7b619139b668";
			var callbackurl = "https://www.globalmatrixmillionaire.com/Blockchain?invoice=" + transId + "&secret=" + secret;
			callbackurl = HttpUtility.UrlEncode(callbackurl);
			var client = new RestClient();
			var request = new RestRequest("https://api.blockchain.info/v2/receive?key=e57b3908-874a-42e7-be52-7b619139b668&xpub=xpub6CNC9WdGrQWko18roC5xQyYDddFLZrwGkzYgrV4rKJ5Gowg8QyrBPNsboaSN7kufMPdwbaA2D61YW7cpVFaA4wmQZq2cDbgRfVJJJkdY4s3&callback="+callbackurl, Method.GET);
			var queryResult = client.Execute<dynamic>(request).Data;

			request = new RestRequest("https://chart.googleapis.com/chart?chs=250x250&cht=qr&chl=" + queryResult.address, Method.GET);
			var imgResult = client.Execute<dynamic>(request).Data;
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success",data = queryResult.address, });
		}

	}
}