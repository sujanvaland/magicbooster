using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartStore.Admin.Models.AdCampaign
{
	public class AdCampaignModel : EntityModelBase
	{
		public AdCampaignModel()
		{
			ListCreditType = new List<SelectListItem>();
			ListAdType = new List<SelectListItem>();
		}
		[SmartResourceDisplayName("Admin.AdCampaign.Name")]
		public string Name { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.WebsiteUrl")]
		public string WebsiteUrl { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.Banner125")]
		public string Banner125 { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.Banner486")]
		public string Banner486 { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.Banner728")]
		public string Banner728 { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.AssignedCredit")]
		public int AssignedCredit { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.UsedCredit")]
		public int UsedCredit { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.AvailableCredit")]
		public int AvailableCredit { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.CreditType")]
		public string CreditType { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.AdType")]
		public string AdType { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.ExpiryDate")] 
		public DateTime? ExpiryDate { get; set; }

		public int CustomerId { get; set; }

		public int? PictureId { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.Enabled")]
		public bool Enabled { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.Published")]
		public bool Published { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.Deleted")]
		public bool Deleted { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.CreditType")]
		public IList<SelectListItem> ListCreditType { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.AdType")]
		public IList<SelectListItem> ListAdType { get; set; }

		[SmartResourceDisplayName("Common.CreatedOn")]
		public DateTime? CreatedOn { get; set; }

		[SmartResourceDisplayName("Common.UpdatedOn")]
		public DateTime? UpdatedOn { get; set; }

		[SmartResourceDisplayName("AdCampaign.AvailableImpression")]
		public int AvailableImpression { get; set; }
		[SmartResourceDisplayName("AdCampaign.AvailableClick")]
		public int AvailableClicks { get; set; }

	}

	public class AdCampaignListModel : ModelBase
	{
		public AdCampaignListModel()
		{
			ListCreditType = new List<SelectListItem>();
			ListAdType = new List<SelectListItem>();
		}

		[SmartResourceDisplayName("Admin.AdCampaign.CampaignName")]
		[AllowHtml]
		public string SearchCampaignName { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.WebsiteUrl")]
		public string SearchWebsiteUrl { get; set; }

		public string CreditType { get; set; }

		public string AdType { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.CreditType")]
		public IList<SelectListItem> ListCreditType { get; set; }

		[SmartResourceDisplayName("Admin.AdCampaign.AdType")]
		public IList<SelectListItem> ListAdType { get; set; }

		public int AvailableImpression { get; set; }
		public int AvailableClicks { get; set; }
		public int GridPageSize { get; set; }
	}

	public class AdPacks
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public float Cost { get; set; }
		public int Visitor { get; set; }
		public int BannerImpression { get; set; }
		public int LoginAdCredit { get; set; }
		public int TransactionId { get; set; }
		public int CustomerId { get; set; }
	}

	public class VaccationMode
	{
		[SmartResourceDisplayName("Admin.VacationMode.Fields.CurrentExpiryDate")]
		public DateTime? CurrentExpiryDate { get; set; }
		[SmartResourceDisplayName("Admin.VacationMode.Fields.StartDate")]
		public DateTime StartDate { get; set; }
		[SmartResourceDisplayName("Admin.VacationMode.Fields.EndDate")]
		public DateTime EndDate { get; set; }
		public float AvailableBalance { get; set; }
		public int CustomerId { get; set; }
	}
}