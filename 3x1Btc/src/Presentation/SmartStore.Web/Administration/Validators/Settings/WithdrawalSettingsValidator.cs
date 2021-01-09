using FluentValidation;
using SmartStore.Admin.Models.Settings;
using SmartStore.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Validators.Settings
{
	public class WithdrawalSettingsValidator : AbstractValidator<WithdrawalSettingsModel>
	{
		public WithdrawalSettingsValidator(ILocalizationService localizationService)
		{
			RuleFor(x=>x.MinWithdrawal).NotEmpty().WithMessage(localizationService.GetResource("Admin.Hyip.WrongMinWithdrawal"));
		}
	}
}