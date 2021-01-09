using FluentValidation;
using SmartStore.Admin.Models.Investment;
using SmartStore.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Validators.Investment
{
	public class TransactionValidator : AbstractValidator<TransactionModel>
	{
		public TransactionValidator(ILocalizationService localizationService)
		{
			RuleFor(x => x.CustomerId).NotNull().WithMessage(localizationService.GetResource("Admin.Transaction.Fields.CustomerId.Required"));
			RuleFor(x => x.Amount).NotNull().WithMessage(localizationService.GetResource("Admin.Transaction.Fields.Amount.Required"));
			RuleFor(x => x.TransStatus).NotNull().WithMessage(localizationService.GetResource("Admin.Transaction.Fields.Status.Required"));
			RuleFor(x => x.TranscationType).NotNull().WithMessage(localizationService.GetResource("Admin.Transaction.Fields.TranscationType.Required"));
		}
	}
}