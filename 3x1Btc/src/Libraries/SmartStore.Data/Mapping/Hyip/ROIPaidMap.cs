using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Hyip;

namespace SmartStore.Data.Mapping.Catalog
{
    public partial class ROIPaidMap : EntityTypeConfiguration<ROIPaid>
    {
        public ROIPaidMap()
        {
            this.ToTable("ROIPaid");
            this.HasKey(c => c.Id);
            this.Property(c => c.PlanId).IsRequired();
			this.Property(c => c.Amount).IsRequired();
			this.HasRequired(c => c.Customer)
				.WithMany(c => c.ROIPaid)
				.HasForeignKey(c => c.CustomerId);
		}
	}
}
