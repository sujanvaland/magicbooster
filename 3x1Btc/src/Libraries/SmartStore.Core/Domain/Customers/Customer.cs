using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using SmartStore.Core.Domain.Blogs;
using SmartStore.Core.Domain.Boards;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Forums;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Domain.Orders;

namespace SmartStore.Core.Domain.Customers
{
    /// <summary>
    /// Represents a customer
    /// </summary>
    [DataContract]
	public partial class Customer : BaseEntity, ISoftDeletable
    {
        private ICollection<ExternalAuthenticationRecord> _externalAuthenticationRecords;
        private ICollection<CustomerContent> _customerContent;
		private ICollection<CustomerTraffic> _customerTraffic;
		private ICollection<CustomerRole> _customerRoles;
        private ICollection<ShoppingCartItem> _shoppingCartItems;
        private ICollection<Order> _orders;
        private ICollection<RewardPointsHistory> _rewardPointsHistory;
		private ICollection<WalletHistory> _walletHistory;
		private ICollection<ReturnRequest> _returnRequests;
        private ICollection<Address> _addresses;
        private ICollection<ForumTopic> _forumTopics;
        private ICollection<ForumPost> _forumPosts;
		private ICollection<Transaction> _transaction;
		private ICollection<ROIPaid> _roiPaid;
		private ICollection<CustomerPlan> _customerPlans;
		private ICollection<CustomerPosition> _customerPosition;
		private ICollection<BlogPost> _customerBlogs;
		/// <summary>
		/// Ctor
		/// </summary>
		public Customer()
        {
            this.CustomerGuid = Guid.NewGuid();
            this.PasswordFormat = PasswordFormat.Clear;
        }

		/// <summary>
		/// Gets or sets the Stokist Commision
		/// </summary>
		//[DataMember]
		//public float? StokistCommision { get; set; }

		/// <summary>
		/// Gets or sets the Country Manager
		/// </summary>
		[DataMember]
		public bool? IsCountryManager { get; set; }

		/// <summary>
		/// Gets or sets the Position
		/// </summary>
		[DataMember]
		public string Position { get; set; }

		/// <summary>
		/// Gets or sets the Sponsors Name identifier
		/// </summary>
		[DataMember]
		public string SponsorsName { get; set; }

		/// <summary>
		/// Gets or sets the Placement User Name identifier
		/// </summary>
		[DataMember]
		public string PlacementUserName { get; set; }

		/// <summary>
		/// Gets or sets the Placement identifier
		/// </summary>
		[DataMember]
		public int PlacementId { get; set; }

		/// <summary>
		/// Gets or sets the customer Guid
		/// </summary>
		[DataMember]
        public Guid CustomerGuid { get; set; }

		/// <summary>
		/// Gets or sets the username
		/// </summary>
        [DataMember]
        public string Username { get; set; }

		/// <summary>
		/// Gets or sets the email
		/// </summary>
        [DataMember]
        public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password
		/// </summary>
        public string Password { get; set; }

		/// <summary>
		/// Gets or sets the password format
		/// </summary>
        public int PasswordFormatId { get; set; }

		/// <summary>
		/// Gets or sets the password format
		/// </summary>
        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }

