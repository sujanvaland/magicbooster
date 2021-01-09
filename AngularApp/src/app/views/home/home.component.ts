import { Component,OnInit } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import {LoginserviceService } from '../../services/loginservice.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { environment } from '../../../environments/environment';
import { CommonService } from '../../services/common.service';
// Import your library
import * as $ from 'jquery';


@Component({
  selector: 'app-home',
  templateUrl: 'home.component.html'
})
export class HomeComponent  implements OnInit { 
  login: FormGroup;
  submitted = false;
  telegram = environment.telegram;
  facebook = environment.facebook;
  twitter = environment.twitter;
  TotalPurchase = 0;
  TotalMembers24Hours = 0;
  TotalMembers = 0;
  SitePayout = 0;
  CustomerId : number = parseInt(localStorage.getItem("CustomerId"));
  ShowLogin = true;
  LaunchDate = "";
  Phases = {};
  MobileMenu = true;
  referralId = "0";
  Images = ['assets/img/bg-1.jpg', 'assets/img/bg-2.jpg', 'assets/img/bg-3.jpg'];
  constructor(private formBuilder: FormBuilder,
    private commonservice: CommonService,
    private route: ActivatedRoute,
    private router: Router) { }

ngOnInit (){
    this.Phases = environment.Phases;
    this.commonservice.GetSiteStats().subscribe(
      res =>{
        this.TotalMembers = res.data.TotalCustomer;
        this.TotalPurchase = res.data.TotalSiteDeposit;
        this.TotalMembers24Hours = res.data.OnlineVistors;
        this.SitePayout = res.data.TotalSiteWithdrawal;
        this.LaunchDate = res.data.LaunchDate;
      }
    )
    
    this.route.queryParams
    .subscribe(params => {
      if(params.r){
        this.referralId = params.r;
        localStorage.setItem("inviter",this.referralId);
      }
      else{
        this.referralId = localStorage.getItem("inviter");
      }
    });

    
    

    if(this.CustomerId > 0){
      this.ShowLogin = false;
    }
    let inviter = "";
    this.route.queryParams
    .subscribe(params => {
      if(params.r){
        inviter = params.r;
        localStorage.setItem("inviter",inviter);
      }
      else{
        inviter = localStorage.getItem("inviter");
      }
      if(inviter == null || inviter == undefined || inviter == ""){
        inviter = "2";
        localStorage.setItem("inviter",inviter);
      }
     
    });

    
    // Set the date we're counting down to
    var countDownDate = new Date(this.LaunchDate).getTime();

    // Update the count down every 1 second
    var x = setInterval(function() {

      // Get today's date and time
      var now = new Date().getTime();

      // Find the distance between now and the count down date
      var distance = countDownDate - now;

      // Time calculations for days, hours, minutes and seconds
      var days = Math.floor(distance / (1000 * 60 * 60 * 24));
      var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
      var seconds = Math.floor((distance % (1000 * 60)) / 1000);
      
      // Display the result in the element with id="demo"
      var html = "<div class='cd-item'><span>"+days+"</span><p>Days</p></div><div class='cd-item'><span>"+hours+"</span><p>Hours</p></div>";
      html = html + "<div class='cd-item'><span>"+minutes+"</span><p>Minutes</p></div><div class='cd-item'><span>"+seconds+"</span><p>Seconds</p></div>"
      if(document.getElementById("countdown")){
        document.getElementById("countdown").innerHTML = html;
        // If the count down is finished, write some text
        if (distance < 0) {
          clearInterval(x);
          document.getElementById("demo").innerHTML = "EXPIRED";
        }
      }
      
    }, 1000);
  
}
 
showMobileMenu(){
  this.MobileMenu = !this.MobileMenu;
}

SlideOptions = { 
  loop: true,
  margin: 0,
  nav: false,
  items: 1,
  dots: true,
  animateOut: 'fadeOut',
  animateIn: 'fadeIn',
  navText: ['<i class="flaticon-left-arrow-1"></i>', '<i class="flaticon-right-arrow-1"></i>'],
  smartSpeed: 1200,
  autoplay: true,
};  
CarouselOptions = { items: 3, dots: true, nav: true };  

}
