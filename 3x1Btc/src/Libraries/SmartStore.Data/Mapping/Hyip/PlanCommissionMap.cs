using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Hyip;

namespace SmartStore.Data.Mapping.Catalog
{
    public partial class PlanCommissionMap : EntityTypeConfiguration<PlanCommission>
    {
        public PlanCommissionMap()
        {
            this.ToTable("PlanCommission");
            this.HasKey(c => c.Id);
            this.Property(c => c.PlanId).IsRequired();
			this.Property(c => c.LevelId).IsRequired();
			this.Property(c => c.CommissionPercentage).IsRequired();
			this.HasRequired(o => o.Plan)
				.WithMany(c => c.PlanCommission)
				.HasForeignKey(o => o.PlanId);
		}
	}
}
