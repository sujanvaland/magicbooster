using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Models.Hyip
{
	public class PlanListModel : ModelBase
	{
		public PlanListModel()
		{
			AvailableStores = new List<SelectListItem>();
		}

		[SmartResourceDisplayName("Admin.HYIP.Plans.List.SearchPlanName")]
		[AllowHtml]
		public string SearchPlanName { get; set; }

		[SmartResourceDisplayName("Admin.HYIP.Plans.List.SearchAlias")]
		public string SearchAlias { get; set; }

		[SmartResourceDisplayName("Admin.Common.Store.SearchFor")]
		public int SearchStoreId { get; set; }
		public IList<SelectListItem> AvailableStores { get; set; }

		public int GridPageSize { get; set; }
	}
}