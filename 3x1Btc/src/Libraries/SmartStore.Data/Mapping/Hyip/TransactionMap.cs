using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Hyip;

namespace SmartStore.Data.Mapping.Catalog
{
    public partial class TransactionMap : EntityTypeConfiguration<Transaction>
    {
        public TransactionMap()
        {
            this.ToTable("Transaction");
            this.HasKey(c => c.Id);
			this.Property(c => c.Amount).IsRequired();
			this.Property(c => c.FinalAmount).IsRequired();
			this.Property(c => c.StatusId).IsRequired();
			this.Property(c => c.TranscationTypeId).IsRequired();

			this.Ignore(c => c.Status);
			this.Ignore(c => c.TranscationType);
			this.HasRequired(o => o.Customer)
				.WithMany(c => c.Transaction)
				.HasForeignKey(o => o.CustomerId);
		}
	}
}
