using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Investment
{
	public class MyInvestmentPlan
	{
		public int ShareId { get; set; }
		public int PlanId { get; set; }
		public string PlanName { get; set; }
		public float TotalFunding { get; set; }
		public float ROIToPay { get; set; }
		public float ROIPaid { get; set; }
		public float ROIPending { get; set; }
		public float RepurchaseWallet { get; set; }
		public string TotalFundingString { get; set; }
		public string ROIToPayString { get; set; }
		public string ROIPaidString { get; set; }
		public string ROIPendingString { get; set; }
		public string RepurchaseWalletString { get; set; }
		public DateTime PurchaseDate { get; set; }
		public DateTime ExpireDate { get; set; }
		public bool IsActive { get; set; }
		public string Status { get; set; }
		public float MyTotalInvestment { get; set; }
		public float MyTotalROIToPay { get; set; }
		public float MyTotalROIPaid { get; set; }
		public float MyTotalROIPending { get; set; }
	}
}