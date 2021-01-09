using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Settings
{
	public partial class HyipPaymentSettingsModel : ModelBase
	{
		public HyipPaymentSettingsModel()
		{
			CoinPaymentSettings = new CoinPaymentSettingModel();
			SolitTrustPaySettings = new SolidTrustPaySettingModel();
			PayzaSettings = new PayzaSettingModel();
			PMSettings = new PMSettingModel();
			PayeerSettings = new PayeerSettingModel();
		}

		public CoinPaymentSettingModel CoinPaymentSettings { get; set; }
		public SolidTrustPaySettingModel SolitTrustPaySettings { get; set; }
		public PayzaSettingModel PayzaSettings { get; set; }
		public PayeerSettingModel PayeerSettings { get; set; }
		public PMSettingModel PMSettings { get; set; }
		#region Nested Class
		public partial class CoinPaymentSettingModel
		{
			[SmartResourceDisplayName("Admin.Configuration.Settings.CoinPayment.MerchantId")]
			public string CP_MerchantId { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.CoinPayment.PublicKey")]
			public string CP_PublicKey { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.CoinPayment.ApiSecretKey")]
			public string CP_ApiSecretKey { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.CoinPayment.SecretKey")]
			public string CP_SecretKey { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.CoinPayment.TestMode")]
			public bool CP_TestMode { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.CoinPayment.IsActivePaymentMethod")]
			public bool CP_IsActivePaymentMethod { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.CoinPayment.DepositFees")]
			public decimal DepositFees { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.CoinPayment.EmailConfirmationRequired")]
			public bool CP_EmailConfirmationRequired { get; set; }
		}

		public partial class SolidTrustPaySettingModel
		{
			[SmartResourceDisplayName("Admin.Configuration.Settings.SolitTrustPay.MerchantAccount")]
			public string STP_MerchantAccount { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.SolitTrustPay.Sci_Name")]
			public string STP_Sci_Name { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.SolitTrustPay.CancelUrl")]
			public string STP_CancelUrl { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.SolitTrustPay.NotifyUrl")]
			public string STP_NotifyUrl { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.SolitTrustPay.TestMode")]
			public bool STP_TestMode { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.SolitTrustPay.IsActivePaymentMethod")]
			public bool STP_IsActivePaymentMethod { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.SolitTrustPay.DepositFees")]
			public decimal DepositFees { get; set; }
		}

		public partial class PayzaSettingModel
		{
			[SmartResourceDisplayName("Admin.Configuration.Settings.Payza.MerchantAccount")]
			public string PZ_MerchantAccount { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.Payza.Sci_Name")]
			public string PZ_Sci_Name { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.Payza.CancelUrl")]
			public string PZ_CancelUrl { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.Payza.NotifyUrl")]
			public string PZ_NotifyUrl { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.Payza.NotifyUrl")]
			public string PZ_ReturnUrl { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.Payza.IsActivePaymentMethod")]
			public bool PZ_IsActivePaymentMethod { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.Payza.DepositFees")]
			public decimal DepositFees { get; set; }
		}

		public partial class PMSettingModel
		{
			[SmartResourceDisplayName("Admin.Configuration.Settings.PM.PayeeAccount")]
			public string PM_PayeeAccount { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.PM.CancelUrl")]
			public string PM_CancelUrl { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.PM.NotifyUrl")]
			public string PM_NotifyUrl { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.PM.NotifyUrl")]
			public string PM_ReturnUrl { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.PM.IsActivePaymentMethod")]
			public bool PM_IsActivePaymentMethod { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.PM.DepositFees")]
			public decimal DepositFees { get; set; }
		}

		public partial class PayeerSettingModel
		{
			[SmartResourceDisplayName("Admin.Configuration.Settings.Payeer.PayeeAccount")]
			public string PY_MerchantShop { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.Payeer.CancelUrl")]
			public string PY_SecretKey { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.Payeer.IsActivePaymentMethod")]
			public bool PY_IsActivePaymentMethod { get; set; }

			[SmartResourceDisplayName("Admin.Configuration.Settings.Payeer.DepositFees")]
			public decimal DepositFees { get; set; }
		}
		#endregion
	}

}