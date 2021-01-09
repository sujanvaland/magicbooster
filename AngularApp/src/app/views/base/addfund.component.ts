import { Component,OnInit  } from '@angular/core';
import { FormGroup,FormBuilder,Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MatrixService } from '../../services/matrix.service';
import { environment } from '../../../environments/environment';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'addfund',
  templateUrl: 'addfund.component.html'
})
export class AddFundComponent implements OnInit {
addfund: FormGroup;
submitted = false;

CustomerId: string = "";
TransactionId:number = 0;
MerchantAcc:string = "";
CurrencyCode:string = "USD";
FinalAmount:number;
PaymentMemo:string = "Crypto Global Team Purchase";
ipn_url:string = '';
showform: boolean = true;
constructor(private formBuilder: FormBuilder,
  private toastr:ToastrService,
  private matrixservice: MatrixService,
  private router: Router) { }
  
ngOnInit (){
  this.CustomerId = localStorage.getItem("CustomerId");
  this.CurrencyCode = (localStorage.getItem("CurrencyCode") == null) ? "USD" : localStorage.getItem("CurrencyCode");
  this.ipn_url = environment.siteUrl + '/IPNHandler';
  
    this.addfund =this.formBuilder.group({
    amount: ['', Validators.required],
    selectprocessor:['0']
  });

  if(!environment.AllowFund){
    this.toastr.info("Purchase is diabled till launch date");
  }
}

get f() { return this.addfund.controls; }

    onSubmit() {
        this.submitted = true;
       
        if(!environment.AllowFund){
          this.toastr.info("Purchase is diabled till launch date");
          return;
        }
        // stop here if form is invalid
        if (this.addfund.invalid) {
            return;
        }

        if(this.addfund.value.amount < 10){
          this.toastr.error("Minimum amount to add is $10")
          return;
        }

        let transactionModel ={
          Amount : this.addfund.value.amount,
          CustomerId : this.CustomerId,
          FinalAmount : this.addfund.value.amount,
          NoOfPosition : 1,
          RefId : 0,
          ProcessorId : 0,
          TranscationTypeId : 1
        }
        this.matrixservice.AddTransaction(transactionModel).subscribe(
          res => {
            if(res.Message){
                this.TransactionId = res.data.Id;
                this.showform = false;
                this.FinalAmount = this.addfund.value.amount;
                this.MerchantAcc = environment.coinPaymentMerAcc;
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
        );

       
    }

    
    onReset() {
        this.submitted = false;
        this.addfund.reset();
    }
}
