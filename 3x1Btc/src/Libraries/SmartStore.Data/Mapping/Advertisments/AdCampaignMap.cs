using SmartStore.Core.Domain.Advertisments;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Data.Mapping.Advertisments
{
	public class AdCampaignMap : EntityTypeConfiguration<AdCampaign>
	{
		public AdCampaignMap()
		{
			this.ToTable("AdCampaign");
			this.HasKey(a => a.Id);
			this.Property(a => a.WebsiteUrl).HasMaxLength(200);
			this.Property(a => a.Banner125).HasMaxLength(200);
			this.Property(a => a.Banner486).HasMaxLength(200);
			this.Property(a => a.Banner728).HasMaxLength(200);
			this.Property(a => a.CreditType).HasMaxLength(12);
			this.Property(a => a.AdType).HasMaxLength(12);
			//this.Ignore(p => p.Customer);
			this.Ignore(p => p.Picture);
			//this.HasOptional(a => a.Picture).WithMany().HasForeignKey(x => x.PictureId).WillCascadeOnDelete(false);
			this.HasRequired(a => a.Customer).WithMany().HasForeignKey(x => x.CustomerId).WillCascadeOnDelete(false);
		}
	}
}
