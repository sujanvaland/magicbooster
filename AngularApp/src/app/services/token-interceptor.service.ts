import { Injectable } from '@angular/core';
import{HttpInterceptor, HttpHeaders} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor{

  constructor() { }
  intercept(req,next)
  {
    if(!req.url.includes("https://api.blockcypher.com")){
      if(localStorage.getItem("token")){
        let tokenizedReq =req.clone({
          headers : req.headers.append('Authorization', 'Basic '+ localStorage.getItem("token"))
        });
        let newtokenizedReq =tokenizedReq.clone({
          headers : tokenizedReq.headers.append('CustomerGUID', localStorage.getItem("CustomerGuid"))
        });
        
        return next.handle(newtokenizedReq)
      }
    }
    return next.handle(req)
  }
}
