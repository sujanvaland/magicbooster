import { Component,OnInit } from '@angular/core';
import { Router } from '@angular/router';
// Import your library

import * as $ from 'jquery';

@Component({
  selector: 'faq-home',
  templateUrl: 'faq.component.html'
})
export class FaqComponent  implements OnInit { 
  constructor(
    private router: Router) { }
  
  MobileMenu = false;
  ngOnInit (){
      
  }
   
  showMobileMenu(){
    this.MobileMenu = !this.MobileMenu;
  }
}
