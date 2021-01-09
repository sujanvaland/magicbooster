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
	public partial class Transaction : BaseEntity, IAuditable, ISoftDeletable
    {
		public object customer;

		/// <summary>
		/// Gets or sets the customer who made this transcation
		/// </summary>
		[DataMember]
        public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets a amount of transaction
		/// </summary>
		[DataMember]
		public float Amount { get; set; }

		/// <summary>
		/// Gets or sets final amount after applying fees (withdrawal/funding etc)
		/// </summary>
		[DataMember]
		public float FinalAmount { get; set; }

        /// <summary>
        /// Gets or sets the transcation date
        /// </summary>
        [DataMember]
        public DateTime TransactionDate { get; set; }

        /// <summary>
		/// Gets or sets a status of transaction
		/// </summary>
        [DataMember]
        public int StatusId { get; set; }

		/// <summary>
		/// Gets or sets a payment processor used for the transcation
		/// </summary>
		[DataMember]
        public int ProcessorId { get; set; }

		/// <summary>
		/// Gets or sets the RefId for the transactions
		/// </summary>
		[DataMember]
		public int RefId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
		[Index]
        public bool Deleted { get; set; }

		
		/// <summary>
		/// Gets or sets a TranscationType 
		/// </summary>
		[DataMember]
		public int TranscationTypeId { get; set; }
		/// <summary>
		/// Gets or sets a Note for the transaction displayed to user
		/// </summary>
		[DataMember]
		public  string WithdrawalAddress { get; set; }

		[DataMember]
		public string TranscationNote { get; set; }

		[DataMember]
		public int NoOfPosition { get; set; }
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

		[DataMember]
		public Status Status
		{
			get
			{
				return (Status)this.StatusId;
			}
			set
			{
				this.StatusId = (int)value;
			}
		}

		/// <summary>
		/// Gets or sets a TranscationType 
		/// </summary>
		[DataMember]
		public TransactionType TranscationType
		{
			get
			{
				return (TransactionType)this.TranscationTypeId;
			}
			set
			{
				this.TranscationTypeId = (int)value;
			}
		}

		#endregion
	}
}
