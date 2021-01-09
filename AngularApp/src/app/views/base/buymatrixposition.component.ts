import { Component } from '@angular/core';
import { MatrixService } from '../../services/matrix.service';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import * as $ from 'jquery';
@Component({
  templateUrl: 'buymatrixposition.component.html'
})
export class BuyMatrixPositionComponent {

  constructor(private matrixservice: MatrixService,
    private toastr: ToastrService,
    private router: Router) { }

  isCollapsed: boolean = false;
  iconCollapse: string = 'icon-arrow-up';
  Phase:number = 1;
  CustomerId: string = "";
  TransactionId:number = 0;
  MerchantAcc:string = "";
  CurrencyCode:string = "USD";
  FinalAmount:number;
  PaymentMemo:string = "Matrix Position Purchase";
  ipn_url:string = '';
  showform: boolean = true;
  matrixPlan = [];
  Phases = {};
  plan = { Id : 1, Price:10, Name:"" };
  ngOnInit (){
    this.Phases = environment.Phases;
    if(!environment.AllowPurchase){
      this.toastr.info("Purchase is diabled till launch date");
    }
    this.CustomerId = localStorage.getItem("CustomerId");
    this.CurrencyCode = (localStorage.getItem("CurrencyCode") == null) ? "USD" : localStorage.getItem("CurrencyCode");
    this.ipn_url = environment.siteUrl + '/IPNHandler';
    this.matrixservice.GetMatrixPlan().subscribe(
      res => {
        this.matrixPlan = res.data;
        this.plan = this.matrixPlan[0];
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
    );
  }

  confirmPayment(): void {
    if(!environment.AllowPurchase){
      this.toastr.info("Purchase is diabled till launch date");
      return;
    }
    if(this.plan.Id > 0){
      let transactionModel ={
        CustomerId : this.CustomerId,
        NoOfPosition : 1,
        ProcessorId : 0,
        PlanId : this.plan.Id
      }

      $('.loaderbo').show();
      this.matrixservice.BuyPosition(transactionModel).subscribe(
        res => {
          if(res.Message == "success"){
            this.router.navigate(['/dashboard']);
            this.toastr.success("Your purchase was successfull","Success")
          }
          else{
            this.toastr.error(res.Message,"Error")
          }
          $('.loaderbo').hide();
        },
        err => {
          if(err.status == 401){
            localStorage.clear();
            this.router.navigate(['/login']);
          }
          else{
            this.toastr.error("Something went wrong, contact support","Error")
          }
          $('.loaderbo').hide();
        }
      );
    }
  }

  reset(): void{
    this.showform = true;
    this.plan =this.matrixPlan[0];
  }
}
