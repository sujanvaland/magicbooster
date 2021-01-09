using SmartStore.Core;
using SmartStore.Core.Domain.Hyip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Hyip
{
	public partial interface ICustomerPlanService
	{
		void InsertCustomerPlan(CustomerPlan plan);
		void UpdateCustomerPlan(CustomerPlan customerPlan);
		void DiseableOldCustomerPlan(int CustomerId);
		void DeleteCustomerPlan(CustomerPlan customerPlan);
		CustomerPlan GetCustomerPlanById(int customerplanid=0);

		IPagedList<CustomerPlan> GetAllCustomerPlans(
			int customerid=0,
			int planid=0,
			int pageIndex = 0,
			int pageSize = int.MaxValue,
			int storeId = 0);
	}
}
