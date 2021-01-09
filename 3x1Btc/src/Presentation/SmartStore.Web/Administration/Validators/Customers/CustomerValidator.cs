using FluentValidation;
using SmartStore.Admin.Models.Customers;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Services;
using SmartStore.Services.Localization;

namespace SmartStore.Admin.Validators.Customers
{
	public partial class CustomerValidator : AbstractValidator<CustomerModel>
    {
        public CustomerValidator(ILocalizationService localizationService, CustomerSettings customerSettings, ICommonServices commonServices)
        {
			var storeScope = 0;

			//var coinpaymentSettings = commonServices.Settings.LoadSetting<CoinPaymentSettings>(storeScope);
			
			//var SolidTrustPaySettings = commonServices.Settings.LoadSetting<SolidTrustPaySettings>(storeScope);
			
			//var PayzaSettings = commonServices.Settings.LoadSetting<PayzaSettings>(storeScope);
			
			//var PMSettings = commonServices.Settings.LoadSetting<PMSettings>(storeScope);
			
			//var PayeerSettings = commonServices.Settings.LoadSetting<PayeerSettings>(storeScope);
			
			//form fields
			if (customerSettings.FirstNameRequired)
				RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.FirstName.Required"));
			if (customerSettings.LastNameRequired)
				RuleFor(x => x.LastName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.LastName.Required"));
			if (customerSettings.CompanyRequired && customerSettings.CompanyEnabled)
                RuleFor(x => x.Company).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Company.Required"));
            if (customerSettings.StreetAddressRequired && customerSettings.StreetAddressEnabled)
                RuleFor(x => x.StreetAddress).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.StreetAddress.Required"));
            if (customerSettings.StreetAddress2Required && customerSettings.StreetAddress2Enabled)
                RuleFor(x => x.StreetAddress2).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.StreetAddress2.Required"));
            if (customerSettings.ZipPostalCodeRequired && customerSettings.ZipPostalCodeEnabled)
                RuleFor(x => x.ZipPostalCode).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.ZipPostalCode.Required"));
            if (customerSettings.CityRequired && customerSettings.CityEnabled)
                RuleFor(x => x.City).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.City.Required"));
            if (customerSettings.PhoneRequired && customerSettings.PhoneEnabled)
                RuleFor(x => x.Phone).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Phone.Required"));
            if (customerSettings.FaxRequired && customerSettings.FaxEnabled)
                RuleFor(x => x.Fax).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Fax.Required"));
			//if (coinpaymentSettings.CP_IsActivePaymentMethod)
			//	RuleFor(x => x.BitcoinAddressAcc).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.BitcoinAddressAcc.Required"));
			//if (SolidTrustPaySettings.STP_IsActivePaymentMethod)
			//	RuleFor(x => x.SolidTrustPayAcc).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.SolidTrustPayAcc.Required"));
			//if (PayzaSettings.PZ_IsActivePaymentMethod)
			//	RuleFor(x => x.PayzaAcc).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.PayzaAcc.Required"));
			//if (PMSettings.PM_IsActivePaymentMethod)
			//	RuleFor(x => x.PMAcc).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.PMAcc.Required"));
			//if (PayeerSettings.PY_IsActivePaymentMethod)
			//	RuleFor(x => x.PayeerAcc).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.PayeerAcc.Required"));
		}
	}
}