using SmartStore.Core.Domain.Boards;
using SmartStore.Core.Domain.Hyip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Web.Models.Home
{
	public class HomeModel
	{
		public HomeModel()
		{
			Last10Withdrawal = new List<WithdrawalList>();
			Last10Deposit = new List<DepositList>();
			plans = new List<Package>();
		}
		public string LaunchDate { get; set; }
		public string RunningDays { get; set; }
		public string OnlineVistors { get; set; }
		public string TotalCustomer { get; set; }
		public string TotalSiteWithdrawal { get; set; }
		public string TotalSiteDeposit { get; set; }
		public string AmountRaw { get; set; }
		public List<WithdrawalList> Last10Withdrawal { get; set; }
		public List<DepositList> Last10Deposit { get; set; }
		public List<Package> plans { get; set; }
	}

	public class WithdrawalList
	{
		public string AmountRaw { get; set; }
		public string Email { get; set; }
		public string TransDate { get; set; }
		public string PaymentMethodIcon { get; set; }
	}

	public class DepositList
	{
		public string AmountRaw { get; set; }
		public string Email { get; set; }
		public string TransDate { get; set; }
		public string PaymentMethodIcon { get; set; }
	}
}