using SmartStore.Admin.Models.AddFund;
using SmartStore.Admin.Models.Investment;
using SmartStore.Admin.Models.PaymentMethods;
using SmartStore.Core;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Domain.Localization;
using SmartStore.Services;
using SmartStore.Services.Boards;
using SmartStore.Services.Customers;
using SmartStore.Services.Helpers;
using SmartStore.Services.Hyip;
using SmartStore.Services.Localization;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
	[AdminAuthorize]
	public class AddFundController : AdminControllerBase
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
		private readonly IBoardService _boardService;
		public AddFundController(ICommonServices commonServices,
			ICustomerService customerService,
			IPlanService planService,
			ICustomerPlanService customerPlanService,
			IDateTimeHelper dateTimeHelper,
			IWorkContext workContext,
			ITransactionService transactionService,
			ICommonServices services,
			ILocalizationService localizationService,
			LocalizationSettings localizationSettings,
			IBoardService boardService)
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
			_boardService = boardService;
		}
		// GET: AddFund
		public ActionResult Index()
        {
			CustomerPlanModel model = new CustomerPlanModel();
			var plans = _planService.GetAllPackage();

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
			
			//model.AvailableProcessor.Add(new SelectListItem()
			//{
			//	Text = "Bank Transfer(Only For Indian)",
			//	Value = "5"
			//});
			
			var Status = _workContext.CurrentCustomer.Transaction.Where(x => x.StatusId == 2 && x.TranscationNote == "Membership").Sum(x => x.Amount) > 0 ? "Active" : "Inactive";
			var AllowPurchase = System.Configuration.ConfigurationManager.AppSettings["AllowPurchase"].ToSafe();
			if (AllowPurchase == "false")
			{
				NotifyInfo("Purchase is disabled now");
				return RedirectToAction("Index", "AddFund");
			}
			ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			return View(model);
        }

		[HttpPost]
		public ActionResult Index(CustomerPlanModel model)
		{
			if(model.AmountInvested <= 0)
			{
				NotifyInfo("Enter correct amount");
				return RedirectToAction("Index", "AddFund");
			}
			TransactionModel transactionModel = new TransactionModel();
			transactionModel.Amount = Convert.ToInt64(model.AmountInvested);
			transactionModel.CustomerId = _workContext.CurrentCustomer.Id;
			transactionModel.FinalAmount = transactionModel.Amount;
			transactionModel.NoOfPosition = 0;
			transactionModel.TransactionDate = DateTime.Now;
			transactionModel.ProcessorId = model.ProcessorId;
			transactionModel.TranscationTypeId = (int)TransactionType.Funding;
			var transcation = transactionModel.ToEntity();
			transcation.StatusId = (int)Status.Pending;
			transcation.TranscationTypeId = (int)TransactionType.Funding;
			_transactionService.InsertTransaction(transcation);
			int value = model.ProcessorId;
			PaymentMethod paymentmethod = (PaymentMethod)value;

			model.Id = 0;
			model.ProcessorName = paymentmethod.ToString();
			model.PaymentMethod = paymentmethod;
			model.TransactionId = transcation.Id;
			model.AmountInvested = (decimal)transactionModel.Amount;
			return RedirectToAction("ConfirmPayment","Investment", model);
		}

		public ActionResult TransferFund()
		{
			ViewBag.AvailableBalance = _customerService.GetAvailableBalance(_workContext.CurrentCustomer.Id);
			var model = new TransactionModel();
			return View(model);
		}

		[HttpPost]
		public ActionResult TransferFund(TransactionModel model)
		{
			var availableBalance = _customerService.GetAvailableBalance(_workContext.CurrentCustomer.Id);
			if (model.Amount <= 0)
			{
				NotifyError("Enter correct amount");
				return RedirectToAction("TransferFund");
			}
			if (availableBalance < model.Amount)
			{
				NotifyError("You do not have enough fund");
				return RedirectToAction("TransferFund");
			}
			var exists = _customerService.GetCustomerByEmail(model.CustomerEmail);
			if(exists != null)
			{
				if(exists.Id > 0)
				{
					TransactionModel transactionModel = new TransactionModel();
					transactionModel.Amount = Convert.ToInt64(model.Amount);
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
					transactionModel.Amount = Convert.ToInt64(model.Amount);
					transactionModel.CustomerId = _workContext.CurrentCustomer.Id;
					transactionModel.FinalAmount = transactionModel.Amount;
					transactionModel.NoOfPosition = 0;
					transactionModel.TransactionDate = DateTime.Now;
					transactionModel.ProcessorId = 5;
					transactionModel.TranscationTypeId = (int)TransactionType.Transfer;
					transcation = transactionModel.ToEntity();
					transcation.StatusId = (int)Status.Completed;
					transcation.TranscationTypeId = (int)TransactionType.Transfer;
					_transactionService.InsertTransaction(transcation);

					NotifySuccess("Fund Transfer Successful");
					return RedirectToAction("TransferFund");
				}
			}
			return RedirectToAction("TransferFund");
		}
	}
}