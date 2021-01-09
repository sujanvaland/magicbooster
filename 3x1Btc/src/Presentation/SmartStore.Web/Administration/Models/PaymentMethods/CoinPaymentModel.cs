using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.PaymentMethods
{
	public class CoinPaymentModel
	{
		public CoinPaymentModel()
		{
			PaymentURL = "https://www.coinpayments.net/index.php";
		}
		public int CustomerPlanId { get; set; }
		public string MerchantAcc { get; set; }
		public string CurrencyCode { get; set; }
		public decimal Amount { get; set; }
		public decimal FinalAmount { get; set; }
		public string PaymentMemo { get; set; }
		public string PlanName { get; set; }
		public string ProcessorName { get; set; }
		public string PaymentURL { get; set; }
		public decimal DepositFees { get; set; }
		public int TransactionId { get; set; }
	}
}