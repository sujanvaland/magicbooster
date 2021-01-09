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
    public class PayeerHandlerController : PublicControllerBase
	{
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ITransactionService _transactionService;
		private readonly ICommonServices _commonService;
		private readonly IStoreContext _storeContext;
		private readonly IPlanService _planService;
		private readonly ICustomerService _customerService;
		private readonly LocalizationSettings _localizationSettings;
		private readonly IBoardService _boardService;
		public PayeerHandlerController(ICustomerPlanService customerPlanService,
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
		
		// GET: IPNHandler
		public ActionResult IPN()
        {
			var payeerSettings = _commonService.Settings.LoadSetting<PayeerSettings>(_storeContext.CurrentStore.Id);
			string m_operation_id = (Request.Form["m_operation_id"] == null) ? "" : Request.Form["m_operation_id"].ToString();
			string m_operation_ps = (Request.Form["m_operation_ps"] == null) ? "" : Request.Form["m_operation_ps"].ToString();
			string m_operation_date = (Request.Form["m_operation_date"] == null) ? "" : Request.Form["m_operation_date"].ToString();
			string m_operation_pay_date = (Request.Form["m_operation_pay_date"] == null) ? "" : Request.Form["m_operation_pay_date"].ToString();
			string m_shop = (Request.Form["m_shop"] == null) ? "" : Request.Form["m_shop"].ToString();
			string m_orderid = (Request.Form["m_orderid"] == null) ? "" : Request.Form["m_orderid"].ToString();
			string m_amount = (Request.Form["m_amount"] == null) ? "" : Request.Form["m_amount"].ToString();
			string m_curr = (Request.Form["m_curr"] == null) ? "" : Request.Form["m_curr"].ToString();
			string m_desc = (Request.Form["m_desc"] == null) ? "" : Request.Form["m_desc"].ToString();
			string m_status = (Request.Form["m_status"] == null) ? "" : Request.Form["m_status"].ToString();
			string hashstring = m_operation_id + ":" + m_operation_ps + ":" + m_operation_date + ":" + m_operation_pay_date + ":" + m_shop + ":" + m_orderid + ":" + m_amount + ":" + m_curr + ":" + m_desc + ":" + m_status + ":" + payeerSettings.PY_SecretKey;
			string hashreceived = (Request.Form["m_sign"] == null) ? "" : Request.Form["m_sign"].ToString();
			string[] stingVariable = m_desc.Split(':');

			hashstring = GetSha256FromString(hashstring).ToString().ToUpper();

			string serverMerchantId = (Request["merchant"] == null) ? "" : Request["merchant"].ToString();
			if (m_status.ToLower() == "success")
			{
				var transaction = _transactionService.GetTransactionById(int.Parse(m_orderid));
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
			
			return Content("Invalid hash");
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
				transaction.FinalAmount = transaction.Amount;
				_transactionService.UpdateTransaction(transaction);
				Services.MessageFactory.SendDepositNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
			}
		}
		public void WritetoLog(string message)
		{
			System.IO.File.WriteAllText(Server.MapPath("/WriteLines.txt"),message);
		}
		
		public static string GetSha256FromString(string strData)
		{
			System.Security.Cryptography.SHA256Managed hm = new System.Security.Cryptography.SHA256Managed();
			byte[] hashValue = hm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(strData));
			return System.BitConverter.ToString(hashValue).Replace("-", "").ToLower();
		}
		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}
	}
}