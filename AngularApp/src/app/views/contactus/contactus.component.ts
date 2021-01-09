import { Component,OnInit } from '@angular/core';
import { FormGroup,FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
// Import your library

import * as $ from 'jquery';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'faq-home',
  templateUrl: 'contactus.component.html'
})
export class ContactUsComponent  implements OnInit { 
  constructor(private commonservice: CommonService,
    private formBuilder: FormBuilder,
    private router: Router) { }
  
  Managers =[]
  MobileMenu = false;
  contactUs: FormGroup;
submitted = false;

  ngOnInit (){
    this.contactUs =this.formBuilder.group({
      Fname: '',
      Lname:'',
      Email:'',
      Subject:'',
      Enquiry:''
    });
  }
   
  showMobileMenu(){
    this.MobileMenu = !this.MobileMenu;
  }

  onSubmit() {
    this.commonservice.ContactUsNew(this.contactUs.value).subscribe();
  }
}
