import { Component,OnInit } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import {LoginserviceService } from '../../services/loginservice.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import * as $ from 'jquery';
@Component({
  selector: 'app-dashboard',
  templateUrl: 'passwordrecovery.component.html'
})
export class PasswordRecoveryComponent implements OnInit{
  login: FormGroup;
  submitted = false;
  ConfirmNewPassword ="";
  NewPassword ="";
  constructor(private formBuilder: FormBuilder,
    private loginservice: LoginserviceService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) { }

ngOnInit (){
  this.login =this.formBuilder.group({
    NewPassword: ['', [Validators.required]],
    ConfirmNewPassword: ['', [Validators.required]]
});
}

get f() { return this.login.controls; }

onSubmit() {
        
        if (this.login.value.NewPassword == "") {
            this.toastr.error("Password is requried","Error");
            return;
        }
       
        if (this.login.value.ConfirmNewPassword == "") {
            this.toastr.error("Confirm NewPassword is requried","Error");
            return;
        }

        if (this.login.value.ConfirmNewPassword != this.login.value.NewPassword) {
          this.toastr.error("NewPassword and Confirm NewPassword did not matched","Error");
            return;
        }

        let token="";
        let email="";
        this.route.queryParams
      .subscribe(params => {
        if(params.token){
          token = params.token;
        }
        if(params.emai){
          email = params.emai;
        }
      });
        
      let passwordrecovery = {
        NewPassword : this.login.value.NewPassword,
        ConfirmNewPassword : this.login.value.ConfirmNewPassword
      }
      $('.loaderbo').show();
        this.loginservice.PasswordRecoveryConfirmPOST(token,email,passwordrecovery)
        .subscribe(
          res => {
            if(res.Message == "success"){
              this.toastr.success("Password reset successfull","Success");
            }
            else{
              this.toastr.error("Something is wrong, contact support","Error");
            }
            $('.loaderbo').hide();
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
