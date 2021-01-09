using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Boards
{
	public partial class Board : BaseEntity, ISoftDeletable
	{
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public int Height { get; set; }
		[DataMember]
		public int Width { get; set; }
		[DataMember]
		public decimal Price { get; set; }
		[DataMember]
		public decimal Payout { get; set; }
		[DataMember]
		public bool PayOnComplete { get; set; }
		[DataMember]
		public bool Active { get; set; }
		[DataMember]
		public int DisplayOrder { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
		[DataMember]
		public decimal SponsorBonus { get; set; }
	}

	public partial class AutoPurchaseSetting : BaseEntity
	{
		[DataMember]
		public int CycledBoardId { get; set; }
		[DataMember]
		public int PurchaseInBoardId { get; set; }
		[DataMember]
		public int NoOfPosition { get; set; }
	}
}
