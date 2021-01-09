import { Component } from '@angular/core';
import { CommonService } from '../../services/common.service';
import * as $ from 'jquery';
import { ToastrService } from 'ngx-toastr';
import { CustomerService } from '../../services/customer.service';

@Component({
  templateUrl: 'magiccoin.component.html'
})
export class MagicCoinComponent {

  constructor(private commonservice:CommonService,
    private customerservice:CustomerService,
    private toastr:ToastrService) { }
  CustomerId:string = localStorage.getItem("CustomerId");
  CustomerEmail="";
  CustomerInfoModel = { BitcoinAddress : "",AvailableCoin:""};
  Amount=0;
  ngOnInit(): void {
    $('.loaderbo').show();
    this.customerservice.GetCoinRequestList(this.CustomerId)
    .subscribe(
      res => {
        this.CustomerInfoModel = res.data;
        $('.loaderbo').hide();
      },
      err => console.log(err)
    )
  }

  transfercoin(){
    let model = { CustomerId : this.CustomerId,CustomerEmail:"",Amount:0 };
    model.CustomerEmail = this.CustomerEmail;
    model.Amount = this.Amount;
    model.CustomerId = this.CustomerId;

    if(model.CustomerEmail == ""){
      this.toastr.error("Please enter email");
      return;
    }
    if(model.Amount <= 0){
      this.toastr.error("Please enter No of Coin");
      return;
    }

    $('.loaderbo').show();
    this.commonservice.TransferCoin(model)
    .subscribe(
      res => {
        if(res.Message == "success"){
          this.CustomerEmail= "";
          this.Amount = 0;
          this.ngOnInit();
          this.toastr.success("Transfer Successfull");          
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
