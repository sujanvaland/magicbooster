import { Component ,OnInit } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { RevShareService } from '../../services/revshare.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import * as $ from 'jquery';
import { CustomerService } from '../../services/customer.service';
@Component({
  selector: 'buyadpack',
  templateUrl: 'buyadpack.component.html'
})
export class BuyAdPackComponent  implements OnInit {

stName: FormGroup;
CustomerId = localStorage.getItem("CustomerId");
Email = "";
Phone = "";
confirm = false;
submitted = false;
PlanId = 0;
PlanName = "";
Name = "";
UserList = [];
AmountInvested = 0;

constructor(
  private formBuilder: FormBuilder,
  private toastr:ToastrService,
  private revshare:RevShareService,
  private customerservice: CustomerService,
  private router: Router) { }

    ngOnInit (){
      this.stName =this.formBuilder.group({
        StokistId:[0]
      });

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

      this.customerservice.GetStokistDetail(this.stName.value.StokistName)
      .subscribe(res => {
        this.UserList = res.data.Data;
        console.log(this.UserList)
      });

        if(!environment.AllowAdPack){
          this.toastr.info("Purchase is diabled till launch date");
        }
    }

    onConfirm(planid,planName){
      this.confirm = true;
      this.PlanId = planid;
      this.PlanName = planName;

      if(this.stName.value.StokistId > 1){
          if(confirm("Are you sure You Want to Purchase "+ this.PlanName + " Plan")) {  
            $('.loaderbo').show();          
            let CustomerPlanModel = {
              CustomerId : this.CustomerId,
              PlanId : this.PlanId,
              NoOfPosition:1,
              StokistId : this.stName.value.StokistId
            }              
            this.revshare.BuyShareWithCoin(CustomerPlanModel).subscribe(res =>{                
              if(res.Message == "success"){
                this.toastr.success("Your purchase was successful");
                $('.loaderbo').hide();
              }
              else{
                this.toastr.error(res.Message);
                $('.loaderbo').hide();
              }                
            },
            err => {
              if(err.status == 401){
                localStorage.clear();
                this.router.navigate(['/login']);
                $('.loaderbo').hide();
              }
              else{
                this.toastr.error("Something went wrong, contact support","Error");
                $('.loaderbo').hide();
              }                      
            })
          }
         }
         else{
          this.toastr.error("Please Select Stokist Name","Error");
         }
        
        
    }
    
    onSubmit() {
      $('.loaderbo').show();      
        if(confirm("Are you sure You Want to Purchase "+ this.PlanName + " Plan")) {          
          let CustomerPlanModel = {
            CustomerId : this.CustomerId,
            PlanId : this.PlanId,
            NoOfPosition:1,
            StokistId : this.stName.value.StokistId
          }              
          this.revshare.BuyShareWithCoin(CustomerPlanModel).subscribe(res =>{                
            if(res.Message == "success"){
              this.toastr.success("Your purchase was successful");
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
          })
        }
        $('.loaderbo').hide();
     }
}
