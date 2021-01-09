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
	public class BlockchainController : PublicControllerBase
	{
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ITransactionService _transactionService;
		private readonly ICommonServices _commonService;
		private readonly IStoreContext _storeContext;
		private readonly IPlanService _planService;
		private readonly ICustomerService _customerService;
		private readonly LocalizationSettings _localizationSettings;
		private readonly IBoardService _boardService;
		public BlockchainController(ICustomerPlanService customerPlanService,
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
		static string ByteToString(byte[] buff)
		{
			string sbinary = "";
			for (int i = 0; i < buff.Length; i++)
				sbinary += buff[i].ToString("X2"); /* hex format */
			return sbinary;
		}
		// GET: IPNHandler
		public ActionResult Index()
		{
			var transaction = _transactionService.GetTransactionById(int.Parse(Request["invoice"].ToString()));
			string secret = (Request["secret"] == null) ? "" : Request["secret"].ToString();
			if (secret == "e57b3908-874a-42e7-be52-7b619139b668")
			{
				if (int.Parse(Request["status"].ToString()) >= 1 && Convert.ToInt64(Request["amount1"].ToSafe()) >= transaction.Amount)
				{
					if (transaction.StatusId != 2)
					{
						transaction.Status = Status.Completed;
						transaction.StatusId = (int)Status.Completed;
						transaction.FinalAmount = transaction.Amount;
						transaction.TranscationNote = Request["txn_id"].ToSafe();
						
						_transactionService.UpdateTransaction(transaction);
						if(transaction.TranscationTypeId == 2)
						{
							Transaction transactionf = new Transaction();
							transactionf.Amount =transaction.Amount;
							transactionf.CustomerId = transaction.CustomerId;
							transactionf.FinalAmount = transaction.Amount;
							transactionf.NoOfPosition = 0;
							transactionf.TransactionDate = DateTime.Now;
							transactionf.RefId = 0;
							transactionf.ProcessorId = 0;
							transactionf.StatusId = (int)Status.Completed;
							transactionf.TranscationTypeId = (int)TransactionType.Purchase;
							transaction.TranscationNote = Request["txn_id"].ToSafe();
							_transactionService.InsertTransaction(transactionf);

							ReleaseLevelCommission(transaction.RefId, transaction.Customer);
							Services.MessageFactory.SendUnilevelPositionPurchasedNotificationMessageToUser(transaction.Customer, "", "", _localizationSettings.DefaultAdminLanguageId);
						}
						else
						{
							Services.MessageFactory.SendDepositNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
						}
					}
				}
			}

			return Content("MerchantId not matched");
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
				if (transaction.TranscationTypeId == 2)
				{
					Transaction transactionf = new Transaction();
					transactionf.Amount = transaction.Amount;
					transactionf.CustomerId = transaction.CustomerId;
					transactionf.FinalAmount = transaction.Amount;
					transactionf.NoOfPosition = 0;
					transactionf.TransactionDate = DateTime.Now;
					transactionf.RefId = 0;
					transactionf.ProcessorId = 0;
					transactionf.StatusId = (int)Status.Completed;
					transactionf.TranscationTypeId = (int)TransactionType.Purchase;
					transaction.TranscationNote = Request["txn_id"].ToSafe();
					_transactionService.InsertTransaction(transactionf);

					ReleaseLevelCommission(transaction.RefId, transaction.Customer);
					Services.MessageFactory.SendUnilevelPositionPurchasedNotificationMessageToUser(transaction.Customer, "", "", _localizationSettings.DefaultAdminLanguageId);
				}
				else
				{
					Services.MessageFactory.SendDepositNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
				}
			}
		}

		public void WritetoLog(string message)
		{
			System.IO.File.WriteAllText(Server.MapPath("/WriteLines.txt"), message);
		}

		public void ReleaseLevelCommission(int planid, Customer customer)
		{
			//Save board position
			int customerid = customer.Id;
			_customerService.SaveCusomerPosition(customerid, planid);
			//var cycledpositionformail = _boardService.GetAllPositionForEmailNotification();
			Transaction transaction;
			Customer levelcustomer = _customerService.GetCustomerById(customer.AffiliateId);
			var board = _boardService.GetBoardById(planid);
			//Direct Bonus
			if (levelcustomer != null)
			{
				//Send Direct Bonus
				try
				{
					//var directcount = _customerService.GetCustomerPaidDirectReferral(levelcustomer.Id);
					if (levelcustomer.CustomerPosition.Count > 1)
					{
						if (levelcustomer.Transaction.Where(x => x.StatusId == 2).Sum(x => x.Amount) > 0)
						{
							transaction = new Transaction();
							transaction.CustomerId = levelcustomer.Id;
							transaction.Amount = (float)board.DisplayOrder;
							transaction.FinalAmount = (float)board.DisplayOrder;
							transaction.TransactionDate = DateTime.Now;
							transaction.StatusId = (int)Status.Completed;
							transaction.TranscationTypeId = (int)TransactionType.DirectBonus;
							transaction.TranscationNote = board.Name + " Direct Bonus";
							_transactionService.InsertTransaction(transaction);
							Services.MessageFactory.SendDirectBonusNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
						}
					}
				}
				catch (Exception ex)
				{
					//WritetoLog("Direct Bonus error :" + ex.ToString());
				}
			}

			//Unilevel Bonus
			//for (int i = 0; i < board.Height; i++)
			//{
			//	if (levelcustomer != null)
			//	{
			//		//Send Direct Bonus
			//		try
			//		{
			//			//var directcount = _customerService.GetCustomerPaidDirectReferral(levelcustomer.Id);
			//			if (levelcustomer.CustomerPosition.Count > 1)
			//			{
			//				if (levelcustomer.Transaction.Where(x => x.StatusId == 2).Sum(x => x.Amount) > 0)
			//				{
			//					transaction = new Transaction();
			//					transaction.CustomerId = levelcustomer.Id;
			//					if (board.Id == 1 && i == 4)
			//					{
			//						transaction.Amount = (float)3;
			//						transaction.FinalAmount = (float)3;
			//					}
			//					else
			//					{
			//						transaction.Amount = (float)board.Payout;
			//						transaction.FinalAmount = (float)board.Payout;
			//					}
			//					transaction.TransactionDate = DateTime.Now;
			//					transaction.StatusId = (int)Status.Completed;
			//					transaction.TranscationTypeId = (int)TransactionType.UnilevelBonus;
			//					transaction.TranscationNote = board.Name + " Earning";
			//					_transactionService.InsertTransaction(transaction);
			//					Services.MessageFactory.SendUnilevelBonusNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);
			//				}
			//			}
			//		}
			//		catch (Exception ex)
			//		{
			//			//WritetoLog("Direct Bonus error :" + ex.ToString());
			//		}
			//		levelcustomer = _customerService.GetCustomerById(levelcustomer.AffiliateId);
			//	}
			//}

		}
	}
}