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
using SmartStore.Admin;
using Telerik.Web.Mvc;
using SmartStore.Web.Framework.WebApi.Security;

namespace SmartStore.WebApi.Controllers.Api
{
	[CustomWebApiAuthenticate]
	public class RevShareController : ApiController
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
		private readonly ICustomerRegistrationService _customerRegistrationService;
		private readonly ITaxService _taxService;
		private readonly CustomerSettings _customerSettings;
		private readonly ICurrencyService _currencyService;
		private readonly IPriceFormatter _priceFormatter;
		private readonly IPictureService _pictureService;
		private readonly IOpenAuthenticationService _openAuthenticationService;
		private readonly IDownloadService _downloadService;
		private readonly IWebHelper _webHelper;
		private readonly ICustomerActivityService _customerActivityService;
		private readonly MediaSettings _mediaSettings;
		private readonly LocalizationSettings _localizationSettings;
		private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
		private readonly PluginMediator _pluginMediator;
		private readonly IPermissionService _permissionService;
		private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
		private readonly IBoardService _boardService;
		private readonly IPlanService _planService;
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ITransactionService _transactionService;
		#endregion

		#region Ctor

		public RevShareController(
			ITransactionService transactionService,
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
			ICustomerRegistrationService customerRegistrationService,
			ITaxService taxService, RewardPointsSettings rewardPointsSettings,
			CustomerSettings customerSettings, AddressSettings addressSettings, ForumSettings forumSettings,
			ICurrencyService currencyService,
			IPriceFormatter priceFormatter,
			IPictureService pictureService, INewsLetterSubscriptionService newsLetterSubscriptionService,
			ICustomerActivityService customerActivityService,
			MediaSettings mediaSettings,
			LocalizationSettings localizationSettings,
			ExternalAuthenticationSettings externalAuthenticationSettings,
			PluginMediator pluginMediator,
			IPermissionService permissionService,
			IBoardService boardService)
		{
			_planService = planService;
			_customerPlanService = customerPlanService;
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
			_customerRegistrationService = customerRegistrationService;
			_taxService = taxService;
			_customerSettings = customerSettings;
			_currencyService = currencyService;
			_priceFormatter = priceFormatter;
			_pictureService = pictureService;
			_newsLetterSubscriptionService = newsLetterSubscriptionService;
			_customerActivityService = customerActivityService;
			_mediaSettings = mediaSettings;
			_localizationSettings = localizationSettings;
			_externalAuthenticationSettings = externalAuthenticationSettings;
			_pluginMediator = pluginMediator;
			_permissionService = permissionService;
			_boardService = boardService;

		}

		#endregion

