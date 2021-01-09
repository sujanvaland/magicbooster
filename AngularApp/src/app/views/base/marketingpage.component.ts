import { Component, ViewChild } from '@angular/core';
import { AdvertismentService } from '../../services/advertisment.service';
import { ToastrService } from 'ngx-toastr';
import { ModalDirective } from 'ngx-bootstrap';

@Component({
  templateUrl: 'marketingpage.component.html'
})
export class MarketingPageComponent {

  constructor(private advertismenetSerive:AdvertismentService,
    private toastr: ToastrService) { }

  CustomerId :string = localStorage.getItem("CustomerId");
  youtubeVideo = {
    VideoLink:"",
    CustomerId:this.CustomerId
  };
  facebookPost = {
    VideoLink:"",
    CustomerId:this.CustomerId
  };
  Title = "";
  Type = "";
  AssignedCredit = 1000;
  LinkUrl ="";
  AvailableCredits = 0;
  AdCampaing = []
  @ViewChild('youtubeModal') public youtubeModal: ModalDirective;


  ngOnInit (){
      this.getWebsiteCampaign();
      this.getAvailableCredits();
  };

  getYoutube(){
    this.advertismenetSerive.GetYouTubeVideos(false,true,this.CustomerId,0,999).subscribe(res=>{
      if(res.Message == "sucess"){
        console.log("youtube")
        console.log(res.data)
      }
    })
  }
  getFacebookPost(){
    this.advertismenetSerive.GetFacebookPosts(false,true,this.CustomerId,0,999).subscribe(res=>{
      if(res.Message == "success"){
        console.log("facebook")
        console.log(res.data)
      }
    })
  }
  getAvailableCredits(){
    this.advertismenetSerive.GetAvailableAdCredits(this.CustomerId).subscribe(res=>{
      if(res.Message == "success"){
        this.AvailableCredits = res.data[0].AvailableClick;
        console.log(res.data[0].AvailableClick)
      }
    })
  }

  getWebsiteCampaign(){
    this.advertismenetSerive.GetWebsiteCampaigns(this.CustomerId,0,999).subscribe(res=>{
      console.log(res);
      if(res.Message == "success"){
        console.log("campaigns")
        this.AdCampaing = res.data;
        console.log(res.data)
      }
    })
  }
  openModel(type){
    this.Type = type;
    this.LinkUrl = "";
    if(type == "youtube"){
      this.Title = "Add Youtube Video Link"
    }
    if(type == "facebook"){
      this.Title = "Add Facebook Post Link"
    }
    if(type == "website"){
      this.Title = "Add Website Link"
    }
    if(type == "youtubead"){
      this.Title = "Add Youtube Video Link"
    }
    if(type == "facebookad"){
      this.Title = "Add Facebook Page"
    }
    this.youtubeModal.show();
  }

  is_url(str)
  {
    let regexp =  /^(?:(?:https?|ftp):\/\/)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})))(?::\d{2,5})?(?:\/\S*)?$/;
    if (regexp.test(str))
    {
      return true;
    }
    else
    {
      return false;
    }
  }
  saveLink(){
    if(!this.is_url(this.LinkUrl)){
      this.toastr.error("Invalid Url","Invalid");
      return;
    }
   
    if(this.Type == "youtube"){
      this.youtubeVideo.VideoLink = this.LinkUrl;
      this.advertismenetSerive.AddYouTubeVideo(this.youtubeVideo)
      .subscribe(res =>{
        if(res.Message == "success"){
          this.toastr.success("Your video ad is submitted !!");
          this.youtubeModal.hide();
        }
        else{
          this.toastr.error("Something went wrong");
        }
      },
      err =>{
        this.toastr.error("Something went wrong");
      })
    }
    else if(this.Type == "website" || this.Type == "youtubead" || this.Type == "facebookad"){
      if(this.AssignedCredit > this.AvailableCredits){
        this.toastr.error("You do not have enough credit for this campaign","Invalid");
        return;
      }
      let adcampaign = {
        AssignedCredit : this.AssignedCredit,
        AdType : this.Type,
        UsedCredit:0,
        AvailableCredit:0,
        CustomerId:this.CustomerId,
        WebsiteUrl:this.LinkUrl
      }
      this.advertismenetSerive.AddWebsiteUrl(adcampaign)
      .subscribe(res =>{
        if(res.Message == "success"){
          this.toastr.success("Your adcampaign is submitted !!");
          this.youtubeModal.hide();
        }
        else{
          this.toastr.error("Something went wrong");
        }
      },
      err =>{
        this.toastr.error("Something went wrong");
      })
    }
  }
}
