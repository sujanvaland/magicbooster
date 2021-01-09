using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core;
using SmartStore.Core.Caching;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Core.Domain.Security;
using SmartStore.Core.Domain.Stores;
using SmartStore.Data.Caching;

namespace SmartStore.Services.Hyip
{
	public partial class CustomerPlanService : ICustomerPlanService
	{
		private readonly IRepository<CustomerPlan> _customerPlanRepository;
		private readonly IRequestCache _requestCache;
		private const string CUSTOMERPLANS_PATTERN_KEY = "customerplan.*";
		public CustomerPlanService(IRepository<CustomerPlan> customerPlanRepository,
			IRepository<StoreMapping> storeMappingRepository,
			IRepository<AclRecord> aclRepository,
			IWorkContext workContext,
			IRequestCache requestCache)
		{
			_customerPlanRepository = customerPlanRepository;
			_requestCache = requestCache;
		}

		

		public void InsertCustomerPlan(CustomerPlan customerPlan)
		{
			Guard.NotNull(customerPlan, nameof(customerPlan));

			_customerPlanRepository.Insert(customerPlan);

			_requestCache.RemoveByPattern(CUSTOMERPLANS_PATTERN_KEY);
		}

		public void DeleteCustomerPlan(CustomerPlan customerPlan)
		{
			Guard.NotNull(customerPlan, nameof(customerPlan));
			customerPlan.Deleted = true;
			_customerPlanRepository.Update(customerPlan);
		}

		public void UpdateCustomerPlan(CustomerPlan customerPlan)
		{
			Guard.NotNull(customerPlan, nameof(customerPlan));

			_customerPlanRepository.Update(customerPlan);
		}

		public void DiseableOldCustomerPlan(int CustomerId)
		{
			if(CustomerId != 0)
			{
				var cp = _customerPlanRepository.Table.Where(c => c.CustomerId == CustomerId).FirstOrDefault();
				if(cp != null)
				{
					cp.IsActive = false;
					UpdateCustomerPlan(cp);
				}
			}
		}

		public CustomerPlan GetCustomerPlanById(int customerplanid = 0)
		{
			if (customerplanid == 0)
				return null;

			return _customerPlanRepository.GetByIdCached(customerplanid, "db.customerplan.id-" + customerplanid);
		}

		public IPagedList<CustomerPlan> GetAllCustomerPlans(int customerid = 0, int planid = 0, int pageIndex = 0, int pageSize = int.MaxValue, int storeId = 0)
		{
			var query = _customerPlanRepository.Table;
			
			if(customerid > 0)
			{
				query = query.Where(c => c.CustomerId == customerid);
			}

			if (planid > 0)
			{
				query = query.Where(c => c.PlanId == planid);
			}

			query = query.Where(c => !c.Deleted);

			query = query
				.OrderBy(x => x.Id);

			var customerplans = query.ToList();

			// Paging
			return new PagedList<CustomerPlan>(customerplans, pageIndex, pageSize);
		}
	}
}
