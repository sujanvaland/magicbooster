using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Boards
{
	public partial class CustomerPayment : BaseEntity, ISoftDeletable
	{
		[DataMember]
		public int PayToCustomerId { get; set; }
		[DataMember]
		public int PayByCustomerId { get; set; }
		[DataMember]
		public string BitcoinAddress { get; set; }
		[DataMember]
		public string PaymentProcessor1 { get; set; }
		[DataMember]
		public string PaymentProcessor2 { get; set; }
		[DataMember]
		public string PaymentProcessor3 { get; set; }
		[DataMember]
		public string PaymentProcessor4 { get; set; }
		[DataMember]
		public string PaymentProcessor5 { get; set; }
		[DataMember]
		public DateTime? Paymentdate { get; set; }
		[DataMember]
		public string Status { get; set; }
		[DataMember]
		public string PaymentProff { get; set; }
		[DataMember]
		public string Remarks { get; set; }
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public int BoardId { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
		public Customers.Customer Customer { get; set; }
	}
}
