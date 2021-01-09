using SmartStore.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Models.ActiveSubscription
{
	public class ActiveSubscriptionModel
	{
		public ActiveSubscriptionModel()
		{
			AvailableProcessor = new List<SelectListItem>();
		}
		public int Id { get; set; }
		public List<SelectListItem> AvailableProcessor = new List<SelectListItem>();
		public int ProcessorId { get; set; }
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
		public bool CoinPaymentEnabled { get; set; }
		public bool STPEnabled { get; set; }

		public int NoOfMonths { get; set; }
	}
}