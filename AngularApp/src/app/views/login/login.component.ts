import { Component,OnInit } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import {LoginserviceService } from '../../services/loginservice.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import * as $ from 'jquery';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  templateUrl: 'login.component.html'
})
export class LoginComponent  implements OnInit { 
  login: FormGroup;
  submitted = false;
  Is2FAEnable : boolean = false;
  Pin2FA : string = "";
  constructor(private formBuilder: FormBuilder,
    private loginservice: LoginserviceService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) { }

ngOnInit (){
  $('.loaderbo').hide();
    this.login =this.formBuilder.group({
      Email: ['', Validators.required],
      Password:['', Validators.required]
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
        
        this.login.value.UsernamesEnabled = false;
        this.loginservice.loginUser(this.login.value)
        .subscribe(
          res => {
            if(res.Message == "success"){
              if(!res.data.Enable2FA){
                localStorage.setItem("token",res.token)
                localStorage.setItem("CustomerGuid",res.data.CustomerGuid)
                localStorage.setItem("CustomerId",res.data.Id)
                localStorage.setItem("CustomerName",res.data.FullName)
                $('.loaderbo').hide();
                this.router.navigate(['/dashboard']);
              }
              else{
                $('.loaderbo').hide();
                this.Is2FAEnable = res.data.Enable2FA;
              }
            }else{
              $('.loaderbo').hide();
              this.toastr.error(res.Message,"Error");
            }
          },
          err => console.log(err)
        )
    }

    onReset() {
        this.submitted = false;
        this.login.reset();
    }

    ValidatedPin2FA(){
      if(this.Pin2FA == ""){
        this.toastr.error("2FA pin is required","Error");
        return;
      }
      this.login.value.Pin2FA = this.Pin2FA

      $('.loaderbo').show();

      this.loginservice.ValidateLoginWith2FA(this.login.value)
      .subscribe(
        res => {
          if(res.Message == "success"){
              localStorage.setItem("token",res.token)
              localStorage.setItem("CustomerGuid",res.data.CustomerGuid)
              localStorage.setItem("CustomerId",res.data.Id)
              localStorage.setItem("CustomerName",res.data.FullName)
              $('.loaderbo').hide();
              this.router.navigate(['/dashboard']);
          }else{
            $('.loaderbo').hide();
            this.toastr.error(res.Message,"Error");
          }
        },
        err => console.log(err)
      )
    }
}
