using SmartStore.Core.Domain.Advertisments;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Data.Mapping.Advertisments
{
	public class YoutubeVideosMap : EntityTypeConfiguration<YoutubeVideos>
	{
		public YoutubeVideosMap()
		{
			this.ToTable("YoutubeVideos");
			this.HasKey(a => a.Id);
			this.HasRequired(a => a.Customer).WithMany().HasForeignKey(x => x.CustomerId).WillCascadeOnDelete(false);
		}
	}
	public class FacebookPostMap : EntityTypeConfiguration<FacebookPost>
	{
		public FacebookPostMap()
		{
			this.ToTable("FacebookPost");
			this.HasKey(a => a.Id);
			this.HasRequired(a => a.Customer).WithMany().HasForeignKey(x => x.CustomerId).WillCascadeOnDelete(false);
		}
	}
	public class SupportRequestMap : EntityTypeConfiguration<SupportRequest>
	{
		public SupportRequestMap()
		{
			this.ToTable("SupportRequest");
			this.HasKey(a => a.Id);
			this.HasRequired(a => a.Customer).WithMany().HasForeignKey(x => x.CustomerId).WillCascadeOnDelete(false);
		}
	}
	public class FAQMap : EntityTypeConfiguration<FAQ>
	{
		public FAQMap()
		{
			this.ToTable("FAQ");
			this.HasKey(a => a.Id);
		}
	}

	public class CountryManagerMap : EntityTypeConfiguration<CountryManager>
	{
		public CountryManagerMap()
		{
			this.ToTable("CountryManager");
			this.HasKey(a => a.Id);
		}
	}

	public class CustomerTokenMap : EntityTypeConfiguration<CustomerToken>
	{
		public CustomerTokenMap()
		{
			this.ToTable("CustomerToken");
			this.HasKey(a => a.Id);
			this.HasRequired(a => a.Customer).WithMany().HasForeignKey(x => x.CustomerId).WillCascadeOnDelete(false);
		}
	}

	public class CustomerBlogPostMap : EntityTypeConfiguration<CustomerBlogPost>
	{
		public CustomerBlogPostMap()
		{
			this.ToTable("CustomerBlogPost");
			this.HasKey(a => a.Id);
			this.HasRequired(a => a.Customer).WithMany().HasForeignKey(x => x.CustomerId).WillCascadeOnDelete(false);
		}
	}
}
