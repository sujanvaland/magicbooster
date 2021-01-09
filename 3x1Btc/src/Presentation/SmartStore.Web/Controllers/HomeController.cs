using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Mvc;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Seo;
using SmartStore.Core.Email;
using SmartStore.Services;
using SmartStore.Services.Catalog;
using SmartStore.Services.Customers;
using SmartStore.Services.Localization;
using SmartStore.Services.Messages;
using SmartStore.Services.Search;
using SmartStore.Services.Seo;
using SmartStore.Services.Topics;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;
using SmartStore.Web.Models.Common;
using SmartStore.Web.Framework.Filters;
using SmartStore.Core.Domain.Tasks;
using SmartStore.Services.Tasks;
using SmartStore.Web.Models.Home;
using SmartStore.Services.Hyip;
using SmartStore.Core.Domain.Hyip;
using System.Linq;
using SmartStore.Core;
using System.Web;
using SmartStore.Admin.Models.Investment;
using SmartStore.Services.Boards;
using SmartStore.Admin.Models.Board;
using RestSharp;
using System.Security.Cryptography;

namespace SmartStore.Web.Controllers
{
    public partial class HomeController : PublicControllerBase
	{
		private readonly ICommonServices _services;
		private readonly Lazy<ICategoryService> _categoryService;
		private readonly Lazy<IProductService> _productService;
		private readonly Lazy<IManufacturerService> _manufacturerService;
		private readonly Lazy<ICatalogSearchService> _catalogSearchService;
		private readonly Lazy<CatalogHelper> _catalogHelper;
		private readonly Lazy<ITopicService> _topicService;
		private readonly Lazy<IXmlSitemapGenerator> _sitemapGenerator;
		private readonly Lazy<CaptchaSettings> _captchaSettings;
		private readonly Lazy<CommonSettings> _commonSettings;
		private readonly Lazy<SeoSettings> _seoSettings;
		private readonly Lazy<CustomerSettings> _customerSettings;
		private readonly Lazy<PrivacySettings> _privacySettings;
		private readonly IScheduleTaskService _scheduleTaskService;
		private readonly ITransactionService _transcationService;
		private readonly IWorkContext _workContext;
		private readonly ICustomerService _customerService;
		private readonly AdminAreaSettings _adminAreaSettings;
		private readonly IPlanService _planService;
		private readonly ILocalizationService _localizationService;
		private readonly IBoardService _boardService;
		private readonly IWebHelper _webHelper;
		
		public HomeController(
			IBoardService boardService,
			ILocalizationService localizationService,
			ICommonServices services,
			Lazy<ICategoryService> categoryService,
			Lazy<IProductService> productService,
			Lazy<IManufacturerService> manufacturerService,
			Lazy<ICatalogSearchService> catalogSearchService,
			Lazy<CatalogHelper> catalogHelper,
			Lazy<ITopicService> topicService,
			Lazy<IXmlSitemapGenerator> sitemapGenerator,
			Lazy<CaptchaSettings> captchaSettings,
			Lazy<CommonSettings> commonSettings,
			Lazy<SeoSettings> seoSettings,
			Lazy<CustomerSettings> customerSettings,
			Lazy<PrivacySettings> privacySettings,
			IScheduleTaskService scheduleTaskService,
			ITransactionService transactionService,
			IWorkContext workContext,
			ICustomerService customerService, AdminAreaSettings adminAreaSettings,
			IPlanService planService,
			IWebHelper webHelper)
        {
			this._boardService = boardService;
			this._localizationService = localizationService;
			this._services = services;
			this._categoryService = categoryService;
			this._productService = productService;
			this._manufacturerService = manufacturerService;
			this._catalogSearchService = catalogSearchService;
			this._catalogHelper = catalogHelper;
			this._topicService = topicService;
			this._sitemapGenerator = sitemapGenerator;
			this._captchaSettings = captchaSettings;
			this._commonSettings = commonSettings;
			this._seoSettings = seoSettings;
            this._customerSettings = customerSettings;
			this._privacySettings = privacySettings;
			this._scheduleTaskService = scheduleTaskService;
			this._transcationService = transactionService;
			this._workContext = workContext;
			this._customerService = customerService;
			this._adminAreaSettings = adminAreaSettings;
			this._planService = planService;
			this._webHelper = webHelper;
		}

