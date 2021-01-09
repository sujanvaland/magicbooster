using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core.Domain.Boards;

namespace SmartStore.Data.Mapping.Boards
{
	public partial class PackageMap : EntityTypeConfiguration<Package>
	{
		public PackageMap()
		{
			this.ToTable("Package");
			this.HasKey(m => m.Id);
			this.Property(m => m.Name).IsRequired().HasMaxLength(400);
			this.Property(m => m.DirectBonus).IsRequired();
		}
	}
}
