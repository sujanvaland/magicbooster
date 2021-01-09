using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartStore.Admin.Models.Investment;
using SmartStore.Services.Helpers;
using SmartStore.Services.Hyip;
using SmartStore.Services.Localization;
using SmartStore.Services.Media;
using SmartStore.Services.Security;
using SmartStore.Services.Seo;
using SmartStore.Services.Stores;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Modelling;
using SmartStore.Web.Framework.Security;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using SmartStore.Admin.Models.PaymentMethods;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core;
using SmartStore.Services;
using SmartStore.Core.Domain.Customers;
using SmartStore.Services.Customers;
using SmartStore.Services.Common;
using SmartStore.Core.Domain.Localization;
using SmartStore.Services.Boards;
using System.Security.Cryptography;
using System.Text;

namespace SmartStore.Admin.Controllers
{
	[AdminAuthorize]
	public class InvestmentController : AdminControllerBase
	{
		private readonly ICommonServices _commonServices;
		private readonly ICustomerService _customerService;
		private readonly IPlanService _planService;
		private readonly ICustomerPlanService _customerPlanService;
		private readonly ITransactionService _transactionService;
		private readonly IDateTimeHelper _dateTimeHelper;
		private readonly IWorkContext _workContext;
		private readonly ICommonServices _services;
		private readonly ILocalizationService _localizationService;
		private readonly LocalizationSettings _localizationSettings;
		private readonly IBoardService _boardService;
		private readonly IStoreContext _storeContext;
		public InvestmentController(ICommonServices commonServices,
			ICustomerService customerService,
			IPlanService planService,
			ICustomerPlanService customerPlanService,
			IDateTimeHelper dateTimeHelper,
			IWorkContext workContext,
			ITransactionService transactionService,
			ICommonServices services,
			ILocalizationService localizationService,
			LocalizationSettings localizationSettings,
			IBoardService boardService,
			IStoreContext storeContext)
		{
			_commonServices = commonServices;
			_customerService = customerService;
			_planService = planService;
			_customerPlanService = customerPlanService;
			_dateTimeHelper = dateTimeHelper;
			_workContext = workContext;
			_transactionService = transactionService;
			_services = services;
			_localizationService = localizationService;
			_localizationSettings = localizationSettings;
			_boardService = boardService;
			_storeContext = storeContext;
		}

		public void PrepareCustomerPlanModel(CustomerPlanModel model)
		{
			var plans = _planService.GetAllPlans().OrderBy(x => x.Name);
			foreach (var plan in plans)
			{
				model.AvailablePlans.Add(new SelectListItem()
				{
					Text = plan.Name + "(" + plan.PlanDetails + ")",
					Value = plan.Id.ToString()
				});
			}

			var storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);

			var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(storeScope);

			var SolitTrustPaySettings = _services.Settings.LoadSetting<SolidTrustPaySettings>(storeScope);

			var PayzaSettings = _services.Settings.LoadSetting<PayzaSettings>(storeScope);

			var PMSettings = _services.Settings.LoadSetting<PMSettings>(storeScope);

			var PayeerSettings = _services.Settings.LoadSetting<PayeerSettings>(storeScope);

			//if (coinpaymentSettings.CP_IsActivePaymentMethod)
			//{
			//	model.AvailableProcessor.Add(new SelectListItem()
			//	{
			//		Text = "Bitcoin",
			//		Value = "0"
			//	});
			//}
			//if (PayzaSettings.PZ_IsActivePaymentMethod)
			//{
			//	model.AvailableProcessor.Add(new SelectListItem()
			//	{
			//		Text = "Payza",
			//		Value = "1"
			//	});
			//}
			//if (PMSettings.PM_IsActivePaymentMethod)
			//{
			//	model.AvailableProcessor.Add(new SelectListItem()
			//	{
			//		Text = "PM",
			//		Value = "2"
			//	});
			//}
			//if (PayeerSettings.PY_IsActivePaymentMethod)
			//{
			//	model.AvailableProcessor.Add(new SelectListItem()
			//	{
			//		Text = "Payeer",
			//		Value = "3"
			//	});
			//}
			//if (SolitTrustPaySettings.STP_IsActivePaymentMethod)
			//{
			//	model.AvailableProcessor.Add(new SelectListItem()
			//	{
			//		Text = "SolidTrustPay",
			//		Value = "4"
			//	});
			//}

			model.AvailableProcessor.Add(new SelectListItem()
			{
				Text = "Available Balance",
				Value = "5"
			});

		}

