
namespace SmartStore.Core.Domain.Customers
{
	public static partial class SystemCustomerAttributeNames
	{
		//Form fields
		public static string StokistCommision { get { return "StokistCommision"; } }
		public static string Title { get { return "Title"; } }
		public static string FirstName { get { return "FirstName"; } }
		public static string LastName { get { return "LastName"; } }
		public static string Gender { get { return "Gender"; } }
		public static string DateOfBirth { get { return "DateOfBirth"; } }
		public static string Company { get { return "Company"; } }
		public static string StreetAddress { get { return "StreetAddress"; } }
		public static string StreetAddress2 { get { return "StreetAddress2"; } }
		public static string ZipPostalCode { get { return "ZipPostalCode"; } }
		public static string City { get { return "City"; } }
		public static string CountryId { get { return "CountryId"; } }
		public static string StateProvinceId { get { return "StateProvinceId"; } }
		public static string Phone { get { return "Phone"; } }
		public static string Fax { get { return "Fax"; } }
		public static string VatNumber { get { return "VatNumber"; } }
		public static string VatNumberStatusId { get { return "VatNumberStatusId"; } }
		public static string TimeZoneId { get { return "TimeZoneId"; } }
		public static string CustomerNumber { get { return "CustomerNumber"; } }
		public static string SolidTrustPayAcc { get { return "SolidTrustPayAcc"; } }
		public static string BitcoinAddressAcc { get { return "BitcoinAddressAcc"; } }

		public static string AccountNumber { get { return "AccountNumber"; } }
		public static string NICR { get { return "NICR"; } }
		public static string BankName { get { return "BankName"; } }
		public static string AccountHolderName { get { return "AccountHolderName"; } }

		public static string PayeerAcc { get { return "PayeerAcc"; } }
		public static string PMAcc { get { return "PMAcc"; } }
		public static string PayzaAcc { get { return "PayzaAcc"; } }
		public static string AdvanceCashAcc { get { return "AdvanceCashAcc"; } }
		public static string VacationModeExpiryDate { get { return "VacationModeExpiryDate"; } }
		public static string AllowROI { get { return "AllowROI"; } }
		public static string LastSurfDate { get { return "LastSurfDate"; } }
		public static string NextSurfDate { get { return "NextSurfDate"; } }
		public static string NoOfAdsSurfed { get { return "NoOfAdsSurfed"; } }
		public static string UPIPaymentNumber { get { return "UPIPaymentNumber"; } }
		public static string Enable2FA { get { return "Enable2FA"; } }
		//Other attributes
		public static string DiscountCouponCode { get { return "DiscountCouponCode"; } }
		public static string GiftCardCouponCodes { get { return "GiftCardCouponCodes"; } }
		public static string CheckoutAttributes { get { return "CheckoutAttributes"; } }
        public static string AvatarPictureId { get { return "AvatarPictureId"; } }
        public static string ForumPostCount { get { return "ForumPostCount"; } }
        public static string Signature { get { return "Signature"; } }
        public static string PasswordRecoveryToken { get { return "PasswordRecoveryToken"; } }
        public static string AccountActivationToken { get { return "AccountActivationToken"; } }
        public static string LastVisitedPage { get { return "LastVisitedPage"; } }
		public static string LastUserAgent { get { return "LastUserAgent"; } }
		public static string ImpersonatedCustomerId { get { return "ImpersonatedCustomerId"; } }
		public static string AdminAreaStoreScopeConfiguration { get { return "AdminAreaStoreScopeConfiguration"; } }
		public static string MostRecentlyUsedCategories { get { return "MostRecentlyUsedCategories"; } }
		public static string MostRecentlyUsedManufacturers { get { return "MostRecentlyUsedManufacturers"; } }
		public static string WalletEnabled { get { return "WalletEnabled"; } }
		public static string HasConsentedToGdpr { get { return "HasConsentedToGdpr"; } }

		//depends on store
		public static string CurrencyId { get { return "CurrencyId"; } }
		public static string LanguageId { get { return "LanguageId"; } }
		public static string SelectedPaymentMethod { get { return "SelectedPaymentMethod"; } }
		public static string SelectedShippingOption { get { return "SelectedShippingOption"; } }
		public static string OfferedShippingOptions { get { return "OfferedShippingOptions"; } }
		public static string LastContinueShoppingPage { get { return "LastContinueShoppingPage"; } }
		public static string NotifiedAboutNewPrivateMessages { get { return "NotifiedAboutNewPrivateMessages"; } }
		public static string WorkingThemeName { get { return "WorkingThemeName"; } }
		public static string TaxDisplayTypeId { get { return "TaxDisplayTypeId"; } }
		public static string UseRewardPointsDuringCheckout { get { return "UseRewardPointsDuringCheckout"; } }
		public static string UseCreditBalanceDuringCheckout { get { return "UseCreditBalanceDuringCheckout"; } }
	}
}