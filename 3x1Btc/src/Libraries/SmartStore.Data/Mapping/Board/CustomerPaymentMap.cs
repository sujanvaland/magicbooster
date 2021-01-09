using SmartStore.Core.Domain.Boards;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Data.Mapping.Board
{
	public partial class CustomerPaymentMap : EntityTypeConfiguration<CustomerPayment>
	{
		public CustomerPaymentMap()
		{
			this.ToTable("CustomerPayment");
			this.HasKey(m => m.Id);
			this.Property(m => m.Amount).HasPrecision(18, 4);
			this.HasRequired(c => c.Customer)
				.WithMany(c => c.CustomerPayment)
				.HasForeignKey(c => c.PayToCustomerId);
		}
		
	}
}
