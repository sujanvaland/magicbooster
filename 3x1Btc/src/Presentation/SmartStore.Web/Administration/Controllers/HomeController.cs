using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using SmartStore.Admin.Models.Common;
using SmartStore.Admin.Models.Hyip;
using SmartStore.Admin.Models.Investment;
using SmartStore.Admin.Models.PaymentMethods;
using SmartStore.Core;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Services;
using SmartStore.Services.Advertisments;
using SmartStore.Services.Boards;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Services.Hyip;
using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Security;

namespace SmartStore.Admin.Controllers
{
    [AdminAuthorize]
    public class HomeController : AdminControllerBase
    {
        #region Fields

		private readonly ICommonServices _services;
		private readonly CommonSettings _commonSettings;
		private readonly Lazy<IUserAgent> _userAgent;
		private readonly ICustomerService _customerService;
		private readonly ICustomerPlanService _customerPlanService;
		private readonly IWorkContext _workContext;
		private readonly ITransactionService _transactionService;
		private readonly IPlanService _planService;
		private readonly IAdCampaignService _adCampaignService;
		private readonly IGenericAttributeService _genericAttributeService;
		private readonly IBoardService _boardService;
		#endregion

		#region Ctor

		public HomeController(IBoardService boardService,ICommonServices services, CommonSettings commonSettings, 
			Lazy<IUserAgent> userAgent,
			ICustomerService customerService,IWorkContext workContext,
			ICustomerPlanService customerPlanService,
			ITransactionService transactionService,
			IPlanService planService,
			IAdCampaignService adCampaignService,
			IGenericAttributeService genericAttributeService)
        {
			this._boardService = boardService;
			this._commonSettings = commonSettings;
			this._services = services;
			this._userAgent = userAgent;
			this._customerService = customerService;
			this._workContext = workContext;
			this._customerPlanService = customerPlanService;
			this._transactionService = transactionService;
			this._planService = planService;
			this._adCampaignService = adCampaignService;
			this._genericAttributeService = genericAttributeService;
		}

		#endregion

		#region Methods
		public ActionResult LoginAd()
		{
			//var banner = _adCampaignService.GetRandomAds("Login", "Click", "", 1);
			//var b = banner.FirstOrDefault();
			//if (b == null || _workContext.CurrentCustomer.IsAdmin())
			//{
				return RedirectToAction("Index");
			//}
			//return View(b);
		}

