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
    public class STPIPNController : PublicControllerBase
	{
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ITransactionService _transactionService;
		private readonly ICommonServices _commonService;
		private readonly IStoreContext _storeContext;
		private readonly IPlanService _planService;
		private readonly ICustomerService _customerService;
		private readonly LocalizationSettings _localizationSettings;
		private readonly IBoardService _boardService;
		public STPIPNController(ICustomerPlanService customerPlanService,
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
			string tr_id = (Request.Form["tr_id"] == null) ? "" : Request.Form["tr_id"].ToString();
			string amount = (Request.Form["amount"] == null) ? "" : Request.Form["amount"].ToString();
			string merchantAccount = (Request.Form["merchantAccount"] == null) ? "" : Request.Form["merchantAccount"].ToString();
			string payerAccount = (Request.Form["payerAccount"] == null) ? "" : Request.Form["payerAccount"].ToString();
			string status = (Request.Form["status"] == null) ? "" : Request.Form["status"].ToString();
			string hash = (Request.Form["hash"] == null) ? "" : Request.Form["hash"].ToString();
			string user1 = (Request.Form["user1"] == null) ? "" : Request.Form["user1"].ToString();
			string sci_pwd = "Xentnex@123";
			sci_pwd = CalculateMD5Hash(sci_pwd + "s+E_a*");  //encryption for db
			string hash_received = CalculateMD5Hash(tr_id + ":" + CalculateMD5Hash(sci_pwd) + ":" + amount + ":" + merchantAccount + ":" + payerAccount);
			//WritetoLog(hash_received + " : " + hash);
			if (status.ToLower() == "complete")
			{
				var transaction = _transactionService.GetTransactionById(int.Parse(Request["user1"].ToString()));
				if (transaction.StatusId != 2)
				{
					transaction.Status = Status.Completed;
					transaction.StatusId = (int)Status.Completed;
					transaction.FinalAmount = transaction.Amount;
					_transactionService.UpdateTransaction(transaction);
					Services.MessageFactory.SendDepositNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
				}
			}
			
			return Content("Invalid Hash");
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
	}
}