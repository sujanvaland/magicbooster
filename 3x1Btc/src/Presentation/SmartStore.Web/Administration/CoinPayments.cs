/**
Copyright (c) 2017, CoinPayments.net
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the CoinPayments.net nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL CoinPayments.net BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SmartStore.Admin
{
	public class CoinPayments
	{
		private string s_privkey = "";
		private string s_pubkey = "";
		private static readonly Encoding encoding = Encoding.UTF8;

		public CoinPayments()
		{

		}
		public CoinPayments(string privkey, string pubkey)
		{
			s_privkey = privkey;
			s_pubkey = pubkey;
			if (s_privkey.Length == 0 || s_pubkey.Length == 0)
			{
				throw new ArgumentException("Private or Public Key is empty");
			}
		}

		public Dictionary<string, dynamic> CallAPI(string cmd, SortedList<string, string> parms = null)
		{
			if (parms == null)
			{
				parms = new SortedList<string, string>();
			}
			parms["version"] = "1";
			parms["key"] = s_pubkey;
			parms["cmd"] = cmd;

			string post_data = "";
			foreach (KeyValuePair<string, string> parm in parms)
			{
				if (post_data.Length > 0) { post_data += "&"; }
				post_data += parm.Key + "=" + Uri.EscapeDataString(parm.Value);
			}

			byte[] keyBytes = encoding.GetBytes(s_privkey);
			byte[] postBytes = encoding.GetBytes(post_data);
			var hmacsha512 = new System.Security.Cryptography.HMACSHA512(keyBytes);
			string hmac = BitConverter.ToString(hmacsha512.ComputeHash(postBytes)).Replace("-", string.Empty);

			// do the post:
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			System.Net.WebClient cl = new System.Net.WebClient();
			cl.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
			cl.Headers.Add("HMAC", hmac);
			cl.Encoding = encoding;

			var ret = new Dictionary<string, dynamic>();
			try
			{
				string resp = cl.UploadString("https://www.coinpayments.net/api.php", post_data);
				var decoder = new System.Web.Script.Serialization.JavaScriptSerializer();
				ret = decoder.Deserialize<Dictionary<string, dynamic>>(resp);
			}
			catch (System.Net.WebException e)
			{
				ret["error"] = "Exception while contacting CoinPayments.net: " + e.Message;
			}
			catch (Exception e)
			{
				ret["error"] = "Unknown exception: " + e.Message;
			}

			return ret;
		}
	}
}