		public ActionResult Index()
        {
			if (_workContext.CurrentCustomer.IsInCustomerRole("Administrators"))
			{
				var allRoles = _customerService.GetAllCustomerRoles(true);
				var registeredRoleId = allRoles.First(x => x.SystemName.IsCaseInsensitiveEqual(SystemCustomerRoleNames.Registered)).Id;

				AdminDashboardModel model = new AdminDashboardModel();
				model.RegisteredMembers = _customerService.GetAllCustomers(null, null, new int[] { registeredRoleId }, null,
				null, null, null, 0, 0, null, null, null,
				false, null, 0, int.MaxValue,null).TotalCount;
				model.ActiveMembers = _customerPlanService.GetAllCustomerPlans().Select(x => x.CustomerId).Distinct().Count();
				model.ActiveMembers = model.ActiveMembers + _boardService.GetAllPosition(1,0,false,0,int.MaxValue).Select(x => x.CustomerId).Distinct().Count();
				int[] StatusIds = "2".ToIntArray();
				int[] TranscationTypeIds = { (int)TransactionType.Withdrawal };
				var transcation = _transactionService.GetAllTransactions(0, 0, null, null,
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);

				model.CompletedWithdrawals = transcation.Sum(x => x.Amount);

				StatusIds = "1".ToIntArray();
				transcation = _transactionService.GetAllTransactions(0, 0, null, null,
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);
				model.PendingWithdrawals = transcation.Sum(x => x.Amount);

				StatusIds = "2".ToIntArray();
				int[] TranscationTypeIdsC = { (int)TransactionType.Commission };
				transcation = _transactionService.GetAllTransactions(0, 0, null, null,
							  StatusIds, TranscationTypeIdsC, 0, int.MaxValue);
				model.CommissionPaid = transcation.Sum(x => x.Amount);

				StatusIds = "2".ToIntArray();
				int[] TranscationTypeIdsD = { (int)TransactionType.Funding };
				transcation = _transactionService.GetAllTransactions(0, 0, null, null,
							  StatusIds, TranscationTypeIdsD, 0, int.MaxValue);
				model.TotalDeposit = transcation.Sum(x => x.Amount);
				model.TodaysDeposit = transcation.Where(x => x.TransactionDate.Day == DateTime.Today.Day &&
				x.TransactionDate.Month == DateTime.Today.Month &&
				x.TransactionDate.Year == DateTime.Today.Year).Sum(x => x.Amount);

				StatusIds = "1,2".ToIntArray();
				transcation = _transactionService.GetAllTransactions(0, 0, null, null,
							  StatusIds, TranscationTypeIds, 0, int.MaxValue);
				model.TodaysWithdrawal = transcation.Where(x=> x.TransactionDate.Day == DateTime.Today.Day &&
				x.TransactionDate.Month == DateTime.Today.Month &&
				x.TransactionDate.Year == DateTime.Today.Year).Sum(x => x.Amount);
				
				var plans = _planService.GetAllPlans();
				double totalInvestors = 0, totalInvestment = 0, totalROIPaid = 0, totalROIToPay = 0, totalPendingROI = 0;
				foreach(var plan in plans)
				{
					var plandetails = _customerPlanService.GetAllCustomerPlans(0, plan.Id).ToList();
					DashboardPlan dashboardPlan = new DashboardPlan();
					dashboardPlan.Name = plan.Name;
					dashboardPlan.TotalInvestors = plandetails.Select(x => x.CustomerId).Distinct().Count().ToString();
					dashboardPlan.TotalInvestment = plandetails.Sum(x => x.AmountInvested).ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
					dashboardPlan.ROIPaid = plandetails.Sum(x => x.ROIPaid).ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
					dashboardPlan.ROIToPay = plandetails.Sum(x => x.ROIToPay).ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
					dashboardPlan.PendingROI = (plandetails.Sum(x => x.ROIToPay) - plandetails.Sum(x => x.ROIPaid)).ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
					dashboardPlan.TotalInvestorsInt = plandetails.Select(x => x.CustomerId).Distinct().Count();
					dashboardPlan.TotalInvestmentInt = plandetails.Sum(x => x.AmountInvested);
					dashboardPlan.ROIPaidInt = plandetails.Sum(x => x.ROIPaid);
					dashboardPlan.ROIToPayInt = plandetails.Sum(x => x.ROIToPay);
					dashboardPlan.PendingROIInt = (plandetails.Sum(x => x.ROIToPay) - plandetails.Sum(x => x.ROIPaid));

					totalInvestors = Convert.ToInt32(totalInvestors) + Convert.ToInt32(dashboardPlan.TotalInvestorsInt);
					totalInvestment = Convert.ToDouble(totalInvestment) + Convert.ToDouble(dashboardPlan.TotalInvestmentInt);
					totalROIPaid = Convert.ToDouble(totalROIPaid) + Convert.ToDouble(dashboardPlan.ROIPaidInt);
					totalROIToPay = Convert.ToDouble(totalROIToPay) + Convert.ToDouble(dashboardPlan.ROIToPayInt);
					totalPendingROI = Convert.ToDouble(totalPendingROI) + Convert.ToDouble(dashboardPlan.PendingROIInt);
					model.Plans.Add(dashboardPlan);
				}
				DashboardPlan plantotal = new DashboardPlan();
				plantotal.Name = "Total";
				plantotal.TotalInvestors = totalInvestors.ToString();
				plantotal.TotalInvestment = totalInvestment.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				plantotal.ROIPaid = totalROIPaid.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				plantotal.ROIToPay = totalROIToPay.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				plantotal.PendingROI = totalPendingROI.ToString() + " " + _workContext.WorkingCurrency.CurrencyCode;
				model.Plans.Add(plantotal);
				return View("Dashboard",model);
			}
			else
			{
				HomeModel model = new HomeModel();
				model.ReferralLink = _services.StoreContext.CurrentStore.Url + "?r=" + _workContext.CurrentCustomer.Id;
				var id = _workContext.CurrentCustomer.Id;
				model.AvailableBalance = _customerService.GetAvailableBalance(id);
				//model.RepurchaseBalance = _customerService.GetRepurchaseBalance(id);
				model.CompletedWithdrawal = _customerService.GetCustomerCompletedWithdrawal(id);
				model.PendingWithdrawal = _customerService.GetCustomerPendingWithdrawal(id);
				model.TotalEarning = _customerService.GetCustomerTotalEarnings(id);
				model.TotalIncome = model.TotalEarning;
				model.CyclerIncome = _customerService.GetCustomerCyclerBonus(id);
				model.DirectBonus = _customerService.GetCustomerDirectBonus(id);
				model.UnilevelEarning = _customerService.GetCustomerUnilevelBonus(id);
				model.PoolShare = _customerService.GetCustomerROI(id) + _customerService.GetRepurchaseROI(id);
				model.TotalReferral = _customerService.GetCustomerDirectReferral(id).Count();
				model.InvestorId = id.ToString();
				model.Name = _workContext.CurrentCustomer.GetFullName();
				model.ReferredBy = _customerService.GetCustomerById(_workContext.CurrentCustomer.AffiliateId).GetFullName();
				model.RegistrationDate = _workContext.CurrentCustomer.CreatedOnUtc.ToLongDateString();
				model.Status = _workContext.CurrentCustomer.CustomerPosition.Count() > 0 ? "Active" : "Inactive";
				model.VacationModelExpiryDate = _workContext.CurrentCustomer.GetAttribute<DateTime>(SystemCustomerAttributeNames.VacationModeExpiryDate).ToShortDateString();
				var custPositions = _boardService.GetAllPosition(0, _workContext.CurrentCustomer.Id,false, 0, int.MaxValue).ToList();
				var cycledPositions = _boardService.GetAllPosition(0, _workContext.CurrentCustomer.Id, true, 0, int.MaxValue).ToList();
				var lastsurfeddate = _workContext.CurrentCustomer.GetAttribute<DateTime>(SystemCustomerAttributeNames.LastSurfDate);
				ViewBag.Standard = custPositions.Where(x => x.BoardId == 1).Count();
				ViewBag.StandardCycled = cycledPositions.Where(x => x.BoardId == 1 && x.IsCycled == true).Count();
				ViewBag.Business = custPositions.Where(x => x.BoardId == 2).Count();
				ViewBag.BusinessCycled = cycledPositions.Where(x => x.BoardId == 2 && x.IsCycled == true).Count();
				ViewBag.Premium = custPositions.Where(x => x.BoardId == 3).Count();
				ViewBag.PremiumCycled = cycledPositions.Where(x => x.BoardId == 3 && x.IsCycled == true).Count();
				ViewBag.Ultimate = custPositions.Where(x => x.BoardId == 4).Count();
				ViewBag.UltimateCycled = cycledPositions.Where(x => x.BoardId == 4 && x.IsCycled == true).Count();
				ViewBag.Diamond = custPositions.Where(x => x.BoardId == 5).Count();
				ViewBag.DiamondCycled = cycledPositions.Where(x => x.BoardId == 5 && x.IsCycled == true).Count();
				ViewBag.CustomerId = _workContext.CurrentCustomer.Id;

				//if(lastsurfeddate.Date < DateTime.Today)
				//{
				//	model.NoOfAdsToSurf = 10;
				//}
				//else
				//{
				//	model.NoOfAdsToSurf = _workContext.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.NoOfAdsSurfed);
				//	int ads = (10 - model.NoOfAdsToSurf);
				//	if (ads < 0)
				//	{
				//		model.NoOfAdsToSurf = 0;
				//	}
				//	else
				//	{
				//		model.NoOfAdsToSurf = (10 - model.NoOfAdsToSurf);
				//	}
				//}
				//var vacationdate = _workContext.CurrentCustomer.GetAttribute<DateTime>(SystemCustomerAttributeNames.VacationModeExpiryDate);
				//if(vacationdate > DateTime.Today)
				//{
				//	model.NoOfAdsToSurf = 0;
				//}
				ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
				var substrans = _workContext.CurrentCustomer.Transaction.Where(x => x.StatusId == 2 && x.TranscationNote == "subscription").FirstOrDefault();
				if(substrans != null)
				{
					var noOfDays = substrans.NoOfPosition * 30;
					ViewBag.SubscriptionDate = substrans.CreatedOnUtc.AddDays(noOfDays).ToShortDateString();
				}
				return View(model);
			}
        }

