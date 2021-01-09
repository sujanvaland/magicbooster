import { Injectable } from '@angular/core';
import { HttpClient,HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class MatrixService {

  constructor(private http: HttpClient) { }
  
  GetMatrixPlan()
  {
    let url = environment.baseApiUrl + "Matrix/GetMatrixPlan";
    return this.http.get<any>(url);
  }

  GetTreeView(PositionId)
  {
    let url = environment.baseApiUrl + "Matrix/GetTreeView?PositionId="+PositionId;
    return this.http.post<any>(url,null);
  }

  GetTreeBalance(CustomerId)
  {
    let url = environment.baseApiUrl + "Matrix/GetTreeBalance?CustomerId="+CustomerId;
    return this.http.post<any>(url,null);
  }

  BuyPosition(CustomerPlanModel)
  {
    let url = environment.baseApiUrl + "Matrix/BuyPosition";
    return this.http.post<any>(url,
      CustomerPlanModel
    );
  }

  AddTransaction(transactionModel)
  {
    let url = environment.baseApiUrl + "Matrix/AddTransaction";
    return this.http.post<any>(url,
      transactionModel
    );
  }

}
