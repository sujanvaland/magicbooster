using FluentValidation.Attributes;
using SmartStore.Admin.Validators.Settings;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Settings
{
	[Validator(typeof(WithdrawalSettingsValidator))]
	public class WithdrawalSettingsModel : ModelBase
	{
		public WithdrawalSettingsModel()
		{

		}
		[SmartResourceDisplayName("Admin.Configuration.Settings.Withdrawal.AllowWithdrawal")]
		public bool AllowWithdrawal { get; set; }
		[SmartResourceDisplayName("Admin.Configuration.Settings.Withdrawal.MinWithdrawal")]
		public float MinWithdrawal { get; set; }
		[SmartResourceDisplayName("Admin.Configuration.Settings.Withdrawal.MaxWithdrawal")]
		public float MaxWithdrawal { get; set; }
		[SmartResourceDisplayName("Admin.Configuration.Settings.Withdrawal.AllowAutoWithdrawal")]
		public bool AllowAutoWithdrawal { get; set; }
		[SmartResourceDisplayName("Admin.Configuration.Settings.Withdrawal.AutoWithdrawalHour")]
		public int AutoWithdrawalHour { get; set; }
		[SmartResourceDisplayName("Admin.Configuration.Settings.Withdrawal.MaxRequestPerDay")]
		public int MaxRequestPerDay { get; set; }
		[SmartResourceDisplayName("Admin.Configuration.Settings.Withdrawal.WithdrawalFees")]
		public decimal WithdrawalFees { get; set; }
		[SmartResourceDisplayName("Admin.Configuration.Settings.Withdrawal.PaymentNote")]
		public string PaymentNote { get; set; }
		[SmartResourceDisplayName("Admin.Configuration.Settings.Withdrawal.NotifyWithdrawalRequestToUser")]
		public bool NotifyWithdrawalRequestToUser { get; set; }
		[SmartResourceDisplayName("Admin.Configuration.Settings.Withdrawal.NotifyWithdrawalRequestToAdmin")]
		public bool NotifyWithdrawalRequestToAdmin { get; set; }
	}
}