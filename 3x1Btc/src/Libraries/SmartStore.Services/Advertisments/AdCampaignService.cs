using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStore.Core;
using SmartStore.Core.Data;
using SmartStore.Core.Domain.Advertisments;
using SmartStore.Core.Domain.Customers;

namespace SmartStore.Services.Advertisments
{
	public partial class AdCampaignService : IAdCampaignService
	{
		private readonly IRepository<AdCampaign> _addCampaignRepository;
		private readonly IRepository<YoutubeVideos> _youtubeRepository;
		private readonly IRepository<FacebookPost> _facebookRepository;
		private readonly IRepository<SupportRequest> _supportRepository;
		private readonly IRepository<FAQ> _faqRepository;
		private readonly IRepository<CountryManager> _countryManagerRepository;
		private readonly IRepository<Customer> _customerRepository;
		private readonly IDbContext _dbContext;
		public AdCampaignService(IRepository<AdCampaign> adrepository,
			IRepository<YoutubeVideos> youtubeRepository,
			IRepository<FacebookPost> facebookRepository,
			IRepository<SupportRequest> supportRepository,
			IRepository<FAQ> faqRepository,
			IDbContext dbContext,
			IRepository<CountryManager> countryManagerRepository,
			IRepository<Customer> customerRepository)
		{
			_addCampaignRepository = adrepository;
			_youtubeRepository = youtubeRepository;
			_facebookRepository = facebookRepository;
			_supportRepository = supportRepository;
			_faqRepository = faqRepository;
			_dbContext = dbContext;
			_countryManagerRepository = countryManagerRepository;
			_customerRepository = customerRepository;
		}
		public void DeleteAdCampaign(AdCampaign adCampaign)
		{
			if (adCampaign == null)
				throw new ArgumentNullException("adCampaign");

			adCampaign.Deleted = true;
			UpdateAdCampaign(adCampaign);
		}

		public AdCampaign GetAdCampaignById(int Id)
		{
			if (Id == 0)
				return null;

			return _addCampaignRepository.GetById(Id);
		}

		public IPagedList<AdCampaign> GetAdCampaigns(string Name = "", string WebsiteUrl = "", string CreditType = "", string AdType = "", int CustomerId = 0, int pageIndex = 0,
			int pageSize = int.MaxValue)
		{
			var query = _addCampaignRepository.Table;
			if (!Name.IsEmpty())
			{
				query = query.Where(x => Name.Contains(x.Name));
			}
			if (!WebsiteUrl.IsEmpty())
			{
				query = query.Where(x => WebsiteUrl.Contains(x.Name));
			}
			if (!CreditType.IsEmpty())
			{
				query = query.Where(x => x.CreditType == CreditType);
			}
			if (!AdType.IsEmpty())
			{
				query = query.Where(x => x.AdType == AdType);
			}
			if (CustomerId > 0)
			{
				query = query.Where(x => x.CustomerId == CustomerId);
			}

			query = query.Where(x=>x.Deleted == false).OrderByDescending(x => x.CreatedOnUtc);

			return new PagedList<AdCampaign>(query, pageIndex, pageSize);
		}

		public AdCampaign GetBannerAds(string Size)
		{
			var query = _addCampaignRepository.Table.Where(x=>x.Enabled == true 
			&& x.Deleted == false);

			if (Size == "125")
			{
				query = query.Where(x => x.Banner125 != "");
			}
			else if (Size == "486")
			{
				query = query.Where(x => x.Banner486 != "");
			}
			else if (Size == "728")
			{
				query = query.Where(x => x.Banner728 != "");
			}

			query = query.OrderByDescending(x => x.AvailableCredit).Take(1);

			var Ad = query.FirstOrDefault();

			Ad.AvailableCredit = Ad.AvailableCredit - 1;
			Ad.UsedCredit = Ad.UsedCredit + 1;
			if(Ad.AvailableCredit <= 0)
			{
				Ad.Enabled = false;
				Ad.ExpiryDate = DateTime.UtcNow;
			}

			return query.FirstOrDefault();
		}

		public AdCampaign GetAdByType(string AdType)
		{
			var query = _addCampaignRepository.Table.Where(x => x.Enabled == true
			&& x.Deleted == false && x.AdType == AdType);

			query = query.OrderByDescending(x => x.AvailableCredit).Take(1);

			var Ad = query.FirstOrDefault();

			Ad.AvailableCredit = Ad.AvailableCredit - 1;
			Ad.UsedCredit = Ad.UsedCredit + 1;
			if (Ad.AvailableCredit <= 0)
			{
				Ad.Enabled = false;
				Ad.ExpiryDate = DateTime.UtcNow;
			}

			return query.FirstOrDefault();
		}

		public void InsertAdCampaign(AdCampaign adCampaign)
		{
			if (adCampaign == null)
				throw new ArgumentNullException("adCampaign");

			_addCampaignRepository.Insert(adCampaign);
		}

		public void UpdateAdCampaign(AdCampaign adCampaign)
		{
			if (adCampaign == null)
				throw new ArgumentNullException("adCampaign");

			_addCampaignRepository.Update(adCampaign);
		}

