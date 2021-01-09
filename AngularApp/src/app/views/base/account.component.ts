import { Component ,OnInit } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';
import { CommonService } from '../../services/common.service';
import { LoginserviceService } from '../../services/loginservice.service';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import * as $ from 'jquery';
@Component({
  templateUrl: 'account.component.html'
})
export class AccountComponent implements OnInit{

accountdetail: FormGroup;
submitted = false;
changepassword : FormGroup;

constructor(private formBuilder: FormBuilder,
  private customerservice:CustomerService,
  private commonservice:CommonService,
  private loginserviceService:LoginserviceService,
  private toastr:ToastrService,
  private router: Router) { }

CustomerId:string = localStorage.getItem("CustomerId");
CustomerInfoModel = { FirstName:'',LastName:'',BitcoinAddress:'',AccountNumber:'',NICR:'',BankName:'',AccountHolderName:'',CountryId:53,Gender:'',Enable2FA:false,CustomerId:0,Email:"",ReferredBy:"",Username:""};
CurrencyCode:string;
Countries=[];
Pin2FA = "";
Is2FAEnableAlready : boolean;
MemberId = 0;
Email = "";
Inviter = "";
User = "";
ngOnInit(): void {
  this.accountdetail =this.formBuilder.group({
    FirstName: ['', Validators.required],
    LastName:['', Validators.required],
    // BitcoinAddress:['', Validators.required],

    AccountNumber:['', Validators.required],
      NICR:['', Validators.required],
      BankName:['', Validators.required],
      AccountHolderName:['', Validators.required],

    CountryId:['', Validators.required],
    Gender:['']
  });

  this.changepassword = this.formBuilder.group({
    OldPassword: ['', Validators.required],
    NewPassword:['', Validators.required],
    ConfirmNewPassword:['', Validators.required],
  });

  $('.loaderbo').show();
  this.customerservice.GetCustomerInfo(this.CustomerId)
  .subscribe(
    res => {
      this.CustomerInfoModel = res.data;
      this.MemberId= this.CustomerInfoModel.CustomerId;
      this.Email= this.CustomerInfoModel.Email;
      this.Inviter= this.CustomerInfoModel.ReferredBy;
      this.User= this.CustomerInfoModel.Username;
      this.Is2FAEnableAlready = this.CustomerInfoModel.Enable2FA;
      this.accountdetail.get('FirstName').setValue(this.CustomerInfoModel.FirstName);
      this.accountdetail.get('LastName').setValue(this.CustomerInfoModel.LastName);
      //this.accountdetail.get('BitcoinAddress').setValue(this.CustomerInfoModel.BitcoinAddress);
      this.accountdetail.get('AccountNumber').setValue(this.CustomerInfoModel.AccountNumber);
      this.accountdetail.get('NICR').setValue(this.CustomerInfoModel.NICR);
      this.accountdetail.get('BankName').setValue(this.CustomerInfoModel.BankName);
      this.accountdetail.get('AccountHolderName').setValue(this.CustomerInfoModel.AccountHolderName);
      if(this.CustomerInfoModel.CountryId){
        this.accountdetail.get('CountryId').setValue(this.CustomerInfoModel.CountryId);
      }
      else{
        this.accountdetail.get('CountryId').setValue(53);
      }
      
      this.accountdetail.get('Gender').setValue(this.CustomerInfoModel.Gender);
      $('.loaderbo').hide();
    },
    err => console.log(err)
  )

  this.commonservice.GetAllCountry()
  .subscribe(
    res =>{
      this.Countries = res.data;
    },
    err => {
      if(err.status == 401){
        localStorage.clear();
        this.router.navigate(['/login']);
      }
    }
  )
}

get f() { return this.accountdetail.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.accountdetail.invalid) {
            return;
        }
        var valid = false;
        $('.loaderbo').show();
        // this.commonservice.ValidateBitcoinAddress(this.accountdetail.value.BitcoinAddress).subscribe(
        //   res =>{
            // if(res.address){
            //   valid = true;
            // }
            // if(!valid){
            //   this.toastr.error('Invalid Bitcoin address',"Invalid");
            //   $('.loaderbo').hide();
            //   return;
            // }
            
            this.accountdetail.value.CustomerId = this.CustomerId;
            this.accountdetail.value.Pin2FA = this.Pin2FA;
            this.customerservice.UpdateCustomerInfo(this.accountdetail.value)
            .subscribe(
              res =>{
                if(res.Message === "success"){
                  this.toastr.success("Your account details has been updated","Success");
                }
                else{
                  this.toastr.error("Invalid 2FA Pin","Error")
                }
                $('.loaderbo').hide();
              },
              err => {
                if(err.status == 401){
                  localStorage.clear();
                  $('.loaderbo').hide();
                  this.router.navigate(['/login']);
                }
                else{
                  this.toastr.error("Something went wrong, contact support","Error")
                  $('.loaderbo').hide();
                }
              }
            );
        //   },
        //   err =>{
        //     this.toastr.error('Invalid Bitcoin address',"Invalid");
        //       return;
        //   }
        // );
    }

    ChangePassword() {
      // stop here if form is invalid
      if (this.changepassword.invalid) {
          return;
      }

      if(this.changepassword.value.NewPassword != this.changepassword.value.NewPassword){
        return;
      }

      this.changepassword.value.CustomerId = this.CustomerId;
      this.loginserviceService.ChangePassword(this.changepassword.value)
      .subscribe(
        res =>{
          if(res.Message === "success"){
            this.toastr.success("Your password has been updated","Success");
          }
          else{
            this.toastr.error(res.Message);
          }
        },
        err => {
          if(err.status == 401){
            localStorage.clear();
            this.router.navigate(['/login']);
          }
          else{
            this.toastr.error("Something went wrong, contact support","Error")
          }
        }
      )
        this.accountdetail.reset();
    }

    onReset() {
        this.submitted = false;
        this.accountdetail.reset();
        this.changepassword.reset();
    }
}
