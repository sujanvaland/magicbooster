using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Hyip;

namespace SmartStore.Data.Mapping.Catalog
{
    public partial class PlanMap : EntityTypeConfiguration<Plan>
    {
        public PlanMap()
        {
            this.ToTable("Plan");
            this.HasKey(c => c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(400);
			this.Property(c => c.NoOfPayouts).IsRequired();
			this.Property(c => c.PayEveryXDays).IsRequired();
			this.Property(c => c.ROIPercentage).IsRequired();
		}
	}
}
