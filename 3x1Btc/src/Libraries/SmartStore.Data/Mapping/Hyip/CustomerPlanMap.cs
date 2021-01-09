using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Hyip;

namespace SmartStore.Data.Mapping.Catalog
{
    public partial class CusomerPlanMap : EntityTypeConfiguration<CustomerPlan>
    {
        public CusomerPlanMap()
        {
            this.ToTable("CusomerPlan");
            this.HasKey(c => c.Id);
            this.Property(c => c.CustomerId).IsRequired();
			this.Property(c => c.PlanId).IsRequired();
			this.Property(c => c.AmountInvested).IsRequired();
			this.HasRequired(c => c.Customer)
				.WithMany(c => c.CustomerPlan)
				.HasForeignKey(c => c.CustomerId);
		}
	}
}
