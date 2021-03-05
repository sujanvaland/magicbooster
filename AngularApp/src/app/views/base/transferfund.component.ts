import { Component } from '@angular/core';
import { CommonService } from '../../services/common.service';
import * as $ from 'jquery';
import { ToastrService } from 'ngx-toastr';
import { CustomerService } from '../../services/customer.service';

@Component({
  templateUrl: 'transferfund.component.html'
})
export class TransferFundComponent {

  constructor(private commonservice:CommonService,
    private customerservice:CustomerService,
    private toastr:ToastrService) { }
  CustomerId:string = localStorage.getItem("CustomerId");
  CustomerEmail="";
  CustomerInfoModel = { BitcoinAddress : "",AvailableBalance:""};
  Amount=0;
  ngOnInit(): void {
    $('.loaderbo').show();
    this.customerservice.GetCustomerInfo(this.CustomerId)
    .subscribe(
      res => {
        this.CustomerInfoModel = res.data;
        $('.loaderbo').hide();
      },
      err => console.log(err)
    )
  }

  transferfund(){
    let model = { CustomerId : this.CustomerId,CustomerEmail:"",Amount:0 };
    model.CustomerEmail = this.CustomerEmail;
    model.Amount = this.Amount;
    model.CustomerId = this.CustomerId;

    if(model.CustomerEmail == ""){
      this.toastr.error("Please enter email");
      return;
    }
    if(model.Amount <= 0){
      this.toastr.error("Please enter Amount");
      return;
    }
    if(model.Amount > 100){
      this.toastr.error("Maximum Transfer 100$ Per Day.");
      return;
    }

    $('.loaderbo').show();
    this.commonservice.TransferFund(model)
    .subscribe(
      res => {
        if(res.Message == "success"){
          this.CustomerEmail= "";
          this.Amount = 0;
          this.toastr.success("Transfer Successfull");
          this.ngOnInit();
        }
        else{
          this.toastr.success(res.Message);
        }
        $('.loaderbo').hide();
      },
      err => {
        this.toastr.error("Something went wrong");
        $('.loaderbo').hide();
      }
    )
  }
}
