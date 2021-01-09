import { Component } from '@angular/core';
import { CommonService } from '../../services/common.service';
import * as $ from 'jquery';
import { ToastrService } from 'ngx-toastr';
import { CustomerService } from '../../services/customer.service';
import { ReportService } from '../../services/report.service';

@Component({
  templateUrl: 'transferhistory.component.html'
})
export class TransferHistoryComponent {

  constructor(private reportservice:ReportService) { }
  CustomerId:string = localStorage.getItem("CustomerId");
  TransferData = [];
  TranscationTypeId = 17;
  ngOnInit(): void {
    $('.loaderbo').show();
    let model = { CustomerId : this.CustomerId, TranscationTypeId : 17 };
  //   this.reportservice.Withdrawalreport(model)
  //   .subscribe(
  //     res => {
  //       this.TransferData = res.data.Data;
  //     },
  //     err => console.log(err)
  //   )
  // }
  this.reportservice.TransactionReport(model)
    .subscribe(
      res => {
        this.TransferData = res.data.Data;
        console.log(res);
        $('.loaderbo').hide();
      },
      err => console.log(err)
    )
  }  

  onChange(){
    $('.loaderbo').show();
    this.TransferData = [];
    let model = { CustomerId : this.CustomerId, TranscationTypeId : 17 };
    model.TranscationTypeId = this.TranscationTypeId;
    this.reportservice.TransactionReport(model)
    .subscribe(
      res => {
        this.TransferData = res.data.Data;
        console.log(res);
        $('.loaderbo').hide();
      },
      err => console.log(err)
    )
  }
}