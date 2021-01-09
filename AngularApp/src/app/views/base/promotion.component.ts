import { Component, Pipe,PipeTransform } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { AdvertismentService } from '../../services/advertisment.service';
import { DomSanitizer} from '@angular/platform-browser';
@Pipe({ name: 'newsafe' })
export class NewSafePipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) {}
  transform(url) {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
} 

@Component({
  templateUrl: 'promotion.component.html'
})
export class PromotionComponent {
  constructor(private advertismenetSerive:AdvertismentService,
    private toastr:ToastrService) { }
  Banner125 = environment.Banner125
  Banner468 = environment.Banner468
  Banner728 = environment.Banner728
  Banner250 = environment.Banner250
  Banner820 = environment.BannerFB
  CustomerId :string = localStorage.getItem("CustomerId");
  CompanyPresentation = environment.CompanyPresentation
  CompanyVideos = [];
  ngOnInit (){
    this.getYoutube();
  };

  getYoutube(){
    this.advertismenetSerive.GetYouTubeVideos(null,true,0,0,999).subscribe(res=>{
      if(res.Message == "success"){
        console.log(res.data)
        this.CompanyVideos = res.data;
      }
    })
  }
  
  copyInputMessage(inputElement) {  
    inputElement.select();  
    document.execCommand('copy');  
    inputElement.setSelectionRange(0, 0);  
    this.toastr.success("Banner Url copied !"); 
  }  
}
