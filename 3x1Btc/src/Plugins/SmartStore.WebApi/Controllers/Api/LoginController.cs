using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartStore.Core;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Forums;
using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Domain.Media;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Tax;
using SmartStore.Core.Localization;
using SmartStore.Core.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SmartStore.Services;
using SmartStore.Services.Authentication;
using SmartStore.Services.Authentication.External;
using SmartStore.Services.Catalog;
using SmartStore.Services.Catalog.Extensions;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Services.Directory;
using SmartStore.Services.Forums;
using SmartStore.Services.Helpers;
using SmartStore.Services.Localization;
using SmartStore.Services.Media;
using SmartStore.Services.Messages;
using SmartStore.Services.Orders;
using SmartStore.Services.Payments;
using SmartStore.Services.Security;
using SmartStore.Services.Tax;
using SmartStore.Web.Framework.Plugins;
using SmartStore.Web.Framework.WebApi.Security;
using SmartStore.WebApi.Models.Api.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using SmartStore.Services.Seo;
using System.Security.Claims;
using SmartStore.Core.Domain.Messages;
using SmartStore.Services.Advertisments;

namespace SmartStore.WebApi.Controllers.Api
{
    public class LoginController : ApiController
    {
        #region Fields
        public Localizer T { get; set; }//Added by Yagnesh 
        public ICommonServices Services { get; set; }//Added by Yagnesh 
		private static string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";
		private readonly ICommonServices _services;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly TaxSettings _taxSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ITaxService _taxService;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly AddressSettings _addressSettings;
        private readonly ForumSettings _forumSettings;
        private readonly OrderSettings _orderSettings;
        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly ICurrencyService _currencyService;
        private readonly IPaymentService _paymentService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IPictureService _pictureService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IForumService _forumService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly IDownloadService _downloadService;
        private readonly IWebHelper _webHelper;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ProductUrlHelper _productUrlHelper;
		private readonly IAdCampaignService _adCampaignService;
		private readonly MediaSettings _mediaSettings;
        private readonly LocalizationSettings _localizationSettings;
        //private readonly CaptchaSettings _captchaSettings;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly PluginMediator _pluginMediator;
        private readonly IPermissionService _permissionService;

        private readonly IProductService _productService;
        #endregion

        #region Ctor

        public LoginController(
            ICommonServices services,
			IAdCampaignService adCampaignService,
			IAuthenticationService authenticationService,
            IDateTimeHelper dateTimeHelper,
            DateTimeSettings dateTimeSettings, TaxSettings taxSettings,
            ILocalizationService localizationService,
            IWorkContext workContext, IStoreContext storeContext,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ICustomerRegistrationService customerRegistrationService,
            ITaxService taxService, RewardPointsSettings rewardPointsSettings,
            CustomerSettings customerSettings, AddressSettings addressSettings, ForumSettings forumSettings,
            OrderSettings orderSettings, IAddressService addressService,
            ICountryService countryService, IStateProvinceService stateProvinceService,
            IOrderTotalCalculationService orderTotalCalculationService,
            IOrderProcessingService orderProcessingService, IOrderService orderService,
            ICurrencyService currencyService,
            IPaymentService paymentService,
            IPriceFormatter priceFormatter,
            IPictureService pictureService, INewsLetterSubscriptionService newsLetterSubscriptionService,
            IForumService forumService, IShoppingCartService shoppingCartService,
            IOpenAuthenticationService openAuthenticationService,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            IDownloadService downloadService, IWebHelper webHelper,
            ICustomerActivityService customerActivityService,
            ProductUrlHelper productUrlHelper,
            MediaSettings mediaSettings,
            LocalizationSettings localizationSettings,
            //CaptchaSettings captchaSettings, 
            ExternalAuthenticationSettings externalAuthenticationSettings,
            PluginMediator pluginMediator,
            IPermissionService permissionService,
            IProductService productService)
        {
            _services = services;
            _authenticationService = authenticationService;
            _dateTimeHelper = dateTimeHelper;
            _dateTimeSettings = dateTimeSettings;
            _taxSettings = taxSettings;
            _localizationService = localizationService;
            _workContext = workContext;
            _storeContext = storeContext;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _customerRegistrationService = customerRegistrationService;
            _taxService = taxService;
            _rewardPointsSettings = rewardPointsSettings;
            _customerSettings = customerSettings;
            _addressSettings = addressSettings;
            _forumSettings = forumSettings;
            _orderSettings = orderSettings;
            _addressService = addressService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _orderProcessingService = orderProcessingService;
            _orderTotalCalculationService = orderTotalCalculationService;
            _orderService = orderService;
            _currencyService = currencyService;
            _paymentService = paymentService;
            _priceFormatter = priceFormatter;
            _pictureService = pictureService;
            _newsLetterSubscriptionService = newsLetterSubscriptionService;
            _forumService = forumService;
            _shoppingCartService = shoppingCartService;
            _openAuthenticationService = openAuthenticationService;
            _backInStockSubscriptionService = backInStockSubscriptionService;
            _downloadService = downloadService;
            _webHelper = webHelper;
            _customerActivityService = customerActivityService;
            _productUrlHelper = productUrlHelper;
			_adCampaignService = adCampaignService;
			_mediaSettings = mediaSettings;
            _localizationSettings = localizationSettings;
            //_captchaSettings = captchaSettings;
            _externalAuthenticationSettings = externalAuthenticationSettings;
            _pluginMediator = pluginMediator;
            _permissionService = permissionService;
            _productService = productService;
        }

