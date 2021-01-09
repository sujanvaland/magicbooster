using System.Data.Entity.ModelConfiguration;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;

namespace SmartStore.Data.Mapping.Customers
{
	public partial class CustomerTrafficMap : EntityTypeConfiguration<CustomerTraffic>
	{
		public CustomerTrafficMap()
		{
			this.ToTable("CustomerTraffic");
			this.HasKey(c => c.Id);
			this.Property(cc => cc.IpAddress).HasMaxLength(200);

			this.HasRequired(cc => cc.Customer)
				.WithMany(c => c.CustomerTraffic)
				.HasForeignKey(cc => cc.CustomerId);
		}
	}
}