using SmartStore.Core.Domain.Hyip;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Data.Mapping.Hyip
{
	public partial class BannerMap : EntityTypeConfiguration<Banner>
	{
		public BannerMap()
		{
			this.ToTable("Banner");
			this.HasKey(c => c.Id);
		}
	}
}
