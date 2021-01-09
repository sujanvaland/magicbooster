using RestSharp;
using SmartStore.Admin.Models.Board;
using SmartStore.Core;
using SmartStore.Core.Domain.Blockchain;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Localization;
using SmartStore.Services.Boards;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
	[AdminAuthorize]
	public class MatrixController : AdminControllerBase
	{
		private readonly IBoardService _boardService;
		private readonly IWorkContext _workContext;
		private readonly ICustomerService _customerService;
		private readonly LocalizationSettings _localizationSettings;
		public MatrixController(IBoardService boardService,
			IWorkContext workContext,
			ICustomerService customerService,
			LocalizationSettings localizationSettings)
		{
			_boardService = boardService;
			_workContext = workContext;
			_customerService = customerService;
			_localizationSettings = localizationSettings;
		}
		// GET: Matrix
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Position()
		{
			List<PositionData> model = new List<PositionData>();
			var obj = _boardService.GetCustomerPositionFilled(_workContext.CurrentCustomer.Id);
			foreach (var item in obj)
			{
				model.Add(new PositionData { Level = item.Level, PositionFilled = item.PositionFilled, PaymentReceived = item.PaymentReceived, PaymentPending = item.PaymentPending });
			}
			
			return View(model);
		}
		public ActionResult PaymentsToSend()
		{
			List<CustomerPaymentModel> model = new List<CustomerPaymentModel>();
			var btcaddress = _workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.BitcoinAddressAcc);
			if(btcaddress == "" || btcaddress == null)
			{
				ViewBag.ErrorMessage = "Please update your bitcoin address, before sending payment";
				return View(model);
			}
			
			var paymenttosend = _boardService.GetPaymentToSend(_workContext.CurrentCustomer.Id);
			foreach (var ptos in paymenttosend)
			{
				CustomerPaymentModel cpmodel = new CustomerPaymentModel();
				cpmodel.Id = ptos.Id;
				cpmodel.PayToCustomerId = ptos.PayToCustomerId;
				cpmodel.PayByCustomerId = ptos.PayByCustomerId;
				cpmodel.BitcoinAddress = ptos.BitcoinAddress;
				cpmodel.PaymentProcessor1 = ptos.PaymentProcessor1;
				cpmodel.PaymentProcessor2 = ptos.PaymentProcessor2;
				cpmodel.PaymentProcessor3 = ptos.PaymentProcessor3;
				cpmodel.PaymentProcessor4 = ptos.PaymentProcessor4;
				cpmodel.PaymentProcessor5 = ptos.PaymentProcessor5;
				cpmodel.Paymentdate = ptos.Paymentdate;
				cpmodel.Status = ptos.Status;
				cpmodel.PaymentProff = ptos.PaymentProff;
				cpmodel.Remarks = ptos.Remarks;
				cpmodel.Amount = ptos.Amount;
				cpmodel.BoardId = ptos.BoardId;
				cpmodel.Deleted = ptos.Deleted;
				
				cpmodel.PayToCustomerEmail = _customerService.GetCustomerById(ptos.PayToCustomerId).Email;
				//cpmodel.PayToCustomerName = ptos.Customer.
				cpmodel.PayByCustomerEmail = _customerService.GetCustomerById(ptos.PayByCustomerId).Email;
				//cpmodel.PayByCustomerName { get; set; }
				model.Add(cpmodel);
			}
			return View(model);
		}

		public ActionResult PaymentsReceived()
		{
			List<CustomerPaymentModel> model = new List<CustomerPaymentModel>();
			var paymentreceived = _boardService.GetPaymentReceived(_workContext.CurrentCustomer.Id);
			foreach (var ptos in paymentreceived)
			{
				CustomerPaymentModel cpmodel = new CustomerPaymentModel();
				cpmodel.Id = ptos.Id;
				cpmodel.PayToCustomerId = ptos.PayToCustomerId;
				cpmodel.PayByCustomerId = ptos.PayByCustomerId;
				cpmodel.BitcoinAddress = ptos.BitcoinAddress;
				cpmodel.PaymentProcessor1 = ptos.PaymentProcessor1;
				cpmodel.PaymentProcessor2 = ptos.PaymentProcessor2;
				cpmodel.PaymentProcessor3 = ptos.PaymentProcessor3;
				cpmodel.PaymentProcessor4 = ptos.PaymentProcessor4;
				cpmodel.PaymentProcessor5 = ptos.PaymentProcessor5;
				cpmodel.Paymentdate = ptos.Paymentdate;
				cpmodel.Status = ptos.Status;
				cpmodel.PaymentProff = ptos.PaymentProff;
				cpmodel.Remarks = ptos.Remarks;
				cpmodel.Amount = ptos.Amount;
				cpmodel.BoardId = ptos.BoardId;
				cpmodel.Deleted = ptos.Deleted;
				cpmodel.PayToCustomerEmail = _customerService.GetCustomerById(ptos.PayToCustomerId).Email;
				//cpmodel.PayToCustomerName = ptos.Customer.
				cpmodel.PayByCustomerEmail = _customerService.GetCustomerById(ptos.PayByCustomerId).Email;
				//cpmodel.PayByCustomerName { get; set; }
				model.Add(cpmodel);
			}
			return View(model);
		}

		[HttpPost]
		public ActionResult UpdatePaymentStatus()
		{
			if (Request.Form["remarks"] == null)
			{
				ViewBag.ErrorMessage = "Please provide Transaction Hash";
				return Json(new { status = "fail", message = "Please provide Transaction Hash" });
			}
			if (Request.Form["remarks"].ToString() == "")
			{
				ViewBag.ErrorMessage = "Please provide Transaction Hash";
				return Json(new { status = "fail", message = "Please provide Transaction Hash" });
			}
			var cp = _boardService.GetPaymentByTransactionHash(Request.Form["remarks"]);
			if (cp != null)
			{
				ViewBag.ErrorMessage = "Duplicate Transaction Hash, Last try to submit the Transaction Hash";
				return Json(new { status = "fail", message = "Duplicate Transaction Hash, Last try to submit the Transaction Hash" });
			}

			var client = new RestClient("https://block.io/api/v2/");
			var request = new RestRequest("is_tx_confirmed/BTC/" + Request.Form["remarks"], Method.GET);
			IRestResponse<TransactionConfirmation.RootObject> response = client.Execute<TransactionConfirmation.RootObject>(request);
			if(response.Data.status == "fail")
			{
				ViewBag.ErrorMessage = "Invalid Transaction Hash";
				return Json(new { status = "fail", message = "Invalid Transaction Hash" });
			}
			request = new RestRequest("get_tx_outputs/BTC/" + Request.Form["remarks"], Method.GET);
			response = client.Execute<TransactionConfirmation.RootObject>(request);
			var amount = ConfigurationManager.AppSettings["matrixamount"].ToSafe();
			var amtoutput = response.Data.data.outputs.Where(x => x.value == amount).FirstOrDefault();
			if (amtoutput == null)
			{
				ViewBag.ErrorMessage = "Invalid Transaction Hash";
				return Json(new { status = "fail", message = "Invalid Transaction Hash" });
			}
			var paymentid = Convert.ToInt32(Request.Form["paymentid"]);
			var customerpayment = _boardService.GetPaymentById(paymentid);
			customerpayment.Remarks = Request.Form["remarks"];
			customerpayment.Status = "Pending For approval";
			_boardService.UpdateCustomerPayment(customerpayment);

			//Services.MessageFactory.SendUnilevelMatrixCycledNotificationMessageToUser(customerpayment, "", "", _localizationSettings.DefaultAdminLanguageId);

			ViewBag.SuccessMessage = "Payment details submitted";
			return Json(new { status = "success", message = "Payment details submitted" });
		}
	}
}