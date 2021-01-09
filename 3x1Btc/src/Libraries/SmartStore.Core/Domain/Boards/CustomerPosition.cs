using SmartStore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Boards
{
	public partial class CustomerPosition : BaseEntity, ISoftDeletable
	{
		[DataMember]
		public int CustomerId { get; set; }
		[DataMember]
		public int BoardId { get; set; }
		[DataMember]
		public int PlacedUnderPositionId { get; set; }
		[DataMember]
		public int PlacedUnderCustomerId { get; set; }
		[DataMember]
		public bool IsCycled { get; set; }
		[DataMember]
		public DateTime PurchaseDate { get; set; }
		[DataMember]
		public DateTime CycledDate { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
		[DataMember]
		public bool EmailSentOnCycle { get; set; }
		public Board Board { get; set; }
		public Customer Customer { get; set; }
	}
}