		// GET: Investment
		public ActionResult BuyPosition()
		{
			var Status = _workContext.CurrentCustomer.Transaction.Where(x => x.StatusId == 2 && x.TranscationNote == "Membership").Sum(x => x.Amount) > 0 ? "Active" : "Inactive";
			var AllowPurchase = System.Configuration.ConfigurationManager.AppSettings["AllowPurchase"].ToSafe();
			if (AllowPurchase == "false")
			{
				NotifyInfo("Purchase is disabled now");
				return RedirectToAction("Index", "Home");
			}
			CustomerPlanModel model = new CustomerPlanModel();
			ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			model.NoOfPosition = 1;
			model.AvailableProcessor.Add(new SelectListItem()
			{
				Text = "Available Balance",
				Value = "5"
			});
			var boards = _boardService.GetAllBoards().OrderBy(x => x.Id);
			foreach (var plan in boards)
			{
				model.AvailablePlans.Add(new SelectListItem()
				{
					Text = plan.Name + " ($" + plan.Price + ")",
					Value = plan.Id.ToString()
				});
			}
			model.AvailableBalance = _customerService.GetAvailableBalance(_workContext.CurrentCustomer.Id);// _customerService.GetRepurchaseBalance(_workContext.CurrentCustomer.Id);
			return View("Deposit", model);
		}
		// GET: Investment
		public ActionResult Deposit()
		{
			var Status = _workContext.CurrentCustomer.Transaction.Where(x => x.StatusId == 2 && x.TranscationNote == "Membership").Sum(x => x.Amount) > 0 ? "Active" : "Inactive";
			var AllowPurchase = System.Configuration.ConfigurationManager.AppSettings["AllowPurchase"].ToSafe();
			if (AllowPurchase == "false")
			{
				NotifyInfo("Purchase is disabled now");
				return RedirectToAction("Index", "Home");
			}
			CustomerPlanModel model = new CustomerPlanModel();
			ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			model.NoOfPosition = 1;
			PrepareCustomerPlanModel(model);
			model.AvailableBalance = _customerService.GetAvailableBalance(_workContext.CurrentCustomer.Id);// _customerService.GetRepurchaseBalance(_workContext.CurrentCustomer.Id);
			return View(model);
		}

