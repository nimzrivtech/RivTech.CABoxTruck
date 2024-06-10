import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserTwoFactorAuthResponse } from '../../shared/models/user-validation/user-two-factor-auth.response';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CommonService } from './common.service';

@Injectable({
  providedIn: 'root'
})
export class ValidationService {

  constructor(private http: HttpClient) { }

  getUserTwoFactorAuth(username: string) {
    return this.http.get(`${environment.GenericServiceUrl}/api/UserTwoFactorAuth/${username}`, {responseType: 'text'});
  }

  postUserTwoFactorAuth(payload: any): Observable<UserTwoFactorAuthResponse> {
    return this.http.post(`${environment.GenericServiceUrl}/api/UserTwoFactorAuth`, payload);
  }

}
