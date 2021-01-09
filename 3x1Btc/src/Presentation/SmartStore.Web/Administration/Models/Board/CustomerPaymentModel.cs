using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Board
{
	public class CustomerPaymentModel
	{
		public int Id { get; set; }
		public int PayToCustomerId { get; set; }
		public int PayByCustomerId { get; set; }
		public string BitcoinAddress { get; set; }
		public string PaymentProcessor1 { get; set; }
		public string PaymentProcessor2 { get; set; }
		public string PaymentProcessor3 { get; set; }
		public string PaymentProcessor4 { get; set; }
		public string PaymentProcessor5 { get; set; }
		public DateTime? Paymentdate { get; set; }
		public string Status { get; set; }
		public string PaymentProff { get; set; }
		public string Remarks { get; set; }
		public decimal Amount { get; set; }
		public int BoardId { get; set; }
		public bool Deleted { get; set; }
		public string PayToCustomerEmail { get; set; }
		public string PayToCustomerName { get; set; }
		public string PayByCustomerEmail { get; set; }
		public string PayByCustomerName { get; set; }
	}
}