using SmartStore.Core.Domain.Hyip;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartStore.WebApi.Models.Api.Customer
{
	public class CoinRequestList : ModelBase
	{
		public int? Id { get; set; }
		public int CustomerId { get; set; }
		public Guid? CustomerGuid { get; set; }
		public float AvailableCoin { get; set; }

		public List<Transaction> Transaction { get; set; }
	}
}