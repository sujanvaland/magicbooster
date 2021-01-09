using SmartStore.Admin.Models.Investment;
using SmartStore.Admin.Models.PaymentMethods;
using SmartStore.Core;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Domain.Localization;
using SmartStore.Services;
using SmartStore.Services.Catalog;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Services.Directory;
using SmartStore.Services.Helpers;
using SmartStore.Services.Hyip;
using SmartStore.Services.Localization;
using SmartStore.Services.Stores;
using SmartStore.Web.Administration.Models.Investment;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;
using SmartStore.Web.Framework.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace SmartStore.WebApi.Controllers.Api
{
	[CustomWebApiAuthenticate]
	public class ReportController : ApiController
	{
		private readonly IPlanService _planService;
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ITransactionService _transactionService;
		private readonly IDateTimeHelper _dateTimeHelper;
		private readonly IWorkContext _workContext;
		private readonly AdminAreaSettings _adminAreaSettings;
		private readonly ILocalizationService _localizationService;
		private readonly IPriceFormatter _priceFormatter;
		private readonly ICurrencyService _currencyService;
		private readonly IStoreService _storeService;
		private readonly ICommonServices _services;
		private readonly ICustomerService _customerService;
		private readonly LocalizationSettings _localizationSettings;
		public ReportController(IPlanService planService,
			ICustomerPlanService customerPlanService,
			IDateTimeHelper dateTimeHelper,
			IWorkContext workContext,
			ITransactionService transactionService,
			AdminAreaSettings adminAreaSettings,
			ILocalizationService localizationService,
			IPriceFormatter priceFormatter,
			ICurrencyService currencyService,
			IStoreService storeService,
			ICommonServices services,
			ICustomerService customerService,
			LocalizationSettings localizationSettings)
		{
			_planService = planService;
			_customerPlanService = customerPlanService;
			_dateTimeHelper = dateTimeHelper;
			_workContext = workContext;
			_transactionService = transactionService;
			_adminAreaSettings = adminAreaSettings;
			_localizationService = localizationService;
			_priceFormatter = priceFormatter;
			_currencyService = currencyService;
			_storeService = storeService;
			_services = services;
			_customerService = customerService;
			_localizationSettings = localizationSettings;
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("Fundingreport")]
		public HttpResponseMessage ListFunding(TransactionModel model)
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
			string message = "";
			var gridModel = new GridModel<TransactionModel>();
			DateTime? startDateValue = (model.StartDate == null) ? null : model.StartDate;
			DateTime? endDateValue = (model.EndDate == null) ? null : model.EndDate;
			var Customer = _customerService.GetCustomerById(model.CustomerId);
			int[] StatusIds = model.StatusIds.ToIntArray();
			int[] TranscationTypeIds = { (int)TransactionType.PurchaseByCoin };
			var customerid = Customer.Id;
			bool Is_Visible = true;
			if (Customer.IsInCustomerRole("Administrators"))
			{
				customerid = 0;
				Is_Visible = false;
			}

			var transcation = _transactionService.GetAllTransactions(0, customerid, startDateValue, endDateValue,
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			gridModel.Data = transcation.Select(x =>
			{
				var transModel = new TransactionModel
				{
					Id = x.Id,
					FinalAmountRaw = x.Amount + " " + currency,
					FinalAmount = x.Amount,
					TransactionDate = x.TransactionDate,
					IsVisible = Is_Visible,
					StatusId = x.StatusId,
					TransStatusString = x.Status.GetLocalizedEnum(_localizationService, _workContext),
					TranscationTypeString = x.TranscationType.GetLocalizedEnum(_localizationService, _workContext),
					ProcessorId = x.ProcessorId,
					ProcessorName = ((PaymentMethod)x.ProcessorId).GetLocalizedEnum(_localizationService, _workContext),
					WithdrawalAccount = x.WithdrawalAddress,
					CustomerEmail = x.Customer.Email
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();

			gridModel.Total = transcation.TotalCount;
			message = "success";
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = message, data = gridModel });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("Withdrawalreport")]
		public HttpResponseMessage ListWithdrawal(TransactionModel model)
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
			var gridModel = new GridModel<TransactionModel>();
			DateTime? startDateValue = (model.StartDate == null) ? null : model.StartDate;
			DateTime? endDateValue = (model.EndDate == null) ? null : model.EndDate;
			var Customer = _customerService.GetCustomerById(model.CustomerId);

			int[] StatusIds = model.StatusIds.ToIntArray();
			int[] TranscationTypeIds = { (int)TransactionType.Withdrawal };
			bool Is_Visible = false;
			int customerid = Customer.Id;
			if (Customer.IsInCustomerRole("Administrators"))
			{
				customerid = 0;
				Is_Visible = true;
			}

			var transcation = _transactionService.GetAllTransactions(0, Customer.Id, startDateValue, endDateValue,
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			gridModel.Data = transcation.Select(x =>
			{
				var transModel = new TransactionModel
				{
					Id = x.Id,
					FinalAmountRaw = x.Amount + " " + currency,
					FinalAmount = x.Amount,
					TransactionDate = x.TransactionDate,
					StatusId = x.StatusId,
					IsVisible = Is_Visible,
					UPIPaymentNumber = x.Customer.GetAttribute<string>(SystemCustomerAttributeNames.UPIPaymentNumber),
					BitcoinAddress = x.Customer.GetAttribute<string>(SystemCustomerAttributeNames.BitcoinAddressAcc),

					BankName = x.Customer.GetAttribute<string>(SystemCustomerAttributeNames.BankName),
					AccountHolderName = x.Customer.GetAttribute<string>(SystemCustomerAttributeNames.AccountHolderName),
					AccountNumber = x.Customer.GetAttribute<string>(SystemCustomerAttributeNames.AccountNumber),
					NICR = x.Customer.GetAttribute<string>(SystemCustomerAttributeNames.NICR),

					PayeerAcc = x.Customer.GetAttribute<string>(SystemCustomerAttributeNames.PayeerAcc),
					PMAcc = x.Customer.GetAttribute<string>(SystemCustomerAttributeNames.PMAcc),
					SolidTrustPayAcc = x.Customer.GetAttribute<string>(SystemCustomerAttributeNames.SolidTrustPayAcc),
					TransStatusString = x.Status.GetLocalizedEnum(_localizationService, _workContext),
					TranscationTypeString = x.TranscationType.GetLocalizedEnum(_localizationService, _workContext),
					ProcessorId = x.ProcessorId,
					ProcessorName = ((PaymentMethod)x.ProcessorId).GetLocalizedEnum(_localizationService, _workContext),
					WithdrawalAccount = x.WithdrawalAddress,
					CustomerEmail = x.Customer.Email
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();

			gridModel.Total = transcation.TotalCount;

			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = gridModel });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("Unilevelreport")]
		public HttpResponseMessage ListUnilevelBonus(TransactionModel model)
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
			var gridModel = new GridModel<TransactionModel>();
			DateTime? startDateValue = (model.StartDate == null) ? null : model.StartDate;
			DateTime? endDateValue = (model.EndDate == null) ? null : model.EndDate;
			var Customer = _customerService.GetCustomerById(model.CustomerId);
			int[] StatusIds = model.StatusIds.ToIntArray();
			int[] TranscationTypeIds = { (int)TransactionType.UnilevelBonus };
			var customerid = Customer.Id;
			bool Is_Visible = false;
			if (Customer.IsInCustomerRole("Administrators"))
			{
				customerid = 0;
				Is_Visible = true;
			}

			var transcation = _transactionService.GetAllTransactions(0, customerid, startDateValue, endDateValue,
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			gridModel.Data = transcation.Select(x =>
			{
				var transModel = new TransactionModel
				{
					Id = x.Id,
					FinalAmountRaw = x.Amount + " " + currency,
					FinalAmount = x.Amount,
					TransactionDate = x.TransactionDate,
					StatusId = x.StatusId,
					IsVisible = Is_Visible,
					TransStatusString = x.Status.GetLocalizedEnum(_localizationService, _workContext),
					TranscationTypeString = x.TranscationType.GetLocalizedEnum(_localizationService, _workContext),
					ProcessorId = x.ProcessorId,
					CustomerEmail = x.Customer.Email
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();

			gridModel.Total = transcation.TotalCount;

			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = gridModel });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("Directbonusreport")]
		public HttpResponseMessage ListDirectBonus(TransactionModel model)
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
			var gridModel = new GridModel<TransactionModel>();
			DateTime? startDateValue = (model.StartDate == null) ? null : model.StartDate;
			DateTime? endDateValue = (model.EndDate == null) ? null : model.EndDate;
			var Customer = _customerService.GetCustomerById(model.CustomerId);
			int[] StatusIds = model.StatusIds.ToIntArray();
			int[] TranscationTypeIds = { (int)TransactionType.DirectBonus };
			var customerid = Customer.Id;
			bool Is_Visible = false;
			if (Customer.IsInCustomerRole("Administrators"))
			{
				customerid = 0;
				Is_Visible = true;
			}

			var transcation = _transactionService.GetAllTransactions(0, customerid, startDateValue, endDateValue,
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			gridModel.Data = transcation.Select(x =>
			{
				var transModel = new TransactionModel
				{
					Id = x.Id,
					FinalAmountRaw = x.Amount + " " + currency,
					FinalAmount = x.Amount,
					TransactionDate = x.TransactionDate,
					StatusId = x.StatusId,
					IsVisible = Is_Visible,
					TransStatusString = x.Status.GetLocalizedEnum(_localizationService, _workContext),
					TranscationTypeString = x.TranscationType.GetLocalizedEnum(_localizationService, _workContext),
					ProcessorId = x.ProcessorId,
					CustomerEmail = x.Customer.Email
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();

			gridModel.Total = transcation.TotalCount;

			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = gridModel });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("Cyclerreport")]
		public HttpResponseMessage ListCyclerBonus(TransactionModel model)
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
			var gridModel = new GridModel<TransactionModel>();
			DateTime? startDateValue = (model.StartDate == null) ? null : model.StartDate;
			DateTime? endDateValue = (model.EndDate == null) ? null : model.EndDate;
			var Customer = _customerService.GetCustomerById(model.CustomerId);
			int[] StatusIds = model.StatusIds.ToIntArray();
			int[] TranscationTypeIds = { (int)TransactionType.CyclerBonus };
			var customerid = Customer.Id;
			bool Is_Visible = false;
			if (Customer.IsInCustomerRole("Administrators"))
			{
				customerid = 0;
				Is_Visible = true;
			}

			var transcation = _transactionService.GetAllTransactions(0, customerid, startDateValue, endDateValue,
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			gridModel.Data = transcation.Select(x =>
			{
				var transModel = new TransactionModel
				{
					Id = x.Id,
					FinalAmountRaw = x.Amount + " " + currency,
					FinalAmount = x.Amount,
					TransactionDate = x.TransactionDate,
					StatusId = x.StatusId,
					IsVisible = Is_Visible,
					TransStatusString = x.Status.GetLocalizedEnum(_localizationService, _workContext),
					TranscationTypeString = x.TranscationType.GetLocalizedEnum(_localizationService, _workContext),
					ProcessorId = x.ProcessorId,
					CustomerEmail = x.Customer.Email
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();

			gridModel.Total = transcation.TotalCount;

			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = gridModel });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("Poolbonusreport")]
		public HttpResponseMessage ListPoolBonus(TransactionModel model)
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
			var gridModel = new GridModel<TransactionModel>();
			DateTime? startDateValue = (model.StartDate == null) ? null : model.StartDate;
			DateTime? endDateValue = (model.EndDate == null) ? null : model.EndDate;
			var Customer = _customerService.GetCustomerById(model.CustomerId);
			int[] StatusIds = model.StatusIds.ToIntArray();
			int[] TranscationTypeIds = { (int)TransactionType.PoolBonus };
			var customerid = Customer.Id;
			bool Is_Visible = false;
			if (Customer.IsInCustomerRole("Administrators"))
			{
				customerid = 0;
				Is_Visible = true;
			}

			var transcation = _transactionService.GetAllTransactions(0, customerid, startDateValue, endDateValue,
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			gridModel.Data = transcation.Select(x =>
			{
				var transModel = new TransactionModel
				{
					Id = x.Id,
					FinalAmountRaw = x.Amount + " " + currency,
					FinalAmount = x.Amount,
					TransactionDate = x.TransactionDate,
					StatusId = x.StatusId,
					IsVisible = Is_Visible,
					TransStatusString = x.Status.GetLocalizedEnum(_localizationService, _workContext),
					TranscationTypeString = x.TranscationType.GetLocalizedEnum(_localizationService, _workContext),
					ProcessorId = x.ProcessorId,
					CustomerEmail = x.Customer.Email
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();

			gridModel.Total = transcation.TotalCount;


			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = gridModel });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("Transferhistoryreport")]
		public HttpResponseMessage ListTransferHistory(TransactionModel model)
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
			var gridModel = new GridModel<TransactionModel>();
			DateTime? startDateValue = (model.StartDate == null) ? null : model.StartDate;
			DateTime? endDateValue = (model.EndDate == null) ? null : model.EndDate;
			var Customer = _customerService.GetCustomerById(model.CustomerId);
			int[] StatusIds = model.StatusIds.ToIntArray();
			int[] TranscationTypeIds = { (int)TransactionType.Transfer };
			var customerid = Customer.Id;
			bool Is_Visible = false;
			if (Customer.IsInCustomerRole("Administrators"))
			{
				customerid = 0;
				Is_Visible = true;
			}

			var transcation = _transactionService.GetAllTransactions(0, customerid, startDateValue, endDateValue,
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			gridModel.Data = transcation.Select(x =>
			{
				var transModel = new TransactionModel
				{
					Id = x.Id,
					FinalAmountRaw = x.Amount + " " + currency,
					FinalAmount = x.Amount,
					TransactionDate = x.TransactionDate,
					StatusId = x.StatusId,
					IsVisible = Is_Visible,
					TransStatusString = x.Status.GetLocalizedEnum(_localizationService, _workContext),
					TranscationTypeString = x.TranscationType.GetLocalizedEnum(_localizationService, _workContext),
					TranscationNote = x.TranscationNote,
					ProcessorId = x.ProcessorId,
					CustomerEmail = x.Customer.Email
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();

			gridModel.Total = transcation.TotalCount;

			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = gridModel });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("Transferreceivedreport")]
		public HttpResponseMessage ListTransferReceivedHistory(GridCommand command, TransactionModel model)
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
			var gridModel = new GridModel<TransactionModel>();
			DateTime? startDateValue = (model.StartDate == null) ? null : model.StartDate;
			DateTime? endDateValue = (model.EndDate == null) ? null : model.EndDate;
			var Customer = _customerService.GetCustomerById(model.CustomerId);
			int[] StatusIds = model.StatusIds.ToIntArray();
			int[] TranscationTypeIds = { (int)TransactionType.Funding };
			var customerid = Customer.Id;
			bool Is_Visible = false;
			if (Customer.IsInCustomerRole("Administrators"))
			{
				customerid = 0;
				Is_Visible = true;
			}

			var transcation = _transactionService.GetAllTransactions(0, customerid, startDateValue, endDateValue, "5".ToIntArray(),
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			gridModel.Data = transcation.Select(x =>
			{
				var transModel = new TransactionModel
				{
					Id = x.Id,
					FinalAmountRaw = x.Amount + " " + currency,
					FinalAmount = x.Amount,
					TransactionDate = x.TransactionDate,
					StatusId = x.StatusId,
					IsVisible = Is_Visible,
					TransStatusString = x.Status.GetLocalizedEnum(_localizationService, _workContext),
					TranscationTypeString = x.TranscationType.GetLocalizedEnum(_localizationService, _workContext),
					TranscationNote = x.TranscationNote,
					ProcessorId = x.ProcessorId,
					CustomerEmail = x.Customer.Email
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();

			gridModel.Total = transcation.TotalCount;

			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = gridModel });
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("TransactionReport")]
		public HttpResponseMessage ListTransactionReport(TransactionModel model)
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
			var gridModel = new GridModel<AllTransactionModel>();

			var transcation = _transactionService.GetAllTransactions(model.CustomerId, model.TranscationTypeId,model.StartDate,model.EndDate);

			var currency = _workContext.WorkingCurrency.CurrencyCode;
			int[] Coin = new int[] { 15, 16, 17 };
			gridModel.Data = transcation.Select(x =>
			{
				var transModel = new AllTransactionModel
				{
					CustomerId = x.CustomerId,
					FinalAmountRaw = x.Amount + " " + (Coin.Contains(x.TranscationTypeId) == true ? "Coin" : currency),
					TransactionDate = x.TransactionDate,
					CustomerUserName = x.SenderUserName,
					RefUserName = x.ReceiverUserName,
					StokistName = x.StokistName
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();

			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = gridModel });
		}
	}
}