import { Injectable } from '@angular/core';
import { HttpClient,HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class LoginserviceService {

  constructor(private http: HttpClient) { }
  
  loginUser(loginuserdata)
  {
    let url = environment.baseApiUrl + "Login/Login";
    return this.http.post<any>(url,
      loginuserdata)
  }

  ValidateLoginWith2FA(loginuserdata)
  {
    let url = environment.baseApiUrl + "Login/ValidateLoginWith2FA";
    return this.http.post<any>(url,
      loginuserdata)
  }

  Register(RegisterModel)
  {
    let url = environment.baseApiUrl + "Login/Register";
    return this.http.post<any>(url,
      RegisterModel)
  }

  PasswordRecovery(PasswordRecoveryModel)
  {
    let url = environment.baseApiUrl + "Login/PasswordRecovery";
    return this.http.post<any>(url,
      PasswordRecoveryModel)
  }

  PasswordRecoveryConfirmPOST(token,email,PasswordRecoveryModel)
  {
    let url = environment.baseApiUrl + "Login/PasswordRecoveryConfirmPOST?token="+token+"&email="+email;
    return this.http.post<any>(url,
      PasswordRecoveryModel)
  }

  ChangePassword(ChangePasswordModel)
  {
    let url = environment.baseApiUrl + "Login/ChangePassword";
    return this.http.post<any>(url,
      ChangePasswordModel)
  }

  GetInviterDetail(inviter)
  {
    let url = environment.baseApiUrl + "Login/GetInviterDetail?inviter="+inviter;
    return this.http.get<any>(url)
  }

  loggedIn(){
    return !!localStorage.getItem("token")
  }
}
