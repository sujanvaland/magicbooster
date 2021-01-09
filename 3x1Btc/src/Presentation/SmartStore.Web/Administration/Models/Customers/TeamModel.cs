using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Customers
{
	public class TeamModel
	{
		public int CustomerId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string EmailId { get; set; }
		public string MobileNo { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}