		public void ReleaseLevelCommission(int planid, Customer customer, float amountinvested)
		{
			//Save board position
			int customerid = customer.AffiliateId;
			Transaction transaction;
			Customer levelcustomer = _customerService.GetCustomerById(customerid);
			var plan = _planService.GetPlanById(planid);
			var planCommission = _planService.GetPlanCommissions(plan.Id).FirstOrDefault();
			//Direct Bonus
			if (levelcustomer != null)
			{
				//Send Direct Bonus
				try
				{
					if (levelcustomer.Transaction.Where(x => x.StatusId == 2).Sum(x => x.Amount) > 0)
					{
						transaction = new Transaction();
						transaction.CustomerId = levelcustomer.Id;
						transaction.RefId = customer.Id;
						transaction.Amount = plan.PayEveryXDays;
						transaction.FinalAmount = plan.PayEveryXDays;
						transaction.TransactionDate = DateTime.Now;
						transaction.StatusId = (int)Status.Completed;
						transaction.TranscationTypeId = (int)TransactionType.DirectBonus;
						transaction.TranscationNote = plan.Name + " Direct Bonus";
						_transactionService.InsertTransaction(transaction);
						Services.MessageFactory.SendDirectBonusNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
					}
				}
				catch (Exception ex)
				{
					//WritetoLog("Direct Bonus error :" + ex.ToString());
				}
			}

			for (int i = 0; i < 10; i++)
			{
				if (levelcustomer != null)
				{
					try
					{
						if (levelcustomer.CustomerPlan.Count() > 0)
						{
							if (levelcustomer.CustomerPlan.Max(x => x.PlanId) == 3)
							{
								transaction = new Transaction();
								transaction.CustomerId = levelcustomer.Id;
								transaction.Amount = (float)planCommission.CommissionPercentage * plan.MinimumInvestment;
								transaction.FinalAmount = (float)planCommission.CommissionPercentage * plan.MinimumInvestment;
								transaction.TransactionDate = DateTime.Now;
								transaction.StatusId = (int)Status.Completed;
								transaction.RefId = customer.Id;
								transaction.TranscationTypeId = (int)TransactionType.CyclerBonus;
								transaction.TranscationNote = "Unilevel Earning (" + plan.Name + ")";
								_transactionService.InsertTransaction(transaction);
								Services.MessageFactory.SendUnilevelBonusNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
							}
						}
					}
					catch (Exception ex)
					{
						//WritetoLog("Direct Bonus error :" + ex.ToString());
					}
					levelcustomer = _customerService.GetCustomerById(levelcustomer.AffiliateId);
				}
			}

			for (int i = 0; i < 5; i++)
			{
				if (levelcustomer != null)
				{
					try
					{
						if (levelcustomer.CustomerPlan.Count() > 0)
						{
							if (levelcustomer.CustomerPlan.Max(x => x.PlanId) == 2)
							{
								transaction = new Transaction();
								transaction.CustomerId = levelcustomer.Id;
								transaction.Amount = (float)planCommission.CommissionPercentage * plan.MinimumInvestment;
								transaction.FinalAmount = (float)planCommission.CommissionPercentage * plan.MinimumInvestment;
								transaction.TransactionDate = DateTime.Now;
								transaction.StatusId = (int)Status.Completed;
								transaction.RefId = customer.Id;
								transaction.TranscationTypeId = (int)TransactionType.CyclerBonus;
								transaction.TranscationNote = "Unilevel Earning (" + plan.Name + ")";
								_transactionService.InsertTransaction(transaction);
								Services.MessageFactory.SendUnilevelBonusNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
							}
						}
					}
					catch (Exception ex)
					{
						//WritetoLog("Direct Bonus error :" + ex.ToString());
					}
					levelcustomer = _customerService.GetCustomerById(levelcustomer.AffiliateId);
				}
			}
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("BuyShare")]
		public HttpResponseMessage BuyShare(CustomerPlanModel customerPlanModel)
		{
			string message = "";
			try
			{
				var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
				if (customerguid != null)
				{
					var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
					if (customerPlanModel.CustomerId != cust.Id)
					{
						return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
					}
				}
				var Customer = _customerService.GetCustomerById(customerPlanModel.CustomerId);
				if (ModelState.IsValid)
				{

					PrepareCustomerPlanModel(customerPlanModel);
					customerPlanModel.AvailableBalance = _customerService.GetAvailableBalance(Customer.Id);// _customerService.GetRepurchaseBalance(_workContext.CurrentCustomer.Id);

					if (customerPlanModel.PlanId > 0)
					{
						var plan = _planService.GetPlanById(customerPlanModel.PlanId);
						var repurchasebalance = _customerService.GetAvailableBalance(Customer.Id);
						var amountreq = Convert.ToInt64(plan.ROIPercentage); //Convert.ToInt64(plan.MinimumInvestment) * ((customerPlanModel.NoOfPosition == 0) ? 1 : customerPlanModel.NoOfPosition);

						if (repurchasebalance < amountreq)
						{
							message = "You do not have enough balance, Please add funds";
							return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = message });
						}

						TransactionModel transactionModel = new TransactionModel();
						transactionModel.Amount = amountreq;
						transactionModel.CustomerId = Customer.Id;
						transactionModel.FinalAmount = transactionModel.Amount;
						transactionModel.NoOfPosition = customerPlanModel.NoOfPosition;
						transactionModel.TransactionDate = DateTime.Now;
						transactionModel.RefId = plan.Id;
						transactionModel.ProcessorId = customerPlanModel.ProcessorId;
						transactionModel.TranscationTypeId = (int)TransactionType.Purchase;
						var transcation = transactionModel.ToEntity();

						transcation.NoOfPosition = customerPlanModel.NoOfPosition;
						transcation.TranscationTypeId = (int)TransactionType.Purchase;
						transcation.StatusId = (int)Status.Completed;
						_transactionService.InsertTransaction(transcation);

						var customerplan = new CustomerPlan();
						customerplan.CustomerId = transcation.CustomerId;
						customerplan.PurchaseDate = DateTime.Now;
						customerplan.CreatedOnUtc = DateTime.Now;
						customerplan.UpdatedOnUtc = DateTime.Now;
						customerplan.PlanId = plan.Id;
						customerplan.AmountInvested = plan.ROIPercentage;
						customerplan.ROIToPay = 0;
						customerplan.NoOfPayout = 0;
						customerplan.ExpiredDate = DateTime.Today;
						customerplan.IsActive = true;
						if (plan.StartROIAfterHours > 0)
							customerplan.LastPaidDate = DateTime.Today.AddHours(plan.StartROIAfterHours);
						else
							customerplan.LastPaidDate = DateTime.Today;
						_customerPlanService.InsertCustomerPlan(customerplan);

						_customerService.SpPayNetworkIncome(customerplan.CustomerId, customerplan.PlanId);

						message = "Your purchase was successfull";
						ReleaseLevelCommission(plan.Id, Customer, transactionModel.Amount);
						return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success" });
					}
					else
					{
						message = "Please select Package";
					}
				}
			}
			catch (Exception ex)
			{
				message = T("Invesment.Deposit.FundingError").Text;
			}
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = message });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("BuyShareWithCoin")]
		public HttpResponseMessage BuyShareWithCoin(CustomerPlanModel customerPlanModel)
		{
			string message = "";
			try
			{
				var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
				if (customerguid != null)
				{
					var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
					if (customerPlanModel.CustomerId != cust.Id)
					{
						return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
					}
				}
				var Customer = _customerService.GetCustomerById(customerPlanModel.CustomerId);
				if (ModelState.IsValid)
				{

					PrepareCustomerPlanModel(customerPlanModel);
					customerPlanModel.AvailableBalance = _customerService.GetAvailableCoin(Customer.Id);// _customerService.GetRepurchaseBalance(_workContext.CurrentCustomer.Id);

					if (customerPlanModel.PlanId > 0)
					{
						var plan = _planService.GetPlanById(customerPlanModel.PlanId);
						var repurchasebalance = _customerService.GetAvailableCoin(Customer.Id);
						var amountreq = Convert.ToInt64(plan.MinimumInvestment); //Convert.ToInt64(plan.MinimumInvestment) * ((customerPlanModel.NoOfPosition == 0) ? 1 : customerPlanModel.NoOfPosition);

						var Count = _customerService.GetPlanCount(Customer.Id);

						if (Count < 2)
						{
							var PlanId = _customerService.GetCurrentPlan(Customer.Id);
							var List = _customerService.GetCurrentPlanList(Customer.Id);

							var Previousplan = _planService.GetPlanById(PlanId);
							if (PlanId != 0)
							{
								var Date = DateTime.Now;
								if (List.PurchaseDate.Date == Date.Date)
								{
									return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "You can not Upgrade Plan Within 24 Hrs" });
								}
								else if (Previousplan.MinimumInvestment == plan.MinimumInvestment)
								{
									return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "You Have Already Purchased this Plan" });
								}
								else if (Previousplan.MinimumInvestment > plan.MinimumInvestment)
								{
									return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "You Have Already Purchased Higer Plan" });
								}
								else if (Previousplan.MinimumInvestment < plan.MinimumInvestment)
								{
									amountreq = Convert.ToInt64(plan.MinimumInvestment) - Convert.ToInt64(Previousplan.MinimumInvestment);
								}
							}

							if (repurchasebalance < amountreq)
							{
								message = "You do not have enough coin, Please add coins";
								return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = message });
							}

							_customerPlanService.DiseableOldCustomerPlan(Customer.Id);

							TransactionModel transactionModel = new TransactionModel();
							transactionModel.Amount = plan.MinimumInvestment;
							transactionModel.CustomerId = Customer.Id;
							transactionModel.FinalAmount = transactionModel.Amount;
							transactionModel.NoOfPosition = customerPlanModel.NoOfPosition;
							transactionModel.TransactionDate = DateTime.Now;
							transactionModel.RefId = plan.Id;
							transactionModel.ProcessorId = customerPlanModel.ProcessorId;
							transactionModel.TranscationTypeId = (int)TransactionType.PurchaseByCoin;
							var transcation = transactionModel.ToEntity();

							transcation.NoOfPosition = customerPlanModel.NoOfPosition;
							transcation.TranscationTypeId = (int)TransactionType.PurchaseByCoin;
							transcation.StatusId = (int)Status.Completed;
							_transactionService.InsertTransaction(transcation);

							var customerplan = new CustomerPlan();
							customerplan.CustomerId = transcation.CustomerId;
							customerplan.PurchaseDate = DateTime.Now;
							customerplan.CreatedOnUtc = DateTime.Now;
							customerplan.UpdatedOnUtc = DateTime.Now;
							customerplan.PlanId = plan.Id;
							customerplan.AmountInvested = plan.MinimumInvestment; //plan.ROIPercentage;
							customerplan.ROIToPay = 0;
							customerplan.NoOfPayout = 0;
							customerplan.ExpiredDate = DateTime.Today;
							customerplan.IsActive = true;
							customerplan.RepurchaseWallet = customerPlanModel.StokistId;
							if (plan.StartROIAfterHours > 0)
								customerplan.LastPaidDate = DateTime.Today.AddHours(plan.StartROIAfterHours);
							else
								customerplan.LastPaidDate = DateTime.Today;
							_customerPlanService.InsertCustomerPlan(customerplan);

							try
							{
								//Stokist Commision Code Start
								var StokistDetail = _customerService.GetCustomerById(customerPlanModel.StokistId);

								var StokistCommision = StokistDetail.GetAttribute<float>(SystemCustomerAttributeNames.StokistCommision);

								transactionModel = new TransactionModel();
								transactionModel.Amount = StokistCommision * plan.MinimumInvestment;
								transactionModel.CustomerId = StokistDetail.Id;
								transactionModel.FinalAmount = transactionModel.Amount;
								transactionModel.NoOfPosition = 0;
								transactionModel.TransactionDate = DateTime.Now;
								transactionModel.RefId = Customer.Id;
								transactionModel.ProcessorId = 5;
								transactionModel.TranscationTypeId = (int)TransactionType.PoolBonus;
								var transcation1 = transactionModel.ToEntity();
								transcation1.StatusId = (int)Status.Completed;
								transcation1.TranscationTypeId = (int)TransactionType.PoolBonus;
								transcation1.TranscationNote = "Received Stokist Commission";
								_transactionService.InsertTransaction(transcation1);
								//Stokist Commision Code End
							}
							catch (Exception ex)
							{

							}
							_customerService.SpPayNetworkIncome(customerplan.CustomerId, customerplan.PlanId);

							message = "Your purchase was successfull";
							ReleaseLevelCommission(plan.Id, Customer, transactionModel.Amount);
							return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success" });
						}
						else
						{
							return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "You Have Already Reached Maximum Package Limit" });
						}
					}
					else
					{
						message = "Please select Package";
					}
				}
			}
			catch (Exception ex)
			{
				message = T("Invesment.Deposit.FundingError").Text;
			}
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = message });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("MyShare")]
		public HttpResponseMessage MyShare(int CustomerId)
		{
			string message = "";
			try
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
				var Customer = _customerService.GetCustomerById(CustomerId);
				var gridModel = new GridModel<MyInvestmentPlan>();
				var customerplan = Customer.CustomerPlan.Where(x => x.IsActive == true).ToList();
				gridModel.Data = customerplan.Select(x =>
				{
					var myInvestment = new MyInvestmentPlan();
					myInvestment.ShareId = x.Id;
					myInvestment.PlanId = x.PlanId;
					myInvestment.PlanName = _planService.GetPlanById(x.PlanId).Name;
					myInvestment.PurchaseDate = x.PurchaseDate;
					myInvestment.ROIPaid = x.ROIPaid;
					myInvestment.ROIToPay = x.ROIToPay;
					myInvestment.ROIPending = (x.ROIToPay - x.ROIPaid);
					myInvestment.ROIPaidString = x.ROIPaid.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
					myInvestment.ROIToPayString = x.ROIToPay.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
					myInvestment.ROIPendingString = myInvestment.ROIPending.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
					myInvestment.TotalFundingString = x.AmountInvested.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
					myInvestment.IsActive = x.IsActive;
					myInvestment.Status = (x.IsActive) ? ((x.IsExpired == false) ? "Active" : "Expired") : "InActive";
					myInvestment.ExpireDate = x.ExpiredDate;
					return myInvestment;
				});

				gridModel.Total = customerplan.Count;
				message = "success";
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = message, data = gridModel });
			}
			catch (Exception ex)
			{
				message = T("Invesment.Deposit.FundingError").Text;
			}
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = message });
		}

		public void PrepareCustomerPlanModel(CustomerPlanModel model)
		{
			var plans = _planService.GetAllPlans().OrderBy(x => x.Name);
			foreach (var plan in plans)
			{
				model.AvailablePlans.Add(new SelectListItem()
				{
					Text = plan.Name + "(" + plan.PlanDetails + ")",
					Value = plan.Id.ToString()
				});
			}

			var storeScope = _storeContext.CurrentStore.Id;

			var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(storeScope);

			var SolitTrustPaySettings = _services.Settings.LoadSetting<SolidTrustPaySettings>(storeScope);

			var PayzaSettings = _services.Settings.LoadSetting<PayzaSettings>(storeScope);

			var PMSettings = _services.Settings.LoadSetting<PMSettings>(storeScope);

			var PayeerSettings = _services.Settings.LoadSetting<PayeerSettings>(storeScope);

			//if (coinpaymentSettings.CP_IsActivePaymentMethod)
			//{
			//	model.AvailableProcessor.Add(new SelectListItem()
			//	{
			//		Text = "Bitcoin",
			//		Value = "0"
			//	});
			//}
			//if (PayzaSettings.PZ_IsActivePaymentMethod)
			//{
			//	model.AvailableProcessor.Add(new SelectListItem()
			//	{
			//		Text = "Payza",
			//		Value = "1"
			//	});
			//}
			//if (PMSettings.PM_IsActivePaymentMethod)
			//{
			//	model.AvailableProcessor.Add(new SelectListItem()
			//	{
			//		Text = "PM",
			//		Value = "2"
			//	});
			//}
			//if (PayeerSettings.PY_IsActivePaymentMethod)
			//{
			//	model.AvailableProcessor.Add(new SelectListItem()
			//	{
			//		Text = "Payeer",
			//		Value = "3"
			//	});
			//}
			//if (SolitTrustPaySettings.STP_IsActivePaymentMethod)
			//{
			//	model.AvailableProcessor.Add(new SelectListItem()
			//	{
			//		Text = "SolidTrustPay",
			//		Value = "4"
			//	});
			//}

			model.AvailableProcessor.Add(new SelectListItem()
			{
				Text = "Available Balance",
				Value = "5"
			});

		}

		private bool CheckIfPositionExists(int planid, Customer customer)
		{
			int boardid = 0;
			if (planid == 5)
				boardid = 18;
			else
				boardid = planid;

			if (customer.CustomerPosition.Where(x => x.BoardId == boardid).Count() > 0)
			{
				return true;
			}
			return false;
		}
	}
}