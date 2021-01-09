using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Boards
{
	public partial class Package : BaseEntity, ISoftDeletable
	{
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public decimal Price { get; set; }
		[DataMember]
		public decimal DirectBonus { get; set; }
		[DataMember]
		public bool Active { get; set; }
		[DataMember]
		public int DisplayOrder { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
	}
}