		public List<AdCampaign> GetRandomAds(string AdType,string CreditType,string BannerSize,int NoOfAds)
		{
			SqlParameter pAdType = new SqlParameter();
			pAdType.ParameterName = "AdType";
			pAdType.Value = AdType;
			pAdType.DbType = DbType.String;

			SqlParameter pCreditType = new SqlParameter();
			pCreditType.ParameterName = "CreditType";
			pCreditType.Value = CreditType;
			pCreditType.DbType = DbType.String;

			SqlParameter pBannerSize = new SqlParameter();
			pBannerSize.ParameterName = "BannerSize";
			pBannerSize.Value = BannerSize;
			pBannerSize.DbType = DbType.String;

			SqlParameter pNoOfAds = new SqlParameter();
			pNoOfAds.ParameterName = "NoOfAds";
			pNoOfAds.Value = NoOfAds;
			pNoOfAds.DbType = DbType.Int32;

			var custTraffic = _dbContext.SqlQuery<AdCampaign>("Exec SpGetRandomAds @AdType,@CreditType,@BannerSize,@NoOfAds", pAdType,pCreditType,pBannerSize, pNoOfAds).ToList();
			return custTraffic;
		}

		public YoutubeVideos AddUpdateYoutubeVideo(YoutubeVideos youtubeVideos)
		{
			if(youtubeVideos.Id > 0)
			{
				_youtubeRepository.Update(youtubeVideos);
				return youtubeVideos;
			}
			else
			{
				youtubeVideos.CreatedDate = DateTime.Now;
				_youtubeRepository.Insert(youtubeVideos);
				return youtubeVideos;
			}
		}

		public IPagedList<YoutubeVideos> GetYoutubeVideos(bool? IsPaid, bool? Approved, int CustomerId,int PageIndex,int MaxCount)
		{
			var query = _youtubeRepository.Table.Where(x=>x.Deleted == false);
			if(CustomerId > 0)
			{
				query = query.Where(x => x.CustomerId == CustomerId);
			}
			if (IsPaid != null)
			{
				query = query.Where(x => x.IsPaid == IsPaid);
			}
			if (Approved != null)
			{
				query = query.Where(x => x.Approved == Approved);
			}
			query = query.OrderBy(x => x.Id);
			return new PagedList<YoutubeVideos>(query, PageIndex, MaxCount);
		}
		public FacebookPost AddUpdateFacebookPost(FacebookPost facebookPost)
		{
			if (facebookPost.Id > 0)
			{
				_facebookRepository.Update(facebookPost);
				return facebookPost;
			}
			else
			{
				_facebookRepository.Insert(facebookPost);
				return facebookPost;
			}
		}

		public IPagedList<FacebookPost> GetFacebookPost(bool? IsPaid, bool? Approved, int CustomerId, int PageIndex, int MaxCount)
		{
			var query = _facebookRepository.Table.Where(x => x.Deleted == false);
			if (CustomerId > 0)
			{
				query = query.Where(x => x.CustomerId == CustomerId);
			}
			if (IsPaid != null)
			{
				query = query.Where(x => x.IsPaid == IsPaid);
			}
			if (Approved != null)
			{
				query = query.Where(x => x.Approved == Approved);
			}
			query = query.OrderBy(x => x.Id);

			return new PagedList<FacebookPost>(query, PageIndex, MaxCount);
		}
		public SupportRequest AddUpdateSupportRequest(SupportRequest supportRequest)
		{
			if (supportRequest.Id > 0)
			{
				_supportRepository.Update(supportRequest);
				return supportRequest;
			}
			else
			{
				_supportRepository.Insert(supportRequest);
				return supportRequest;
			}
		}

		public IPagedList<SupportRequest> GetSupportRequest(bool? ShowNotReplied, int CustomerId, int PageIndex, int MaxCount)
		{
			var query = _supportRepository.Table.Where(x => x.Deleted == false);
			if (CustomerId > 0)
			{
				query = query.Where(x => x.CustomerId == CustomerId);
			}
			if(ShowNotReplied == true)
			{
				query = query.Where(x => x.Status == "Open");
			}

			query = query.OrderBy(x => x.Id);

			return new PagedList<SupportRequest>(query, PageIndex, MaxCount);
		}

		public SupportRequest GetTicketById(int Id)
		{
			return _supportRepository.GetById(Id);
		}

		public SupportRequest GetLatestSupportRequest(int CustomerId)
		{
			var query = _supportRepository.Table.Where(x => x.Deleted == false && x.CustomerId == CustomerId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

			return query;
		}

		public FAQ AddUpdateFAQ(FAQ faq)
		{
			if (faq.Id > 0)
			{
				_faqRepository.Update(faq);
				return faq;
			}
			else
			{
				_faqRepository.Insert(faq);
				return faq;
			}
		}

		public IList<FAQ> GetFaqs()
		{
			var query = _faqRepository.Table.Where(x => x.Deleted == false);
			query = query.OrderBy(x => x.Id);

			return query.ToList();
		}

		public IList<CountryManager> GetCountryManager()
		{
			var query = _countryManagerRepository.Table;
			query = query.OrderBy(x => x.Id);

			return query.ToList();
		}

		public IList<Customer> GetCountryManagerFromCustomer()
		{
			var customer = _customerRepository.Table;
			customer = customer.Where(x => x.IsCountryManager == true);

			return customer.ToList();
		}
	}
}
