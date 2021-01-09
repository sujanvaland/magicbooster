using System;
using System.Collections.Generic;
using SmartStore.Collections;
using SmartStore.Core;
using SmartStore.Core.Domain.Advertisments;
using SmartStore.Core.Domain.Catalog;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Domain.Orders;

namespace SmartStore.Services.Customers
{
    /// <summary>
    /// Customer service interface
    /// </summary>
    public partial interface ICustomerService
    {
        #region Customers

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <param name="registrationFrom">Customer registration from; null to load all customers</param>
        /// <param name="registrationTo">Customer registration to; null to load all customers</param>
        /// <param name="customerRoleIds">A list of customer role identifiers to filter by (at least one match); pass null or empty list in order to load all customers; </param>
        /// <param name="email">Email; null to load all customers</param>
        /// <param name="username">Username; null to load all customers</param>
        /// <param name="firstName">First name; null to load all customers</param>
        /// <param name="lastName">Last name; null to load all customers</param>
        /// <param name="dayOfBirth">Day of birth; 0 to load all customers</param>
        /// <param name="monthOfBirth">Month of birth; 0 to load all customers</param>
        /// <param name="company">Company; null to load all customers</param>
        /// <param name="phone">Phone; null to load all customers</param>
        /// <param name="zipPostalCode">Phone; null to load all customers</param>
        /// <param name="loadOnlyWithShoppingCart">Value indicating whther to load customers only with shopping cart</param>
        /// <param name="sct">Value indicating what shopping cart type to filter; userd when 'loadOnlyWithShoppingCart' param is 'true'</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
		/// <param name="deletedOnly">Whether only (soft)-deleted records should be loaded</param>
        /// <returns>Customer collection</returns>
        IPagedList<Customer> GetAllCustomers(
			DateTime? registrationFrom,
			DateTime? registrationTo, 
			int[] customerRoleIds, 
			string email, 
			string username,
			string firstName, 
			string lastName, 
			int dayOfBirth, 
			int monthOfBirth,
			string company, 
			string phone, 
			string zipPostalCode,
			bool loadOnlyWithShoppingCart, 
			ShoppingCartType? sct, 
			int pageIndex, 
			int pageSize,
			bool? isCountryManager,
			bool deletedOnly = false
			);

        /// <summary>
        /// Gets all customers by affiliate identifier
        /// </summary>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Customers</returns>
        IPagedList<Customer> GetAllCustomers(int affiliateId, int pageIndex, int pageSize);

        /// <summary>
        /// Gets all customers by customer format (including deleted ones)
        /// </summary>
        /// <param name="passwordFormat">Password format</param>
        /// <returns>Customers</returns>
        IList<Customer> GetAllCustomersByPasswordFormat(PasswordFormat passwordFormat);

        /// <summary>
        /// Gets online customers
        /// </summary>
        /// <param name="lastActivityFromUtc">Customer last activity date (from)</param>
        /// <param name="customerRoleIds">A list of customer role identifiers to filter by (at least one match); pass null or empty list in order to load all customers; </param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Customer collection</returns>
        IPagedList<Customer> GetOnlineCustomers(DateTime lastActivityFromUtc,
            int[] customerRoleIds, int pageIndex, int pageSize);

        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="customer">Customer</param>
        void DeleteCustomer(Customer customer);

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        Customer GetCustomerById(int customerId);

		Customer GetCustomerByAffilateId(int affilateid);
		/// <summary>
		/// Get customers by identifiers
		/// </summary>
		/// <param name="customerIds">Customer identifiers</param>
		/// <returns>Customers</returns>
		IList<Customer> GetCustomersByIds(int[] customerIds);

		/// <summary>
		/// Get system account customers
		/// </summary>
		/// <returns>System account customers</returns>
		IList<Customer> GetSystemAccountCustomers();

		/// <summary>
		/// Gets a customer by GUID
		/// </summary>
		/// <param name="customerGuid">Customer GUID</param>
		/// <returns>A customer</returns>
		Customer GetCustomerByGuid(Guid customerGuid);

        /// <summary>
        /// Get customer by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Customer</returns>
        Customer GetCustomerByEmail(string email);
        
        /// <summary>
        /// Get customer by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Customer</returns>
        Customer GetCustomerBySystemName(string systemName);

        /// <summary>
        /// Get customer by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Customer</returns>
        Customer GetCustomerByUsername(string username);

		/// <summary>
		/// Get Stokist/Country Manager by username
		/// </summary>
		/// <param name="username">Username</param>
		/// <returns>Customer</returns>
		/// 
		List<Customer> GetStokistByUsername(string username);

		/// <summary>
		/// Get Stokist/Country Manager List
		/// </summary>
		/// <param name="username">Username</param>
		/// <returns>Customer</returns>
		/// 
		List<Customer> GetStokistList();

		/// <summary>
		/// Get Receiver Is From Team
		/// </summary>
		/// <param name="senderid, receiverid">senderid, receiverid</param>
		/// <returns>int</returns>
		int GetIsReceiverInTeam(int SenderId, int ReceiverId);

		/// <summary>
		/// Insert a guest customer
		/// </summary>
		/// <param name="customerGuid">The customer GUID. Pass <c>null</c> to create a random one.</param>
		/// <returns>Customer</returns>
		Customer InsertGuestCustomer(Guid? customerGuid = null);
		Customer InsertGuestCustomerNew(int PlacementId, string Position, Guid? customerGuid = null);


