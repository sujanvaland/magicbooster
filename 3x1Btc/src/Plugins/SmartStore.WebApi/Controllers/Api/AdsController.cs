using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartStore.Core;
using SmartStore.Core.Domain.Common;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Domain.Forums;
using SmartStore.Core.Domain.Localization;
using SmartStore.Core.Domain.Media;
using SmartStore.Core.Domain.Orders;
using SmartStore.Core.Domain.Tax;
using SmartStore.Core.Localization;
using SmartStore.Core.Logging;
using SmartStore.Services;
using SmartStore.Services.Common;
using SmartStore.Services.Customers;
using SmartStore.Services.Directory;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartStore.Core.Domain.Hyip;
using SmartStore.Admin.Models.Investment;
using SmartStore.Admin.Models.PaymentMethods;
using SmartStore.Admin;
using SmartStore.Services.Advertisments;
using SmartStore.Core.Domain.Advertisments;
using System.Collections.Generic;
using SmartStore.Web.Framework.WebApi.Security;

namespace SmartStore.WebApi.Controllers.Api
{
	[CustomWebApiAuthenticate]
	public class AdsController : ApiController
    {
        #region Fields
        public ICommonServices Services { get; set; } 
		private readonly IAdCampaignService _adCampaignService;
		private readonly ICustomerService _customerService;
		private readonly IGenericAttributeService _genericAttributeService;
		#endregion

		#region Ctor

		public AdsController(
			IAdCampaignService adCampaignService,
			ICustomerService customerService,
			IGenericAttributeService genericAttributeService)
        {
			_adCampaignService = adCampaignService;
			_customerService = customerService;
			_genericAttributeService = genericAttributeService;
		}

