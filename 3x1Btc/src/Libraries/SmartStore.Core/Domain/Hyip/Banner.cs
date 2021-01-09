using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Hyip
{
	[DataContract]
	public class Banner : BaseEntity, IAuditable, ISoftDeletable
	{
		/// <summary>
		/// Gets or sets Picture Banner
		/// </summary>
		[DataMember]
		public int PictureId { get; set; }

		/// <summary>
		/// Gets or sets Banner Size
		/// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets Banner Size
		/// </summary>
		[DataMember]
		public string Size { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity is published
		/// </summary>
		[DataMember]
		public bool Published { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity has been deleted
		/// </summary>
		[Index]
		public bool Deleted { get; set; }

		/// <summary>
		/// Gets or sets the display order
		/// </summary>
		[DataMember]
		public int DisplayOrder { get; set; }

		/// <summary>
		/// Gets or sets the date and time of instance creation
		/// </summary>
		[DataMember]
		public DateTime CreatedOnUtc { get; set; }

		/// <summary>
		/// Gets or sets the date and time of instance update
		/// </summary>
		[DataMember]
		public DateTime UpdatedOnUtc { get; set; }

	}
}
