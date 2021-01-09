using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Customers
{
	[Validator(typeof(ProfileModelValidator))]
	public class ProfileModel
	{
		public string EmailId { get; set; }
		public string ReferredBy { get; set; }
		public string CreatedDate { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MobileNo { get; set; }
		public string BitcoinAddress { get; set; }
		
	}

	public partial class ProfileModelValidator : AbstractValidator<ProfileModel>
	{
		public ProfileModelValidator()
		{
			RuleFor(x => x.FirstName).NotEmpty().WithMessage("Firstname is required");
			RuleFor(x => x.LastName).NotEmpty().WithMessage("Lastname is required");
			RuleFor(x => x.EmailId).NotEmpty().WithMessage("Email Id is required");
			RuleFor(x => x.MobileNo).NotEmpty().WithMessage("Mobile No is required");
			RuleFor(x => x.BitcoinAddress).NotEmpty().WithMessage("Bitcoin Address is required");
		}
	}
}