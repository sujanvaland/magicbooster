using FluentValidation.Attributes;
using SmartStore.Admin.Validators.Investment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartStore.Admin.Models.PaymentMethods;
namespace SmartStore.Admin.Models.Investment
{
	[Validator(typeof(CusotmerPlanValidator))]
	public class CustomerPlanModel
	{
		public CustomerPlanModel()
		{
			AvailablePlans = new List<SelectListItem>();
			AvailableProcessor = new List<SelectListItem>();
			PaymentMethod = new PaymentMethod();
		}

		public int StokistId { get; set; }
		public int Id { get; set; }
		public int NoOfPosition { get; set; }
		public int CustomerId { get; set; }
		public int PlanId { get; set; }
		public DateTime PurchaseDate { get; set; }
		public int ROIToPay { get; set; }
		public int ROIPaid { get; set; }
		public int NoOfPayout { get; set; }
		public int NoOfPayoutPaid { get; set; }
		public int IsExpired { get; set; }
		public DateTime ExpiredDate { get; set; }
		public int Deleted { get; set; }
		public string PlanName { get; set; }
		public decimal AmountInvested { get; set; }
		public int ProcessorId { get; set; }
		public string ProcessorName { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public float DepositFees { get; set; }
		public int TransactionId { get; set; }
		public float RepurchaseBalance { get; set; }
		public float AvailableBalance { get; set; }
		public List<SelectListItem> AvailablePlans = new List<SelectListItem>();
		public List<SelectListItem> AvailableProcessor = new List<SelectListItem>();
	}
}