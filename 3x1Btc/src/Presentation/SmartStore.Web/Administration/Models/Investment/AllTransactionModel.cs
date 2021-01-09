using SmartStore.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Web.Administration.Models.Investment
{
	public class AllTransactionModel
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public string StokistName { get; set; }
		public float Amount { get; set; }
		[SmartResourceDisplayName("Admin.Transactions.Amount")]
		public string FinalAmountRaw { get; set; }
		public DateTime TransactionDate { get; set; }
		public string TransactionDateString { get; set; }
		public string CustomerUserName { get; set; }
		public string RefUserName { get; set; }
	}
}