using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Board
{
	public class PositionData
	{
		public int Level { get; set; }
		public int PositionFilled { get; set; }
		public decimal PaymentReceived { get; set; }
		public decimal PaymentPending { get; set; }
	}
}