		#endregion

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("AddYoutTubeVideo")]
		public HttpResponseMessage AddYoutTubeVideo(YoutubeVideos youtubeVideos)
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
				if (customerguid != null)
				{
					var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
					if (youtubeVideos.CustomerId != cust.Id)
					{
						return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
					}
				}
				if (youtubeVideos.VideoLink == "")
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Invalid Url", data = "" });
				}
				youtubeVideos.CustomerId = youtubeVideos.CustomerId;
				youtubeVideos.NoOfViews = 0;
				youtubeVideos.Price = 0;
				youtubeVideos.IsPaid = false;
				youtubeVideos.Deleted = false;
				youtubeVideos.CreatedDate = DateTime.Now;
				youtubeVideos.Approved = false;
				var youtubevideo = _adCampaignService.AddUpdateYoutubeVideo(youtubeVideos);
				var c = JsonConvert.SerializeObject(youtubevideo, new JsonSerializerSettings
				{
					Formatting = Newtonsoft.Json.Formatting.Indented,
					ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
				});
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = c });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("AddFacebookPost")]
		public HttpResponseMessage AddFacebookPost(FacebookPost facebookPost)
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
				if (customerguid != null)
				{
					var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
					if (facebookPost.CustomerId != cust.Id)
					{
						return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
					}
				}
				if (facebookPost.VideoLink == "")
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Invalid Url", data = "" });
				}
				facebookPost.CustomerId = facebookPost.CustomerId;
				facebookPost.NoOfLikes = 0;
				facebookPost.Price = 0;
				facebookPost.IsPaid = false;
				facebookPost.Deleted = false;
				facebookPost.CreatedDate = DateTime.Now;
				facebookPost.Approved = false;
				var facebookpost = _adCampaignService.AddUpdateFacebookPost(facebookPost);
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = facebookpost });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("AddWebsiteUrl")]
		public HttpResponseMessage AddWebsiteUrl(AdCampaign adCampaign)
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
				if (customerguid != null)
				{
					var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
					if (adCampaign.CustomerId != cust.Id)
					{
						return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
					}
				}
				if (adCampaign.WebsiteUrl == "")
				{
					return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Invalid Url", data = "" });
				}
				adCampaign.CreditType = "click";
				adCampaign.AdType = adCampaign.AdType;
				adCampaign.AssignedCredit = adCampaign.AssignedCredit;
				adCampaign.CustomerId = adCampaign.CustomerId;
				adCampaign.Deleted = false;
				adCampaign.Enabled = false;
				adCampaign.CreatedOnUtc = DateTime.Now;
				adCampaign.UpdatedOnUtc = DateTime.Now;
				_adCampaignService.InsertAdCampaign(adCampaign);
				var c = JsonConvert.SerializeObject(adCampaign, new JsonSerializerSettings
				{
					Formatting = Newtonsoft.Json.Formatting.Indented,
					ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
				});
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = c });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("AddFAQ")]
		public HttpResponseMessage AddFAQ(FAQ faq)
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				var faqs = _adCampaignService.AddUpdateFAQ(faq);
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = faqs });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("AddSupportTicket")]
		public HttpResponseMessage AddSupportTicket(SupportRequest supportRequest)
		{
			var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
			var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
			if (customerguid != null)
			{
				if (supportRequest.CustomerId != cust.Id)
				{
					return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
				}
			}
			var LastReq = _adCampaignService.GetLatestSupportRequest(cust.Id);
			if(LastReq.CreatedDate.AddDays(30) >= DateTime.Now.Date)
			{
				return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 1, Message = "Request Allowed Once in Month" });
			}
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				supportRequest.Name = cust.GetFullName();
				supportRequest.Email = cust.Email;
				supportRequest.Status = "Open";
				if (supportRequest.Id > 0)
				{
					supportRequest.LastReplied = supportRequest.Message;
				}
				else
				{
					supportRequest.CreatedDate = DateTime.Now;
				}
				var supportRequests = _adCampaignService.AddUpdateSupportRequest(supportRequest);
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = "" });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetAvailableAdCredits")]
		public HttpResponseMessage GetAvailableAdCredits(int CustomerId)
		{
			var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
			if (customerguid != null)
			{
				var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
				if (CustomerId != cust.Id)
				{
					return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
				}
			}
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				var availableTraffic = _customerService.GetAvailableCredits(CustomerId);
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = availableTraffic });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetRandomAds")]
		public HttpResponseMessage GetRandomAds(int CustomerId,string AdType)
		{
			var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
			var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
			if (customerguid != null)
			{
				if (CustomerId != cust.Id)
				{
					return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
				}
			}
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				var adstoshow = _adCampaignService.GetRandomAds(AdType,"click","",1).ToList();
				var customer = cust;
				_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.LastSurfDate, DateTime.Now);
				var noOfAdsSurfed = customer.GetAttribute<int>(SystemCustomerAttributeNames.NoOfAdsSurfed);
				var lastsurfed = customer.GetAttribute<DateTime>(SystemCustomerAttributeNames.LastSurfDate);
				noOfAdsSurfed = noOfAdsSurfed + 1;
				if (noOfAdsSurfed == 10 && lastsurfed.Date == DateTime.Today)
				{
					_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.NextSurfDate, DateTime.Now.AddHours(24));
				}
				_genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.NoOfAdsSurfed, noOfAdsSurfed);
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = adstoshow });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetYouTubeVideos")]
		public HttpResponseMessage GetYouTubeVideos(bool? IsPaid, bool? Approved, int CustomerId, int PageIndex, int MaxCount)
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				List<YoutubeVideos> list = new List<YoutubeVideos>();
				var facebookPosts = _adCampaignService.GetYoutubeVideos(IsPaid, Approved, CustomerId, 0, int.MaxValue).ToList();
				foreach (var fblink in facebookPosts)
				{
					YoutubeVideos fp = new YoutubeVideos();
					fp.VideoLink = fblink.VideoLink;
					fp.CustomerId = fblink.CustomerId;
					fp.NoOfViews = fblink.NoOfViews;
					fp.IsPaid = fblink.IsPaid;
					fp.Approved = fblink.Approved;
					fp.Price = fblink.Price;
					fp.CreatedDate = fblink.CreatedDate;
					list.Add(fp);
				}
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = list });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetFacebookPosts")]
		public HttpResponseMessage GetFacebookPosts(int CustomerId)
		{
			var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
			if (customerguid != null)
			{
				var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
				if (CustomerId != cust.Id)
				{
					return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
				}
			}
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				List<FacebookPost> list = new List<FacebookPost>();
				var facebookPosts = _adCampaignService.GetFacebookPost(false, true, CustomerId, 0, int.MaxValue).ToList();
				foreach(var fblink in facebookPosts)
				{
					FacebookPost fp = new FacebookPost();
					fp.VideoLink = fblink.VideoLink;
					fp.CustomerId = fblink.CustomerId;
					fp.NoOfLikes = fblink.NoOfLikes;
					fp.IsPaid = fblink.IsPaid;
					fp.Approved = fblink.Approved;
					fp.Price = fblink.Price;
					fp.CreatedDate = fblink.CreatedDate;
					list.Add(fp);
				}
				
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = list });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpPost]
		[System.Web.Http.ActionName("GetWebsiteCampaigns")]
		public HttpResponseMessage GetWebsiteCampaigns(int CustomerId, int PageIndex, int MaxCount)
		{
			var customerguid = Request.Headers.GetValues("CustomerGUID").FirstOrDefault();
			if (customerguid != null)
			{
				var cust = _customerService.GetCustomerByGuid(Guid.Parse(customerguid));
				if (CustomerId != cust.Id)
				{
					return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = 0, Message = "something went wrong" });
				}
			}
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				List<AdCampaign> list = new List<AdCampaign>();
				var adCampaigns = _adCampaignService.GetAdCampaigns("", "", "", "", CustomerId, 0, int.MaxValue).ToList();
				foreach(var ads in adCampaigns)
				{
					AdCampaign adCampaign = new AdCampaign();
					adCampaign.Id = ads.Id;
					adCampaign.WebsiteUrl = ads.WebsiteUrl;
					adCampaign.AssignedCredit = ads.AssignedCredit;
					adCampaign.UsedCredit = ads.UsedCredit;
					adCampaign.AdType = ads.AdType;
					adCampaign.CreatedOnUtc = ads.CreatedOnUtc;
					adCampaign.Enabled = ads.Enabled;
					list.Add(adCampaign);
				}
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success",data = list });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetSupportRequest")]
		public HttpResponseMessage GetSupportRequest(bool? ShowNotReplied, int CustomerId, int PageIndex, int MaxCount)
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				var supportRequests = _adCampaignService.GetSupportRequest(false, CustomerId, PageIndex, MaxCount);
				List<SupportRequest> list = new List<SupportRequest>();
				foreach(var ticket in supportRequests)
				{
					SupportRequest supportRequest = new SupportRequest();
					supportRequest.Id = ticket.Id;
					supportRequest.Subject = ticket.Subject;
					supportRequest.Message = ticket.Message;
					supportRequest.CustomerId = ticket.Id;
					supportRequest.CreatedDate = ticket.CreatedDate;
					supportRequest.LastReplied = ticket.LastReplied;
					//supportRequest.Customer = ticket.Customer;
					list.Add(supportRequest);
				}
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = list });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetFaqs")]
		public HttpResponseMessage GetFaqs()
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				var faqs = _adCampaignService.GetFaqs();
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = faqs });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}

		[System.Web.Http.HttpGet]
		[System.Web.Http.ActionName("GetCountryManager")]
		public HttpResponseMessage GetCountryManager()
		{
			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			try
			{
				var newsletters = _adCampaignService.GetCountryManagerFromCustomer();
				//var newsletters = _adCampaignService.GetCountryManager();
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "success", data = newsletters });
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new { code = 0, Message = "Some thing went wrong" });
			}
		}
	}
}