		[RequireHttpsByConfig(SslRequirement.No)]
		public ActionResult Index()
		{
			////var task = new ScheduleTask
			////{
			////	CronExpression = "0 */24 * * *",
			////	Type = "SmartStore.Services.Boards.CheckBlockChainTransactionStatus, SmartStore.Services",//typeof("SmartStore.Services.Hyip.SendROITask,SmartStore.Services").AssemblyQualifiedNameWithoutVersion(),
			////	Enabled = false,
			////	StopOnError = false,
			////	IsHidden = false
			////};

			////task.Name = string.Concat("Check Blockchain Transaction Status", " Task");
			////_scheduleTaskService.InsertTask(task);
			if (Request["r"] != null)
			{
				System.Web.HttpCookie cookie = new System.Web.HttpCookie("3x1btcreferral", Request["r"].ToSafe());
				HttpContext.Response.Cookies.Remove("3x1btcreferral");
				HttpContext.Response.SetCookie(cookie);
			}
			var reff = Request.Cookies["3x1btcreferral"];
			var customer = _services.WorkContext.CurrentCustomer;
			var isAdmin = customer.IsAdmin();
			var isRegistered = isAdmin || customer.IsRegistered();
			HomeModel homemodel = new HomeModel();
			homemodel.LaunchDate = DateTime.Parse("04/15/2020").ToShortDateString(); //DateTime.Today.AddDays(-95).ToShortDateString();
			TimeSpan dt = DateTime.Now - DateTime.Parse(homemodel.LaunchDate);
			if(dt.Days > 0)
			{
				homemodel.RunningDays = dt.Days.ToString();
			}
			else
			{
				homemodel.RunningDays = "0";
			}
			var customers = _customerService.GetOnlineCustomers(DateTime.UtcNow.AddMinutes(-_customerSettings.Value.OnlineCustomerMinutes),
				null, 0, _adminAreaSettings.GridPageSize);

			homemodel.OnlineVistors = customers.TotalCount.ToString();
			int[] StatusIds = { (int)Status.Completed };
			int[] TranscationTypeIds = { (int)TransactionType.Purchase };
			var totalfundinglist = _transcationService.GetAllTransactions(0, 0, null, null, StatusIds, TranscationTypeIds, 0, int.MaxValue).OrderByDescending(x => x.TransactionDate).Take(10);
			homemodel.TotalSiteDeposit = _transcationService.GetAllTransactions(0, 0, null, null, StatusIds, TranscationTypeIds, 0, int.MaxValue).Select(x => x.Amount).Sum().ToString();
			int[] WithTranscationTypeIds = { (int)TransactionType.Withdrawal };
			var totalWithdrawallist = _transcationService.GetAllTransactions(0, 0, null, null, StatusIds, WithTranscationTypeIds, 0, int.MaxValue).OrderByDescending(x => x.TransactionDate).Take(10);
			homemodel.TotalSiteWithdrawal = _transcationService.GetAllTransactions(0, 0, null, null, StatusIds, WithTranscationTypeIds, 0, int.MaxValue).Select(x => x.Amount).Sum().ToString();

			var withdrawals = totalWithdrawallist.OrderByDescending(x => x.TransactionDate).ToList();
			foreach (var d in withdrawals)
			{
				WithdrawalList w = new WithdrawalList();
				w.Email = "GM-" + d.Customer.Id;
				w.AmountRaw = d.Amount.ToString() + _workContext.WorkingCurrency.CurrencyCode;
				w.TransDate = d.TransactionDate.ToShortDateString();
				w.PaymentMethodIcon = d.ProcessorId + ".svg";
				homemodel.Last10Withdrawal.Add(w);
			}

			var deposits = totalfundinglist.OrderByDescending(x => x.TransactionDate).ToList();
			foreach (var d in deposits)
			{
				DepositList w = new DepositList();
				w.Email = "GM-" + d.Customer.Id;
				w.AmountRaw = d.Amount.ToString() + _workContext.WorkingCurrency.CurrencyCode;
				w.TransDate = d.TransactionDate.ToShortDateString();
				w.PaymentMethodIcon = d.ProcessorId + ".svg";
				homemodel.Last10Deposit.Add(w);
			}
			homemodel.plans = _planService.GetAllPackage().ToList();
			ViewBag.SiteName = "3x1 BTC";
			return View(homemodel);
		}

