using SmartStore.Web.Framework;
using SmartStore.Web.Framework.Modelling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartStore.Admin.Models.Hyip
{
	public class BannerModel : ModelBase
	{
		public BannerModel()
		{
			bannerslist = new List<Banners>();
		}
		public int Id { get; set; }
		[SmartResourceDisplayName("Admin.HYIP.Banner.Size")]
		public string Size { get; set; }

		[UIHint("Picture")]
		[SmartResourceDisplayName("Admin.HYIP.Banner.Picture")]
		public int PictureId { get; set; }

		[SmartResourceDisplayName("Admin.HYIP.Banner.Published")]
		public bool Published { get; set; }

		[SmartResourceDisplayName("Admin.HYIP.Banner.Deleted")]
		public bool Deleted { get; set; }

		[SmartResourceDisplayName("Admin.HYIP.Banner.DisplayOrder")]
		public int DisplayOrder { get; set; }
		public string ReferralLink { get; set; }
		public List<Banners> bannerslist { get; set; }
	}

	public class Banners
	{
		public int BannerId { get; set; }
		public int PictureId { get; set; }
		public string Size { get; set; }
		public bool Published { get; set; }
		public string BannerUrl { get; set; }
	}

}