		private bool CheckIfPositionExists(int planid)
		{
			//if (_workContext.CurrentCustomer.CustomerPosition.Count() > 0)
			//{
			return true;
			//}
			//return false;
		}
		[HttpPost]
		public ActionResult Deposit(CustomerPlanModel customerPlanModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
					PrepareCustomerPlanModel(customerPlanModel);
					customerPlanModel.AvailableBalance = _customerService.GetAvailableBalance(_workContext.CurrentCustomer.Id);// _customerService.GetRepurchaseBalance(_workContext.CurrentCustomer.Id);

					if (!CheckIfPositionExists(customerPlanModel.PlanId))
					{
						NotifyError("You require same level board position to buy this Ad Pack");
						return RedirectToAction("Deposit");
					}
					//if (customerPlanModel.PlanId != 30)
					//{
					//	var noOfPack = _workContext.CurrentCustomer.Transaction.Where(x => x.RefId == customerPlanModel.PlanId - 1 && x.TranscationTypeId == 2 && x.StatusId == 2).Sum(x => x.NoOfPosition);
					//	if (noOfPack < 10)
					//	{
					//		NotifyError("You require minimum 10 previous Ad Pack");
					//		return View(customerPlanModel);
					//	}
					//}

					if (customerPlanModel.PlanId > 0)
					{
						var plan = _planService.GetPlanById(customerPlanModel.PlanId);
						var repurchasebalance = _customerService.GetAvailableBalance(_workContext.CurrentCustomer.Id);
						var amountreq = customerPlanModel.NoOfPosition;
						if (amountreq >= plan.MinimumInvestment && amountreq <= plan.MaximumInvestment)
						{
							if (customerPlanModel.ProcessorId == 5)
							{
								if (repurchasebalance < amountreq)
								{
									NotifyError("You do not have enough balance");
									ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
									PrepareCustomerPlanModel(customerPlanModel);
									customerPlanModel.AvailableBalance = _customerService.GetAvailableBalance(_workContext.CurrentCustomer.Id);// _customerService.GetRepurchaseBalance(_workContext.CurrentCustomer.Id);
									return RedirectToAction("Deposit");
								}
							}
							if (amountreq <= 0)
							{
								NotifyError("Enter correct amount");
								return RedirectToAction("Deposit");
							}
							TransactionModel transactionModel = new TransactionModel();
							transactionModel.Amount = amountreq;
							transactionModel.CustomerId = _workContext.CurrentCustomer.Id;
							transactionModel.FinalAmount = transactionModel.Amount;
							transactionModel.NoOfPosition = 0;// customerPlanModel.NoOfPosition;
							transactionModel.TransactionDate = DateTime.Now;
							transactionModel.RefId = plan.Id;
							transactionModel.ProcessorId = customerPlanModel.ProcessorId;
							transactionModel.TranscationTypeId = (int)TransactionType.Purchase;
							var transcation = transactionModel.ToEntity();

							transcation.NoOfPosition = customerPlanModel.NoOfPosition;
							if (customerPlanModel.ProcessorId == 5)
							{
								transcation.TranscationTypeId = (int)TransactionType.Purchase;
								transcation.StatusId = (int)Status.Completed;
							}
							else
							{
								transcation.StatusId = (int)Status.Pending;
								transcation.TranscationTypeId = (int)TransactionType.Purchase;
							}
							_transactionService.InsertTransaction(transcation);

							//for (int i = 0; i < transcation.NoOfPosition; i++)
							//{
							var customerplan = new CustomerPlan();
							customerplan.CustomerId = transcation.CustomerId;
							customerplan.PurchaseDate = DateTime.Now;
							customerplan.CreatedOnUtc = DateTime.Now;
							customerplan.UpdatedOnUtc = DateTime.Now;
							customerplan.PlanId = plan.Id;
							customerplan.AmountInvested = amountreq;
							customerplan.ROIToPay = (amountreq * plan.ROIPercentage) / 100 * plan.NoOfPayouts;
							customerplan.NoOfPayout = plan.NoOfPayouts;
							customerplan.ExpiredDate = DateTime.Today;
							customerplan.IsActive = true;
							if (plan.StartROIAfterHours > 0)
								customerplan.LastPaidDate = DateTime.Today.AddHours(plan.StartROIAfterHours);
							else
								customerplan.LastPaidDate = DateTime.Today;
							_customerPlanService.InsertCustomerPlan(customerplan);
							//}
							if (customerPlanModel.ProcessorId == 5)
							{
								NotifySuccess("Your purchase was successfull");
								ReleaseLevelCommission(plan.Id, _workContext.CurrentCustomer.Id, transactionModel.Amount);
							}
							else
							{
								int value = customerPlanModel.ProcessorId;
								PaymentMethod paymentmethod = (PaymentMethod)value;

								ViewBag.SaveSuccess = true;
								customerPlanModel.Id = plan.Id;
								customerPlanModel.PlanName = plan.Name;
								customerPlanModel.ProcessorName = paymentmethod.ToString();
								customerPlanModel.PaymentMethod = paymentmethod;
								customerPlanModel.TransactionId = transcation.Id;
								customerPlanModel.AmountInvested = (decimal)transactionModel.Amount;
								return RedirectToAction("ConfirmPayment", customerPlanModel);
							}
						}
						else
						{
							RedirectToAction("Deposit");
							NotifyError("Minimum investment for this package is $" + plan.MinimumInvestment + " and Maximum amount is $" + plan.MaximumInvestment);
						}
					}
					else
					{
						RedirectToAction("Deposit");
						NotifyError("Please select Package");
					}
				}
			}
			catch (Exception ex)
			{
				ViewBag.SaveSuccess = false;
				RedirectToAction("Deposit");
				NotifyError(T("Invesment.Deposit.FundingError"));
			}
			ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			customerPlanModel.AvailableBalance = _customerService.GetAvailableBalance(_workContext.CurrentCustomer.Id);// _customerService.GetRepurchaseBalance(_workContext.CurrentCustomer.Id);
			PrepareCustomerPlanModel(customerPlanModel);
			return View(customerPlanModel);
		}

		[HttpPost]
		public ActionResult BuyPosition(CustomerPlanModel customerPlanModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (customerPlanModel.PlanId > 0)
					{
						var CustomerPosition = _boardService.CheckTodayPositionExisit(_workContext.CurrentCustomer.Id);
						if (CustomerPosition != null)
						{
							NotifyError("Only 1 Position Purchase allowed per day");
							return RedirectToAction("BuyPosition");
						}
						if (customerPlanModel.NoOfPosition > 1)
						{
							NotifyError("Only 1 Position Purchase allowed per day");
							return RedirectToAction("BuyPosition");
						}
						var plan = _boardService.GetBoardById(customerPlanModel.PlanId);
						var repurchasebalance = _customerService.GetAvailableBalance(_workContext.CurrentCustomer.Id);
						var amountreq = Convert.ToInt64(plan.Price) * ((customerPlanModel.NoOfPosition == 0) ? 1 : customerPlanModel.NoOfPosition);
						//if (customerPlanModel.ProcessorId == 5)
						//{
						if (repurchasebalance < amountreq)
						{
							NotifyError("You do not have enough balance");
							return RedirectToAction("BuyPosition");
						}
						//}
						if (customerPlanModel.NoOfPosition <= 0)
						{
							NotifyError("Enter correct amount");
							return RedirectToAction("BuyPosition");
						}

						for (int i = 0; i < customerPlanModel.NoOfPosition; i++)
						{
							ReleaseLevelCommission(_workContext.CurrentCustomer, plan.Id);

							//if (plan.Id == 1)
							//{
							//	var plann = _planService.GetPlanById(6);
							//	var customerplan = new CustomerPlan();
							//	customerplan.CustomerId = _workContext.CurrentCustomer.Id;
							//	customerplan.PurchaseDate = DateTime.Now;
							//	customerplan.CreatedOnUtc = DateTime.Now;
							//	customerplan.UpdatedOnUtc = DateTime.Now;
							//	customerplan.PlanId = plan.Id;
							//	customerplan.AmountInvested = 20;
							//	customerplan.ROIToPay = (20 * plann.ROIPercentage) / 100 * plann.NoOfPayouts;
							//	customerplan.NoOfPayout = plann.NoOfPayouts;
							//	customerplan.ExpiredDate = DateTime.Today;
							//	customerplan.IsActive = true;
							//	customerplan.LastPaidDate = DateTime.Today;
							//	_customerPlanService.InsertCustomerPlan(customerplan);
							//}
						}
						TransactionModel transactionModel = new TransactionModel();
						transactionModel.Amount = amountreq;
						transactionModel.CustomerId = _workContext.CurrentCustomer.Id;
						transactionModel.FinalAmount = transactionModel.Amount;
						transactionModel.NoOfPosition = customerPlanModel.NoOfPosition;
						transactionModel.TransactionDate = DateTime.Now;
						transactionModel.RefId = plan.Id;
						transactionModel.ProcessorId = customerPlanModel.ProcessorId;
						transactionModel.TranscationTypeId = (int)TransactionType.Purchase;
						var transcation = transactionModel.ToEntity();
						transcation.NoOfPosition = customerPlanModel.NoOfPosition;
						if (customerPlanModel.ProcessorId == 5)
						{
							transcation.TranscationTypeId = (int)TransactionType.Purchase;
							transcation.StatusId = (int)Status.Completed;
						}
						else
						{
							transcation.StatusId = (int)Status.Pending;
							transcation.TranscationTypeId = (int)TransactionType.Purchase;
						}
						_transactionService.InsertTransaction(transcation);

						NotifySuccess("Your purchase was successfull");
						return RedirectToAction("BuyPosition");
					}
					else
					{
						NotifyError("Please select Board");
					}
				}
			}
			catch (Exception ex)
			{
				ViewBag.SaveSuccess = false;
				NotifyError(T("Invesment.Deposit.FundingError"));
			}

			return RedirectToAction("BuyPosition");
		}

		public ActionResult ConfirmPayment(CustomerPlanModel customerPlanModel)
		{
			return View(customerPlanModel);
		}

		public ActionResult CheckOut(CustomerPlanModel customerPlanModel)
		{
			var storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);

			if (customerPlanModel.PaymentMethod == PaymentMethod.CoinPayment)
			{
				var coinpaymentmodel = PrepareCoinPaymentModel(customerPlanModel);
				ViewBag.StoreUrl = _commonServices.StoreContext.CurrentStore.Url;
				return View("_CoinPayment", coinpaymentmodel);
			}
			if (customerPlanModel.PaymentMethod == PaymentMethod.SolidTrustPay)
			{
				var STPSettings = _services.Settings.LoadSetting<SolidTrustPaySettings>(storeScope);

				ViewBag.AccountNo = STPSettings.STP_Sci_Name;
				ViewBag.MerchantName = STPSettings.STP_MerchantAccount;
				ViewBag.TransactionId = customerPlanModel.TransactionId;
				ViewBag.IPNUrl = STPSettings.STP_NotifyUrl;
				ViewBag.ReturnUrl = STPSettings.STP_CancelUrl;
				ViewBag.Amount = String.Format("{0:0.00}", customerPlanModel.AmountInvested);
				ViewBag.CustomerId = _workContext.CurrentCustomer.Id;
				ViewBag.FinalAmount = customerPlanModel.AmountInvested + ((customerPlanModel.AmountInvested * STPSettings.DepositFees) / 100);
				ViewBag.CustomerPlanId = customerPlanModel.Id;
				ViewBag.PlanName = customerPlanModel.PlanName;
				ViewBag.ProcessorName = customerPlanModel.ProcessorName;
				ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
				ViewBag.PaymentMemo = T("Hyip.PaymentMemo");
				return View("_STPPayment");
			}
			if (customerPlanModel.PaymentMethod == PaymentMethod.Payza)
			{
				var coinpaymentmodel = PrepareCoinPaymentModel(customerPlanModel);
				return View("_CoinPayment", coinpaymentmodel);
			}
			if (customerPlanModel.PaymentMethod == PaymentMethod.PM)
			{
				var PMSettings = _services.Settings.LoadSetting<PMSettings>(storeScope);

				ViewBag.AccountNo = PMSettings.PM_PayeeAccount;
				ViewBag.TransactionId = customerPlanModel.TransactionId;
				ViewBag.IPNUrl = PMSettings.PM_NotifyUrl;
				ViewBag.ReturnUrl = PMSettings.PM_ReturnUrl;
				ViewBag.Amount = String.Format("{0:0.00}", customerPlanModel.AmountInvested);
				ViewBag.CustomerId = customerPlanModel.CustomerId;
				ViewBag.FinalAmount = customerPlanModel.AmountInvested + ((customerPlanModel.AmountInvested * PMSettings.DepositFees) / 100);
				ViewBag.CustomerPlanId = customerPlanModel.Id;
				ViewBag.PlanName = customerPlanModel.PlanName;
				ViewBag.ProcessorName = customerPlanModel.ProcessorName;
				ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
				ViewBag.StoreName = "MyTrafficHub";
				ViewBag.PaymentMemo = T("Hyip.PaymentMemo");
				return View("_PMPayment");
			}
			if (customerPlanModel.PaymentMethod == PaymentMethod.Payeer)
			{
				var payeerpaymentSettings = _services.Settings.LoadSetting<PayeerSettings>(storeScope);

				string hashkey = payeerpaymentSettings.PY_MerchantShop + ":" + customerPlanModel.TransactionId + ":" + String.Format("{0:0.00}", customerPlanModel.AmountInvested) + ":USD:membership:" + customerPlanModel.CustomerId.ToString() + ":" + payeerpaymentSettings.PY_SecretKey;
				hashkey = GetSha256FromString(hashkey).ToUpper();
				ViewBag.AccountNo = payeerpaymentSettings.PY_MerchantShop;
				ViewBag.TransactionId = customerPlanModel.TransactionId;
				ViewBag.Amount = String.Format("{0:0.00}", customerPlanModel.AmountInvested);
				ViewBag.CustomerId = customerPlanModel.CustomerId;
				ViewBag.FinalAmount = customerPlanModel.AmountInvested + ((customerPlanModel.AmountInvested * payeerpaymentSettings.DepositFees) / 100);
				ViewBag.CustomerPlanId = customerPlanModel.Id;
				ViewBag.PlanName = customerPlanModel.PlanName;
				ViewBag.ProcessorName = customerPlanModel.ProcessorName;
				ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
				ViewBag.PaymentMemo = T("Hyip.PaymentMemo");
				ViewBag.Hash = hashkey;
				return View("_PayeerPayment");
			}
			if (customerPlanModel.PaymentMethod == PaymentMethod.BankTransfer)
			{
				ViewBag.TransactionId = customerPlanModel.TransactionId;
				ViewBag.CustomerId = customerPlanModel.CustomerId;
				ViewBag.FinalAmount = customerPlanModel.AmountInvested;
				ViewBag.CustomerPlanId = customerPlanModel.Id;
				ViewBag.PlanName = customerPlanModel.PlanName;
				ViewBag.ProcessorName = customerPlanModel.ProcessorName;
				ViewBag.Email = _workContext.CurrentCustomer.Email;
				ViewBag.Amount = customerPlanModel.AmountInvested;
				//Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_live_Ji5gawZglbhMPN", "0zt8756zIcQzySRP74Wyt0Mo");
				//var dict = new Dictionary<string, object>();
				//dict.Add("amount", customerPlanModel.AmountInvested*100);
				//dict.Add("currency", "INR");
				//dict.Add("payment_capture", 1);
				//var RazorOrder = client.Order.Create(dict);
				//ViewBag.OrderId = RazorOrder["id"].ToString();
				return View("_BankTransfer");
			}
			return View();
		}

		private CoinPaymentModel PrepareCoinPaymentModel(CustomerPlanModel customerPlanModel)
		{
			var storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);

			var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(storeScope);

			CoinPaymentModel coinPaymentModel = new CoinPaymentModel();
			coinPaymentModel.MerchantAcc = coinpaymentSettings.CP_MerchantId;
			coinPaymentModel.Amount = customerPlanModel.AmountInvested;
			coinPaymentModel.FinalAmount = customerPlanModel.AmountInvested + ((customerPlanModel.AmountInvested * coinpaymentSettings.DepositFees) / 100);
			coinPaymentModel.CustomerPlanId = customerPlanModel.Id;
			coinPaymentModel.PlanName = customerPlanModel.PlanName;
			coinPaymentModel.ProcessorName = customerPlanModel.ProcessorName;
			coinPaymentModel.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			coinPaymentModel.PaymentMemo = T("Hyip.PaymentMemo");
			coinPaymentModel.DepositFees = coinpaymentSettings.DepositFees;
			coinPaymentModel.TransactionId = customerPlanModel.TransactionId;
			return coinPaymentModel;
		}



		public ActionResult Withdrawal()
		{
			TransactionModel model = new TransactionModel();
			var storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);

			var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(storeScope);

			var SolitTrustPaySettings = _services.Settings.LoadSetting<SolidTrustPaySettings>(storeScope);

			var PayzaSettings = _services.Settings.LoadSetting<PayzaSettings>(storeScope);

			var PMSettings = _services.Settings.LoadSetting<PMSettings>(storeScope);

			var PayeerSettings = _services.Settings.LoadSetting<PayeerSettings>(storeScope);

			var WithdrawalSettings = _services.Settings.LoadSetting<WithdrawalSettings>(storeScope);

			if (coinpaymentSettings.CP_IsActivePaymentMethod)
			{
				model.CoinPaymentEnabled = true;
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Bitcoin",
					Value = "0"
				});
			}
			if (PayzaSettings.PZ_IsActivePaymentMethod)
			{
				model.PayzaEnabled = true;
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Payza",
					Value = "1"
				});
			}
			if (PMSettings.PM_IsActivePaymentMethod)
			{
				model.PMEnabled = true;
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "PM",
					Value = "2"
				});
			}
			if (PayeerSettings.PY_IsActivePaymentMethod)
			{
				model.PayzaEnabled = true;
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Payeer",
					Value = "3"
				});
			}
			if (SolitTrustPaySettings.STP_IsActivePaymentMethod)
			{
				model.STPEnabled = true;
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "SolidTrustPay",
					Value = "4"
				});
			}

			var customer = _customerService.GetCustomerById(_workContext.CurrentCustomer.Id);
			ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			model.BitcoinAddress = customer.GetAttribute<string>(SystemCustomerAttributeNames.BitcoinAddressAcc);
			model.BankName = customer.GetAttribute<string>(SystemCustomerAttributeNames.BankName);
			model.AccountHolderName = customer.GetAttribute<string>(SystemCustomerAttributeNames.AccountHolderName);
			model.AccountNumber = customer.GetAttribute<string>(SystemCustomerAttributeNames.AccountNumber);
			model.NICR = customer.GetAttribute<string>(SystemCustomerAttributeNames.NICR);
			model.PayzaAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.PayzaAcc);
			model.SolidTrustPayAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.SolidTrustPayAcc);
			model.PayeerAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.PayeerAcc);
			model.PMAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.PMAcc);
			model.AdvanceCashAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.AdvanceCashAcc);
			model.WithdrawalFees = WithdrawalSettings.WithdrawalFees;
			model.AvailableBalance = _customerService.GetAvailableBalance(customer.Id);
			model.PendingWithdrawal = _customerService.GetCustomerPendingWithdrawal(customer.Id);
			model.CompletedWithdrawal = _customerService.GetCustomerCompletedWithdrawal(customer.Id);

			return View(model);
		}

		[HttpPost]
		public ActionResult Withdrawal(TransactionModel model)
		{
			try
			{
				ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
				var storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);
				var StatusIds = new int[] { (int)Status.Pending, (int)Status.Inprogress, (int)Status.Completed };
				var TranscationTypeIds = new int[] { (int)TransactionType.Withdrawal };

				var transcations = _transactionService.GetTodaysWithdrawal(_workContext.CurrentCustomer.Id);
				model.AvailableBalance = _customerService.GetAvailableBalance(_workContext.CurrentCustomer.Id);
				var withdrawalSettings = _services.Settings.LoadSetting<WithdrawalSettings>(storeScope);
				var directcount = _customerService.GetCustomerDirectReferral(_workContext.CurrentCustomer.Id).ToList();
				int activecount = 0;
				foreach (var c in directcount)
				{
					var isactive = c.CustomerPosition.Count() > 0 ? true : false;
					if (isactive)
					{
						activecount = activecount + 1;
					}
					if (activecount >= 2)
					{
						break;
					}
				}

				var substrans = _workContext.CurrentCustomer.Transaction.Where(x => x.StatusId == 2 && x.TranscationNote == "subscription").FirstOrDefault();
				if (substrans != null)
				{
					var noOfDays = substrans.NoOfPosition * 30;
					var SubscriptionEndDate = substrans.CreatedOnUtc.AddDays(noOfDays);
					if (SubscriptionEndDate < DateTime.Today)
					{
						NotifyError("Please activate your Subscription");
						return RedirectToAction("SubscriptionActive", "ActiveSubscription");
					}
				}
				else
				{
					NotifyError("Please activate your Subscription");
					return RedirectToAction("SubscriptionActive", "ActiveSubscription");
				}
				if (model.Amount <= 0)
				{
					NotifyError("Enter correct amount");
					return RedirectToAction("Withdrawal");
				}
				if (model.AvailableBalance < model.Amount)
				{
					NotifyError(T("Investment.Withdrawal.InsufficentBalance"));
					return RedirectToAction("Withdrawal");
				}
				if (transcations.Count >= withdrawalSettings.MaxRequestPerDay)
				{
					NotifyError(T("Investment.Withdrawal.MaxWithdrawalPerDayError", withdrawalSettings.MaxRequestPerDay));
					return RedirectToAction("Withdrawal");
				}
				//if (!withdrawalSettings.AllowAutoWithdrawal)
				//{
				//	NotifyError(T("Investment.Withdrawal.DisabledError"));
				//	return RedirectToAction("Withdrawal");
				//}
				if (!(model.Amount >= withdrawalSettings.MinWithdrawal && model.Amount <= withdrawalSettings.MaxWithdrawal))
				{
					NotifyError("Minimum withdrawal limit " + _workContext.WorkingCurrency.CurrencyCode + " " + withdrawalSettings.MinWithdrawal + ", Maximum withdrawal limit " + _workContext.WorkingCurrency.CurrencyCode + " " + withdrawalSettings.MaxWithdrawal);
					return RedirectToAction("Withdrawal");
				}

				if (ModelState.IsValid)
				{
					if (true)
					{
						try
						{
							model.CustomerId = _workContext.CurrentCustomer.Id;
							model.FinalAmount = model.Amount;
							model.TransactionDate = DateTime.Now;

							model.RefId = 0;
							model.TranscationTypeId = (int)TransactionType.Withdrawal;

							var customer = _customerService.GetCustomerById(model.CustomerId);
							model.BitcoinAddress = customer.GetAttribute<string>(SystemCustomerAttributeNames.BitcoinAddressAcc);

							model.BankName = customer.GetAttribute<string>(SystemCustomerAttributeNames.BankName);
							model.AccountHolderName = customer.GetAttribute<string>(SystemCustomerAttributeNames.AccountHolderName);
							model.AccountNumber = customer.GetAttribute<string>(SystemCustomerAttributeNames.AccountNumber);
							model.NICR = customer.GetAttribute<string>(SystemCustomerAttributeNames.NICR);

							model.PayzaAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.PayzaAcc);
							model.SolidTrustPayAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.SolidTrustPayAcc);
							model.PayeerAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.PayeerAcc);
							model.PMAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.PMAcc);
							model.AdvanceCashAcc = customer.GetAttribute<string>(SystemCustomerAttributeNames.AdvanceCashAcc);
							if (model.ProcessorId == (int)PaymentMethod.CoinPayment)
							{
								model.WithdrawalAccount = model.BitcoinAddress;
							}
							if (model.ProcessorId == (int)PaymentMethod.Payza)
							{
								model.WithdrawalAccount = model.PayzaAcc;
							}
							if (model.ProcessorId == (int)PaymentMethod.SolidTrustPay)
							{
								model.WithdrawalAccount = model.SolidTrustPayAcc;
							}
							if (model.ProcessorId == (int)PaymentMethod.Payeer)
							{
								model.WithdrawalAccount = model.PayeerAcc;
							}
							if (model.ProcessorId == (int)PaymentMethod.PM)
							{
								model.WithdrawalAccount = model.PMAcc;
							}

							if (model.WithdrawalAccount.IsEmpty())
							{
								NotifyError("Please update your Withdrawal account in Profile page");
								return RedirectToAction("Withdrawal");
							}
							var transcation = model.ToEntity();
							transcation.ProcessorId = model.ProcessorId;
							transcation.TranscationTypeId = model.TranscationTypeId;
							transcation.StatusId = (int)Status.Pending;
							transcation.WithdrawalAddress = model.WithdrawalAccount;
							_transactionService.InsertTransaction(transcation);

							//if (model.ProcessorId == (int)PaymentMethod.CoinPayment)
							//{
							//	var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(storeScope);

							//	CoinPayments coinPayments = new CoinPayments(coinpaymentSettings.CP_ApiSecretKey, coinpaymentSettings.CP_PublicKey);
							//	var param = new SortedList<string, string>();
							//	param.Add("amount", model.Amount.ToString());
							//	param.Add("add_tx_fee", "1");
							//	param.Add("currency", "BTC");
							//	param.Add("currency2", "USD");
							//	param.Add("address", model.BitcoinAddress);
							//	if (coinpaymentSettings.CP_EmailConfirmationRequired)
							//		param.Add("auto_confirm", "1");
							//	param.Add("note", withdrawalSettings.PaymentNote);
							//	var result = coinPayments.CallAPI("create_withdrawal", param);
							//	if(result["error"] == "ok")
							//	{
							//		var res = result["result"];
							//		if (res["status"] == 0 || res["status"] == 1)
							//		{
							//			if(res["status"] == 1)
							//			{
							//				transcation.StatusId = (int)Status.Completed;
							//			}
							//			else
							//			{
							//				transcation.StatusId = (int)Status.Pending;
							//			}
							//			transcation.TranscationNote = res["id"];
							//			_transactionService.UpdateTransaction(transcation);
							//		}
							//	}
							//}

							// Notifications
							if (withdrawalSettings.NotifyWithdrawalRequestToUser)
								Services.MessageFactory.SendWithdrawalNotificationMessageToUser(transcation, "", "", _localizationSettings.DefaultAdminLanguageId);
							if (withdrawalSettings.NotifyWithdrawalRequestToAdmin)
								Services.MessageFactory.SendWithdrawalNotificationMessageToAdmin(transcation, "", "", _localizationSettings.DefaultAdminLanguageId);

						}
						catch (Exception ex)
						{

						}

					}

					NotifySuccess(T("Investment.Withdrawal.Success"));
				}
			}
			catch (Exception exception)
			{
				NotifyError(T("Investment.Withdrawal.Error"));
				return RedirectToAction("Withdrawal");
			}
			return RedirectToAction("Withdrawal");
		}

		public ActionResult MyInvestment()
		{
			MyInvestmentPlan model = new MyInvestmentPlan();
			var customerplans = _workContext.CurrentCustomer.CustomerPlan.Where(x => x.IsActive == true).ToList();
			model.MyTotalInvestment = customerplans.Select(x => x.AmountInvested).Sum();
			model.MyTotalROIPaid = customerplans.Select(x => x.ROIPaid).Sum();
			model.MyTotalROIToPay = customerplans.Select(x => x.ROIToPay).Sum();
			model.MyTotalROIPending = model.MyTotalROIToPay - model.MyTotalROIPaid;
			ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			return View(model);
		}

		[HttpPost, GridAction(EnableCustomBinding = true)]
		public ActionResult ListMyInvestment(GridCommand command)
		{
			var gridModel = new GridModel<MyInvestmentPlan>();
			var customerplan = _workContext.CurrentCustomer.CustomerPlan.Where(x => x.IsActive == true).ToList();
			gridModel.Data = customerplan.Select(x =>
			{
				var myInvestment = new MyInvestmentPlan();
				myInvestment.PlanName = _planService.GetPlanById(x.PlanId).Name;
				myInvestment.PurchaseDate = x.PurchaseDate;
				myInvestment.ROIPaid = x.ROIPaid;
				myInvestment.ROIToPay = x.ROIToPay;
				myInvestment.ROIPending = (x.ROIToPay - x.ROIPaid);
				myInvestment.ROIPaidString = x.ROIPaid.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				myInvestment.ROIToPayString = x.ROIToPay.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				myInvestment.ROIPendingString = myInvestment.ROIPending.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				myInvestment.TotalFundingString = x.AmountInvested.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				myInvestment.IsActive = x.IsActive;
				myInvestment.Status = (x.IsActive) ? ((x.IsExpired == false) ? "Active" : "Expired") : "InActive";
				myInvestment.ExpireDate = x.ExpiredDate;
				return myInvestment;
			});

			gridModel.Total = customerplan.Count;

			return new JsonResult
			{
				Data = gridModel
			};
		}

		public void ReleaseLevelCommission(int planid, int customerid, float amountInvested)
		{
			Transaction transaction;
			//var oneUpCustomer = _customerService.SendPassUpBonus(customerid);
			Customer levelcustomer = _customerService.GetCustomerById(customerid);
			float commission = (amountInvested * 7) / 100;

			if (levelcustomer != null)
			{
				//Send Direct Bonus
				try
				{
					if (levelcustomer.Transaction.Where(x => x.StatusId == 2).Sum(x => x.Amount) > 0)
					{
						transaction = new Transaction();
						transaction.CustomerId = levelcustomer.AffiliateId;
						transaction.Amount = commission;
						transaction.FinalAmount = transaction.Amount;
						transaction.TransactionDate = DateTime.Now;
						transaction.StatusId = (int)Status.Completed;
						transaction.TranscationTypeId = (int)TransactionType.DirectBonus;
						_transactionService.InsertTransaction(transaction);
						Services.MessageFactory.SendDirectBonusNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);

					}
				}
				catch (Exception ex)
				{
					//WritetoLog("Direct Bonus error :" + ex.ToString());
				}

				levelcustomer = _customerService.GetCustomerById(levelcustomer.AffiliateId);
				commission = (amountInvested * 2) / 100;
				//Send Direct Bonus
				try
				{
					if (levelcustomer.Transaction.Where(x => x.StatusId == 2).Sum(x => x.Amount) > 0)
					{
						transaction = new Transaction();
						transaction.CustomerId = levelcustomer.AffiliateId;
						transaction.Amount = commission;
						transaction.FinalAmount = transaction.Amount;
						transaction.TransactionDate = DateTime.Now;
						transaction.StatusId = (int)Status.Completed;
						transaction.TranscationTypeId = (int)TransactionType.DirectBonus;
						_transactionService.InsertTransaction(transaction);
						Services.MessageFactory.SendDirectBonusNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);

					}
				}
				catch (Exception ex)
				{
					//WritetoLog("Direct Bonus error :" + ex.ToString());
				}

				levelcustomer = _customerService.GetCustomerById(levelcustomer.AffiliateId);
				commission = (amountInvested * 2) / 100;
				//Send Direct Bonus
				try
				{
					if (levelcustomer.Transaction.Where(x => x.StatusId == 2).Sum(x => x.Amount) > 0)
					{
						transaction = new Transaction();
						transaction.CustomerId = levelcustomer.AffiliateId;
						transaction.Amount = commission;
						transaction.FinalAmount = transaction.Amount;
						transaction.TransactionDate = DateTime.Now;
						transaction.StatusId = (int)Status.Completed;
						transaction.TranscationTypeId = (int)TransactionType.DirectBonus;
						_transactionService.InsertTransaction(transaction);
						Services.MessageFactory.SendDirectBonusNotificationMessageToUser(transaction, "", "", _localizationSettings.DefaultAdminLanguageId);

					}
				}
				catch (Exception ex)
				{
					//WritetoLog("Direct Bonus error :" + ex.ToString());
				}
			}
		}

		public void PurchasePosition(Customer customer, int boardid)
		{
			Transaction transaction;
			//Save board position
			try
			{
				_customerService.SaveCusomerPosition(customer.Id, boardid);
				Services.MessageFactory.SendUnilevelPositionPurchasedNotificationMessageToUser(customer, "", "", _localizationSettings.DefaultAdminLanguageId);
				var cycledpositionformail = _boardService.GetAllPositionForEmailNotification();
				if (cycledpositionformail.Count > 0)
				{
					foreach (var p in cycledpositionformail)
					{
						//WritetoLog("PostionCycled:" + p.Id);
						var cycledcustomer = _customerService.GetCustomerById(p.CustomerId);
						Services.MessageFactory.SendUnilevelMatrixCycledNotificationMessageToUser(p, "", "", _localizationSettings.DefaultAdminLanguageId);
						p.EmailSentOnCycle = true;
						p.IsCycled = true;
						_boardService.UpdateCustomerPosition(p);
						//transaction = new Transaction();
						//var board = _boardService.GetBoardById(p.BoardId);
						//if (board.Payout > 0)
						//{
						//	transaction.StatusId = (int)Status.Pending;
						//	transaction.CustomerId = p.CustomerId;
						//	transaction.Amount = (float)board.Payout;
						//	transaction.FinalAmount = transaction.Amount;
						//	transaction.TransactionDate = DateTime.Now;
						//	transaction.TranscationTypeId = (int)TransactionType.Withdrawal;
						//	transaction.TranscationNote = "Matrix Bonus for Level " + board.Id;
						//	_transactionService.InsertTransaction(transaction);
						//	//if (board.SponsorBonus > 0)
						//	//{
						//	//	var sponsor = _customerService.GetCustomerById(customer.AffiliateId);
						//	//	transaction.StatusId = (int)Status.Pending;
						//	//	transaction.CustomerId = sponsor.Id;
						//	//	transaction.Amount = (float)board.SponsorBonus;
						//	//	transaction.FinalAmount = transaction.Amount;
						//	//	transaction.TransactionDate = DateTime.Now;
						//	//	transaction.TranscationTypeId = (int)TransactionType.Withdrawal;
						//	//	transaction.TranscationNote = "9xBitcoin Cycler Sponsor Bonus for Level " + board.Id;
						//	//	_transactionService.InsertTransaction(transaction);
						//	//}
						//}
					}
				}
			}
			catch (Exception ex)
			{

			}
		}

		public string CalculateMD5Hash(string input)
		{
			// step 1, calculate MD5 hash from input
			MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}
		public static string GetSha256FromString(string strData)
		{
			System.Security.Cryptography.SHA256Managed hm = new System.Security.Cryptography.SHA256Managed();
			byte[] hashValue = hm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(strData));
			return System.BitConverter.ToString(hashValue).Replace("-", "").ToLower();
		}
		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		public void ApproveTransaction(int customerid, int boardid)
		{
			var customer = _customerService.GetCustomerById(customerid);
			ReleaseLevelCommission(customer, boardid);
		}

		public void ReleaseLevelCommission(Customer customer, int boardid)
		{
			//Save board position
			_customerService.SaveCusomerPosition(customer.Id, boardid);
			//Services.MessageFactory.SendUnilevelPositionPurchasedNotificationMessageToUser(customer, "", "", _localizationSettings.DefaultAdminLanguageId);
			var cycledpositionformail = _boardService.GetAllPositionForEmailNotification();
			//WritetoLog("PostionCycled Count:" + cycledpositionformail.Count);
			if (cycledpositionformail.Count > 0)
			{
				foreach (var p in cycledpositionformail)
				{
					//WritetoLog("PostionCycled:" + p.Id);
					var cycledcustomer = _customerService.GetCustomerById(p.CustomerId);
					Services.MessageFactory.SendUnilevelMatrixCycledNotificationMessageToUser(p, "", "", _localizationSettings.DefaultAdminLanguageId);
					p.EmailSentOnCycle = true;
					_boardService.UpdateCustomerPosition(p);
				}
			}
		}

	}
}