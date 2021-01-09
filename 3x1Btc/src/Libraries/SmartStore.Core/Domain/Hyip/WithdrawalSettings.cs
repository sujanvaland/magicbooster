using SmartStore.Core.Configuration;

namespace SmartStore.Core.Domain.Hyip
{
	public class WithdrawalSettings: ISettings
	{
		public bool AllowWithdrawal { get; set; }
		
		public float MinWithdrawal { get; set; }
		
		public float MaxWithdrawal { get; set; }
		
		public bool AllowAutoWithdrawal { get; set; }
		
		public int AutoWithdrawalHour { get; set; }
		
		public int MaxRequestPerDay { get; set; }

		public decimal WithdrawalFees { get; set; }

		public string PaymentNote { get; set; }

		public bool NotifyWithdrawalRequestToUser { get; set; }
		public bool NotifyWithdrawalRequestToAdmin { get; set; }
	}
}
