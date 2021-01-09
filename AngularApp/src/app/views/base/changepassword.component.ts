import { Component ,OnInit } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';
import { CommonService } from '../../services/common.service';
import { LoginserviceService } from '../../services/loginservice.service';
import * as $ from 'jquery';
import { ToastrService } from 'ngx-toastr';

@Component({
  templateUrl: 'changepassword.component.html'
})
export class ChangePasswordComponent implements OnInit{

accountdetail: FormGroup;
submitted = false;
changepassword : FormGroup;

constructor(private formBuilder: FormBuilder,
  private customerservice:CustomerService,
  private commonservice:CommonService,
  private loginserviceService:LoginserviceService,
  private toastr:ToastrService) { }

CustomerId:string = localStorage.getItem("CustomerId");
CustomerInfoModel = { FirstName:'',LastName:'',BitcoinAddress:'',AccountNumber:'',NICR:'',BankName:'',AccountHolderName:'',CountryId:0,Gender:'',Enable2FA:false};
CurrencyCode:string;
Countries=[];
Is2FAEnable : boolean;
Is2FAEnableAlready : boolean;
EnableDisableText = "";
ManualEntryCode = "";
QRCodeUrl = "";
Pin2FA = "";
ShowPinInput = false;
ShowQRCode = false;
ngOnInit(): void {
  this.changepassword = this.formBuilder.group({
    OldPassword: ['', Validators.required],
    NewPassword:['', Validators.required],
    ConfirmNewPassword:['', Validators.required],
  });

  this.customerservice.GetCustomerInfo(this.CustomerId)
  .subscribe(
    res => {
      this.CustomerInfoModel = res.data;
      this.Is2FAEnable = this.CustomerInfoModel.Enable2FA;
      this.Is2FAEnableAlready = this.Is2FAEnable;
      console.log(this.Is2FAEnable);
      this.EnableDisableText = (this.Is2FAEnable) ? "2FA is Enabled for your account, You can Disable 2FA by untick of checkbox" : "2FA is Disabled for your account, you can Enable now";
      if(this.Is2FAEnable){
        this.ShowQRCode = false;
      }
      $('.loaderbo').hide();
    },
    err => console.log(err)
  )
}
get f() { return this.changepassword.controls; }
    ChangePassword() {
     
      // stop here if form is invalid
      if (this.changepassword.invalid) {
          this.toastr.error("Enter all fields please","Error");
          return;
      }

      if(this.changepassword.value.NewPassword != this.changepassword.value.ConfirmNewPassword){
        this.toastr.error("New and confirm password field did not match","Error");
        return;
      }

      $('.loaderbo').show();
      this.changepassword.value.CustomerId = this.CustomerId;
      this.loginserviceService.ChangePassword(this.changepassword.value)
      .subscribe(
        res =>{
          console.log(res.Message)
          if(res.Message === "success"){
            this.toastr.success("Your password has been updated","Success");
          }
          else{
            this.toastr.error(res.Message);
          }
          $('.loaderbo').hide();
        },
        err => {
          this.toastr.error("Something went wrong, contact support","Error")
        }
      )
    }

    onReset() {
        this.submitted = false;
        this.changepassword.reset();
    }

    Setup2FA(){
      if(this.Is2FAEnable){
        if(this.Is2FAEnableAlready){
          this.ShowQRCode = false;
          this.ShowPinInput = true;
        }
        else{
          $('.loaderbo').show();
          this.customerservice.Setup2FA(this.CustomerId).subscribe(
            res => {
              if(res.Message == "success"){
                this.ManualEntryCode = res.data.manualEntrySetupCode;
                this.QRCodeUrl = res.data.qrCodeImageUrl;
                this.ShowQRCode = true;
                this.ShowPinInput = true;
              }
              $('.loaderbo').hide();
            },
            err =>{
              console.log(err);
              $('.loaderbo').hide();
            }
          )
        }
      }else{
        this.ShowQRCode = false;
        this.ShowPinInput = true;
        $('.loaderbo').hide();
      }
    }

    onCancel2FA(){
      
    }

    validate2FA(){
        if(this.Pin2FA == ""){
          this.toastr.error("Enter OTP", "Error")
        }

        $('.loaderbo').show();
        let PinValidateRequest = {
          CustomerId : this.CustomerId,
          pin2FA:this.Pin2FA,
          enableRequest:this.Is2FAEnable
        }

        this.customerservice.Validate2FA(PinValidateRequest).subscribe(
          res =>{
             if(res.Message == "success" && res.data.valid){
                if(res.data.request){
                  this.Is2FAEnable = true;
                  this.EnableDisableText = (this.Is2FAEnable) ? "2FA is Enabled for your account, You can Disable 2FA by untick of checkbox" : "2FA is Disabled for your account, you can Enable now";
                  if(this.Is2FAEnable){
                    this.ShowQRCode = false;
                  }
                  this.ShowPinInput = false;
                  this.toastr.success("2FA Enabled Successfully");
                }
                else{
                  this.ShowPinInput = false;
                  this.ShowQRCode = false;
                  this.Is2FAEnable = false;
                  this.EnableDisableText = (this.Is2FAEnable) ? "2FA is Enabled for your account, You can Disable 2FA by untick of checkbox" : "2FA is Disabled for your account, you can Enable now";
                  this.toastr.success("2FA Disabled Successfully");
                }
             }else{
              this.toastr.error("Incorrect Pin");
             }
             $('.loaderbo').hide();
          },
          err =>{
            this.toastr.error("Something went wrong");
            $('.loaderbo').hide();
          }
        );
    }
}
