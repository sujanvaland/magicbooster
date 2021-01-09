using RestSharp;
using SmartStore.Core.Domain.Blockchain;
using SmartStore.Services.Customers;
using SmartStore.Services.Tasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Boards
{
	class CheckBlockChainTransactionStatus : ITask
	{
		private readonly ICustomerService _customerService;
		private readonly IBoardService _boardService;
		private readonly ICommonServices _services;
		public CheckBlockChainTransactionStatus(ICustomerService customerService,
			IBoardService boardService,
			ICommonServices services)
		{
			this._customerService = customerService;
			this._boardService = boardService;
			this._services = services;
		}

		/// <summary>
		/// Executes a task
		/// </summary>
		public void Execute(TaskExecutionContext ctx)
		{
			var pendingPaymentApproval = _boardService.GetAllPaymentToApprove();
			foreach (var payment in pendingPaymentApproval)
			{
				if (payment.Remarks.HasValue())
				{
					var client = new RestClient("https://chain.so/api/v2/");
					var request = new RestRequest("is_tx_confirmed/BTC/" + payment.Remarks, Method.GET);
					var amount = ConfigurationManager.AppSettings["matrixamount"].ToString();
					IRestResponse<TransactionConfirmation.RootObject> response = client.Execute<TransactionConfirmation.RootObject>(request);
					if (response.Data.data.is_confirmed)
					{
						request = new RestRequest("get_tx_outputs/BTC/" + payment.Remarks, Method.GET);
						response = client.Execute<TransactionConfirmation.RootObject>(request);
						var txtreceived = response.Data.data.outputs.Where(x => x.value == amount && x.address.ToLower() == payment.BitcoinAddress.ToLower()).FirstOrDefault();
						if(txtreceived != null)
						{
							payment.Status = "approved";
							_boardService.UpdateCustomerPayment(payment);
							var customerpayments = _boardService.GetCustomerApprovedPaymentCount(payment.PayToCustomerId);
							if (customerpayments.Count == 12)
							{
								foreach (var item in customerpayments)
								{
									item.Deleted = true;
									_boardService.UpdateCustomerPayment(item);
								}
								var position = _boardService.GetAllPosition(1, payment.PayToCustomerId, false, 0, int.MaxValue).FirstOrDefault();
								position.IsCycled = true;
								position.CycledDate = DateTime.Now;
								_boardService.UpdateCustomerPosition(position);
							}
						}
					}
					else
					{
						payment.Remarks = "";
						payment.Status = "pending";
						_boardService.UpdateCustomerPayment(payment);
					}
					
				}
			}

			var customers = _customerService.GetAllCustomers(
					null, // registrationFrom
					null, // registrationTo
					null, // roleIds
					"",
					"",
					"",
					"",
					0,
					0,
					"",
					"",
					"",
					false, // loadOnlyWithShoppingCart
					null, // shoppingCartType
					1,
					int.MaxValue,
					false);

			foreach(var cust in customers)
			{
				if(DateTime.Now > cust.CreatedOnUtc.ToLocalTime().AddDays(3))
				{
					_customerService.DeleteCustomerAccount(cust.Id);
				}
			}
		}
	}
}
