using SmartStore.Core.Domain.Customers;

namespace SmartStore.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService 
    {
        void SignIn(Customer customer, bool createPersistentCookie);
        void SignOut();
        Customer GetAuthenticatedCustomer();
		Google2FASetup SetupAuth(Customer customer);
		bool Validated2FA(Customer customer, string pin);
	}
}