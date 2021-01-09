using SmartStore.Admin.Models.Hyip;
using SmartStore.Core;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Services;
using SmartStore.Services.Hyip;
using SmartStore.Services.Localization;
using SmartStore.Services.Media;
using SmartStore.Services.Security;
using SmartStore.Web.Framework.Controllers;
using SmartStore.Web.Framework.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Controllers
{
    public class BannerController : AdminControllerBase
	{
		private readonly IPermissionService _permissionService;
		private readonly IBannerService _bannerService;
		private readonly IPictureService _pictureService;
		private readonly ILocalizationService _localizationService;
		private readonly ICommonServices _commonService;
		private readonly IWorkContext _workContext;
		public BannerController(IPermissionService permissionService,
			IBannerService bannerService,
			IPictureService pictureService,
			ILocalizationService localizationService,
			ICommonServices commonService,
			IWorkContext workContext)
		{
			_permissionService = permissionService;
			_bannerService = bannerService;
			_pictureService = pictureService;
			_localizationService = localizationService;
			_commonService = commonService;
			_workContext = workContext;
		}
		public ActionResult Create()
		{
			var model = new BannerModel();
			//var banners = _bannerService.GetAllBanners();
			//foreach(var b in banners)
			//{
			//	Banners banner = new Banners();
			//	banner.PictureId = b.PictureId;
			//	banner.Size = b.Size;
			//	banner.Published = b.Published;
			//	var picture = _pictureService.GetPictureById(b.PictureId);
			//	banner.BannerUrl = _commonService.StoreContext.CurrentStore.Url.EnsureEndsWith("/") + _pictureService.GetUrl(picture, host: "");
			//	model.bannerslist.Add(banner);
			//}
			model.ReferralLink = _commonService.StoreContext.CurrentStore.Url + "?r=" + _workContext.CurrentCustomer.Id;
			model.Published = true;
			return View(model);
		}

		[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
		public ActionResult Create(BannerModel model, bool continueEditing)
		{
			if (!_permissionService.Authorize(StandardPermissionProvider.ManageHyip))
				return AccessDeniedView();

			if (ModelState.IsValid)
			{
				var banner = model.ToEntity();

				MediaHelper.UpdatePictureTransientStateFor(banner, m => m.PictureId);

				_bannerService.InsertBanner(banner);
				
				// update picture seo file name
				UpdatePictureSeoNames(banner);
				NotifySuccess(_localizationService.GetResource("Admin.Catalog.Banner.Added"));
				return continueEditing ? RedirectToAction("Create", new { id = banner.Id }) : RedirectToAction("Create");
			}

			return View(model);
		}

		[NonAction]
		protected void UpdatePictureSeoNames(Banner banner)
		{
			var picture = _pictureService.GetPictureById(banner.PictureId);
			if (picture != null)
				_pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(banner.Name));
		}

	}
}