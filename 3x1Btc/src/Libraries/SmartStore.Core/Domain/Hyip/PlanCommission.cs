using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Core.Domain.Hyip
{
	[DataContract]
	public partial class PlanCommission : BaseEntity  
	{
		public int PlanId { get; set; }
		public int LevelId { get; set; }
		public float CommissionPercentage { get; set; }
		public Plan Plan { get; set; }
	}
	
	
}
