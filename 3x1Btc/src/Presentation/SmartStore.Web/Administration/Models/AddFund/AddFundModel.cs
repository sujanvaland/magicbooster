using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Models.AddFund
{
	public class AddFundModel
	{
		public AddFundModel()
		{
			AvailableProcessor = new List<SelectListItem>();
		}
		public decimal AmountInvested { get; set; }
		public float DepositFees { get; set; }
		public int TransactionId { get; set; }
		public int ProcessorId { get; set; }
		public string ProcessorName { get; set; }
		public List<SelectListItem> AvailableProcessor = new List<SelectListItem>();
	}
}