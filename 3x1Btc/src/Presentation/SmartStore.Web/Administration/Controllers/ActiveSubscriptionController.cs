using SmartStore.Admin.Models.ActiveSubscription;
using SmartStore.Admin.Models.Investment;
using SmartStore.Admin.Models.PaymentMethods;
using SmartStore.Core;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Services;
using SmartStore.Services.Hyip;
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
	public class ActiveSubscriptionController : AdminControllerBase
	{
		private readonly IWorkContext _workContext;
		private readonly ITransactionService _transactionService;
		private readonly ICommonServices _services;
		public ActiveSubscriptionController(IWorkContext workContext, ITransactionService transactionService, ICommonServices services)
		{
			_workContext = workContext;
			_transactionService = transactionService;
			_services = services;
		}
		// GET: ActiveSubscription
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult SubscriptionActive()
		{
			ActiveSubscriptionModel model = new ActiveSubscriptionModel();
			var storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);

			var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(storeScope);

			var SolitTrustPaySettings = _services.Settings.LoadSetting<SolidTrustPaySettings>(storeScope);

			var PayzaSettings = _services.Settings.LoadSetting<PayzaSettings>(storeScope);

			var PMSettings = _services.Settings.LoadSetting<PMSettings>(storeScope);

			var PayeerSettings = _services.Settings.LoadSetting<PayeerSettings>(storeScope);

			var WithdrawalSettings = _services.Settings.LoadSetting<WithdrawalSettings>(storeScope);

			if (coinpaymentSettings.CP_IsActivePaymentMethod)
			{
				model.CoinPaymentEnabled = true;
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Bitcoin",
					Value = "0"
				});
			}
			if (PayzaSettings.PZ_IsActivePaymentMethod)
			{
				model.PayzaEnabled = true;
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Payza",
					Value = "1"
				});
			}
			if (PMSettings.PM_IsActivePaymentMethod)
			{
				model.PMEnabled = true;
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "PM",
					Value = "2"
				});
			}
			if (PayeerSettings.PY_IsActivePaymentMethod)
			{
				model.PayzaEnabled = true;
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Payeer",
					Value = "3"
				});
			}
			if (SolitTrustPaySettings.STP_IsActivePaymentMethod)
			{
				model.STPEnabled = true;
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "SolidTrustPay",
					Value = "4"
				});
			}
			return View(model);
		}

		[HttpPost]
		public ActionResult SubscriptionActive(ActiveSubscriptionModel model)
		{
			if(model.NoOfMonths > 0)
			{
				var amountreq = 1 * model.NoOfMonths;
				TransactionModel transactionModel = new TransactionModel();
				transactionModel.Amount = amountreq;
				transactionModel.CustomerId = _workContext.CurrentCustomer.Id;
				transactionModel.FinalAmount = amountreq;
				transactionModel.NoOfPosition = model.NoOfMonths;
				transactionModel.TransactionDate = DateTime.Now;
				transactionModel.RefId = 0;
				transactionModel.ProcessorId = model.ProcessorId;
				transactionModel.TranscationTypeId = (int)TransactionType.Purchase;
				transactionModel.TranscationNote = "subscription";
				var transcation = transactionModel.ToEntity();
				transcation.TranscationTypeId = (int)TransactionType.Purchase;
				transcation.StatusId = (int)Status.Pending;
				_transactionService.InsertTransaction(transcation);

				int value = model.ProcessorId;
				PaymentMethod paymentmethod = (PaymentMethod)value;
				CustomerPlanModel custmodel = new CustomerPlanModel();
				custmodel.Id = 0;
				custmodel.ProcessorName = paymentmethod.ToString();
				custmodel.PaymentMethod = paymentmethod;
				custmodel.TransactionId = transcation.Id;
				custmodel.AmountInvested = (decimal)transactionModel.Amount;
				return RedirectToAction("ConfirmPayment", "Investment", custmodel);
			}
			else
			{
				NotifyError("Enter Valid Months");
				return View(model);
			}
		}
	}
}