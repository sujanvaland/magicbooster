import { Component, ViewChild, Pipe, PipeTransform } from '@angular/core';
import { CommonService } from '../../services/common.service';
import { ModalDirective } from 'ngx-bootstrap';
import { AdvertismentService } from '../../services/advertisment.service';
import { DomSanitizer} from '@angular/platform-browser';
import * as $ from 'jquery';
@Pipe({ name: 'safe' })
export class SafePipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) {}
  transform(url) {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
} 

@Component({
  templateUrl: 'surfads.component.html'
})
export class SurfAdsComponent {

  constructor(private advertismentservice:AdvertismentService) { }
  CustomerId:string = localStorage.getItem("CustomerId");
  AdCampaign = { WebsiteUrl:'' };
  HideAds = false;
  
  @ViewChild('infoModal') public infoModal: ModalDirective;
  ngOnInit(): void {
      this.GetAds();
      let t = this;
      let timerId = setInterval(countdown, 1000);
      let timeleft = 20;
      function countdown() {
        var elem = document.getElementById('divTimer1');
        if (timeleft == -1) {
          t.GetAds();
          timeleft = 25;
        } else {
          elem.innerHTML = timeleft + ' more';
          timeleft--;
        }
      }
  }

  

  GetAds(){
    $('.loaderbo').show();
    this.advertismentservice.GetRandomAds(this.CustomerId,"website")
    .subscribe(
      res => {
        if(res.Message == "success"){
          $('.loaderbo').hide();
          if(res.data.length > 0){
            this.AdCampaign = res.data[0];
            this.HideAds= false;
          }
          else{
            $('.loaderbo').hide();
            this.HideAds= true;
          }
        }
        
      },
      err => console.log(err)
    )
  }
}
