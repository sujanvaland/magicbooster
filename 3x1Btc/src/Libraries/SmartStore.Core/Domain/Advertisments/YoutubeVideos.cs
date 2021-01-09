using SmartStore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Advertisments
{
	[DataContract]
	public partial class YoutubeVideos : BaseEntity, ISoftDeletable
	{
		[DataMember]
		public string VideoLink { get; set; }
		[DataMember]
		public int CustomerId { get; set; }
		[DataMember]
		public int NoOfViews { get; set; }
		[DataMember]
		public bool Approved { get; set; }
		[DataMember]
		public bool IsPaid { get; set; }
		[DataMember]
		public DateTime CreatedDate { get; set; }
		[DataMember]
		public decimal Price { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
		[DataMember]
		public virtual Customer Customer { get; set; }
	}

	[DataContract]
	public partial class FacebookPost : BaseEntity, ISoftDeletable
	{
		[DataMember]
		public string VideoLink { get; set; }
		[DataMember]
		public int CustomerId { get; set; }
		[DataMember]
		public int NoOfLikes { get; set; }
		[DataMember]
		public bool Approved { get; set; }
		[DataMember]
		public DateTime CreatedDate { get; set; }
		[DataMember]
		public bool IsPaid { get; set; }
		public decimal Price { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
		[DataMember]
		public virtual Customer Customer { get; set; }
	}

	[DataContract]
	public partial class SupportRequest : BaseEntity, ISoftDeletable
	{
		[DataMember]
		public string Subject { get; set; }
		[DataMember]
		public string Message { get; set; }
		[DataMember]
		public string Email { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Status { get; set; }
		[DataMember]
		public int CustomerId { get; set; }
		[DataMember]
		public string LastReplied { get; set; }
		[DataMember]
		public DateTime CreatedDate { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
		[DataMember]
		public virtual Customer Customer { get; set; }
	}

	[DataContract]
	public partial class FAQ : BaseEntity, ISoftDeletable
	{
		[DataMember]
		public string Question { get; set; }
		[DataMember]
		public string Anaswer { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
	}

	[DataContract]
	public partial class CountryManager : BaseEntity
	{
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public DateTime DateOfJoining { get; set; }
		[DataMember]
		public int CountryId { get; set; }
		[DataMember]
		public string CountryCode { get; set; }
		[DataMember]
		public string CountryName { get; set; }
		[DataMember]
		public int TotalTeam { get; set; }
		[DataMember]
		public string WhatsAppLink { get; set; }
		[DataMember]
		public string TelegramLink { get; set; }
		[DataMember]
		public string FacebookLink { get; set; }
	}

	public partial class CustomerToken : BaseEntity, ISoftDeletable
	{
		[DataMember]
		public int CustomerId { get; set; }
		[DataMember]
		public int NoOfToken { get; set; }
		[DataMember]
		public DateTime CreatedDate { get; set; }
		[DataMember]
		public string EarningSource { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
		[DataMember]
		public virtual Customer Customer { get; set; }
	}

	[DataContract]
	public partial class CustomerBlogPost : BaseEntity, ISoftDeletable
	{
		[DataMember]
		public string BlogUrl { get; set; }
		[DataMember]
		public int CustomerId { get; set; }
		[DataMember]
		public bool Approved { get; set; }
		[DataMember]
		public DateTime CreatedDate { get; set; }
		[DataMember]
		public bool IsPaid { get; set; }
		[DataMember]
		public bool Deleted { get; set; }
		[DataMember]
		public virtual Customer Customer { get; set; }
	}
}