		public ActionResult GetAuthCode()
		{
			var code_challenge = MakeCodeVerifier(32);
			var client = new RestSharp.RestClient("https://login.xero.com/");
			var request = new RestRequest("identity/connect/authorize/", Method.GET);
			request.AddParameter("response_type", "code");
			request.AddParameter("client_id", "437ACD16C59E4C17A43FE968BA55CE06");
			request.AddParameter("redirect_uri", "https://globalmatrixmillionaire.com");
			request.AddParameter("scope", "accounting.transactions");
			request.AddParameter("code_challenge", code_challenge);
			request.AddParameter("code_challenge_method", "S256");
			var queryResult = client.Execute(request);

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

		public ActionResult Faq()
		{
			Dictionary<string, string> FAQs = new Dictionary<string, string>();
			FAQs.Add("How this program works", "This is team forced matrix of 12 Level, you need to activate your membership with $10 once your payment is approved, your referral link will be generated.");
			FAQs.Add("How to get started", "Just click on signup link and create your account, Login and click on activate now button and make the $10 Payment");
			FAQs.Add("How much minimum fund do i require to join this program", "You will need $10 to join this program");
			FAQs.Add("How are payment processed ?", "All payments are processed automatically by the system there is no human involvement");
			FAQs.Add("How much time do i have to make payment after signup", "You will have 48 Hours to make payment, after that your account will be deleted if payment is not confirmed");
			FAQs.Add("Do i need to refer someone", "We suggest you to get atleast 3 referral, All positions are filled as per Team force logic, so it is not mandatory to refer someone if you have good working upline");
			var customer = _services.WorkContext.CurrentCustomer;
			var isAdmin = customer.IsAdmin();
			var isRegistered = isAdmin || customer.IsRegistered();
			ViewBag.IsAuthenticated = isRegistered;
			return View("FAQ", FAQs);
		}
		public ActionResult Withdrawals()
		{
			var transcation = _transcationService.GetAllTransactions(0, 0, null, null,
							  null, "5".ToIntArray(), 0, int.MaxValue);


			List<TransactionModel> model = new List<TransactionModel>();
			model = transcation.Select(x =>
			{
				var transModel = new TransactionModel
				{
					Id = x.Id,
					FinalAmountRaw = x.Amount + " USD",
					FinalAmount = x.Amount,
					TransactionDate = x.TransactionDate,
					StatusId = x.StatusId,
					TransStatusString = x.Status.GetLocalizedEnum(_localizationService, _workContext),
					TranscationTypeString = x.TranscationType.GetLocalizedEnum(_localizationService, _workContext),
					ProcessorId = x.ProcessorId,
					WithdrawalAccount = x.WithdrawalAddress,
					CustomerEmail = x.Customer.GetFullName()
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();
			var customer = _services.WorkContext.CurrentCustomer;
			var isAdmin = customer.IsAdmin();
			var isRegistered = isAdmin || customer.IsRegistered();
			ViewBag.TotalMembers = _customerService.GetAllCustomers(0, 0, int.MaxValue).Where(x => !x.Email.IsEmpty()).Count();
			ViewBag.IsAuthenticated = isRegistered;

			var cou = transcation.Count;

			return View(model);
		}

		public ActionResult CompanyBoard()
		{
			var custPositions = _boardService.GetAllPosition(1, 0, false, 0, int.MaxValue);

			var currency = _workContext.WorkingCurrency.CurrencyCode;

			var data = custPositions.Select(x =>
			{
				var myMatrix = _boardService.GetMyMatrixByPositionId(x.Id).FirstOrDefault();
				var transModel = new BoardModel
				{
					Id = x.Id,
					BoardId = x.BoardId,
					BoardName = _boardService.GetBoardById(x.BoardId).Name,
					CustomerName = x.Customer.GetFullName(),
					PurchaseDate = x.PurchaseDate,
					NoOfPositionFilled = myMatrix.PositionFilled,
					NoOfPositionRemaining = myMatrix.PositionRemaining,
					IsCycledString = (x.IsCycled) ? "Cycled" : "Active",
					PlacedUnderPositionId = x.PlacedUnderPositionId,
				};
				return transModel;
			});
			return View(data.ToList());
		}
		public ActionResult Deposit()
		{
			var transcation = _transcationService.GetAllTransactions(0, 0, null, null,
							  null, "1".ToIntArray(), 0, int.MaxValue);


			List<TransactionModel> model = new List<TransactionModel>();
			model = transcation.Select(x =>
			{
				var transModel = new TransactionModel
				{
					Id = x.Id,
					FinalAmountRaw = x.Amount + " USD",
					FinalAmount = x.Amount,
					TransactionDate = x.TransactionDate,
					StatusId = x.StatusId,
					TransStatusString = x.Status.GetLocalizedEnum(_localizationService, _workContext),
					TranscationTypeString = x.TranscationType.GetLocalizedEnum(_localizationService, _workContext),
					ProcessorId = x.ProcessorId,
					WithdrawalAccount = x.WithdrawalAddress,
					CustomerEmail = x.Customer.GetFullName()
				};

				transModel.TransactionDateString = transModel.TransactionDate.ToString("g");

				return transModel;
			}).ToList();
			var customer = _services.WorkContext.CurrentCustomer;
			var isAdmin = customer.IsAdmin();
			var isRegistered = isAdmin || customer.IsRegistered();
			ViewBag.TotalMembers = _customerService.GetAllCustomers(0, 0, int.MaxValue).Where(x => !x.Email.IsEmpty()).Count();
			ViewBag.IsAuthenticated = isRegistered;

			var cou = transcation.Count;

			return View(model);
		}

		[HttpPost]
		public ActionResult CalculateReturn(float amountinvested,int planid)
		{
			var plan = _planService.GetPlanById(planid);
			if(plan != null)
			{
				var profit = 0.00;
				if(amountinvested >= plan.MinimumInvestment && amountinvested <= plan.MaximumInvestment)
				{
					if (plan.IsPrincipalBack)
						profit = (((plan.ROIPercentage * amountinvested) / 100) * plan.NoOfPayouts) + amountinvested;
					else
						profit = (((plan.ROIPercentage * amountinvested) / 100) * plan.NoOfPayouts);

					return new JsonResult
					{
						Data = new { success = true, totalprofit = profit }
					};
				}
				else
				{
					return new JsonResult
					{
						Data = new { success = false, totalprofit = 0, Message = "Please enter valid investment" }
					};
				}
			}
			return new JsonResult
			{
				Data = new { success = false, totalprofit = 0 }
			};
		}
		public ActionResult Plan()
		{
			var plans = _planService.GetAllPlans().ToList();

			return View(plans);
		}
		public ActionResult StoreClosed()
		{
			return View();
		}

		[RequireHttpsByConfig(SslRequirement.No)]
		[GdprConsent]
		public ActionResult ContactUs()
		{
            var topic = _topicService.Value.GetTopicBySystemName("ContactUs");

            var model = new ContactUsModel
			{
				Email = _services.WorkContext.CurrentCustomer.Email,
				FullName = _services.WorkContext.CurrentCustomer.GetFullName(),
				FullNameRequired = _privacySettings.Value.FullNameOnContactUsRequired,
				DisplayCaptcha = _captchaSettings.Value.Enabled && _captchaSettings.Value.ShowOnContactUsPage,
                MetaKeywords = topic?.GetLocalized(x => x.MetaKeywords),
                MetaDescription = topic?.GetLocalized(x => x.MetaDescription),
                MetaTitle = topic?.GetLocalized(x => x.MetaTitle),
            };
			var customer = _services.WorkContext.CurrentCustomer;
			var isAdmin = customer.IsAdmin();
			var isRegistered = isAdmin || customer.IsRegistered();
			ViewBag.TotalMembers = _customerService.GetAllCustomers(0, 0, int.MaxValue).Where(x => !x.Email.IsEmpty()).Count();
			ViewBag.IsAuthenticated = isRegistered;

			return View(model);
		}

		[HttpPost, ActionName("ContactUs")]
		[ValidateCaptcha, ValidateHoneypot]
		[GdprConsent]
		public ActionResult ContactUsSend(ContactUsModel model, bool captchaValid)
		{
			// Validate CAPTCHA
			var customer = _services.WorkContext.CurrentCustomer;
			if (_captchaSettings.Value.Enabled && _captchaSettings.Value.ShowOnContactUsPage && !captchaValid)
			{
				ModelState.AddModelError("", T("Common.WrongCaptcha"));
			}

			if (ModelState.IsValid)
			{
				var email = model.Email.Trim();
				var fullName = model.FullName;
				var subject = T("ContactUs.EmailSubject", _services.StoreContext.CurrentStore.Name);
				var body = Core.Html.HtmlUtils.FormatText(model.Enquiry, false, true, false, false, false, false);

				// Required for some SMTP servers
				EmailAddress sender = null;
				if (!_commonSettings.Value.UseSystemEmailForContactUsForm)
				{
					sender = new EmailAddress(email, fullName);
				}

				// email
				var msg = Services.MessageFactory.SendContactUsMessage(customer, email, fullName, subject, body, sender);

				if (msg?.Email?.Id != null)
				{
					model.SuccessfullySent = true;
					model.Result = T("ContactUs.YourEnquiryHasBeenSent");
					_services.CustomerActivity.InsertActivity("PublicStore.ContactUs", T("ActivityLog.PublicStore.ContactUs"));
				}
				else
				{
					ModelState.AddModelError("", T("Common.Error.SendMail"));
					model.Result = T("Common.Error.SendMail");
				}

				return View(model);
			}
			
			var isAdmin = customer.IsAdmin();
			var isRegistered = isAdmin || customer.IsRegistered();
			ViewBag.TotalMembers = _customerService.GetAllCustomers(0, 0, int.MaxValue).Where(x => !x.Email.IsEmpty()).Count();
			ViewBag.IsAuthenticated = isRegistered;

			model.DisplayCaptcha = _captchaSettings.Value.Enabled && _captchaSettings.Value.ShowOnContactUsPage;
			return View(model);
		}

		[RequireHttpsByConfigAttribute(SslRequirement.No)]
		public ActionResult SitemapSeo(int? index = null)
		{
			if (!_seoSettings.Value.XmlSitemapEnabled)
				return HttpNotFound();
			
			string content = _sitemapGenerator.Value.GetSitemap(index);

			if (content == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Sitemap index is out of range.");
			}

			return Content(content, "text/xml", Encoding.UTF8);
		}

		[RequireHttpsByConfigAttribute(SslRequirement.No)]
		public ActionResult Sitemap()
		{
            return RedirectPermanent(_services.StoreContext.CurrentStore.Url);
		}

		
    }

	public class InvoiceResponse
	{
		public class Address
		{
			public string AddressType { get; set; }
			public string AddressLine1 { get; set; }
			public string AddressLine2 { get; set; }
			public string City { get; set; }
			public string PostalCode { get; set; }
		}

		public class Phone
		{
			public string PhoneType { get; set; }
		}

		public class Contact
		{
			public string ContactID { get; set; }
			public string ContactStatus { get; set; }
			public string Name { get; set; }
			public List<Address> Addresses { get; set; }
			public List<Phone> Phones { get; set; }
			public DateTime UpdatedDateUTC { get; set; }
			public string IsSupplier { get; set; }
			public string IsCustomer { get; set; }
		}

		public class Tracking
		{
			public string TrackingCategoryID { get; set; }
			public string Name { get; set; }
			public string Option { get; set; }
		}

		public class LineItem
		{
			public string Description { get; set; }
			public string Quantity { get; set; }
			public string UnitAmount { get; set; }
			public string TaxType { get; set; }
			public string TaxAmount { get; set; }
			public string LineAmount { get; set; }
			public string AccountCode { get; set; }
			public List<Tracking> Tracking { get; set; }
			public string LineItemID { get; set; }
		}

		public class Payment
		{
			public DateTime Date { get; set; }
			public string Amount { get; set; }
			public string PaymentID { get; set; }
		}

		public class Invoice
		{
			public string Type { get; set; }
			public Contact Contact { get; set; }
			public DateTime Date { get; set; }
			public DateTime DueDate { get; set; }
			public DateTime DateString { get; set; }
			public DateTime DueDateString { get; set; }
			public string Status { get; set; }
			public string LineAmountTypes { get; set; }
			public List<LineItem> LineItems { get; set; }
			public string SubTotal { get; set; }
			public string TotalTax { get; set; }
			public string Total { get; set; }
			public DateTime UpdatedDateUTC { get; set; }
			public string CurrencyCode { get; set; }
			public string InvoiceID { get; set; }
			public string InvoiceNumber { get; set; }
			public List<Payment> Payments { get; set; }
			public string AmountDue { get; set; }
			public string AmountPaid { get; set; }
			public string AmountCredited { get; set; }
		}

		public class RootObject
		{
			public List<Invoice> Invoices { get; set; }
		}
	}
}
