using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartStore.Admin.Models.Investment;
using SmartStore.Services.Helpers;
using SmartStore.Services.Hyip;
using SmartStore.Services.Localization;
using SmartStore.Services.Media;
using SmartStore.Services.Security;
using SmartStore.Services.Seo;
using SmartStore.Services.Stores;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Modelling;
using SmartStore.Web.Framework.Security;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using SmartStore.Admin.Models.PaymentMethods;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core;
using SmartStore.Services;
using SmartStore.Core.Domain.Customers;
using SmartStore.Services.Customers;
using SmartStore.Services.Common;
using SmartStore.Core.Domain.Localization;

namespace SmartStore.Admin.Controllers
{
	[AdminAuthorize]
	public class ShareController : AdminControllerBase
	{
		private readonly ICommonServices _commonServices;
		private readonly ICustomerService _customerService;
		private readonly IPlanService _planService;
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ITransactionService _transactionService;
		private readonly IDateTimeHelper _dateTimeHelper;
		private readonly IWorkContext _workContext;
		private readonly ICommonServices _services;
		private readonly ILocalizationService _localizationService;
		private readonly LocalizationSettings _localizationSettings;
		public ShareController(ICommonServices commonServices,
			ICustomerService customerService,
			IPlanService planService,
			ICustomerPlanService customerPlanService,
			IDateTimeHelper dateTimeHelper,
			IWorkContext workContext,
			ITransactionService transactionService,
			ICommonServices services,
			ILocalizationService localizationService,
			LocalizationSettings localizationSettings)
		{
			_commonServices = commonServices;
			_customerService = customerService;
			_planService = planService;
			_customerPlanService = customerPlanService;
			_dateTimeHelper = dateTimeHelper;
			_workContext = workContext;
			_transactionService = transactionService;
			_services = services;
			_localizationService = localizationService;
			_localizationSettings = localizationSettings;
		}

		public void PrepareCustomerPlanModel(CustomerPlanModel model)
		{
			var plans = _planService.GetAllPlans();
			foreach (var plan in plans)
			{
				model.AvailablePlans.Add(new SelectListItem()
				{
					Text = plan.Name,
					Value = plan.Id.ToString()
				});
			}

			var storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);

			var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(storeScope);

			var SolitTrustPaySettings = _services.Settings.LoadSetting<SolidTrustPaySettings>(storeScope);

			var PayzaSettings = _services.Settings.LoadSetting<PayzaSettings>(storeScope);

			var PMSettings = _services.Settings.LoadSetting<PMSettings>(storeScope);

			var PayeerSettings = _services.Settings.LoadSetting<PayeerSettings>(storeScope);

