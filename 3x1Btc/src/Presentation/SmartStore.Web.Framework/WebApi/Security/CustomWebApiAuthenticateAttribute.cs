using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using Microsoft.IdentityModel.Tokens;
using SmartStore.Core;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Logging;
using SmartStore.Services.Customers;
using SmartStore.Services.Localization;
using SmartStore.Services.Security;
using SmartStore.Web.Framework.WebApi.Caching;

namespace SmartStore.Web.Framework.WebApi.Security
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
	public class CustomWebApiAuthenticateAttribute : System.Web.Http.AuthorizeAttribute
	{
		protected HmacAuthentication _hmac = new HmacAuthentication();
		private static string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";

		/// <summary>
		/// The system name of the permission
		/// </summary>
		public string Permission { get; set; }

		protected string CreateContentMd5Hash(HttpRequestMessage request)
		{
			if (request != null && request.Content != null)
			{
				byte[] contentBytes = request.Content.ReadAsByteArrayAsync().Result;

				if (contentBytes != null && contentBytes.Length > 0)
					return _hmac.CreateContentMd5Hash(contentBytes);
			}
			return "";
		}

		protected virtual bool HasPermission(IDependencyScope dependencyScope, Customer customer)
		{
			var result = true;

			if (Permission.HasValue())
			{
				try
				{
					var permissionService = dependencyScope.GetService<IPermissionService>();

					if (permissionService.GetPermissionRecordBySystemName(Permission) != null)
					{
						result = permissionService.Authorize(Permission, customer);
					}
				}
				catch { }
			}

			return result;
		}

		protected virtual void LogUnauthorized(HttpActionContext actionContext, IDependencyScope dependencyScope, HmacResult result, Customer customer)
		{
			try
			{
				var localization = dependencyScope.GetService<ILocalizationService>();
				var loggerFactory = dependencyScope.GetService<ILoggerFactory>();
				var logger = loggerFactory.GetLogger(this.GetType());

				var strResult = result.ToString();
				var description = localization.GetResource("Admin.WebApi.AuthResult." + strResult, 0, false, strResult);
				
				logger.Warn(
					new SecurityException("{0}\r\n{1}".FormatInvariant(description, actionContext.Request.Headers.ToString())),
					localization.GetResource("Admin.WebApi.UnauthorizedRequest").FormatInvariant(strResult)
				);
			}
			catch (Exception exception)
			{
				exception.Dump();
			}
		}

		protected virtual Customer GetCustomer(IDependencyScope dependencyScope, int customerId)
		{
			Customer customer = null;

			try
			{
				var customerService = dependencyScope.GetService<ICustomerService>();
				customer = customerService.GetCustomerById(customerId);
			}
			catch (Exception exception)
			{
				exception.Dump();
			}

			return customer;
		}

		protected virtual Customer GetCustomerByGuId(IDependencyScope dependencyScope, string customerGuId)
		{
			Customer customer = null;

			try
			{
				var customerService = dependencyScope.GetService<ICustomerService>();
				customer = customerService.GetCustomerByGuid(Guid.Parse(customerGuId));
			}
			catch (Exception exception)
			{
				exception.Dump();
			}

			return customer;
		}

		protected virtual HmacResult IsAuthenticated(
			HttpActionContext actionContext,
			IDependencyScope dependencyScope,
			WebApiControllingCacheData controllingData,
			DateTime utcNow,
			out Customer customer)
		{
			customer = null;
			var request = HttpContext.Current.Request;
			var authorization = actionContext.Request.Headers.Authorization;

			if (request == null)
				return HmacResult.FailedForUnknownReason;

			if (controllingData.ApiUnavailable)
				return HmacResult.ApiUnavailable;

			if (authorization == null || authorization.Scheme.IsEmpty() || authorization.Parameter.IsEmpty() || (authorization.Scheme.CompareTo("Basic") != 0) || !actionContext.Request.Headers.Contains("CustomerGUID"))
				return HmacResult.InvalidAuthorizationHeader;
			
			string tokenUsername = ValidateToken(authorization.Parameter);
			if (tokenUsername == null)
				return HmacResult.UserUnknown;

			var GUID = actionContext.Request.Headers.GetValues("CustomerGuid").FirstOrDefault();
			if (GUID == null)
				return HmacResult.UserUnknown;

			customer = GetCustomerByGuId(dependencyScope, GUID);

			if (customer == null)
				return HmacResult.UserUnknown;

			if (!HasPermission(dependencyScope, customer))
				return HmacResult.UserHasNoPermission;

			if (customer.Email.Equals(tokenUsername))
			{
				return HmacResult.Success;
			}
			else
			{
				return HmacResult.UserUnknown;
			}
		}

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			System.Net.Http.Headers.AuthenticationHeaderValue authorizationHeader = actionContext.Request.Headers.Authorization;
			var result = HmacResult.FailedForUnknownReason;
			var controllingData = WebApiCachingControllingData.Data();
			var dependencyScope = actionContext.Request.GetDependencyScope();
			var utcNow = DateTime.UtcNow;
			Customer customer = null;

			try
			{
				result = IsAuthenticated(actionContext, dependencyScope, controllingData, utcNow, out customer);
			}
			catch (Exception exception)
			{
				exception.Dump();
			}

			if (result == HmacResult.Success)
			{
				// Inform core about the authentication. Note, you cannot use IWorkContext.set_CurrentCustomer here.
				HttpContext.Current.User = new SmartStorePrincipal(customer, HmacAuthentication.Scheme1);

				var response = HttpContext.Current.Response;

				response.AddHeader(WebApiGlobal.Header.AppVersion, SmartStoreVersion.CurrentFullVersion);
				response.AddHeader(WebApiGlobal.Header.Version, controllingData.Version);
				response.AddHeader(WebApiGlobal.Header.MaxTop, controllingData.MaxTop.ToString());
				response.AddHeader(WebApiGlobal.Header.Date, utcNow.ToString("o"));
				response.AddHeader(WebApiGlobal.Header.CustomerId, customer.Id.ToString());

				response.Cache.SetCacheability(HttpCacheability.NoCache);
			}
			else
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

				var headers = actionContext.Response.Headers;
				var authorization = actionContext.Request.Headers.Authorization;

				// See RFC-2616
				var scheme = _hmac.GetWwwAuthenticateScheme(authorization != null ? authorization.Scheme : null);
				headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(scheme));

				headers.Add(WebApiGlobal.Header.AppVersion, SmartStoreVersion.CurrentFullVersion);
				headers.Add(WebApiGlobal.Header.Version, controllingData.Version);
				headers.Add(WebApiGlobal.Header.MaxTop, controllingData.MaxTop.ToString());
				headers.Add(WebApiGlobal.Header.Date, utcNow.ToString("o"));
				headers.Add(WebApiGlobal.Header.HmacResultId, ((int)result).ToString());
				headers.Add(WebApiGlobal.Header.HmacResultDescription, result.ToString());

				if (controllingData.LogUnauthorized)
				{
					LogUnauthorized(actionContext, dependencyScope, result, customer);
				}
			}
			
			
		}

		/// <remarks>we should never get here... just for security reason</remarks>
		protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
		{
			var message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
			throw new HttpResponseException(message);
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
	}
}
