using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SmartStore.Core.Domain.Security;

namespace SmartStore.Core.Domain.Customers
{
	[DataContract]
	public class CustomerTraffic : BaseEntity, IAuditable
	{
		[DataMember]
		public int CustomerId { get; set; }

		[DataMember]
		public string IpAddress { get; set; }

		[DataMember]
		public DateTime CreatedOnUtc { get; set; }

		/// <summary>
		/// Gets or sets the date and time of instance update
		/// </summary>
		[DataMember]
		public DateTime UpdatedOnUtc { get; set; }

		[DataMember]
		public virtual Customer Customer { get; set; }
	}
}
