using SmartStore.Services.Hyip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Services;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Core;
using SmartStore.Services.Customers;
using SmartStore.Core.Domain.Customers;
using SmartStore.Services.Localization;
using SmartStore.Core.Domain.Localization;
using SmartStore.Services.Boards;
using Razorpay.Api;
namespace SmartStore.Web.Controllers
{
	public class RazorPayController : PublicControllerBase
	{
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ITransactionService _transactionService;
		private readonly ICommonServices _commonService;
		private readonly IStoreContext _storeContext;
		private readonly IPlanService _planService;
		private readonly ICustomerService _customerService;
		private readonly LocalizationSettings _localizationSettings;
		private readonly IBoardService _boardService;
		public RazorPayController(ICustomerPlanService customerPlanService,
			ITransactionService transactionService,
			ICommonServices commonService,
			IStoreContext storeContext,
			IPlanService planService,
			ICustomerService customerService,
			LocalizationSettings localizationSettings,
			IBoardService boardService)
		{
			_customerPlanService = customerPlanService;
			_transactionService = transactionService;
			_commonService = commonService;
			_storeContext = storeContext;
			_planService = planService;
			_customerService = customerService;
			_localizationSettings = localizationSettings;
			_boardService = boardService;
		}

		public string CalculateMD5Hash(string input)
		{
			// step 1, calculate MD5 hash from input
			MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}
		// GET: IPNHandler
		public ActionResult Index()
		{
			if(Request.Headers["X-Razorpay-Signature"] != null)
			{
				var razorpay_payment_id = Request["razorpay_payment_id"].ToSafe();
				var razorpay_order_id = Request["razorpay_order_id"].ToSafe();
				var razorpay_signature = Request["razorpay_signature"].ToSafe();
				var receivedSign = Request.Headers["X-Razorpay-Signature"].ToSafe();
				WritetoLog(receivedSign +":"+ RequestBody());
				Utils.verifyWebhookSignature(RequestBody(), Request.Headers["X-Razorpay-Signature"].ToSafe(), "0zt8756zIcQzySRP74Wyt0Mo");
				var calculatedSign = HmacSha256Digest(RequestBody(), "0zt8756zIcQzySRP74Wyt0Mo");
				if(calculatedSign == razorpay_signature)
				{
					var transaction = _transactionService.GetTransactionById(int.Parse(razorpay_order_id));
					if (transaction.StatusId != 2)
					{
						transaction.Status = Status.Completed;
						transaction.StatusId = (int)Status.Completed;
						transaction.FinalAmount = transaction.Amount;
						_transactionService.UpdateTransaction(transaction);
						Services.MessageFactory.SendDepositNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
					}
				}
			}
			
			return Content("Invalid Hash");
		}
		public string RequestBody()
		{
			Stream req = Request.InputStream;
			req.Seek(0, System.IO.SeekOrigin.Begin);
			string bodyText = new StreamReader(req).ReadToEnd();
			return bodyText;
		}
		public ActionResult TestTransaction(int id)
		{
			//ApproveTransaction(id);
			return Content("Done");
		}
		public void ApproveTransaction(int transid)
		{
			var transaction = _transactionService.GetTransactionById(transid);
			if (transaction.StatusId != 2)
			{
				transaction.Status = Status.Completed;
				transaction.StatusId = (int)Status.Completed;
				//transaction.TranscationNote = Request["txn_id"].ToString();
				transaction.FinalAmount = transaction.Amount;
				_transactionService.UpdateTransaction(transaction);
				Services.MessageFactory.SendDepositNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
				//Services.MessageFactory.SendDepositNotificationMessageToAdmin(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
			}
		}

		public string HmacSha256Digest(string message, string secret)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] keyBytes = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(message);
			System.Security.Cryptography.HMACSHA256 cryptographer = new System.Security.Cryptography.HMACSHA256(keyBytes);

			byte[] bytes = cryptographer.ComputeHash(messageBytes);

			return BitConverter.ToString(bytes).Replace("-", "").ToLower();
		}

		public void WritetoLog(string message)
		{
			System.IO.File.WriteAllText(Server.MapPath("/WriteLines.txt"), message);
		}
	}
}