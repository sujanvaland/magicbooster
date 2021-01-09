import { Component,OnInit } from '@angular/core';
import { Router } from '@angular/router';
// Import your library

import * as $ from 'jquery';
import { AdvertismentService } from '../../services/advertisment.service';

@Component({
  selector: 'faq-home',
  templateUrl: 'countrymanager.component.html'
})
export class CountryManagerComponent  implements OnInit { 
  constructor(
    private router: Router,
    private advertismenetSerive:AdvertismentService) { }
  
  Managers =[]
  MobileMenu = false;
  ngOnInit (){
    this.advertismenetSerive.GetCountryManager().subscribe(
      res => {
        if(res.Message == "success"){
          console.log(res.data);
          this.Managers = res.data;
          console.log(res.data);
        }
      }
    )
  }
   
  showMobileMenu(){
    this.MobileMenu = !this.MobileMenu;
  }
}
