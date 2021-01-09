import { Injectable } from '@angular/core';
import { HttpClient,HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(private http: HttpClient) { }
  
  GetPaymentProcessor()
  {
    let url = environment.baseApiUrl + "Common/PaymentProcessor";
    return this.http.get<any>(url)
  }

  Withdrawfund(TransactionModel)
  {
    let url = environment.baseApiUrl + "Common/Withdrawfund";
    return this.http.post<any>(url,TransactionModel)
  }
  
  GetAllCountry()
  {
    let url = environment.baseApiUrl + "Common/GetAllCountry";
    return this.http.get<any>(url)
  }
 
  GetSiteStats(){
    let url = environment.baseApiUrl + "Home/GetSiteStats";
    return this.http.get<any>(url)
  }
  
  AddFaq(faq){
    let url = environment.baseApiUrl + "Adverisment/AddFAQ";
    return this.http.post<any>(url,faq)
  }

  GetFaqs(){
    let url = environment.baseApiUrl + "Adverisment/GetFaqs";
    return this.http.get<any>(url)
  }

  GetNewsletter(){
    let url = environment.baseApiUrl + "Home/GetNewsletter";
    return this.http.get<any>(url)
  }

  AddCustomerTraffic(CustomerId){
    let url = environment.baseApiUrl + "Home/AddCustomerTraffic?CustomerId="+CustomerId;
    return this.http.get<any>(url)
  }

  ContactUsNew(contactUs){
    let url = environment.baseApiUrl + "Home/ContactUsNew";
    return this.http.post<any>(url,contactUs)
  }

  GetNewsletterById(campaignId){
    let url = environment.baseApiUrl + "Home/GetNewsletterById?campaignId="+campaignId;
    return this.http.get<any>(url)
  }

  AddSupportTicket(supportrequest){
    let url = environment.baseApiUrl + "Adverisment/AddSupportTicket";
    return this.http.post<any>(url,supportrequest)
  }

  GetSupportRequest(ShowNotReplied,CustomerId,PageIndex,MaxCount){
    let url = environment.baseApiUrl + "Adverisment/GetSupportRequest?ShowNotReplied="+ShowNotReplied+
    "&CustomerId="+CustomerId+"&PageIndex="+PageIndex+"&MaxCount="+MaxCount;
    return this.http.get<any>(url)
  }

  ValidateBitcoinAddress(address){
    let url = "https://api.blockcypher.com/v1/btc/main/addrs/"+address;
    return this.http.get<any>(url);
  }

  
  TransferFund(transferfund){
    let url = environment.baseApiUrl + "Common/TransferFund";
    return this.http.post<any>(url,transferfund)
  }

  TransferCoin(transfercoin){
    let url = environment.baseApiUrl + "Common/TransferCoin";
    return this.http.post<any>(url,transfercoin)
  }
}
