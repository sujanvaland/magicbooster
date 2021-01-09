using FluentValidation.Attributes;
using SmartStore.Admin.Models.PaymentMethods;
using SmartStore.Admin.Validators.Investment;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Models.Investment
{
	[Validator(typeof(TransactionValidator))]
	public class TransactionModel
	{
		public TransactionModel()
		{
			STPEnabled = false;
			CoinPaymentEnabled = false;
			PayzaEnabled = false;
			PMEnabled = false;
			PayeerEnabled = false;
			AdvanceCashEnabled = false;
			AvailableProcessor = new List<SelectListItem>();
			AvailablePlans = new List<SelectListItem>();
			AvailableStatus = new List<SelectListItem>();
		}

		public string AccountNumber { get; set; }
		public string NICR { get; set; }
		public string BankName { get; set; }
		public string AccountHolderName { get; set; }

		public int Id { get; set; }
		public int NoOfPosition { get; set; }
		public bool IsVisible { get; set; }
		public int CustomerId { get; set; }

		public float Amount { get; set; }
		[SmartResourceDisplayName("Admin.Transactions.Amount")]
		public string FinalAmountRaw { get; set; }
		public float FinalAmount { get; set; }

		public DateTime TransactionDate { get; set; }
		public string TransactionDateString { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public List<SelectListItem> AvailablePlans { get; set; }
		[SmartResourceDisplayName("Admin.Hyip.List.Plans")]
		public string PlanIds { get; set; }
		public string PlanName { get; set; }

		public int GridPageSize { get; set; }
		public int ProcessorId { get; set; }

		public int RefId { get; set; }

		public int StatusId { get; set; }
		public Status TransStatus { get; set; }
		[SmartResourceDisplayName("Admin.Transactions.TransStatus")]
		public string TransStatusString { get; set; }
		[SmartResourceDisplayName("Admin.Transactions.TransStatus")]
		public string StatusIds { get; set; }

		public int TranscationTypeId { get; set; }
		public TransactionType TranscationType { get; set; }
		public string TranscationTypeString { get; set; }
		public string TranscationTypeIds { get; set; }

		public string TranscationNote { get; set; }

		public string ProcessorName { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public DateTime CreatedOnUtc { get; set; }

		public DateTime UpdatedOnUtc { get; set; }

		public List<SelectListItem> AvailableProcessor { get; set; }
		public List<SelectListItem> AvailableStatus { get; set; }

		public bool STPEnabled { get; set; }
		[SmartResourceDisplayName("Admin.Withdrawal.Fields.SolidTrustPayAcc")]
		public string SolidTrustPayAcc { get; set; }

		public bool CoinPaymentEnabled { get; set; }
		[SmartResourceDisplayName("Admin.Withdrawal.Fields.BitcoinAddress")]
		public string BitcoinAddress { get; set; }

		public bool PayzaEnabled { get; set; }
		[SmartResourceDisplayName("Admin.Withdrawal.Fields.PayzaAcc")]
		public string PayzaAcc { get; set; }

		public bool PMEnabled { get; set; }
		[SmartResourceDisplayName("Admin.Withdrawal.Fields.PMAcc")]
		public string PMAcc { get; set; }

		public bool PayeerEnabled { get; set; }
		[SmartResourceDisplayName("Admin.Withdrawal.Fields.PayeerAcc")]
		public string PayeerAcc { get; set; }

		public bool AdvanceCashEnabled { get; set; }
		[SmartResourceDisplayName("Admin.Withdrawal.Fields.AdvanceCashAcc")]
		public string AdvanceCashAcc { get; set; }

		[SmartResourceDisplayName("Admin.Withdrawal.Fields.WithdrawalFees")]
		public decimal WithdrawalFees { get; set; }

		public float CompletedWithdrawal { get; set; }

		public float PendingWithdrawal { get; set; }

		public float AvailableBalance { get; set; }

		public string WithdrawalAccount { get; set; }

		public string CustomerEmail { get; set; }
		public string UPIPaymentNumber { get; set; }
		public string CustomerUserName { get; set; }
		public string RefUserName { get; set; }
	}

	public partial class SupportRequestModel
	{
		public int GridPageSize { get; set; }
		public int Id { get; set; }
		public string Subject { get; set; }
		
		public string Message { get; set; }
		
		public string Email { get; set; }
		
		public string Name { get; set; }
		
		public string Status { get; set; }
		
		public int CustomerId { get; set; }
		
		public string LastReplied { get; set; }
		
		public DateTime CreatedDate { get; set; }
		
		public bool Deleted { get; set; }
	}
}