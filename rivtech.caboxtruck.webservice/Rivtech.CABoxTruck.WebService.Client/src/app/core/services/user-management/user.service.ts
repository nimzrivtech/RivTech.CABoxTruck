import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { environment } from "../../../../environments/environment";
import { Observable } from 'rxjs';
import { UserViewModel } from '../../../shared/models/user-management/user-view-model';
import { UserDTO, UserAccessRight } from '../../../shared/models/user-management/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  getUsers(applicationId): Observable<UserViewModel[]>{
    return this.http.get(`${environment.GenericServiceUrl}/api/User/${applicationId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  postUser(user): Observable<any>{
    return this.http.post(`${environment.GenericServiceUrl}/api/User`, user, {responseType: 'text'})
      .catch(this.commonService.handleObservableHttpError);
  }

  getSpecificUser(applicationId, username): Observable<UserDTO>{
    return this.http.get(`${environment.GenericServiceUrl}/api/User/${applicationId}/${username}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getUserAccessRightsByUsername(username): Observable<any[]>{
    return this.http.get(`${environment.IdentityServiceUrl}/api/Users/${username}/access-rights`)
      .catch(this.commonService.handleObservableHttpError);
  }

  putUser(user): Observable<any>{
    return this.http.put(`${environment.GenericServiceUrl}/api/User/${environment.ApplicationId}`, user, {responseType: 'text'})
      .catch(this.commonService.handleObservableHttpError);
  }

  checkIfUserExist(applicationId, username, emailAddress):Observable<any>{
    return this.http.get(`${environment.GenericServiceUrl}/api/User/UserExist/${applicationId}/${username}/${emailAddress}`)
      .catch(this.commonService.handleObservableHttpError);
  }
}
