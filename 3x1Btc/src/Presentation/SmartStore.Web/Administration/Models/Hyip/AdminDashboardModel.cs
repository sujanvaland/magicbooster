using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Hyip
{
	public class AdminDashboardModel
	{
		public AdminDashboardModel()
		{
			Plans = new List<DashboardPlan>();
		}
		public int RegisteredMembers { get; set; }
		public int ActiveMembers { get; set; }
		public double CompletedWithdrawals { get; set; }
		public double PendingWithdrawals { get; set; }
		public double CommissionPaid { get; set; }
		public double TotalDeposit { get; set; }
		public double TodaysDeposit { get; set; }
		public double TodaysWithdrawal { get; set; }
		public List<DashboardPlan> Plans { get; set; }
	}

	public class DashboardPlan
	{
		public int PlanId { get; set; }
		public string Name { get; set; }
		public string TotalInvestment { get; set; }
		public string TotalInvestors { get; set; }
		public string ROIToPay { get; set; }
		public string ROIPaid { get; set; }
		public string PendingROI { get; set; }

		public float TotalInvestmentInt { get; set; }
		public int TotalInvestorsInt { get; set; }
		public float ROIToPayInt { get; set; }
		public float ROIPaidInt { get; set; }
		public float PendingROIInt { get; set; }
	}
}