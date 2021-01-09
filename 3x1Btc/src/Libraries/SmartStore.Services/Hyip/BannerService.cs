using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Hyip;

namespace SmartStore.Services.Hyip
{
	public partial class BannerService : IBannerService
	{
		private readonly IRepository<Banner> _bannerRepository;
		public BannerService(IRepository<Banner> bannerRepository)
		{
			_bannerRepository = bannerRepository;
		}
		public void DeleteBanner(int bannerid)
		{
			var banner = GetBannerById(bannerid);
			banner.Deleted = true;
			UpdateBanner(banner);
		}

		public List<Banner> GetAllBanners()
		{
			return _bannerRepository.Table.ToList();
		}

		public Banner GetBannerById(int bannerid = 0)
		{
			return _bannerRepository.Table.Where(b => b.Id == bannerid).FirstOrDefault();
		}

		public void InsertBanner(Banner banner)
		{
			Guard.NotNull(banner, nameof(Banner));

			_bannerRepository.Insert(banner);
		}

		public void UpdateBanner(Banner banner)
		{
			Guard.NotNull(banner, nameof(Banner));

			_bannerRepository.Update(banner);
		}
	}
}
