import { Component ,OnInit } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';
import { CommonService } from '../../services/common.service';
import { environment } from '../../../environments/environment';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import * as $ from 'jquery';
@Component({
  templateUrl: 'withdrawal.component.html'
})
export class WithdrawalComponent implements OnInit{

withdrawal: FormGroup;
submitted = false;

constructor(private formBuilder: FormBuilder,
  private customerservice:CustomerService,
  private commonservice:CommonService,
  private router: Router,
  private toastr:ToastrService) { }

CustomerId:string = localStorage.getItem("CustomerId");
CustomerInfoModel = { BitcoinAddress : ""};
FinalAmount:number = 0;
WithdrawalFees:number = 0;
CurrencyCode:string;
PaymentProcessor=[];
ngOnInit(): void {
  this.CurrencyCode = (localStorage.getItem("CurrencyCode") == null) ? "USD" : localStorage.getItem("CurrencyCode");
  this.withdrawal =this.formBuilder.group({
    Amount: ['', Validators.required],
    ProcessorId:['', Validators.required]
  });
  
  this.customerservice.GetCustomerInfo(this.CustomerId)
  .subscribe(
    res => {
      this.CustomerInfoModel = res.data;
    },
    err => console.log(err)
  )

  this.commonservice.GetPaymentProcessor()
  .subscribe(
    res =>{
      this.PaymentProcessor = res.data;
    }
  )
}

get f() { return this.withdrawal.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.withdrawal.invalid) {
            return;
        }

        this.withdrawal.value.CustomerId = this.CustomerId;
        $('.loaderbo').show();
        this.commonservice.Withdrawfund(this.withdrawal.value)
        .subscribe(
          res =>{
            if(res.Message === "success"){
              this.toastr.success("Yieepiee your withdrawal request is accepted","Congratulations !!")
              this.router.navigate(['/dashboard']);
            }
            else{
              this.toastr.error(res.Message)
            }
            $('.loaderbo').hide();
          },
          err =>{
            this.toastr.error("Something went wrong");
            $('.loaderbo').hide();
          }
        )
    }

    CalculateFees(){
      this.WithdrawalFees = 0;//environment.withdrawalFees;
      this.FinalAmount = this.withdrawal.value.Amount - this.WithdrawalFees;
    }

    onReset() {
        this.submitted = false;
        this.withdrawal.reset();
    }
}
