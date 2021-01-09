using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core;
using SmartStore.Core.Caching;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Boards;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Domain.Security;
using SmartStore.Core.Domain.Stores;
using SmartStore.Data.Caching;

namespace SmartStore.Services.Hyip
{
	public partial class PlanService : IPlanService
	{
		private readonly IRepository<Plan> _planRepository;
		private readonly IRepository<Package> _packageRepository;
		private readonly IRepository<PlanCommission> _planCommissionRepository;
		private readonly IRepository<CustomerPlan> _customerPlanRepository;
		private readonly IRepository<AclRecord> _aclRepository;
		private readonly IRepository<StoreMapping> _storeMappingRepository;

		private readonly IWorkContext _workContext;
		public DbQuerySettings QuerySettings { get; set; }
		private readonly IRequestCache _requestCache;
		private const string PLANS_PATTERN_KEY = "plan.*";
		private const string PLANCOMMISSION_PATTERN_KEY = "plancommission.*";

		public PlanService(IRepository<Plan> planRepository,
			IRepository<Package> packageRepository,
			IRepository<PlanCommission> planCommissionRepository,
			IRepository<StoreMapping> storeMappingRepository,
			IRepository<AclRecord> aclRepository,
			IWorkContext workContext,
			IRequestCache requestCache)
		{
			_planRepository = planRepository;
			_packageRepository = packageRepository;
			_planCommissionRepository = planCommissionRepository;
			_storeMappingRepository = storeMappingRepository;
			QuerySettings = DbQuerySettings.Default;
			_aclRepository = aclRepository;
			_workContext = workContext;
			_requestCache = requestCache;
		}
		public void DeletePlan(Plan plan)
		{
			Guard.NotNull(plan, nameof(plan));

			plan.Deleted = true;
			UpdatePlan(plan);
		}

		public virtual IQueryable<Plan> BuildPlansQuery(
			string planName = "",
			bool showHidden = false,
			int storeId = 0)
		{
			var query = _planRepository.Table;

			if (!showHidden)
				query = query.Where(c => c.Published);

			if (planName.HasValue())
				query = query.Where(c => c.Name.Contains(planName));

			if (showHidden)
			{
				if (!QuerySettings.IgnoreMultiStore && storeId > 0)
				{
					query = from c in query
							join sm in _storeMappingRepository.Table
							on new { c1 = c.Id, c2 = "Plan" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
							from sm in c_sm.DefaultIfEmpty()
							where !c.LimitedToStores || storeId == sm.StoreId
							select c;

					query = from c in query
							group c by c.Id into cGroup
							orderby cGroup.Key
							select cGroup.FirstOrDefault();
				}
			}
			else
			{
				query = ApplyHiddenPlansFilter(query, storeId);
			}

			query = query.Where(c => !c.Deleted);

			return query;
		}
		protected virtual IQueryable<Plan> ApplyHiddenPlansFilter(IQueryable<Plan> query, int storeId = 0)
		{
			// ACL (access control list)
			if (!QuerySettings.IgnoreAcl)
			{
				var allowedCustomerRolesIds = _workContext.CurrentCustomer.CustomerRoles.Where(x => x.Active).Select(x => x.Id).ToList();

				query = from c in query
						join acl in _aclRepository.Table
						on new { c1 = c.Id, c2 = "Plan" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into c_acl
						from acl in c_acl.DefaultIfEmpty()
						where !c.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
						select c;
			}

			// Store mapping
			if (!QuerySettings.IgnoreMultiStore && storeId > 0)
			{
				query = from c in query
						join sm in _storeMappingRepository.Table
						on new { c1 = c.Id, c2 = "Plan" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into c_sm
						from sm in c_sm.DefaultIfEmpty()
						where !c.LimitedToStores || storeId == sm.StoreId
						select c;
			}

			// Only distinct plans (group by ID)
			query = from c in query
					group c by c.Id into cGroup
					orderby cGroup.Key
					select cGroup.FirstOrDefault();

			return query;
		}

		public IPagedList<Plan> GetAllPlans(string planName = "",bool showHidden = false, int pageIndex = 0, int pageSize = int.MaxValue, int storeId = 0)
		{
			var query = BuildPlansQuery(planName, showHidden, storeId);

			query = query
				.OrderBy(x => x.Id)
				.ThenBy(x => x.Name);
				

			var plans = query.ToList();

			// Paging
			return new PagedList<Plan>(plans, pageIndex, pageSize);
		}

		public Plan GetPlanById(int planid)
		{
			if (planid == 0)
				return null;

			return _planRepository.GetByIdCached(planid, "db.plan.id-" + planid);
		}

		public List<PlanCommission> GetPlanCommissions(int PlanId)
		{
			return _planCommissionRepository.Table.Where(x => x.PlanId == PlanId).ToList();
		}
		public void InsertPlan(Plan plan)
		{
			Guard.NotNull(plan, nameof(plan));

			_planRepository.Insert(plan);

			_requestCache.RemoveByPattern(PLANS_PATTERN_KEY);
		}

		public void UpdatePlan(Plan plan)
		{
			Guard.NotNull(plan, nameof(plan));
			
			_planRepository.Update(plan);

			_requestCache.RemoveByPattern(PLANS_PATTERN_KEY);
		}

		public void InsertPlanCommission(PlanCommission plancommission)
		{
			Guard.NotNull(plancommission, nameof(plancommission));

			_planCommissionRepository.Insert(plancommission);

			_requestCache.RemoveByPattern(PLANCOMMISSION_PATTERN_KEY);
		}

		public void UpdatePlanCommission(PlanCommission plancommission)
		{
			Guard.NotNull(plancommission, nameof(plancommission));

			_planCommissionRepository.Update(plancommission);
		}

		public PlanCommission GetPlanCommissionById(int id)
		{
			if (id == 0)
				return null;

			return _planCommissionRepository.Table.Where(x => x.Id == id).FirstOrDefault();
		}

		public PlanCommission GetPlanCommissionPlanLevelId(int planid,int levelid)
		{
			if (planid == 0)
				return null;

			return _planCommissionRepository.Table.Where(x=>x.PlanId == planid && x.LevelId == levelid).FirstOrDefault();
		}

		public List<PlanCommission> GetPlanCommissionPlanId(int planid)
		{
			if (planid == 0)
				return null;

			return _planCommissionRepository.Table.Where(x => x.PlanId == planid).ToList();
		}

		public Package GetPackageById(int packageid)
		{
			if (packageid == 0)
				return null;

			return _packageRepository.Table.Where(x => x.Id == packageid && x.Deleted == false && x.Active == true).FirstOrDefault();
		}
		public List<Package> GetAllPackage()
		{
			return _packageRepository.Table.Where(x=>x.Deleted == false && x.Active == true).ToList();
		}
	}
}
