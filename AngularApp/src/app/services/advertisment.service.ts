import { Injectable } from '@angular/core';
import { HttpClient,HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class AdvertismentService {

  constructor(private http: HttpClient) { }
  
  AddYouTubeVideo(youtubevideo)
  {
    let url = environment.baseApiUrl + "Ads/AddYoutTubeVideo";
    return this.http.post<any>(url,youtubevideo)
  }

  AddFacebookPost(facebookpost)
  {
    let url = environment.baseApiUrl + "Ads/AddFacebookPost";
    return this.http.post<any>(url,facebookpost)
  }

  AddWebsiteUrl(adCampaign)
  {
    let url = environment.baseApiUrl + "Ads/AddWebsiteUrl";
    return this.http.post<any>(url,adCampaign)
  }

  GetYouTubeVideos(IsPaid,Approved,CustomerId,PageIndex,MaxCount){
    let url = environment.baseApiUrl + "Ads/GetYouTubeVideos?IsPaid="+IsPaid+
    "&Approved="+Approved+"&CustomerId="+CustomerId+"&PageIndex="+PageIndex+"&MaxCount="+MaxCount;
    return this.http.get<any>(url)
  }

  GetFacebookPosts(IsPaid,Approved,CustomerId,PageIndex,MaxCount){
    let url = environment.baseApiUrl + "Ads/GetFacebookPosts?CustomerId="+CustomerId;
    return this.http.get<any>(url)
  }

  GetWebsiteCampaigns(CustomerId,PageIndex,MaxCount){
    let url = environment.baseApiUrl + "Ads/GetWebsiteCampaigns?CustomerId="+CustomerId+"&PageIndex="+PageIndex+"&MaxCount="+MaxCount;
    return this.http.post<any>(url,null)
  }
  
  GetAvailableAdCredits(CustomerId){
    let url = environment.baseApiUrl + "Ads/GetAvailableAdCredits?CustomerId="+CustomerId;
    return this.http.get<any>(url)
  }

  GetRandomAds(CustomerId,AdType){
    let url = environment.baseApiUrl + "Ads/GetRandomAds?CustomerId="+CustomerId+"&AdType="+AdType;
    return this.http.get<any>(url)
  }

  GetCountryManager(){
    let url = environment.baseApiUrl + "Login/GetCountryManager";
    return this.http.get<any>(url)
  }
}
