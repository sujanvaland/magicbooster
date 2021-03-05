import { Component } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { RevShareService } from '../../services/revshare.service';
import { CommonService } from '../../services/common.service';
import * as $ from 'jquery';
import { CustomerService } from '../../services/customer.service';

@Component({
  templateUrl: 'myshares.component.html'
})
export class MySharesComponent {

  CustomerId = localStorage.getItem("CustomerId");
  PlanId = 0;
  //dynamic: number = 70;
  type: string = "success";
  amount : number = 10;
  charges:number = 5;
  TotalAmount:number;
  FinalAmount:number = 0;
  AmountInvested = 0;
  CustomerBeginnerPlan=[];
  CustomerProfessionalPlan=[];
  CustomerEnterprisePlan=[];
  CustomerInfoModel = { AvailableBalance:""};

  constructor(
    private toastr:ToastrService,
    private customerservice:CustomerService,
    private commonservice:CommonService,
    private revshare:RevShareService) { }
  
    ngOnInit (){
      $('.loaderbo').show();
      this.customerservice.GetCustomerInfo(this.CustomerId)
      .subscribe(
        res => {
          this.CustomerInfoModel = res.data;
          $('.loaderbo').hide();
        },
        err => console.log(err)
      )
      this.customerservice.GetPlanDetail(this.CustomerId)
      .subscribe(res => {
        if(res.data != null)
        {
          this.AmountInvested = res.data.AmountInvested;
        }
        else{
          this.AmountInvested = 0;
        }
      });

      // this.revshare.MyShare(this.CustomerId).subscribe(
      //   res =>{
      //     if(res.Message == "success"){
      //       this.CustomerBeginnerPlan = res.data.Data.filter(x=>x.PlanName == "Beginner Pack");
      //       this.CustomerProfessionalPlan = res.data.Data.filter(x=>x.PlanName == "Professional Pack");
      //       this.CustomerEnterprisePlan = res.data.Data.filter(x=>x.PlanName == "Enterprise Pack");
      //     }
      //   }
      // )
  }
  calculate(){
    this.TotalAmount = this.amount * this.PlanId;
    this.FinalAmount = this.TotalAmount + (this.TotalAmount * this.charges / 100);
  }

  save(){
    let model = { CustomerId : this.CustomerId,Amount:0,FinalAmount:0,NoOfPosition:0 };
    model.Amount = this.amount + (this.amount * this.charges / 100);
    model.CustomerId = this.CustomerId;
    model.FinalAmount = this.FinalAmount;
    model.NoOfPosition = this.PlanId;

    $('.loaderbo').show();
    this.commonservice.MyInvestment(model)
    .subscribe(
      res => {
        if(res.Message == "success"){
          this.toastr.success("Fund Share Successfull");
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
