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

namespace SmartStore.Web.Controllers
{
	public class PMIPNHandlerController : PublicControllerBase
	{
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ITransactionService _transactionService;
		private readonly ICommonServices _commonService;
		private readonly IStoreContext _storeContext;
		private readonly IPlanService _planService;
		private readonly ICustomerService _customerService;
		private readonly LocalizationSettings _localizationSettings;
		private readonly IBoardService _boardService;
		public PMIPNHandlerController(ICustomerPlanService customerPlanService,
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
		public ActionResult IPN()
		{
			string log = "";
			string strWallet, strPAYMENT_ID, strPAYEE_ACCOUNT, strPAYMENT_AMOUNT, strPAYMENT_UNITS, strPAYMENT_BATCH_NUM, strPAYER_ACCOUNT, strTIMESTAMPGMT;
			string strPASSPHRASE, strStringToHash, strHASH1, strHASH2, struserid;
			var pmSettings = _commonService.Settings.LoadSetting<PMSettings>(_storeContext.CurrentStore.Id);
			strHASH2 = (Request["V2_HASH"] == null) ? "" : Request["V2_HASH"].ToString();
			strPASSPHRASE = "81p4ZgZz8P0wJCqSiIlDLoQzl";
			strPASSPHRASE = CalculateMD5Hash(strPASSPHRASE).ToUpper();
			log = log +":"+ strHASH2;
			strWallet = (Request["BAGGAGE_FIELDS"] == null) ? "" : Request["BAGGAGE_FIELDS"].ToString();
			strPAYMENT_ID = (Request["PAYMENT_ID"] == null) ? "" : Request["PAYMENT_ID"].ToString();
			strPAYEE_ACCOUNT = (Request["PAYEE_ACCOUNT"] == null) ? "" : Request["PAYEE_ACCOUNT"].ToString();
			strPAYMENT_AMOUNT = (Request["PAYMENT_AMOUNT"] == null) ? "" : Request["PAYMENT_AMOUNT"].ToString();
			strPAYMENT_UNITS = (Request["PAYMENT_UNITS"] == null) ? "" : Request["PAYMENT_UNITS"].ToString();
			strPAYMENT_BATCH_NUM = (Request["PAYMENT_BATCH_NUM"] == null) ? "" : Request["PAYMENT_BATCH_NUM"].ToString();
			strPAYER_ACCOUNT = (Request["PAYER_ACCOUNT"] == null) ? "" : Request["PAYER_ACCOUNT"].ToString();
			strTIMESTAMPGMT = (Request["TIMESTAMPGMT"] == null) ? "" : Request["TIMESTAMPGMT"].ToString();
			strStringToHash = strPAYMENT_ID + ":" + strPAYEE_ACCOUNT + ":" + strPAYMENT_AMOUNT + ":" + strPAYMENT_UNITS + ":" + strPAYMENT_BATCH_NUM + ":" + strPAYER_ACCOUNT + ":" + strPASSPHRASE + ":" + strTIMESTAMPGMT;
			log = log + ":" + strStringToHash + strHASH2;
			strHASH1 = CalculateMD5Hash(strStringToHash).ToUpper();
			log = log + ":CAL:" + strHASH1 + ":Baggage" + strWallet;
			WritetoLog(log);
			if (strHASH2 == strHASH1)
			{
				var transaction = _transactionService.GetTransactionById(int.Parse(strPAYMENT_ID));
				if (transaction.StatusId != 2)
				{
					transaction.Status = Status.Completed;
					transaction.StatusId = (int)Status.Completed;
					transaction.FinalAmount = transaction.Amount;
					_transactionService.UpdateTransaction(transaction);
					Services.MessageFactory.SendDepositNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
					//Services.MessageFactory.SendDepositNotificationMessageToAdmin(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
				}
			}
			WritetoLog(Request["BAGGAGE_FIELDS"].ToString());
			return Content("Invalid Hash");
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

		public void WritetoLog(string message)
		{
			System.IO.File.WriteAllText(Server.MapPath("/WriteLines.txt"), message);
		}
	}
}