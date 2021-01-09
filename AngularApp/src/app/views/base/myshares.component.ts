import { Component } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { RevShareService } from '../../services/revshare.service';
@Component({
  templateUrl: 'myshares.component.html'
})
export class MySharesComponent {

  CustomerId = localStorage.getItem("CustomerId");
  PlanId = 0;
  dynamic: number = 70;
  type: string = "success";
  max : number = 100;
  CustomerBeginnerPlan=[];
  CustomerProfessionalPlan=[];
  CustomerEnterprisePlan=[];
  constructor(
    private toastr:ToastrService,
    private revshare:RevShareService) { }
  
    ngOnInit (){
      this.revshare.MyShare(this.CustomerId).subscribe(
        res =>{
          if(res.Message == "success"){
            this.CustomerBeginnerPlan = res.data.Data.filter(x=>x.PlanName == "Beginner Pack");
            this.CustomerProfessionalPlan = res.data.Data.filter(x=>x.PlanName == "Professional Pack");
            this.CustomerEnterprisePlan = res.data.Data.filter(x=>x.PlanName == "Enterprise Pack");
          }
        }
      )
  }
  
}
