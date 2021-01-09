using SmartStore.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Hyip
{
	public class CoinPaymentSettings: ISettings
	{
		public string CP_MerchantId { get; set; }

		public string CP_PublicKey { get; set; }

		public string CP_ApiSecretKey { get; set; }

		public string CP_SecretKey { get; set; }

		public bool CP_TestMode { get; set; }

		public bool CP_IsActivePaymentMethod { get; set; }

		public decimal DepositFees { get; set; }

		public bool CP_EmailConfirmationRequired { get; set; }
	}

	public class SolidTrustPaySettings : ISettings
	{
		public string STP_MerchantAccount { get; set; }

		public string STP_Sci_Name { get; set; }

		public string STP_CancelUrl { get; set; }

		public string STP_NotifyUrl { get; set; }

		public bool STP_TestMode { get; set; }

		public bool STP_IsActivePaymentMethod { get; set; }
		public decimal DepositFees { get; set; }
	}

	public class PayzaSettings : ISettings
	{
		public string PZ_MerchantAccount { get; set; }

		public string PZ_Sci_Name { get; set; }

		public string PZ_CancelUrl { get; set; }

		public string PZ_NotifyUrl { get; set; }

		public string PZ_ReturnUrl { get; set; }

		public bool PZ_IsActivePaymentMethod { get; set; }
		public decimal DepositFees { get; set; }

	}

	public class PMSettings : ISettings
	{
		public string PM_PayeeAccount { get; set; }

		public string PM_CancelUrl { get; set; }

		public string PM_NotifyUrl { get; set; }

		public string PM_ReturnUrl { get; set; }

		public bool PM_IsActivePaymentMethod { get; set; }

		public string PY_MerchantShop { get; set; }

		public string PY_SecretKey { get; set; }

		public bool PY_IsActivePaymentMethod { get; set; }
		public decimal DepositFees { get; set; }
	}

	public class PayeerSettings : ISettings
	{
		public string PY_MerchantShop { get; set; }

		public string PY_SecretKey { get; set; }

		public bool PY_IsActivePaymentMethod { get; set; }
		public decimal DepositFees { get; set; }
	}
}
