using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core.Domain.Boards;

namespace SmartStore.Data.Mapping.Boards
{
	public partial class BoardMap : EntityTypeConfiguration<Board>
	{
		public BoardMap()
		{
			this.ToTable("Board");
			this.HasKey(m => m.Id);
			this.Property(m => m.Name).IsRequired().HasMaxLength(400);
			this.Property(m => m.Height).IsRequired();
			this.Property(m => m.Width).IsRequired();
			this.Property(m => m.PayOnComplete).IsRequired();
		}
	}

	public partial class CustomerPositionMap : EntityTypeConfiguration<CustomerPosition>
	{
		public CustomerPositionMap()
		{
			this.ToTable("CustomerPosition");
			this.HasKey(m => m.Id);
			this.Property(m => m.CustomerId).IsRequired();
			this.Property(m => m.PlacedUnderPositionId).IsRequired();
			this.Property(m => m.PlacedUnderCustomerId).IsRequired();
			this.HasRequired(c => c.Customer)
				.WithMany(c => c.CustomerPosition)
				.HasForeignKey(c => c.CustomerId);
		}
	}

	public partial class AutoPurchaseSettingMap : EntityTypeConfiguration<AutoPurchaseSetting>
	{
		public AutoPurchaseSettingMap()
		{
			this.ToTable("AutoPurchaseSetting");
			this.HasKey(m => m.Id);
		}
	}
}
