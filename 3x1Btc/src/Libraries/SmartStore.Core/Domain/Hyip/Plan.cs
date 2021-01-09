using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.Serialization;
using SmartStore.Core.Domain.Discounts;
using SmartStore.Core.Domain.Media;

namespace SmartStore.Core.Domain.Hyip
{
    /// <summary>
    /// Represents a plan
    /// </summary>
    [DataContract]
	public partial class Plan : BaseEntity, IAuditable, ISoftDeletable
    {
		private ICollection<PlanCommission> _planCommissions;
		/// <summary>
		/// Gets or sets the name
		/// </summary>
		[DataMember]
        public string Name { get; set; }

		/// <summary>
		/// Gets or sets a description displayed at the bottom of the plan page
		/// </summary>
		[DataMember]
		public string PlanDetails { get; set; }

		/// <summary>
		/// Gets or sets the NoOfDays ROI will be paid
		/// </summary>
		[DataMember]
		public int NoOfPayouts { get; set; }

        /// <summary>
        /// Gets or sets the ROI Percentage paid every x days
        /// </summary>
        [DataMember]
        public float ROIPercentage { get; set; }

        /// <summary>
		/// Gets or sets a minimum investment required for the plan
		/// </summary>
        [DataMember]
        public float MinimumInvestment { get; set; }

		/// <summary>
		/// Gets or sets a maximum investment required for the plan
		/// </summary>
		[DataMember]
        public float MaximumInvestment { get; set; }

		/// <summary>
		/// Gets or sets the frequency of payout (every X days)
		/// </summary>
		[DataMember]
		public int PayEveryXDays { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is subject to ACL
        /// </summary>
		[DataMember]
		public bool SubjectToAcl { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
		/// </summary>
		[DataMember]
		public bool LimitedToStores { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        [DataMember]
        public bool Published { get; set; }

		/// <summary>
		/// Gets or sets a if principal is returned 
		/// </summary>
		[DataMember]
		public bool IsPrincipalBack { get; set; }

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
		/// Gets or sets Start ROI After hours
		/// </summary>
		[DataMember]
        public int StartROIAfterHours { get; set; }

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
		/// Gets or sets Plan commissions
		/// </summary>
		[DataMember]
		public virtual ICollection<PlanCommission> PlanCommission
		{
			get { return _planCommissions ?? (_planCommissions = new HashSet<PlanCommission>()); }
			protected set { _planCommissions = value; }
		}
		#endregion

	}
}
