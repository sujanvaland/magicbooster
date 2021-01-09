using FluentValidation;
using SmartStore.Admin.Models.Hyip;
using SmartStore.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Validators.Hyip
{
	public partial class PlanValidator : AbstractValidator<PlanModel>
	{
		public PlanValidator(ILocalizationService localizationService)
		{
			RuleFor(x => x.Name).NotNull().WithMessage(localizationService.GetResource("Admin.Hyip.Plan.Fields.Name.Required"));
			RuleFor(x => x.NoOfPayouts).NotEmpty().WithMessage(localizationService.GetResource("Admin.Hyip.Plan.Fields.NoOfDays.Required"));
			RuleFor(x => x.ROIPercentage).NotEmpty().WithMessage(localizationService.GetResource("Admin.Hyip.Plan.Fields.ROIPercentage.Required"));
			RuleFor(x => x.MinimumInvestment).NotEmpty().WithMessage(localizationService.GetResource("Admin.Hyip.Plan.Fields.MinimumInvestment.Required"));
			RuleFor(x => x.MaximumInvestment).NotEmpty().WithMessage(localizationService.GetResource("Admin.Hyip.Plan.Fields.MaximumInvestment.Required"));
			RuleFor(x => x.PayEveryXDays).NotEmpty().WithMessage(localizationService.GetResource("Admin.Hyip.Plan.Fields.PayEveryXDays.Required"));
		}
	}
}