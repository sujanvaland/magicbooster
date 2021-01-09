using SmartStore.Web.Framework.Modelling;

namespace SmartStore.WebApi.Models.Api.Customer
{
    public partial class ChangePasswordModel : ModelBase
    {
		public int CustomerId { get; set; }

		public string OldPassword { get; set; }

		public string NewPassword { get; set; }
		
		public string ConfirmNewPassword { get; set; }

		public string Result { get; set; }
	}
}