			if (coinpaymentSettings.CP_IsActivePaymentMethod)
			{
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Bitcoin",
					Value = "0"
				});
			}
			if (PayzaSettings.PZ_IsActivePaymentMethod)
			{
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Payza",
					Value = "1"
				});
			}
			if (PMSettings.PM_IsActivePaymentMethod)
			{
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "PM",
					Value = "2"
				});
			}
			if (PayeerSettings.PY_IsActivePaymentMethod)
			{
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Payeer",
					Value = "3"
				});
			}
			if (SolitTrustPaySettings.STP_IsActivePaymentMethod)
			{
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "SolidTrustPay",
					Value = "4"
				});
			}

		}
		// GET: Investment
		public ActionResult Purchase()
		{
			CustomerPlanModel model = new CustomerPlanModel();
			ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			model.NoOfPosition = 1;
			PrepareCustomerPlanModel(model);

			return View(model);
		}

		[HttpPost]
		public ActionResult Purchase(CustomerPlanModel customerPlanModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var plan = _planService.GetPlanById(customerPlanModel.PlanId);
					if (customerPlanModel.PlanId > 0)
					{
						TransactionModel transactionModel = new TransactionModel();
						transactionModel.Amount = Convert.ToInt64(plan.MinimumInvestment) * ((customerPlanModel.NoOfPosition == 0) ? 1 : customerPlanModel.NoOfPosition);
						transactionModel.CustomerId = _workContext.CurrentCustomer.Id;
						transactionModel.FinalAmount = transactionModel.Amount;
						transactionModel.NoOfPosition = customerPlanModel.NoOfPosition;
						transactionModel.TransactionDate = DateTime.Now;
						transactionModel.StatusId = (int)Status.Pending;
						transactionModel.RefId = plan.Id;
						transactionModel.ProcessorId = customerPlanModel.ProcessorId;
						transactionModel.TranscationTypeId = (int)TransactionType.SharePurchase;
						var transcation = transactionModel.ToEntity();
						transcation.TranscationTypeId = (int)TransactionType.SharePurchase;
						transcation.NoOfPosition = customerPlanModel.NoOfPosition;
						_transactionService.InsertTransaction(transcation);

						int value = customerPlanModel.ProcessorId;
						PaymentMethod paymentmethod = (PaymentMethod)value;

						ViewBag.SaveSuccess = true;
						customerPlanModel.Id = plan.Id;
						customerPlanModel.PlanName = plan.Name;
						customerPlanModel.ProcessorName = paymentmethod.ToString();
						customerPlanModel.PaymentMethod = paymentmethod;
						customerPlanModel.TransactionId = transcation.Id;
						customerPlanModel.AmountInvested = (decimal)transactionModel.Amount;
						return RedirectToAction("ConfirmPayment", customerPlanModel);
					}
					else
					{
						NotifyError("Please select Package");
					}
				}
			}
			catch (Exception ex)
			{
				ViewBag.SaveSuccess = false;
				NotifyError(T("Invesment.Deposit.FundingError"));
			}

			PrepareCustomerPlanModel(customerPlanModel);

			return View(customerPlanModel);
		}

		public ActionResult ConfirmPayment(CustomerPlanModel customerPlanModel)
		{
			return View(customerPlanModel);
		}

		public ActionResult CheckOut(CustomerPlanModel customerPlanModel)
		{
			if (customerPlanModel.PaymentMethod == PaymentMethod.CoinPayment)
			{
				var coinpaymentmodel = PrepareCoinPaymentModel(customerPlanModel);
				return View("_CoinPayment", coinpaymentmodel);
			}
			if (customerPlanModel.PaymentMethod == PaymentMethod.SolidTrustPay)
			{
				var coinpaymentmodel = PrepareCoinPaymentModel(customerPlanModel);
				return View("_CoinPayment", coinpaymentmodel);
			}
			if (customerPlanModel.PaymentMethod == PaymentMethod.Payza)
			{
				var coinpaymentmodel = PrepareCoinPaymentModel(customerPlanModel);
				return View("_CoinPayment", coinpaymentmodel);
			}
			if (customerPlanModel.PaymentMethod == PaymentMethod.PM)
			{
				var coinpaymentmodel = PrepareCoinPaymentModel(customerPlanModel);
				return View("_CoinPayment", coinpaymentmodel);
			}
			if (customerPlanModel.PaymentMethod == PaymentMethod.Payeer)
			{
				var coinpaymentmodel = PrepareCoinPaymentModel(customerPlanModel);
				return View("_CoinPayment", coinpaymentmodel);
			}

			return View();
		}

		private CoinPaymentModel PrepareCoinPaymentModel(CustomerPlanModel customerPlanModel)
		{
			var storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);

			var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(storeScope);

			CoinPaymentModel coinPaymentModel = new CoinPaymentModel();
			coinPaymentModel.MerchantAcc = coinpaymentSettings.CP_MerchantId;
			coinPaymentModel.Amount = customerPlanModel.AmountInvested;
			coinPaymentModel.FinalAmount = customerPlanModel.AmountInvested + ((customerPlanModel.AmountInvested * coinpaymentSettings.DepositFees) / 100);
			coinPaymentModel.CustomerPlanId = customerPlanModel.Id;
			coinPaymentModel.PlanName = customerPlanModel.PlanName;
			coinPaymentModel.ProcessorName = customerPlanModel.ProcessorName;
			coinPaymentModel.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			coinPaymentModel.PaymentMemo = T("Hyip.PaymentMemo");
			coinPaymentModel.DepositFees = coinpaymentSettings.DepositFees;
			coinPaymentModel.TransactionId = customerPlanModel.TransactionId;
			return coinPaymentModel;
		}

		public ActionResult MyShare()
		{
			MyInvestmentPlan model = new MyInvestmentPlan();
			var customerplans = _workContext.CurrentCustomer.CustomerPlan.Where(x => x.IsActive == true).ToList();
			model.MyTotalInvestment = customerplans.Select(x => x.AmountInvested).Sum();
			model.MyTotalROIPaid = customerplans.Select(x => x.ROIPaid).Sum();
			model.MyTotalROIToPay = customerplans.Select(x => x.ROIToPay).Sum();
			model.MyTotalROIPending = model.MyTotalROIToPay - model.MyTotalROIPaid;
			ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			return View(model);
		}

		[HttpPost, GridAction(EnableCustomBinding = true)]
		public ActionResult ListMyInvestment(GridCommand command)
		{
			var gridModel = new GridModel<MyInvestmentPlan>();
			var customerplan = _workContext.CurrentCustomer.CustomerPlan.Where(x => x.IsActive == true).ToList();
			gridModel.Data = customerplan.Select(x =>
			{
				var myInvestment = new MyInvestmentPlan();
				myInvestment.PlanName = _planService.GetPlanById(x.PlanId).Name;
				myInvestment.PurchaseDate = x.PurchaseDate;
				myInvestment.ROIPaid = x.ROIPaid;
				myInvestment.RepurchaseWallet = x.RepurchaseWallet;
				myInvestment.ROIToPay = x.ROIToPay;
				myInvestment.ROIPending = (x.ROIToPay - x.ROIPaid);
				myInvestment.ROIPaidString = x.ROIPaid.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				myInvestment.ROIToPayString = x.ROIToPay.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				myInvestment.ROIPendingString = myInvestment.ROIPending.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				myInvestment.RepurchaseWalletString = myInvestment.RepurchaseWallet.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				myInvestment.TotalFundingString = x.AmountInvested.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				myInvestment.IsActive = x.IsActive;
				myInvestment.Status = (x.IsActive) ? ((x.IsExpired == false) ? "Active" : "Expired") : "InActive";
				myInvestment.ExpireDate = x.ExpiredDate;
				return myInvestment;
			});

			gridModel.Total = customerplan.Count;

			return new JsonResult
			{
				Data = gridModel
			};
		}
	}
}