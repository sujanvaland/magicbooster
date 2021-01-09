using Razorpay.Api;
using RestSharp;
using SmartStore.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Web.Controllers
{
    public class OAuthController : PublicControllerBase
	{
        // GET: oAuth
        public ActionResult Index()
        {
			//https://login.xero.com/identity/connect/authorize?response_type=code&client_id=YOURCLIENTID&redirect_uri=YOURREDIRECTURI&scope=openid profile email accounting.transactions&state=123&code_challenge=XXXXXXXXX&code_challenge_method=S256
			var code_challenge = MakeCodeVerifier(32);
			var client = new RestSharp.RestClient("https://login.xero.com/");
			var request = new RestRequest("identity/connect/authorize/", Method.GET);
			request.AddParameter("response_type", "code");
			request.AddParameter("client_id", "437ACD16C59E4C17A43FE968BA55CE06");
			request.AddParameter("redirect_uri", "https://globalmatrixmillionaire.com/oAuth");
			request.AddParameter("scope", "openid profile email accounting.transactions");
			request.AddParameter("code_challenge", code_challenge);
			request.AddParameter("code_challenge_method", "S256");
			var queryResult = client.Execute<InvoiceResponse.RootObject>(request).Data;

			return Content(queryResult.ToString());
        }

		private string MakeCodeVerifier(int numBytes = 32)
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				byte[] bytes = new byte[numBytes];
				rng.GetBytes(bytes);
				return EncodeBase64URL(bytes);
			}
		}

		string EncodeBase64URL(byte[] arg)
		{
			string s = Convert.ToBase64String(arg); // Regular base64 encoder
			s = s.Split('=')[0]; // Remove any trailing '='s
			s = s.Replace('+', '-'); // 62nd char of encoding
			s = s.Replace('/', '_'); // 63rd char of encoding
			return s;
		}
	}

	
}