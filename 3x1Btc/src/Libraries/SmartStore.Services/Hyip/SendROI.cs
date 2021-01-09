using System;
using System.Linq;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Services.Boards;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Services.Hyip;
using SmartStore.Services.Tasks;

namespace SmartStore.Services.Hyip
{
	/// <summary>
	/// Represents a task for sending earnings on purchased plan
	/// </summary>
	public partial class SendROITask : ITask
	{
		private readonly ICustomerService _customerService;
		private readonly ITransactionService _transactionService;
		private readonly IPlanService _planService;
		private readonly ICustomerPlanService _customerPlanService;
		private readonly IBoardService _boardService;
		public SendROITask(ICustomerService customerService,
			ITransactionService transactionService,
			IPlanService planService,
			ICustomerPlanService customerPlanService,
			IBoardService boardService)
		{
			this._customerService = customerService;
			this._transactionService = transactionService;
			this._planService = planService;
			this._customerPlanService = customerPlanService;
			this._boardService = boardService;
		}
		
		/// <summary>
		/// Executes a task
		/// </summary>
		public void Execute(TaskExecutionContext ctx)
		{
			string weekday = DateTime.Today.DayOfWeek.ToString();
			//WritetoLog("Executing roi");
			var customerplans = _customerPlanService.GetAllCustomerPlans().ToList();
			//WritetoLog("totalpurchase"+customerplans.Count().ToString());
			foreach (var cp in customerplans)
			{
				if (cp.Customer.Active && cp.IsActive && !cp.IsExpired)
				{
					//WritetoLog("isactive"+cp.Customer.Id);
					var plan = _planService.GetPlanById(cp.PlanId);
					int PayEveryXHour = plan.PayEveryXDays;
					TimeSpan diff = DateTime.Now - cp.LastPaidDate;
					double LastPaidHours = diff.TotalHours;
					var vacationdate = cp.Customer.GetAttribute<DateTime>(SystemCustomerAttributeNames.VacationModeExpiryDate);
					var nextSurfdate = cp.Customer.GetAttribute<DateTime>(SystemCustomerAttributeNames.NextSurfDate);
					int NoOfAdsToSurf = 10;
					if (vacationdate > DateTime.Today || nextSurfdate > DateTime.Now)
					{
						NoOfAdsToSurf = 0;
					}
					NoOfAdsToSurf = 0;
					if (cp.AmountInvested > 0 && NoOfAdsToSurf == 0)
					{
						//WritetoLog("sharing now");
						float repurchaseAmount = 0;
						float DailyROIToPay = 0;
						float HourlyROIToPay = 0;
						DailyROIToPay = ((cp.AmountInvested * plan.ROIPercentage) / 100);
						HourlyROIToPay = DailyROIToPay;// / 24;
						//repurchaseAmount = (HourlyROIToPay * 30) / 100;
						//Transaction transaction = new Transaction();
						//transaction.CustomerId = cp.CustomerId;
						//transaction.Amount = HourlyROIToPay;
						//transaction.FinalAmount = transaction.Amount;
						//transaction.RefId = cp.Id;
						//transaction.StatusId = (int)Status.Completed;
						//transaction.TransactionDate = DateTime.Now;
						//transaction.TranscationTypeId = (int)TransactionType.ROI;
						//_transactionService.InsertTransaction(transaction);
						//WritetoLog("sharing now1"+ transaction.Amount);
						//transaction = new Transaction();
						//transaction.CustomerId = cp.CustomerId;
						//transaction.Amount = repurchaseAmount;
						//transaction.FinalAmount = transaction.Amount;
						//transaction.RefId = cp.Id;
						//transaction.StatusId = (int)Status.Completed;
						//transaction.TransactionDate = DateTime.Now;
						//transaction.TranscationTypeId = (int)TransactionType.RepurchaseROI;
						//_transactionService.InsertTransaction(transaction);

						//Update CustomerPlan
						cp.LastPaidDate = DateTime.Now;
						cp.ROIPaid = cp.ROIPaid + HourlyROIToPay;
						//cp.RepurchaseWallet = cp.RepurchaseWallet + repurchaseAmount;
						cp.NoOfPayoutPaid = cp.NoOfPayoutPaid + 1;
						if (cp.ROIPaid >= cp.ROIToPay)
						{
							cp.ExpiredDate = DateTime.Now;
							cp.IsExpired = true;
							cp.IsActive = false;
							//if (plan.IsPrincipalBack)
							//{
							//	//Retrun Principal
							//	transaction = new Transaction();
							//	transaction.CustomerId = cp.CustomerId;
							//	transaction.Amount = cp.AmountInvested;
							//	transaction.FinalAmount = transaction.Amount;
							//	transaction.RefId = cp.Id;
							//	transaction.StatusId = (int)Status.Completed;
							//	transaction.TransactionDate = DateTime.Now;
							//	transaction.TranscationTypeId = (int)TransactionType.ROI;
							//	_transactionService.InsertTransaction(transaction);
							//}
						}
						_customerPlanService.UpdateCustomerPlan(cp);
					}
				}
			}
		}

		//public void Execute(TaskExecutionContext ctx)
		//{
		//	string weekday = DateTime.Today.DayOfWeek.ToString();
		//	int[] StatusIds = "2".ToIntArray();
		//	int[] TranscationTypeIds = { (int)TransactionType.Purchase };
		//	var transcation = _transactionService.GetAllTransactions(0, 0, null, null,
		//					  StatusIds, TranscationTypeIds, 0,int.MaxValue);

		//	foreach (var cp in transcation)
		//	{
		//		if (cp.Customer.Active)
		//		{
		//			if (cp.Amount > 0)
		//			{
		//				Transaction transaction = new Transaction();
		//				transaction.CustomerId = cp.CustomerId;
		//				transaction.Amount = (float)0.5;
		//				transaction.FinalAmount = transaction.Amount;
		//				transaction.RefId = cp.Id;
		//				transaction.StatusId = (int)Status.Completed;
		//				transaction.TransactionDate = DateTime.Now;
		//				transaction.TranscationTypeId = (int)TransactionType.PoolBonus;
		//				_transactionService.InsertTransaction(transaction);
		//			}
		//		}
		//	}
		//}
	}
}