		#endregion

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("CreateCustomer")]
		public HttpResponseMessage CreateCustomer()
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			string jsonString = "";
			try
			{
				var customer = new Customer
				{
					CustomerGuid = Guid.NewGuid(),
					AdminComment = "",
					IsTaxExempt = false,
					Active = true,
					CreatedOnUtc = DateTime.UtcNow,
					LastActivityDateUtc = DateTime.UtcNow,
				};
				_customerService.InsertCustomer(customer);

				//Add customer role
				var customerRole = _customerService.GetCustomerRoleById(4);
				customer.CustomerRoles.Add(customerRole);

				_customerService.UpdateCustomer(customer);
				jsonString = "{\"code\":1,\"message\": \"success\",\"data\":{\"GUID\":\"" + customer.CustomerGuid + "\",\"Id\":\"" + customer.Id + "\" }}";
				response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
				return response;
			}
			catch (Exception ex)
			{
				response.StatusCode = HttpStatusCode.OK;
				jsonString = "{\"code\": 0,\"message\": \"" + _localizationService.GetResource("Common.SomethingWentWrong") + "\"}";
				response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
				return response;
			}
		}

		[System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("Login")]
        public HttpResponseMessage Login(LoginModel model)//string returnUrl, bool captchaValid, bool RememberMe, string Username = null, string Email = null, string Password = null)
        {
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			string jsonString = "";
			try
			{
				if (ModelState.IsValid)
				{
					try
					{
						var addr = new System.Net.Mail.MailAddress(model.Email);
					}
					catch
					{
						model.Username = model.Email;
						model.Email = null;
					}

					if (_customerSettings.UsernamesEnabled && model.Username != null)
					{
						model.Username = model.Username.Trim();
					}

					if (_customerRegistrationService.ValidateCustomer(_customerSettings.UsernamesEnabled ? model.Username : model.Email, model.Password))
					{
						var customer = _customerSettings.UsernamesEnabled
							? _customerService.GetCustomerByUsername(model.Username)
							: _customerService.GetCustomerByEmail(model.Email);
												
						if (customer.Id != 0)
						{
							_workContext.CurrentCustomer = customer;
						}

						var Enable2FA = customer.GetAttribute<bool>(SystemCustomerAttributeNames.Enable2FA);
						if (Enable2FA)
						{
							return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = new { Enable2FA = true }});
						}
						_shoppingCartService.MigrateShoppingCart(customer, customer);
						if (customer != null)
						{
							var tokenString = GenerateJSONWebToken(customer.Email);
							_customerActivityService.InsertActivity("PublicStore.Login", _localizationService.GetResource("ActivityLog.PublicStore.Login"), customer);
							var c = JsonConvert.SerializeObject(customer, new JsonSerializerSettings
							{
								Formatting = Newtonsoft.Json.Formatting.Indented,
								ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
							});
							c = c.Replace(System.Environment.NewLine, string.Empty);
							return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = JObject.Parse(c), token = tokenString });
						}
					}
					else
					{
						ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
						return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = _localizationService.GetResource("Account.Login.WrongCredentials") });
					}
				}
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "invalid model" });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = _localizationService.GetResource("Common.SomethingWentWrong") });
			}
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("ValidateLoginWith2FA")]
		public HttpResponseMessage ValidateLoginWith2FA(LoginModel model)//string returnUrl, bool captchaValid, bool RememberMe, string Username = null, string Email = null, string Password = null)
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			string jsonString = "";
			try
			{
				if (ModelState.IsValid)
				{
					if (_customerSettings.UsernamesEnabled && model.Username != null)
					{
						model.Username = model.Username.Trim();
					}

					if (_customerRegistrationService.ValidateCustomer(_customerSettings.UsernamesEnabled ? model.Username : model.Email, model.Password))
					{
						var customer = _customerSettings.UsernamesEnabled
							? _customerService.GetCustomerByUsername(model.Username)
							: _customerService.GetCustomerByEmail(model.Email);

						if (customer.Id != 0)
						{
							_workContext.CurrentCustomer = customer;
						}

						bool isvalid = _authenticationService.Validated2FA(customer,model.Pin2FA);
						if (isvalid)
						{
							if (customer != null)
							{
								var tokenString = GenerateJSONWebToken(_customerSettings.UsernamesEnabled ? customer.Username : customer.Email);
								_customerActivityService.InsertActivity("PublicStore.Login", _localizationService.GetResource("ActivityLog.PublicStore.Login"), customer);
								var c = JsonConvert.SerializeObject(customer, new JsonSerializerSettings
								{
									Formatting = Newtonsoft.Json.Formatting.Indented,
									ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
								});
								c = c.Replace(System.Environment.NewLine, string.Empty);
								return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = JObject.Parse(c), token = tokenString });
							}
						}
						else
						{
							return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Invalid 2FA Code" });
						}
					}
					else
					{
						ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
						return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = _localizationService.GetResource("Account.Login.WrongCredentials") });
					}
				}
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "invalid model" });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = _localizationService.GetResource("Common.SomethingWentWrong") });
			}
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("Logout")]
		public HttpResponseMessage Logout(int customerid)
		{
			if (customerid != 0)
			{
				var customer = _customerService.GetCustomerById(customerid);
				if (customer.Id != 0)
				{
					_workContext.CurrentCustomer = customer;
				}
			}

			var response = this.Request.CreateResponse();
			string jsonString = "";

			try
			{
				//var customer = _customerService.GetCustomerById(customerid);				
				var customer = _workContext.CurrentCustomer;

				try
				{
					//external authentication
					ExternalAuthorizerHelper.RemoveParameters();
				}
				catch { }

				bool isadmin = false;
				if (customer.IsAdmin(true) == true)
				{
					isadmin = true;					
				}
				
				//standard logout 
				//activity log
				_customerActivityService.InsertActivity("PublicStore.Logout", _localizationService.GetResource("ActivityLog.PublicStore.Logout"));

				_authenticationService.SignOut();

				response.StatusCode = HttpStatusCode.OK;
				jsonString = JsonConvert.SerializeObject(new { id = customer.Id, isAdmin = isadmin }, Formatting.None);				

				jsonString = "{\"code\":1,\"message\": \"success\",\"data\":" + jsonString + "}";
				response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
				return response;
			}
			catch (Exception Ex)
			{
				response.StatusCode = HttpStatusCode.InternalServerError;
				jsonString = "{\"code\": 0,\"message\": \"" + _localizationService.GetResource("Common.SomethingWentWrong") + "\"}";
				response.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
				return response;
			}
		}

		[System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("Register")]
        public HttpResponseMessage Register(RegisterModel model)
        {
			var ValidateTree = _customerService.ValidateTree(model.PlacementUserName);
			var Sponsors = _customerService.GetCustomerByUsername(model.SponsorsName);
			var PlaceamentUser = _customerService.GetCustomerByUsername(model.PlacementUserName);
			if (model.Position == "" || model.Position == null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "Select Placement" });
			}
			if (Sponsors == null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "Sponsor Id Incorrect" });
			}
			if (PlaceamentUser == null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "Placement Id Incorrect" });
			}
			
			if(ValidateTree.Contains(model.Position))
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "This Position Is Already Filled" });
			}
			if (_customerService.ValidateEmail(model.Email))
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "Email is already registered" });
			}
			if (_customerService.GetCustomerByUsername(model.Username) != null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "Username is already registered, Please use different username" });
			}
			
			model.AffliateId = Sponsors.Id;
			model.PlacementId = PlaceamentUser.Id;
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				string jsonString = "";

				//check whether registration is allowed
				if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
				{
					//Registrition Disable
					//(int)UserRegistrationType.Disabled
				}

				var customer = _customerService.InsertGuestCustomerNew(model.PlacementId,model.Position);
				if (customer.Id != 0)
				{
					_workContext.CurrentCustomer = customer;
				}

				if (_customerSettings.UsernamesEnabled && model.Username != null)
				{
					model.Username = model.Username.Trim();
				}
				customer.AffiliateId = model.AffliateId;
				customer.PlacementId = model.PlacementId;
				customer.PlacementUserName = model.PlacementUserName;
				customer.SponsorsName = model.SponsorsName;
				customer.Position = model.Position;
				bool isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
				var registrationRequest = new CustomerRegistrationRequest(customer, model.Email,
					_customerSettings.UsernamesEnabled ? model.Username : model.Username, model.Password, _customerSettings.DefaultPasswordFormat, isApproved);
				var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
				if (registrationResult.Success)
				{
					// properties
					if (_dateTimeSettings.AllowCustomersToSetTimeZone)
					{
						_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.TimeZoneId, model.TimeZoneId);
					}

					// form fields
					if (_customerSettings.GenderEnabled)
						_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Gender, model.Gender);
					_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.FirstName, model.FirstName);
					_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.LastName, model.LastName);
					if (_customerSettings.DateOfBirthEnabled)
					{
						DateTime? dateOfBirth = null;
						try
						{
							dateOfBirth = new DateTime(model.DateOfBirthYear.Value, model.DateOfBirthMonth.Value, model.DateOfBirthDay.Value);
						}
						catch { }
						_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.DateOfBirth, dateOfBirth);
					}

					if (_customerSettings.CountryEnabled)
						_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.CountryId, model.CountryId);
					if (_customerSettings.PhoneEnabled)
						_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Phone, model.Phone);
					if (_customerSettings.CustomerNumberMethod == CustomerNumberMethod.AutomaticallySet && String.IsNullOrEmpty(customer.GetAttribute<string>(SystemCustomerAttributeNames.CustomerNumber)))
						_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.CustomerNumber, customer.Id);

					// Notifications
					if (_customerSettings.NotifyNewCustomerRegistration)
						Services.MessageFactory.SendCustomerRegisteredNotificationMessage(customer, _localizationSettings.DefaultAdminLanguageId);

					Services.MessageFactory.SendCustomerWelcomeMessage(customer, _workContext.WorkingLanguage.Id);

					if (_customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation)
					{
						// email validation message
						_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.AccountActivationToken, Guid.NewGuid().ToString());
						Services.MessageFactory.SendCustomerEmailValidationMessage(customer, _workContext.WorkingLanguage.Id);
						var subscription = new NewsLetterSubscription
						{
							NewsLetterSubscriptionGuid = Guid.NewGuid(),
							Email = customer.Email,
							Active = false,
							CreatedOnUtc = DateTime.UtcNow,
							StoreId = _storeContext.CurrentStore.Id
						};

						_newsLetterSubscriptionService.InsertNewsLetterSubscription(subscription);
						//return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.EmailValidation });
						return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = _localizationService.GetResource("Customer.VerifyEmail") });
					}
					else if (_customerSettings.UserRegistrationType == UserRegistrationType.AdminApproval)
					{
						return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = _localizationService.GetResource("Customer.AdminApproval") });
					}
					else
					{
						return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", GUID = customer.CustomerGuid });
					}
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = registrationResult.Errors[0] });
				}
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.InternalServerError, new { code = 1, Message = "Something went wrong, Ex:" + ex.ToString() });
			}
			
        }

		
        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("PasswordRecovery")]
        public HttpResponseMessage PasswordRecoverySend(PasswordRecoveryModel model)
        {
            var response = this.Request.CreateResponse();
			string jsonString = "";

			var customer = _customerService.GetCustomerByEmail(model.Email);
			if (customer.Id > 0)
			{
				_workContext.CurrentCustomer = customer;
			}

			try
            {
                //var customer = _customerService.GetCustomerByEmail(model.Email);
				
                if (customer != null && customer.Active && !customer.Deleted)
                {
                    var passwordRecoveryToken = Guid.NewGuid();
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.PasswordRecoveryToken, passwordRecoveryToken.ToString());
                    Services.MessageFactory.SendCustomerPasswordRecoveryMessage(customer, _workContext.WorkingLanguage.Id);

                    model.ResultMessage = _localizationService.GetResource("Account.PasswordRecovery.EmailHasBeenSent");
                    model.ResultState = PasswordRecoveryResultState.Success;
                    response.StatusCode = HttpStatusCode.OK;
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = model.ResultMessage });
				}
                else
                {
					model.ResultMessage = _localizationService.GetResource("Account.PasswordRecovery.EmailHasBeenSent");
					model.ResultState = PasswordRecoveryResultState.Success;
					//model.ResultMessage = _localizationService.GetResource("Account.PasswordRecovery.EmailNotFound");
					//model.ResultState = PasswordRecoveryResultState.Error;
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = model.ResultMessage });

				}
            }
            catch (Exception ex)
            {
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 1, Message = "something went wrong" });
            }
        }

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("ChangePassword")]
		public HttpResponseMessage ChangePassword(ChangePasswordModel model)
		{
			var response = this.Request.CreateResponse();
			string jsonString = "";

			var customer = _customerService.GetCustomerById(model.CustomerId);
			if (model.CustomerId > 0)
			{
				_workContext.CurrentCustomer = customer;
			}

			try
			{
				//var customer = _customerService.GetCustomerById(customerid);
				if (ModelState.IsValid)
				{
					var changePasswordRequest = new ChangePasswordRequest(customer.Email,
						true, _customerSettings.DefaultPasswordFormat, model.NewPassword, model.OldPassword);
					var changePasswordResult = _customerRegistrationService.ChangePassword(changePasswordRequest);
					if (changePasswordResult.Success)
					{
						model.Result = _localizationService.GetResource("Account.ChangePassword.Success");
						response.StatusCode = HttpStatusCode.OK;
						//jsonString = JsonConvert.SerializeObject(model, Formatting.None);
						return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = model });
					}
					else
					{
						foreach (var error in changePasswordResult.Errors)
							jsonString = jsonString + error;

						return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = jsonString });
					}
				}
				return Request.CreateResponse(HttpStatusCode.InternalServerError, new { code = 0, Message = _localizationService.GetResource("Common.SomethingWentWrong") });
			}
			catch (Exception Ex)
			{
				return Request.CreateResponse(HttpStatusCode.InternalServerError, new { code = 0, Message = _localizationService.GetResource("Common.SomethingWentWrong") });
			}
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("PasswordRecoveryConfirmPOST")]
		public HttpResponseMessage PasswordRecoveryConfirmPOST(string token, string email, PasswordRecoveryConfirmModel model)
		{
			var customer = _customerService.GetCustomerByEmail(email);
			if (customer == null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "something went wrong" });
			}

			var cPrt = customer.GetAttribute<string>(SystemCustomerAttributeNames.PasswordRecoveryToken);
			if (cPrt.IsEmpty() || !cPrt.Equals(token, StringComparison.InvariantCultureIgnoreCase))
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = T("Account.PasswordRecoveryConfirm.InvalidEmailOrToken").Text });
			}

			if (ModelState.IsValid)
			{
				var response = _customerRegistrationService.ChangePassword(new ChangePasswordRequest(email,
					false, _customerSettings.DefaultPasswordFormat, model.NewPassword));
				if (response.Success)
				{
					_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.PasswordRecoveryToken, "");
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success" });

				}
				else
				{
					model.Result = response.Errors.FirstOrDefault();
				}
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "something went wrong" });
			}

			// If we got this far, something failed, redisplay form.
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "something went wrong" });
		}

		[System.Web.Http.NonAction]
		protected bool IsCurrentUserRegistered(int customerid)
		{
			var customer = _customerService.GetCustomerById(customerid);
			if (customerid > 0)
			{
				_workContext.CurrentCustomer = customer;
			}
			return customer.IsRegistered();
			//return _customerService.GetCustomerById(customerid).IsRegistered();
		}

		private string GenerateJSONWebToken(string username)
		{
			byte[] key = Convert.FromBase64String(Secret);
			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
			SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] {
				new Claim(ClaimTypes.Name, username)}),
				Expires = DateTime.UtcNow.AddMinutes(30),
				SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
			};
			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
			return handler.WriteToken(token);
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("ValidateToken")]
		public HttpResponseMessage Validate(string token, string username)
		{
			var response = this.Request.CreateResponse();
			var customer = _customerService.GetCustomerByEmail(username);
			if (customer == null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "User not found" });
			}
			string tokenUsername = ValidateToken(token);
			if (username.Equals(tokenUsername))
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = token });
			}
			return response;
		}

		public static string ValidateToken(string token)
		{
			string username = null;
			ClaimsPrincipal principal = GetPrincipal(token);
			if (principal == null) return null;
			ClaimsIdentity identity = null;
			try
			{
				identity = (ClaimsIdentity)principal.Identity;
			}
			catch (NullReferenceException)
			{
				return null;
			}
			Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
			username = usernameClaim.Value;
			return username;
		}

		public static ClaimsPrincipal GetPrincipal(string token)
		{
			try
			{
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
				if (jwtToken == null) return null;
				byte[] key = Convert.FromBase64String(Secret);
				TokenValidationParameters parameters = new TokenValidationParameters()
				{
					RequireExpirationTime = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = new SymmetricSecurityKey(key)
				};
				SecurityToken securityToken;
				ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
				return principal;
			}
			catch
			{
				return null;
			}
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetInviterDetail")]
		public HttpResponseMessage GetInviterDetail(string inviter)
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			var customer = _customerService.GetCustomerByUsername(inviter);
			if(customer != null)
			{
				var CustomerName = customer.GetFullName();
				if (CustomerName.IsEmpty())
				{
					CustomerName = customer.Email;
				}
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = new { name = CustomerName } });
			}
			return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Invalid Inviter" });
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetCountryManager")]
		public HttpResponseMessage GetCountryManager()
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				var newsletters = _adCampaignService.GetCountryManagerFromCustomer();
				//var newsletters = _adCampaignService.GetCountryManager();
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = newsletters });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}
	}
}