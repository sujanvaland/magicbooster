import { Injectable } from '@angular/core';
import { HttpClient,HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class RevShareService {

  constructor(private http: HttpClient) { }
  
  BuyShare(CustomerPlanModel)
  {
    let url = environment.baseApiUrl + "RevShare/BuyShare";
    return this.http.post<any>(url,CustomerPlanModel);
  }

  BuyShareWithCoin(CustomerPlanModel)
  {
    let url = environment.baseApiUrl + "RevShare/BuyShareWithCoin";
    return this.http.post<any>(url,CustomerPlanModel);
  }

  MyShare(CustomerId)
  {
    let url = environment.baseApiUrl + "RevShare/MyShare?CustomerId="+CustomerId;
    return this.http.post<any>(url,null
    );
  }

}