		/// <summary>
		/// Tries to find a guest/anonymous customer record by client ident. This method should be called when an
		/// anonymous visitor rejects cookies and therefore cannot be identified automatically.
		/// </summary>
		/// <param name="clientIdent">
		/// The client ident string, which is a hashed combination of client IP address and user agent. 
		/// Call <see cref="IWebHelper.GetClientIdent()"/> to obtain an ident string, or pass <c>null</c> to let this method obtain it automatically.</param>
		/// <param name="maxAgeSeconds">The max age of the newly created guest customer record. The shorter, the better (default is 1 min.)</param>
		/// <returns>The identified customer or <c>null</c></returns>
		Customer FindGuestCustomerByClientIdent(string clientIdent = null, int maxAgeSeconds = 60);

		/// <summary>
		/// Insert a customer
		/// </summary>
		/// <param name="customer">Customer</param>
		void InsertCustomer(Customer customer);

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="customer">Customer</param>
        void UpdateCustomer(Customer customer);

		/// <summary>
		/// Reset data required for checkout
		/// </summary>
		/// <param name="customer">Customer</param>
		/// <param name="storeId">Store identifier</param>
		/// <param name="clearCouponCodes">A value indicating whether to clear coupon code</param>
		/// <param name="clearCheckoutAttributes">A value indicating whether to clear selected checkout attributes</param>
		/// <param name="clearRewardPoints">A value indicating whether to clear "Use reward points" flag</param>
		/// <param name="clearShippingMethod">A value indicating whether to clear selected shipping method</param>
		/// <param name="clearPaymentMethod">A value indicating whether to clear selected payment method</param>
		/// <param name="clearCreditBalance">A value indicating whether to clear credit balance.</param>
		void ResetCheckoutData(Customer customer, int storeId,
            bool clearCouponCodes = false, bool clearCheckoutAttributes = false,
            bool clearRewardPoints = false, bool clearShippingMethod = true,
            bool clearPaymentMethod = true,
			bool clearCreditBalance = false);

        /// <summary>
        /// Delete guest customer records
        /// </summary>
        /// <param name="registrationFrom">Customer registration from; null to load all customers</param>
        /// <param name="registrationTo">Customer registration to; null to load all customers</param>
        /// <param name="onlyWithoutShoppingCart">A value indicating whether to delete customers only without shopping cart</param>
        /// <returns>Number of deleted customers</returns>
        int DeleteGuestCustomers(DateTime? registrationFrom, DateTime? registrationTo, bool onlyWithoutShoppingCart, int maxItemsToDelete = 5000);

        #endregion

        #region Customer roles

        /// <summary>
        /// Delete a customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        void DeleteCustomerRole(CustomerRole customerRole);

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <returns>Customer role</returns>
        CustomerRole GetCustomerRoleById(int customerRoleId);

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="systemName">Customer role system name</param>
        /// <returns>Customer role</returns>
        CustomerRole GetCustomerRoleBySystemName(string systemName);

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer role collection</returns>
        IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false);

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        void InsertCustomerRole(CustomerRole customerRole);

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        void UpdateCustomerRole(CustomerRole customerRole);

        #endregion

		#region Reward points

		/// <summary>
		/// Add or remove reward points for a product review
		/// </summary>
		/// <param name="customer">The customer</param>
		/// <param name="product">The product</param>
		/// <param name="add">Whether to add or remove points</param>
		void RewardPointsForProductReview(Customer customer, Product product, bool add);

		/// <summary>
		/// Gets reward points histories
		/// </summary>
		/// <param name="customerIds">Customer identifiers</param>
		/// <returns>Reward points histories</returns>
		Multimap<int, RewardPointsHistory> GetRewardPointsHistoriesByCustomerIds(int[] customerIds);

		#endregion Reward points

		#region TransferCoin
		float GetAvailableCoin(int customerid);
		int GetPlanCount(int CustomerId);
		int GetCurrentPlan(int CustomerId);
		string GetCurrentPlanName(int CustomerId);
		CustomerPlan GetCurrentPlanList(int CustomerId);
		float GetCoinEarning(int customerid);
		float GetCoinPurchase(int customerid);
		float GetCoinTransfer(int customerid);
		#endregion

		#region Transaction
		int GetCustomerToken(int CustomerId);
		int GetTrafficGenerated(int CustomerId);
		float GetAvailableBalance(int customerid);
		float GetAccumulatedPair(int customerid);
		float GetCustomerROI(int customerid);
		float GetCustomerCommission(int customerid);
		float GetCustomerWithdrawal(int customerid);
		float GetCustomerPendingWithdrawal(int customerid);
		float GetCustomerCompletedWithdrawal(int customerid);
		float GetCustomerFunding(int customerid);
		float GetCustomerPurchase(int customerid);
		float GetCustomerCyclerBonus(int customerid);
		float GetCustomerDirectBonus(int customerid);
		float GetCustomerPoolBonus(int customerid);
		float GetCustomerUnilevelBonus(int customerid);
		float GetCustomerTotalEarnings(int customerid);
		float GetRepurchaseBalance(int customerid);
		float GetRepurchaseROI(int customerid);
		float GetRepurchasePurchase(int customerid);
		List<TempReferralList> GetCustomerReferral(int customerid);
		List<CustomerBoardPosition> SaveCusomerPosition(int customerid,int PackageId);
		#endregion

		List<CustomerAvailableTraffic> GetAvailableCredits(int CustomerId);
		List<Customer> GetCustomerDirectReferral(int customerid);
		int SendPassUpBonus(int CustomerId);
		int SpPayNetworkIncome(int CustomerId, int PlanId);
		List<Customer> GetCustomerPaidDirectReferral(int customerid);
		CustomerTraffic InsertCustomerTraffic(CustomerTraffic customerTraffic);
		void InsertCustomerToken(CustomerToken customerToken);
		List<string> ValidateTree(string PlacementId);
		bool ValidateEmail(string EmailId);
		string GetTotalPair(int CustomerId);
		float GetNetworkIncome(int CustomerId);
	}
}