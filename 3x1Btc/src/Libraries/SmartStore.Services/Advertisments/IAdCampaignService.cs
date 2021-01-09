using SmartStore.Core;
using SmartStore.Core.Domain.Advertisments;
using SmartStore.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Services.Advertisments
{
	public partial interface IAdCampaignService
	{
		AdCampaign GetAdCampaignById(int Id);

		void InsertAdCampaign(AdCampaign adCampaign);

		void UpdateAdCampaign(AdCampaign adCampaign);

		void DeleteAdCampaign(AdCampaign adCampaign);
		SupportRequest GetTicketById(int Id);
		SupportRequest GetLatestSupportRequest(int CustomerId);
		IPagedList<AdCampaign> GetAdCampaigns(string Name = "", string WebsiteUrl = "", string CreditType = "",
			string AdType = "", int CustomerId = 0,int PageIndex =0,int PageSize = int.MaxValue);

		AdCampaign GetAdByType(string AdType);

		List<AdCampaign> GetRandomAds(string AdType, string CreditType, string BannerSize, int NoOfAds);
		AdCampaign GetBannerAds(string Size);
		YoutubeVideos AddUpdateYoutubeVideo(YoutubeVideos youtubeVideos);
		IPagedList<YoutubeVideos> GetYoutubeVideos(bool? IsPaid, bool? Approved, int CustomerId, int PageIndex, int MaxCount);
		IPagedList<FacebookPost> GetFacebookPost(bool? IsPaid, bool? Approved, int CustomerId, int PageIndex, int MaxCount);
		IPagedList<SupportRequest> GetSupportRequest(bool? ShowNotReplied, int CustomerId, int PageIndex, int MaxCount);
		FacebookPost AddUpdateFacebookPost(FacebookPost facebookPost);
		SupportRequest AddUpdateSupportRequest(SupportRequest supportRequest);
		FAQ AddUpdateFAQ(FAQ faq);
		IList<FAQ> GetFaqs();
		IList<CountryManager> GetCountryManager();
		IList<Customer> GetCountryManagerFromCustomer();
	}
}
