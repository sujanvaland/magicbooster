using SmartStore.Admin.Models.AdCampaign;
using SmartStore.Admin.Models.Investment;
using SmartStore.Core;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Logging;
using SmartStore.Services.Advertisments;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Services.Hyip;
using SmartStore.Services.Localization;
using SmartStore.Services.Security;
using SmartStore.Services.Stores;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
	[AdminAuthorize]
	public class AdCampaignController : AdminControllerBase
	{
		private readonly IAdCampaignService _adCampaignService;
		private readonly IPermissionService _permissionService;
		private readonly IAclService _aclService;
		private readonly IStoreService _storeService;
		private readonly IStoreMappingService _storeMappingService;
		private readonly IWorkContext _workContext;
		private readonly AdminAreaSettings _adminAreaSettings;
		private readonly ILocalizationService _localizationService;
		private readonly ICustomerActivityService _customerActivityService;
		private readonly ICustomerService _customerService;
		private readonly IGenericAttributeService _genericAttributeService;
		private readonly ITransactionService _transactionService;
		public AdCampaignController(IAdCampaignService adCampaignService, IPermissionService permissionService,
			IAclService aclService, IStoreService storeService, IStoreMappingService storeMappingService,
			AdminAreaSettings adminAreaSettings,
			IWorkContext workContext,
			ILocalizationService localizationService,
			ICustomerActivityService customerActivityService,
			ICustomerService customerService,
			IGenericAttributeService genericAttributeService,
			ITransactionService transactionService)
		{
			_adCampaignService = adCampaignService;
			_permissionService = permissionService;
			_aclService = aclService;
			_storeService = storeService;
			_adminAreaSettings = adminAreaSettings;
			_localizationService = localizationService;
			_customerActivityService = customerActivityService;
			_workContext = workContext;
			_storeMappingService = storeMappingService;
			_customerService = customerService;
			_genericAttributeService = genericAttributeService;
			_transactionService = transactionService;
		}
		
		public ActionResult Index()
        {
			return RedirectToAction("List");
		}

		public ActionResult List()
		{
			var allStores = _storeService.GetAllStores();
			var model = new AdCampaignListModel
			{
				GridPageSize = _adminAreaSettings.GridPageSize
			};

			model.ListAdType.Add(new SelectListItem { Text = "All", Value = "" });
			model.ListAdType.Add(new SelectListItem { Text = "Banner", Value = "Banner" });
			model.ListAdType.Add(new SelectListItem { Text = "Login", Value = "Login" });
			model.ListAdType.Add(new SelectListItem { Text = "Directory", Value = "Directory" });
			model.ListCreditType.Add(new SelectListItem { Text = "All", Value = "" });
			model.ListCreditType.Add(new SelectListItem { Text = "Impression", Value = "Impression" });
			model.ListCreditType.Add(new SelectListItem { Text = "Click", Value = "Click" });
			var traffic = _customerService.GetAvailableCredits(_workContext.CurrentCustomer.Id);
			model.AvailableClicks = traffic.FirstOrDefault().AvailableClick;
			model.AvailableImpression = traffic.FirstOrDefault().AvailableImpression;
			return View(model);
		}

		public ActionResult ListTicket()
		{
			var model = new SupportRequestModel();
			model.GridPageSize = _adminAreaSettings.GridPageSize;
			return View(model);
		}


		[HttpPost, GridAction(EnableCustomBinding = true)]
		public ActionResult List(GridCommand command, AdCampaignListModel model)
		{
			var gridModel = new GridModel<AdCampaignModel>();

			int CustomerId = 0;
			if (!_workContext.CurrentCustomer.IsAdmin())
			{
				CustomerId = _workContext.CurrentCustomer.Id;
			}
			var adCampaigns = _adCampaignService.GetAdCampaigns(model.SearchCampaignName, model.SearchWebsiteUrl,model.CreditType,model.AdType, CustomerId,command.Page - 1, command.PageSize);
			gridModel.Data = adCampaigns.Select(x =>
			{
				var adCampaignModel = x.ToModel();
				adCampaignModel.AvailableCredit = adCampaignModel.AssignedCredit - adCampaignModel.UsedCredit;
				adCampaignModel.Published = adCampaignModel.Enabled;
				return adCampaignModel;
			});

			gridModel.Total = adCampaigns.TotalCount;
			

			return new JsonResult
			{
				Data = gridModel
			};
		}

		[GridAction(EnableCustomBinding = true)]
		public ActionResult ListSupportTicket(GridCommand command, TransactionModel model)
		{
			var gridModel = new GridModel<SupportRequestModel>();
			DateTime? startDateValue = (model.StartDate == null) ? null : model.StartDate;
			DateTime? endDateValue = (model.EndDate == null) ? null : model.EndDate;
			bool status = true;
			var ticksts = _adCampaignService.GetSupportRequest(status, model.CustomerId, command.Page - 1, command.PageSize);

			gridModel.Data = ticksts.Select(x =>
			{
				var transModel = new SupportRequestModel
				{
					Id = x.Id,
					Subject = x.Subject,
					Message = x.Message,
					CreatedDate = x.CreatedDate,
					Name = x.Name,
					CustomerId = x.CustomerId,
					LastReplied = x.LastReplied,
					Status = x.Status,
					Email = x.Customer.Email
				};

				return transModel;
			});

			gridModel.Total = ticksts.TotalCount;

			return new JsonResult
			{
				Data = gridModel
			};
		}

		public ActionResult Create()
		{
			var model = new AdCampaignModel();
			model.ListAdType.Add(new SelectListItem { Text = "Banner", Value = "Banner" });
			model.ListAdType.Add(new SelectListItem { Text = "Login", Value = "Login" });
			model.ListAdType.Add(new SelectListItem { Text = "Directory", Value = "Directory" });

			model.ListCreditType.Add(new SelectListItem { Text = "Impression", Value = "Impression" });
			model.ListCreditType.Add(new SelectListItem { Text = "Click", Value = "Click" });
			model.CustomerId = _workContext.CurrentCustomer.Id;
			var traffic = _customerService.GetAvailableCredits(_workContext.CurrentCustomer.Id);
			model.AvailableClicks = traffic.FirstOrDefault().AvailableClick;
			model.AvailableImpression = traffic.FirstOrDefault().AvailableImpression;
			return View(model);
		}

		[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
		[ValidateInput(false)]
		public ActionResult Create(AdCampaignModel model, bool continueEditing, FormCollection form)
		{
			if (ModelState.IsValid)
			{
				if(model.CreditType == "Impression" && model.AvailableImpression < model.AssignedCredit)
				{
					NotifyError("You do not have enough Ad Credit (Impression)");
					return View(model);
				}
				if (model.CreditType == "Clicks" && model.AvailableClicks < model.AssignedCredit)
				{
					NotifyError("You do not have enough Ad Credit (Clicks)");
					return View(model);
				}

				model.CustomerId = _workContext.CurrentCustomer.Id;
				var adCampaign = model.ToEntity();
				adCampaign.Enabled = true;
				_adCampaignService.InsertAdCampaign(adCampaign);

				NotifySuccess(_localizationService.GetResource("Admin.AdCampaign.Added"));
				return continueEditing ? RedirectToAction("Edit", new { id = adCampaign.Id }) : RedirectToAction("List");
			}

			return View(model);
		}

		public ActionResult Edit(int id)
		{
			var adCampaign = _adCampaignService.GetAdCampaignById(id);
			if (adCampaign == null || adCampaign.Deleted)
				return RedirectToAction("List");

			var model = adCampaign.ToModel();
			model.ListAdType.Add(new SelectListItem { Text = "Banner", Value = "Banner" });
			model.ListAdType.Add(new SelectListItem { Text = "Login", Value = "Login" });
			model.ListAdType.Add(new SelectListItem { Text = "Directory", Value = "Directory" });

			model.ListCreditType.Add(new SelectListItem { Text = "Impression", Value = "Impression" });
			model.ListCreditType.Add(new SelectListItem { Text = "Click", Value = "Click" });
			model.CustomerId = _workContext.CurrentCustomer.Id;
			var traffic = _customerService.GetAvailableCredits(_workContext.CurrentCustomer.Id);
			model.AvailableClicks = traffic.FirstOrDefault().AvailableClick;
			model.AvailableImpression = traffic.FirstOrDefault().AvailableImpression;
			return View(model);
		}

		[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
		[ValidateInput(false)]
		public ActionResult Edit(AdCampaignModel model, bool continueEditing, FormCollection form)
		{
			var adCampaign = _adCampaignService.GetAdCampaignById(model.Id);
			if (adCampaign == null || adCampaign.Deleted)
				return RedirectToAction("List");

			if (ModelState.IsValid)
			{
				if (model.CreditType == "Impression" && model.AvailableImpression < model.AssignedCredit)
				{
					NotifyError("You do not have enough Ad Credit (Impression)");
					return View(model);
				}
				if (model.CreditType == "Clicks" && model.AvailableClicks < model.AssignedCredit)
				{
					NotifyError("You do not have enough Ad Credit (Clicks)");
					return View(model);
				}

				adCampaign = model.ToEntity();
				adCampaign.Enabled = true;
				_adCampaignService.UpdateAdCampaign(adCampaign);
				//activity log
				_customerActivityService.InsertActivity("EditCategory", _localizationService.GetResource("ActivityLog.EditCategory"), adCampaign.Name);

				NotifySuccess(_localizationService.GetResource("Admin.AdCampaign.Updated"));
				return continueEditing ? RedirectToAction("Edit", adCampaign.Id) : RedirectToAction("List");
			}
			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			var adCampaign = _adCampaignService.GetAdCampaignById(id);
			if (adCampaign == null)
				return RedirectToAction("List");

			_adCampaignService.DeleteAdCampaign(adCampaign);

			_customerActivityService.InsertActivity("DeleteAdCampaign", _localizationService.GetResource("ActivityLog.DeleteCategory"), adCampaign.Name);

			NotifySuccess(_localizationService.GetResource("Admin.AdCampaign.Deleted"));
			return RedirectToAction("List");
		}

		public ActionResult PurchaseAds()
		{
			return View();
		}

		public ActionResult PurchaseAds(int AdPackId)
		{
			return View();
		}

		public ActionResult PurchaseVaccationMode()
		{
			VaccationMode model = new VaccationMode();
			var customer = _workContext.CurrentCustomer;
			model.AvailableBalance = _customerService.GetAvailableBalance(customer.Id);
			ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			model.CurrentExpiryDate = customer.GetAttribute<DateTime>(SystemCustomerAttributeNames.VacationModeExpiryDate);
			return View(model);
		}

		[HttpPost]
		public ActionResult PurchaseVaccationMode(VaccationMode model)
		{
			var customer = _workContext.CurrentCustomer;
			model.AvailableBalance = _customerService.GetAvailableBalance(customer.Id);
			ViewBag.CurrencyCode = _workContext.WorkingCurrency.CurrencyCode;
			model.CurrentExpiryDate = customer.GetAttribute<DateTime>(SystemCustomerAttributeNames.VacationModeExpiryDate);
			if(model.StartDate < DateTime.Today)
			{
				NotifyError("Start Date cannot be past date");
				return View(model);
			}
			if (model.EndDate < DateTime.Today)
			{
				NotifyError("Start Date cannot be past date");
				return View(model);
			}
			if (model.StartDate > model.EndDate)
			{
				NotifyError("Start date cannot be greater the End date");
				return View(model);
			}
			if (model.EndDate < model.CurrentExpiryDate)
			{
				NotifyError("Your current Vaccation mode will expire on " + model.CurrentExpiryDate);
				return View(model);
			}
			var noOfDays = (model.EndDate - model.StartDate).TotalDays;
			var amount = ((noOfDays == 0) ? 1 : noOfDays) * 0.2;
			if(model.AvailableBalance < amount)
			{
				NotifyError("You dont have enough balance to make this purchase");
				return View(model);
			}
			
			_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.VacationModeExpiryDate, model.EndDate);
			TransactionModel transactionModel = new TransactionModel();
			transactionModel.Amount = (float)amount;
			transactionModel.CustomerId = customer.Id;
			transactionModel.FinalAmount = transactionModel.Amount;
			transactionModel.NoOfPosition = (int)noOfDays;
			transactionModel.TransactionDate = DateTime.Now;
			transactionModel.RefId = 0;
			transactionModel.ProcessorId = 5;
			transactionModel.TranscationTypeId = (int)TransactionType.Purchase;
			var transcation = transactionModel.ToEntity();
			transcation.NoOfPosition = (int)noOfDays;
			transcation.TranscationTypeId = (int)TransactionType.Purchase;
			transcation.StatusId = (int)Status.Completed;
			transcation.TranscationNote = "Vaccation mode purchase for " + transcation.NoOfPosition + " days";
			_transactionService.InsertTransaction(transcation);
			
			NotifySuccess("Your Vaccation mode purchase was successful");
			return View(model);
		}

		public ActionResult SurfAds()
		{
			var banner = _adCampaignService.GetRandomAds("Login", "Click", "", 1);
			var b = banner.FirstOrDefault();
			var customer = _workContext.CurrentCustomer;
			_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.LastSurfDate, DateTime.Now);
			var noOfAdsSurfed = customer.GetAttribute<int>(SystemCustomerAttributeNames.NoOfAdsSurfed);
			var lastsurfed = customer.GetAttribute<DateTime>(SystemCustomerAttributeNames.LastSurfDate);

			noOfAdsSurfed = noOfAdsSurfed + 1;
			if(noOfAdsSurfed == 10 && lastsurfed.Date == DateTime.Today)
			{
				_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.AllowROI, true);
			}
			_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.NoOfAdsSurfed, noOfAdsSurfed);
			if (b == null || _workContext.CurrentCustomer.IsAdmin())
			{
				NotifyError("No Ads to surf");
				return RedirectToAction("Index");
			}
			return View(b);
		}

		[HttpPost]
		public ActionResult CompleteRequest(int Id)
		{
			try
			{
				var transaction = _adCampaignService.GetAdCampaignById(Id);
				transaction.Enabled = true;
				_adCampaignService.UpdateAdCampaign(transaction);
				return Json(new
				{
					Success = true
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					Success = false
				});
			}
		}

		[HttpPost]
		public ActionResult CloseTicket(int Id)
		{
			try
			{
				var ticket = _adCampaignService.GetTicketById(Id);
				ticket.Status = "Closed";
				var transaction = _adCampaignService.AddUpdateSupportRequest(ticket);
				return Json(new
				{
					Success = true
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					Success = false
				});
			}
		}

		[HttpPost]
		public ActionResult ReplyTicket(int Id, string remarks)
		{
			try
			{
				var ticket = _adCampaignService.GetTicketById(Id);
				ticket.LastReplied =remarks;
				var transaction = _adCampaignService.AddUpdateSupportRequest(ticket);
				return Json(new
				{
					Success = true
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					Success = false
				});
			}
		}


		[HttpPost]
		public ActionResult RejectRequest(int Id)
		{
			try
			{
				var transaction = _adCampaignService.GetAdCampaignById(Id);
				_adCampaignService.DeleteAdCampaign(transaction);
				NotifySuccess("AdCampign Deleted");
				return Json(new
				{
					Success = true
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					Success = false
				});
			}
		}
	}
}