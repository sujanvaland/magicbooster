import { Injectable } from '@angular/core';
import { HttpClient,HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class ReportService {

  constructor(private http: HttpClient) { }
  
  TransactionReport(TransactionModel)
  {
    let url = environment.baseApiUrl + "Report/TransactionReport";
    return this.http.post<any>(url,TransactionModel);
  }

  Fundingreport(TransactionModel)
  {
    let url = environment.baseApiUrl + "Report/Fundingreport";
    return this.http.post<any>(url,TransactionModel);
  }

  Withdrawalreport(TransactionModel)
  {
    let url = environment.baseApiUrl + "Report/Withdrawalreport";
    return this.http.post<any>(url,TransactionModel);
  }

  TransferReport(TransactionModel){
    let url = environment.baseApiUrl + "Report/TransferReport";
    return this.http.post<any>(url,TransactionModel);
  }

  TransferCoinReport(TransactionModel){
    let url = environment.baseApiUrl + "Report/TransferCoinReport";
    return this.http.post<any>(url,TransactionModel);
  }

  Unilevelreport(TransactionModel)
  {
    let url = environment.baseApiUrl + "Report/Unilevelreport";
    return this.http.post<any>(url,TransactionModel);
  }

  Directbonusreport(TransactionModel)
  {
    let url = environment.baseApiUrl + "Report/Directbonusreport";
    return this.http.post<any>(url,TransactionModel);
  }

  Cyclerreport(TransactionModel)
  {
    let url = environment.baseApiUrl + "Report/Cyclerreport";
    return this.http.post<any>(url,TransactionModel);
  }

  Poolbonusreport(TransactionModel)
  {
    let url = environment.baseApiUrl + "Report/Poolbonusreport";
    return this.http.post<any>(url,TransactionModel);
  }

  Transferhistoryreport(TransactionModel)
  {
    let url = environment.baseApiUrl + "Report/Transferhistoryreport";
    return this.http.post<any>(url,TransactionModel);
  }

  Transferreceivedreport(TransactionModel)
  {
    let url = environment.baseApiUrl + "Report/Transferreceivedreport";
    return this.http.post<any>(url,TransactionModel);
  }
}
