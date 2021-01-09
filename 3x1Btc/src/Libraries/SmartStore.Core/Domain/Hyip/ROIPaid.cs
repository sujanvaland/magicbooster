using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.Serialization;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Discounts;
using SmartStore.Core.Domain.Media;

namespace SmartStore.Core.Domain.Hyip
{
    /// <summary>
    /// Represents a plan
    /// </summary>
    [DataContract]
	public partial class ROIPaid : BaseEntity, IAuditable, ISoftDeletable
    {
		/// <summary>
		/// Gets or sets the name
		/// </summary>
		[DataMember]
        public int PlanId { get; set; }

		/// <summary>
		/// Gets or sets a description displayed at the bottom of the plan page
		/// </summary>
		[DataMember]
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets the NoOfDays ROI will be paid
		/// </summary>
		[DataMember]
		public float Amount { get; set; }

        /// <summary>
        /// Gets or sets the ROI Percentage paid every x days
        /// </summary>
        [DataMember]
        public DateTime PaidDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
		[Index]
        public bool Deleted { get; set; }

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

		#region Navigation properties

		/// <summary>
		/// Gets or sets the customer
		/// </summary>
		[DataMember]
		public virtual Customer Customer { get; set; }
		#endregion
	}
}
