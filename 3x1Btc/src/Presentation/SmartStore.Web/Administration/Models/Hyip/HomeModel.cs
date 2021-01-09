using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Hyip
{
	public class HomeModel
	{
		public string ReferralLink { get; set; }
		public float AvailableBalance { get; set; }
		public float MyTotalInvestment { get; set; }
		public float PendingWithdrawal { get; set; }
		public float TotalCommission { get; set; }
		public float TotalReferral { get; set; }
		public float RepurchaseBalance { get; set; }
		public float CompletedWithdrawal { get; set; }
		public float TotalEarning { get; set; }
		public float DirectBonus { get; set; }
		public float UnilevelEarning { get; set; }
		public float PoolShare { get; set; }
		public float CyclerIncome { get; set; }
		public float TotalIncome { get; set; }
		public string VacationModelExpiryDate { get; set; }
		public int NoOfAdsToSurf { get; set; }
		public string InvestorId { get; set; }
		public string Name { get; set; }
		public string RegistrationDate { get; set; }
		public string ReferredBy { get; set; }
		public string Status { get; set; }
	}
}