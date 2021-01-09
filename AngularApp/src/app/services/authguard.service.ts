import { Injectable } from '@angular/core';
import { CanActivate, Router, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import {LoginserviceService} from './loginservice.service';
@Injectable({
  providedIn: 'root'
})
export class AuthguardService  implements CanActivate{

  constructor(private _loginService:LoginserviceService,
    private router:Router){}
    canActivate():boolean{
      if(this._loginService.loggedIn()){
        return true
      }
      else{
        this.router.navigate(["/login"])
        return false
      }
    }
}
