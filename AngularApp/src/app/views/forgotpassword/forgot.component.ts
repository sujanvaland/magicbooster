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
  otpSuccess = false;
  constructor(private formBuilder: FormBuilder,
    private loginservice: LoginserviceService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) { }

ngOnInit (){
    this.login =this.formBuilder.group({
      Email: ['', [Validators.required]],
      Phone: ['', [Validators.required]],
      OTP : ['']
  });
}

get f() { return this.login.controls; }
phonenumber(inputtxt) {
  var phoneno = "/^\+?([0-9]{2})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{4})$/";
  if((inputtxt.match(phoneno)))
        {
      return true;
        }
      else
        {
        alert("message");
        return false;
        }
}
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
            if(res.Message == "otpsent"){
              this.toastr.success("OTP Sent Successfully","Success");
              this.otpSuccess = true;
              $('.loaderbo').hide();
            }
            else if(res.Message == "success"){
              this.toastr.success("New Password sent to your Phone Successfully","Success");
              $('.loaderbo').hide();
              this.router.navigate(['/login']);
            }
            else if(res.Message == "invalidphone"){
              this.toastr.error("Invalid Phone number","Fail");
       
              $('.loaderbo').hide();
            }
            else if(res.Message == "invalidusername"){
              this.toastr.error("Invalid Username","Fail");
    
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
