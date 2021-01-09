using SmartStore.Core.Domain.Hyip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Hyip
{
	public partial interface IBannerService
	{
		void InsertBanner(Banner banner);

		Banner GetBannerById(int bannerid = 0);

		List<Banner> GetAllBanners();

		void UpdateBanner(Banner banner);

		void DeleteBanner(int bannerid);
	}
}
