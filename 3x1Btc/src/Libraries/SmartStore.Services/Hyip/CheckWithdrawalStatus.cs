using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Domain.Localization;
using SmartStore.Services.Customers;
using SmartStore.Services.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmartStore.Services.Hyip
{
	class CheckWithdrawalStatusTask : ITask
	{
		private readonly ICustomerService _customerService;
		private readonly ITransactionService _transactionService;
		private readonly IPlanService _planService;
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ICommonServices _services;
		private readonly ICommonServices _commonService;
		private readonly LocalizationSettings _localizationSettings;
		public CheckWithdrawalStatusTask(ICustomerService customerService,
			ITransactionService transactionService,
			IPlanService planService,
			ICustomerPlanService customerPlanService,
			ICommonServices services,
			ICommonServices commonServices,
			LocalizationSettings localizationSettings)
		{
			this._customerService = customerService;
			this._transactionService = transactionService;
			this._planService = planService;
			this._customerPlanService = customerPlanService;
			this._services = services;
			this._commonService = commonServices;
			this._localizationSettings = localizationSettings;
		}

		/// <summary>
		/// Executes a task
		/// </summary>
		public void Execute(TaskExecutionContext ctx)
		{
			var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(0);
			int[] StatusIds = "1".ToIntArray();
			int[] TranscationTypeIds = { (int)TransactionType.Withdrawal };

			var pendingWithdrawals = _transactionService.GetAllTransactions(0, 0, null, null, StatusIds, TranscationTypeIds, 0, int.MaxValue);
			foreach(var transaction in pendingWithdrawals)
			{
				if(transaction.ProcessorId == (int)NewPaymentMethod.CoinPayment)
				{
					CoinPayments coinPayments = new CoinPayments(coinpaymentSettings.CP_ApiSecretKey, coinpaymentSettings.CP_PublicKey);
					var param = new SortedList<string, string>();
					param.Add("id", transaction.TranscationNote);
					var result = coinPayments.CallAPI("get_withdrawal_info", param);
					if (result["error"] == "ok")
					{
						var res = result["result"];
						if (res["status"] == 2)
						{
							transaction.StatusId = (int)Status.Completed;
							transaction.TranscationNote = res["send_txid"];
							transaction.UpdatedOnUtc = DateTime.Now;
							_transactionService.UpdateTransaction(transaction);

							_commonService.MessageFactory.SendWithdrawalCompletedNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);

						}
					}
				}
			}
		}
	}
	public enum NewPaymentMethod
	{
		CoinPayment,
		Payza,
		PM,
		Payeer,
		SolidTrustPay
	}
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
