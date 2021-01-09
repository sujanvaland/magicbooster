using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace SmartStore.WebApi.Models.Api.Customer
{
    public partial class CustomerInfoModel : ModelBase
    {
		public string StokistCommision { get; set; }

		public float AccumulatedPairing { get; set; }
		public string PackageName { get; set; }
		public string SponsorsName { get; set; }
		public string PlacementUserName { get; set; }
		public string AccountNumber { get; set; }
		public string NICR { get; set; }
		public string BankName { get; set; }
		public string AccountHolderName { get; set; }

		public string Position { get; set; }

		public int PlacementId { get; set; }

		public int? Id { get; set; }
		public int CustomerId { get; set; }
		public Guid? CustomerGuid { get; set; }
		public bool Enable2FA { get; set; }
		public string FullName { get; set; }
		public string Pin2FA { get; set; }
		public string Username { get; set; }

        public string Email { get; set; }
          
        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public string CustomerNumber { get; set; }
        
        public string Gender { get; set; }
        
        public string FirstName { get; set; }
      
        public string LastName { get; set; }
      
        public DateTime? DateOfBirth { get; set; }

		public int? DateOfBirthDay { get; set; }
		public int? DateOfBirthMonth { get; set; }
		public int? DateOfBirthYear { get; set; }

		public string Company { get; set; }
       
        public string City { get; set; }
        
        public int CountryId { get; set; }
      
		public string Phone { get; set; }



        public string Title { get; set; }

        public string StreetAddress { get; set; }
        public string StreetAddress2 { get; set; }
        public string ZipPostalCode { get; set; }

        public string StateProvinceId { get; set; }

        public string Fax { get; set; }
        public string VatNumber { get; set; }
        public string VatNumberStatusId { get; set; }
        public string TimeZoneId { get; set; }

		[SmartResourceDisplayName("Admin.Withdrawal.Fields.SolidTrustPayAcc")]
		public string SolidTrustPayAcc { get; set; }

		[SmartResourceDisplayName("Admin.Withdrawal.Fields.BitcoinAddress")]
		public string BitcoinAddress { get; set; }

		[SmartResourceDisplayName("Admin.Withdrawal.Fields.PayzaAcc")]
		public string PayzaAcc { get; set; }

		[SmartResourceDisplayName("Admin.Withdrawal.Fields.PMAcc")]
		public string PMAcc { get; set; }

		[SmartResourceDisplayName("Admin.Withdrawal.Fields.PayeerAcc")]
		public string PayeerAcc { get; set; }

		[SmartResourceDisplayName("Admin.Withdrawal.Fields.AdvanceCashAcc")]
		public string AdvanceCashAcc { get; set; }

		public float CompletedWithdrawal { get; set; }
		public float PendingWithdrawal { get; set; }
		public float AvailableBalance { get; set; }
		public string TodaysPair { get; set; }
		public float NetworkIncome { get; set; }
		public float AvailableCoins { get; set; }
		public string WithdrawalAccount { get; set; }
		public string UPIPaymentNumber { get; set; }

		public string ReferralLink { get; set; }
		public float MyTotalInvestment { get; set; }
		public float TotalCommission { get; set; }
		public float TotalReferral { get; set; }
		public float RepurchaseBalance { get; set; }
		public float TotalEarning { get; set; }
		public float DirectBonus { get; set; }
		public float UnilevelEarning { get; set; }
		public float PoolShare { get; set; }
		public int GCTBalance { get; set; }
		public float GCTInDollar { get; set; }
		public int AdCredit { get; set; }
		public int TrafficGenerated { get; set; }
		public float CyclerIncome { get; set; }
		public float TotalIncome { get; set; }
		public string VacationModelExpiryDate { get; set; }
		public DateTime NextSurfTime { get; set; }
		public int NoOfAdsToSurf { get; set; }
		public int NoOfSecondsToSurf { get; set; }
		public string InvestorId { get; set; }
		public string Name { get; set; }
		public string RegistrationDate { get; set; }
		public string ServerTime { get; set; }
		public string ReferredBy { get; set; }
		public string Status { get; set; }
		public string CurrencyCode { get; set; }
		public string SubscriptionDate { get; set; }
		public int AffilateId { get; set; }
	}
}