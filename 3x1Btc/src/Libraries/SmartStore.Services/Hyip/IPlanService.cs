using SmartStore.Core;
using SmartStore.Core.Domain.Boards;
using SmartStore.Core.Domain.Hyip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Hyip
{
	public partial interface IPlanService
	{
		void InsertPlan(Plan plan);
		void UpdatePlan(Plan plan);
		void DeletePlan(Plan plan);
		Plan GetPlanById(int planid);
		void InsertPlanCommission(PlanCommission plancommission);
		void UpdatePlanCommission(PlanCommission plancommission);
		List<PlanCommission> GetPlanCommissions(int PlanId);
		PlanCommission GetPlanCommissionById(int id);
		PlanCommission GetPlanCommissionPlanLevelId(int planid, int levelid);
		List<PlanCommission> GetPlanCommissionPlanId(int planid);
		IPagedList<Plan> GetAllPlans(
			string planName = "",
			bool showHidden = false,
			int pageIndex = 0,
			int pageSize = int.MaxValue,
			int storeId = 0);

		Package GetPackageById(int packageid);
		List<Package> GetAllPackage();
	}
}
