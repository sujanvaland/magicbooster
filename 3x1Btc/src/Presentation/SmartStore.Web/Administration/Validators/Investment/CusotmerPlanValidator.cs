using FluentValidation;
using SmartStore.Admin.Models.Investment;
using SmartStore.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Validators.Investment
{
	public class CusotmerPlanValidator : AbstractValidator<CustomerPlanModel>
	{
		public CusotmerPlanValidator(ILocalizationService localizationService)
		{
			RuleFor(x => x.PlanId).NotNull().WithMessage(localizationService.GetResource("Admin.Investment.Deposit.Fields.Id.Required"));
			RuleFor(x => x.AmountInvested).NotNull().WithMessage(localizationService.GetResource("Admin.Investment.Deposit.Fields.Amount.Required"));
			RuleFor(x => x.ProcessorId).NotNull().WithMessage(localizationService.GetResource("Admin.Investment.Deposit.Fields.Processor.Required"));
		}
	}
}