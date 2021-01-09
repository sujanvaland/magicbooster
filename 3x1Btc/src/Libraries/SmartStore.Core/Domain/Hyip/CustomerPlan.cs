using SmartStore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Hyip
{
	/// <summary>
	/// Represents a plan
	/// </summary>
	[DataContract]
	public partial class CustomerPlan : BaseEntity, IAuditable, ISoftDeletable
	{
		/// <summary>
		/// Gets or sets customerid who purchased the plan
		/// </summary>
		[DataMember]
		public int CustomerId { get; set; }
		/// <summary>
		/// Gets or sets which plan customer has purchased
		/// </summary>
		[DataMember]
		public int PlanId { get; set; }

		/// <summary>
		/// Gets or sets amount invested in plan by customer
		/// </summary>
		[DataMember]
		public float AmountInvested { get; set; }

		/// <summary>
		/// Gets or sets purchase date of plan by customer
		/// </summary>
		[DataMember]
		public DateTime PurchaseDate { get; set; }
		/// <summary>
		/// Gets or sets total ROI to Pay
		/// </summary>
		[DataMember]
		public float ROIToPay { get; set; }
		/// <summary>
		/// Gets or sets ROI Paid till date
		/// </summary>
		[DataMember]
		public float ROIPaid { get; set; }
		/// <summary>
		/// Gets or sets ROI Paid till date
		/// </summary>
		[DataMember]
		public float RepurchaseWallet { get; set; }
		/// <summary>
		/// Gets or sets No of Payout to be paid
		/// </summary>
		[DataMember]
		public int NoOfPayout { get; set; }
		/// <summary>
		/// Gets or sets No of Payout paid till date
		/// </summary>
		[DataMember]
		public int NoOfPayoutPaid { get; set; }

		/// <summary>
		/// Gets or sets if plan is expired or not
		/// </summary>
		[DataMember]
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets if plan is expired or not
		/// </summary>
		[DataMember]
		public bool IsExpired { get; set; }
		/// <summary>
		/// Gets or sets when the plan will expire
		/// </summary>
		[DataMember]
		public DateTime ExpiredDate { get; set; }
		/// <summary>
		/// Gets or sets Last ROI paid
		/// </summary>
		[DataMember]
		public DateTime LastPaidDate { get; set; }
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
		public virtual Customer Customer { get; set; }
		#endregion
	}
}