		public ActionResult About()
		{
			return View();
		}

		public ActionResult UaTester(string ua = null)
		{
			if (ua.HasValue())
			{
				_userAgent.Value.RawValue = ua;
			}
			return View(_userAgent.Value);
		}

        [ChildActionOnly]
        public ActionResult SmartStoreNews()
        {
            try
            {
                string feedUrl = string.Format("https://www.smartstore.com/NewsRSS.aspx?Version={0}&Localhost={1}&HideAdvertisements={2}&StoreURL={3}",
                    SmartStoreVersion.CurrentVersion,
                    Request.Url.IsLoopback,
                    _commonSettings.HideAdvertisementsOnAdminArea,
					_services.StoreContext.CurrentStore.Url);

                //specify timeout (5 secs)
                var request = WebRequest.Create(feedUrl);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                using (var reader = XmlReader.Create(response.GetResponseStream()))
                {
                    var rssData = SyndicationFeed.Load(reader);
                    return PartialView(rssData);
                }
            }
            catch (Exception)
            {
                return Content("");
            }
        }

		[ChildActionOnly]
		public ActionResult MarketplaceFeed()
		{
			var result = _services.Cache.Get("admin:marketplacefeed", () => {
				try
				{
					string url = "http://community.smartstore.com/index.php?/rss/downloads/";
					var request = (HttpWebRequest)WebRequest.Create(url);
					request.Timeout = 3000;
					request.UserAgent = "SmartStore.NET {0}".FormatInvariant(SmartStoreVersion.CurrentFullVersion);

					using (WebResponse response = request.GetResponse())
					{
						using (var reader = XmlReader.Create(response.GetResponseStream()))
						{
							var feed = SyndicationFeed.Load(reader);
							var model = new List<FeedItemModel>();
							foreach (var item in feed.Items)
							{
								if (!item.Id.EndsWith("error=1", StringComparison.OrdinalIgnoreCase))
								{
									var modelItem = new FeedItemModel();
									modelItem.Title = item.Title.Text;
									modelItem.Summary = item.Summary.Text.RemoveHtml().Truncate(150, "...");
									modelItem.PublishDate = item.PublishDate.LocalDateTime.RelativeFormat();

									var link = item.Links.FirstOrDefault();
									if (link != null)
									{
										modelItem.Link = link.Uri.ToString();
									}

									model.Add(modelItem);
								}
							}

							return model;
						}
					}
				}
				catch (Exception ex)
				{
					return new List<FeedItemModel> {new FeedItemModel { IsError = true, Summary = ex.Message } };
				}
			}, TimeSpan.FromHours(12));

			if (result.Any() && result.First().IsError)
			{
				ModelState.AddModelError("", result.First().Summary);
			}

			return PartialView(result);
		}