		/// <summary>
		/// Gets or sets the password salt
		/// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        [DataMember]
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is tax exempt
        /// </summary>
        [DataMember]
        public bool IsTaxExempt { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier
        /// </summary>
		[DataMember]
		public int AffiliateId { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
		[DataMember]
		public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
		[Index]
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer account is system
        /// </summary>
		[DataMember]
		public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the customer system name
        /// </summary>
		[DataMember]
		[Index]
		public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
		[DataMember, Index("IX_Customer_LastIpAddress")]
		public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
		[DataMember, Index("IX_Customer_CreatedOn")]
		public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
		[DataMember]
		public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
		[DataMember, Index("IX_Customer_LastActivity")]
		public DateTime LastActivityDateUtc { get; set; }
        
        #region Navigation properties

        /// <summary>
        /// Gets or sets customer generated content
        /// </summary>
        public virtual ICollection<ExternalAuthenticationRecord> ExternalAuthenticationRecords
        {
			get { return _externalAuthenticationRecords ?? (_externalAuthenticationRecords = new HashSet<ExternalAuthenticationRecord>()); }
            protected set { _externalAuthenticationRecords = value; }
        }

        /// <summary>
        /// Gets or sets customer generated content
        /// </summary>
        public virtual ICollection<CustomerContent> CustomerContent
        {
			get { return _customerContent ?? (_customerContent = new HashSet<CustomerContent>()); }
            protected set { _customerContent = value; }
        }

		/// <summary>
		/// Gets or sets customer generated content
		/// </summary>
		public virtual ICollection<CustomerTraffic> CustomerTraffic
		{
			get { return _customerTraffic ?? (_customerTraffic = new HashSet<CustomerTraffic>()); }
			protected set { _customerTraffic = value; }
		}
		
		/// <summary>
		/// Gets or sets the customer roles
		/// </summary>
		[DataMember]
		public virtual ICollection<CustomerRole> CustomerRoles
        {
			get { return _customerRoles ?? (_customerRoles = new HashSet<CustomerRole>()); }
            protected set { _customerRoles = value; }
        }

        /// <summary>
        /// Gets or sets shopping cart items
        /// </summary>
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems
        {
			get { return _shoppingCartItems ?? (_shoppingCartItems = new HashSet<ShoppingCartItem>()); }
            protected set { _shoppingCartItems = value; }            
        }

        /// <summary>
        /// Gets or sets orders
        /// </summary>
		[DataMember]
		public virtual ICollection<Order> Orders
        {
			get { return _orders ?? (_orders = new HashSet<Order>()); }
            protected set { _orders = value; }            
        }

        /// <summary>
        /// Gets or sets reward points history
        /// </summary>
        public virtual ICollection<RewardPointsHistory> RewardPointsHistory
        {
			get { return _rewardPointsHistory ?? (_rewardPointsHistory = new HashSet<RewardPointsHistory>()); }
            protected set { _rewardPointsHistory = value; }            
        }

		/// <summary>
		/// Gets or sets the wallet history.
		/// </summary>
		public virtual ICollection<WalletHistory> WalletHistory
		{
			get
			{
				return _walletHistory ?? (_walletHistory = new HashSet<WalletHistory>());
			}
			protected set
			{
				_walletHistory = value;
			}
		}

        /// <summary>
        /// Gets or sets return request of this customer
        /// </summary>
		[DataMember]
		public virtual ICollection<ReturnRequest> ReturnRequests
        {
			get { return _returnRequests ?? (_returnRequests = new HashSet<ReturnRequest>()); }
            protected set { _returnRequests = value; }            
        }
        
        /// <summary>
        /// Default billing address
        /// </summary>
		[DataMember]
		public virtual Address BillingAddress { get; set; }

        /// <summary>
        /// Default shipping address
        /// </summary>
		[DataMember]
		public virtual Address ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets customer addresses
        /// </summary>
		[DataMember]
		public virtual ICollection<Address> Addresses
        {
			get { return _addresses ?? (_addresses = new HashSet<Address>()); }
            protected set { _addresses = value; }            
        }

        /// <summary>
        /// Gets or sets the created forum topics
        /// </summary>
        public virtual ICollection<ForumTopic> ForumTopics
        {
			get { return _forumTopics ?? (_forumTopics = new HashSet<ForumTopic>()); }
            protected set { _forumTopics = value; }
        }

        /// <summary>
        /// Gets or sets the created forum posts
        /// </summary>
        public virtual ICollection<ForumPost> ForumPosts
        {
			get { return _forumPosts ?? (_forumPosts = new HashSet<ForumPost>()); }
            protected set { _forumPosts = value; }
        }

		/// <summary>
		/// Gets or sets Transactions
		/// </summary>
		[DataMember]
		public virtual ICollection<Transaction> Transaction
		{
			get { return _transaction ?? (_transaction = new HashSet<Transaction>()); }
			protected set { _transaction = value; }
		}

		/// <summary>
		/// Gets or sets ROIPaid
		/// </summary>
		[DataMember]
		public virtual ICollection<ROIPaid> ROIPaid
		{
			get { return _roiPaid ?? (_roiPaid = new HashSet<ROIPaid>()); }
			protected set { _roiPaid = value; }
		}

		/// <summary>
		/// Gets or sets Customer Purchased Plan
		/// </summary>
		[DataMember]
		public virtual ICollection<CustomerPlan> CustomerPlan
		{
			get { return _customerPlans ?? (_customerPlans = new HashSet<CustomerPlan>()); }
			protected set { _customerPlans = value; }
		}

		[DataMember]
		public virtual ICollection<CustomerPosition> CustomerPosition
		{
			get { return _customerPosition ?? (_customerPosition = new HashSet<CustomerPosition>()); }
			protected set { _customerPosition = value; }
		}

		[DataMember]
		public virtual ICollection<BlogPost> BlogPosts
		{
			get { return _customerBlogs ?? (_customerBlogs = new HashSet<BlogPost>()); }
			protected set { _customerBlogs = value; }
		}
		
		#endregion

		#region Utils


		/// <summary>
		/// Gets a string identifier for the customer's roles by joining all role ids
		/// </summary>
		/// <param name="onlyActiveCustomerRoles"><c>true</c> ignores all inactive roles</param>
		/// <returns>The identifier</returns>
		public string GetRolesIdent(bool onlyActiveCustomerRoles = true)
		{
			return string.Join(",", this.CustomerRoles.Where(x => !onlyActiveCustomerRoles || x.Active).Select(x => x.Id));
		}

		public virtual void RemoveAddress(Address address)
		{
			if (this.Addresses.Contains(address))
			{
				if (this.BillingAddress == address) this.BillingAddress = null;
				if (this.ShippingAddress == address) this.ShippingAddress = null;

				this.Addresses.Remove(address);
			}
		}

		public void AddRewardPointsHistoryEntry(
			int points, 
			string message = "",
			Order usedWithOrder = null, 
			decimal usedAmount = 0M)
		{
			int newPointsBalance = this.GetRewardPointsBalance() + points;

			var rewardPointsHistory = new RewardPointsHistory()
			{
				Customer = this,
				UsedWithOrder = usedWithOrder,
				Points = points,
				PointsBalance = newPointsBalance,
				UsedAmount = usedAmount,
				Message = message,
				CreatedOnUtc = DateTime.UtcNow
			};

			this.RewardPointsHistory.Add(rewardPointsHistory);
		}

		/// <summary>
		/// Gets reward points balance
		/// </summary>
		public int GetRewardPointsBalance()
		{
			int result = 0;
			if (this.RewardPointsHistory.Count > 0)
				result = this.RewardPointsHistory.OrderByDescending(rph => rph.CreatedOnUtc).ThenByDescending(rph => rph.Id).FirstOrDefault().PointsBalance;
			return result;
		}

		#endregion
	}

	public class Google2FASetup
	{
		public string qrCodeImageUrl { get; set; }
		public string manualEntrySetupCode { get; set; }
	}
}