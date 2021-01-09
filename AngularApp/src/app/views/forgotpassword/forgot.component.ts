import { Component,OnInit } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import {LoginserviceService } from '../../services/loginservice.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import * as $ from 'jquery';
@Component({
  selector: 'app-dashboard',
  templateUrl: 'forgot.component.html'
})
export class ForgotComponent implements OnInit{
  login: FormGroup;
  submitted = false;
  constructor(private formBuilder: FormBuilder,
    private loginservice: LoginserviceService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) { }

ngOnInit (){
    this.login =this.formBuilder.group({
      Email: ['', [Validators.required,Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")]]
  });
}

get f() { return this.login.controls; }

    onSubmit() {
        this.submitted = true;
          // stop here if form is invalid
        if (this.login.invalid) {
            return;
        }
        $('.loaderbo').show();
        this.loginservice.PasswordRecovery(this.login.value)
        .subscribe(
          res => {
            if(res.Message == "success"){
              this.toastr.success("Password recovery link sent","Success");
              $('.loaderbo').hide();
            }
            else{
              this.toastr.error("Something is wrong, contact support","Error");
              $('.loaderbo').hide();
            }
          },
          err =>{
            this.toastr.error("Something went wrong, contact support","Error");
            $('.loaderbo').hide();
          } 
        )
    }

    onReset() {
        this.submitted = false;
        this.login.reset();
    }
}
