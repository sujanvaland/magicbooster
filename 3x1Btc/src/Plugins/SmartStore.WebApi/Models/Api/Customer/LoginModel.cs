using SmartStore.Web.Framework.Modelling;

namespace SmartStore.WebApi.Models.Api.Customer
{
    public partial class LoginModel : ModelBase
    {
        public bool CheckoutAsGuest { get; set; }

      
        public string Email { get; set; }

        public bool UsernamesEnabled { get; set; }
       
        public string Username { get; set; }

       
        public string Password { get; set; }
		public string Pin2FA { get; set; }
       
        public bool RememberMe { get; set; }

        public bool DisplayCaptcha { get; set; }

 
    }
}