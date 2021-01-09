import { Component } from '@angular/core';
import { ReportService } from '../../services/report.service';

@Component({
  templateUrl: 'purchasehistory.component.html'
})
export class PurchaseHistoryComponent {

  constructor(private reportservice:ReportService) { }
  CustomerId:string = localStorage.getItem("CustomerId");
  PurchaseData = [];

  ngOnInit(): void {
    let model = { CustomerId : this.CustomerId };
    this.reportservice.Fundingreport(model)
    .subscribe(
      res => {
        console.log(res.data.Data);
        this.PurchaseData = res.data.Data;
      },
      err => console.log(err)
    )
  }
}

