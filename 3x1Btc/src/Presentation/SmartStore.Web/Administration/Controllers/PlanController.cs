using SmartStore.Admin.Models.Hyip;
using SmartStore.Core;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Events;
using SmartStore.Core.Logging;
using SmartStore.Services.Customers;
using SmartStore.Services.Helpers;
using SmartStore.Services.Hyip;
using SmartStore.Services.Localization;
using SmartStore.Services.Security;
using SmartStore.Services.Stores;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using SmartStore.Web.Framework.Modelling;
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
	public class PlanController : AdminControllerBase
	{
		#region Fields

		private readonly IPlanService _planService;
		private readonly ICustomerService _customerService;
		private readonly ILanguageService _languageService;
		private readonly ILocalizationService _localizationService;
		private readonly ILocalizedEntityService _localizedEntityService;
		private readonly IPermissionService _permissionService;
		private readonly IAclService _aclService;
		private readonly IStoreService _storeService;
		private readonly IStoreMappingService _storeMappingService;
		private readonly IWorkContext _workContext;
		private readonly ICustomerActivityService _customerActivityService;
		private readonly IDateTimeHelper _dateTimeHelper;
		private readonly AdminAreaSettings _adminAreaSettings;
		private readonly IEventPublisher _eventPublisher;

		#endregion

		#region Constructors

		public PlanController(IPlanService planService, 
			ICustomerService customerService,
			ILanguageService languageService,
			ILocalizationService localizationService, ILocalizedEntityService localizedEntityService,
			IPermissionService permissionService,
			IAclService aclService, IStoreService storeService, IStoreMappingService storeMappingService,
			IWorkContext workContext,
			ICustomerActivityService customerActivityService,
			IDateTimeHelper dateTimeHelper,
			AdminAreaSettings adminAreaSettings,
			IEventPublisher eventPublisher)
		{
			this._planService = planService;
			this._customerService = customerService;
			this._languageService = languageService;
			this._localizationService = localizationService;
			this._localizedEntityService = localizedEntityService;
			this._permissionService = permissionService;
			this._aclService = aclService;
			this._storeService = storeService;
			this._storeMappingService = storeMappingService;
			this._workContext = workContext;
			this._customerActivityService = customerActivityService;
			this._dateTimeHelper = dateTimeHelper;
			this._adminAreaSettings = adminAreaSettings;
			this._eventPublisher = eventPublisher;
		}

		#endregion

		// GET: Plans
		public ActionResult Index()
        {
            return View();
        }

		public ActionResult List()
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageHyip))
				return AccessDeniedView();

			var allStores = _storeService.GetAllStores();
			var model = new PlanListModel
			{
				GridPageSize = _adminAreaSettings.GridPageSize
			};

			foreach (var store in allStores)
			{
				model.AvailableStores.Add(new SelectListItem { Text = store.Name, Value = store.Id.ToString() });
			}

			return View(model);
		}

		[HttpPost, GridAction(EnableCustomBinding = true)]
		public ActionResult List(GridCommand command, PlanListModel model)
		{
			var gridModel = new GridModel<PlanModel>();

			if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
			{
				var plans = _planService.GetAllPlans(model.SearchPlanName,false, command.Page - 1, command.PageSize, model.SearchStoreId);
				
				gridModel.Data = plans.Select(x =>
				{

					var planModel = x.ToModel();
					return planModel;
				});
				
				gridModel.Total = plans.TotalCount;
			}
			else
			{
				gridModel.Data = Enumerable.Empty<PlanModel>();

				NotifyAccessDenied();
			}

			return new JsonResult
			{
				Data = gridModel
			};
		}

		public ActionResult Create()
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageHyip))
				return AccessDeniedView();

			var model = new PlanModel();

			AddLocales(_languageService, model.Locales);

			model.Published = true;

			return View(model);
		}

		[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
		[ValidateInput(false)]
		public ActionResult Create(PlanModel model, bool continueEditing, FormCollection form)
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageHyip))
				return AccessDeniedView();

			if (ModelState.IsValid)
			{
				var plan = model.ToEntity();

				_planService.InsertPlan(plan);
				
				_eventPublisher.Publish(new ModelBoundEvent(model, plan, form));

				//activity log
				_customerActivityService.InsertActivity("AddNewPlan", _localizationService.GetResource("ActivityLog.AddNewPlan"), plan.Name);

				NotifySuccess(_localizationService.GetResource("Admin.Hyip.Plans.Added"));
				return continueEditing ? RedirectToAction("Edit", new { id = plan.Id }) : RedirectToAction("List");
			}

			return View(model);
		}

		public ActionResult Edit(int id)
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageHyip))
				return AccessDeniedView();

			var plan = _planService.GetPlanById(id);
			if (plan == null || plan.Deleted)
				return RedirectToAction("List");

			var model = plan.ToModel();
			
			return View(model);
		}

		[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
		[ValidateInput(false)]
		public ActionResult Edit(PlanModel model, bool continueEditing, FormCollection form)
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageHyip))
				return AccessDeniedView();

			var plan = _planService.GetPlanById(model.Id);
			if (plan == null || plan.Deleted)
				return RedirectToAction("List");
			
			if (ModelState.IsValid)
			{
				plan = model.ToEntity(plan);

				_planService.UpdatePlan(plan);

				_eventPublisher.Publish(new ModelBoundEvent(model, plan, form));

				//activity log
				_customerActivityService.InsertActivity("EditPlan", _localizationService.GetResource("ActivityLog.EditCategory"), plan.Name);

				NotifySuccess(_localizationService.GetResource("Admin.Catalog.Categories.Updated"));
				return continueEditing ? RedirectToAction("Edit", plan.Id) : RedirectToAction("List");
			}
			
			return View(model);
		}

		[HttpPost]
		public ActionResult Delete(int id, string deleteType)
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageHyip))
				return AccessDeniedView();

			var plan = _planService.GetPlanById(id);
			if (plan == null)
				return RedirectToAction("List");

			_planService.DeletePlan(plan);

			_customerActivityService.InsertActivity("DeletePlan", _localizationService.GetResource("ActivityLog.DeletePlan"), plan.Name);

			NotifySuccess(_localizationService.GetResource("Admin.Hyip.Plans.Deleted"));
			return RedirectToAction("List");
		}

		public ActionResult PlanCommission(int planid)
		{
			PlanModel model = new PlanModel();
			model.PlanId = planid;
			var plan = _planService.GetPlanById(planid);
			model.Name = plan.Name;
			for(int i=1;i<=20;i++)
			{
				model.AvailableLevels.Add(new SelectListItem { Text = "Level "+i, Value = i.ToString() });
			}
			return View(model);
		}

		public ActionResult EditCommission(int id)
		{
			var plancommission = _planService.GetPlanCommissionById(id);
			PlanModel model = new PlanModel();
			model.PlanId = plancommission.PlanId;
			model.Id = plancommission.Id;
			model.Commission = plancommission.CommissionPercentage;
			model.LevelId = plancommission.LevelId;
			var plan = _planService.GetPlanById(plancommission.PlanId);
			model.Name = plan.Name;
			for (int i = 1; i <= 20; i++)
			{
				model.AvailableLevels.Add(new SelectListItem { Text = "Level " + i, Value = i.ToString() });
			}
			return View("PlanCommission",model);
		}

		[HttpPost]
		public ActionResult PlanCommission(PlanModel model)
		{
			try
			{
				PlanCommission planCommission = new PlanCommission();
				planCommission.PlanId = model.PlanId;
				planCommission.CommissionPercentage = model.Commission;
				planCommission.Id = model.Id;
				planCommission.LevelId = model.LevelId;
				_planService.InsertPlanCommission(planCommission);
				NotifySuccess(T("Plan.PlancommissionSave"));
			}
			catch(Exception ex)
			{
				NotifyError(T("Plan.PlancommissionSaveError"));
			}

			return View(model);
		}

		[HttpPost, GridAction(EnableCustomBinding = true)]
		public ActionResult ListCommission(GridCommand command, PlanModel model)
		{
			var gridModel = new GridModel<PlanModel>();

			if (_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
			{
				var plancommission = _planService.GetPlanCommissionPlanId(model.PlanId);
				gridModel.Data = plancommission.Select(x =>
				{
					var planModel = new PlanModel();
					planModel.Id = x.Id;
					planModel.PlanId = x.PlanId;
					planModel.Commission = x.CommissionPercentage;
					planModel.LevelId = x.LevelId;
					planModel.Name = _planService.GetPlanById(x.PlanId).Name;
					return planModel;
				});

				gridModel.Total = plancommission.Count;
			}
			else
			{
				gridModel.Data = Enumerable.Empty<PlanModel>();

				NotifyAccessDenied();
			}

			return new JsonResult
			{
				Data = gridModel
			};
		}
	}
}