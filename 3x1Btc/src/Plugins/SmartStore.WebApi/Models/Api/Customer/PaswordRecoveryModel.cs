using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SmartStore.WebApi.Models.Api.Customer
{  
    public partial class PasswordRecoveryModel
    {
        [AllowHtml]
        [SmartResourceDisplayName("Account.PasswordRecovery.Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

        public string ResultMessage { get; set; }

		public string OTP { get; set; }

		public string Phone { get; set; }

		public PasswordRecoveryResultState ResultState { get; set; }
    }

	public partial class PasswordRecoveryConfirmModel : ModelBase
	{
		[AllowHtml]
		[DataType(DataType.Password)]
		[SmartResourceDisplayName("Account.PasswordRecovery.NewPassword")]
		public string NewPassword { get; set; }

		[AllowHtml]
		[DataType(DataType.Password)]
		[SmartResourceDisplayName("Account.PasswordRecovery.ConfirmNewPassword")]
		public string ConfirmNewPassword { get; set; }

		public bool SuccessfullyChanged { get; set; }
		public string Result { get; set; }
	}
	public enum PasswordRecoveryResultState
    {
        Success,
        Error
    }    
}