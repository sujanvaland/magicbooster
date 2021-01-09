using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Advertisments
{
	[DataContract]
	public partial class AdCampaign : BaseEntity, ISoftDeletable, IAuditable
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string WebsiteUrl { get; set; }

		[DataMember]
		public string Banner125 { get; set; }

		[DataMember]
		public string Banner486 { get; set; }

		[DataMember]
		public string Banner728 { get; set; }

		[DataMember]
		public int AssignedCredit { get;set; }

		[DataMember]
		public int UsedCredit { get; set; }

		[DataMember]
		public int AvailableCredit { get; set; }

		[DataMember]
		public string CreditType { get; set; }

		[DataMember]
		public string AdType { get; set; }

		[DataMember]
		public int? NoOfDays { get; set; }

		[DataMember]
		public DateTime? ExpiryDate { get; set; }

		[DataMember]
		public int CustomerId { get; set; }

		[DataMember]
		public int? PictureId { get; set; }

		[DataMember]
		public bool Enabled { get; set; }

		[DataMember]
		public bool Deleted { get; set; }

		[DataMember]
		public DateTime CreatedOnUtc { get; set; }

		[DataMember]
		public DateTime UpdatedOnUtc { get; set; }

		[DataMember]
		public virtual Picture Picture { get; set; }

		[DataMember]
		public virtual Customer Customer { get; set; }

		
	}
}
