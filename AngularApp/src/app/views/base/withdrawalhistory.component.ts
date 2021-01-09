import { Component } from '@angular/core';
import { ReportService } from '../../services/report.service';
import * as $ from 'jquery';

@Component({
  templateUrl: 'withdrawalhistory.component.html'
})
export class WithdrawalHistoryComponent {

  constructor(private reportservice:ReportService) { }
  CustomerId:string = localStorage.getItem("CustomerId");
  WithdrawalData = [];

  ngOnInit(): void {
    let model = { CustomerId : this.CustomerId };
    this.reportservice.Withdrawalreport(model)
    .subscribe(
      res => {
        this.WithdrawalData = res.data.Data;
      },
      err => console.log(err)
    )
  }
}