        [HttpPost]
        public ActionResult SmartStoreNewsHideAdv()
        {
            _commonSettings.HideAdvertisementsOnAdminArea = !_commonSettings.HideAdvertisementsOnAdminArea;
			_services.Settings.SaveSetting(_commonSettings);
            return Content("Setting changed");
        }

		public ActionResult ActivateMemberShip()
		{
			CustomerPlanModel model = new CustomerPlanModel();
			var storeScope = this.GetActiveStoreScopeConfiguration(_services.StoreService, _services.WorkContext);

			var coinpaymentSettings = _services.Settings.LoadSetting<CoinPaymentSettings>(storeScope);

			var SolitTrustPaySettings = _services.Settings.LoadSetting<SolidTrustPaySettings>(storeScope);

			var PayzaSettings = _services.Settings.LoadSetting<PayzaSettings>(storeScope);

			var PMSettings = _services.Settings.LoadSetting<PMSettings>(storeScope);

			var PayeerSettings = _services.Settings.LoadSetting<PayeerSettings>(storeScope);

			if (coinpaymentSettings.CP_IsActivePaymentMethod)
			{
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Bitcoin",
					Value = "0"
				});
			}
			if (PayzaSettings.PZ_IsActivePaymentMethod)
			{
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Payza",
					Value = "1"
				});
			}
			if (PMSettings.PM_IsActivePaymentMethod)
			{
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "PM",
					Value = "2"
				});
			}
			if (PayeerSettings.PY_IsActivePaymentMethod)
			{
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "Payeer",
					Value = "3"
				});
			}
			if (SolitTrustPaySettings.STP_IsActivePaymentMethod)
			{
				model.AvailableProcessor.Add(new SelectListItem()
				{
					Text = "SolidTrustPay",
					Value = "4"
				});
			}

			return View(model);
		}

		[HttpPost]
		public ActionResult ActivateMemberShip(CustomerPlanModel customerPlanModel)
		{
			TransactionModel transactionModel = new TransactionModel();
			transactionModel.Amount = 1; //Move to setting
			transactionModel.CustomerId = _workContext.CurrentCustomer.Id;
			transactionModel.FinalAmount = transactionModel.Amount;
			transactionModel.NoOfPosition = 0;
			transactionModel.TransactionDate = DateTime.Now;
			transactionModel.RefId = 0;
			transactionModel.ProcessorId = customerPlanModel.ProcessorId;
			transactionModel.TranscationTypeId = (int)TransactionType.Purchase;
			var transcation = transactionModel.ToEntity();
			transactionModel.StatusId = (int)Status.Pending;
			transcation.TranscationTypeId = (int)TransactionType.Purchase;
			transcation.TranscationNote = "Membership";
			_transactionService.InsertTransaction(transcation);
			 
			PaymentMethod paymentmethod = (PaymentMethod)customerPlanModel.ProcessorId;

			ViewBag.SaveSuccess = true;
			customerPlanModel.Id = 0;
			customerPlanModel.PlanName = "Membership";
			customerPlanModel.ProcessorName = paymentmethod.ToString();
			customerPlanModel.PaymentMethod = paymentmethod;
			customerPlanModel.TransactionId = transcation.Id;
			customerPlanModel.AmountInvested = (decimal)transactionModel.Amount;
			return RedirectToAction("ConfirmPayment","Investment", customerPlanModel);
		}
		#endregion

		#region ResetDataBase
		public ActionResult ResetDB()
		{
			return null;
		}
		#endregion
	}
}
