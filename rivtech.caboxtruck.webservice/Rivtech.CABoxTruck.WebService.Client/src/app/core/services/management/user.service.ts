import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { environment } from '../../../../environments/environment';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserDTO } from 'app/shared/models/management/user';
import { UserViewModel } from 'app/shared/models/management/user-view-model';
import { SearchDTO } from '../../../shared/models/management/search.dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  getUsers(applicationId): Observable<UserViewModel[]> {
    return this.http.get(`${environment.GenericServiceUrl}/api/User/${applicationId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  postUser(user): Observable<any> {
    return this.http.post(`${environment.GenericServiceUrl}/api/User`, user, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  getSpecificUser(applicationId, username): Observable<UserDTO> {
    return this.http.get(`${environment.GenericServiceUrl}/api/User/${applicationId}/${username}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getUserAccessRightsByUsername(username): Observable<any[]> {
    return this.http.get(`${environment.IdentityServiceUrl}/api/Users/${username}/access-rights`)
      .catch(this.commonService.handleObservableHttpError);
  }

  putUser(user): Observable<any> {
    return this.http.put(`${environment.GenericServiceUrl}/api/User/${environment.ApplicationId}`, user, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  deleteUser(userId): Observable<any> {
    return this.http.delete(`${environment.GenericServiceUrl}/api/User/${environment.ApplicationId}/${userId}`, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  checkIfUserExist(applicationId, username, emailAddress): Observable<any> {
    return this.http.get(`${environment.GenericServiceUrl}/api/User/UserExist/${applicationId}/${username}/${emailAddress}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getUserInfo(username): Observable<UserDTO> {
    return this.http.get(`${environment.IdentityServiceUrl}/api/Users/${username}/${environment.ApplicationId}/user-details`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getUserInfoById(userId): Observable<any> {
    // TODO: Implement in BT backend
    return this.http.get(`${environment.IdentityServiceUrl}/api/Users/${userId}/user-details-id`)
      .catch(this.commonService.handleObservableHttpError);
  }

  searchSystemUser(search: SearchDTO): Observable<UserViewModel[]> {
    return this.http.post(`${environment.GenericServiceUrl}/api/User/Search`, search)
      .catch(this.commonService.handleObservableHttpError);
  }

  getUnderwriterList(): Observable<any> {
    /* return this.http.get(`${environment.GenericServiceUrl}/api/User/Underwriters/${environment.ApplicationId}`)
      .catch(this.commonService.handleObservableHttpError); */

    // Temp use Centauri to get data;
    return this.http.get(`${environment.GenericServiceUrl}/api/User/Underwriters/1`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getInternalUserList(): Observable<any> {
    return this.http.get(`${environment.GenericServiceUrl}/api/User/InternalUsers/${environment.ApplicationId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  doesUserNameAlreadyExists(newUserName: string,oldUserName: string): Observable<boolean> {
    const value = this.http.post(`${environment.GenericServiceUrl}/api/User/DoesUserNameAlreadyExists`, {newUserName , oldUserName, appCode: environment.ApplicationId})
    .catch(this.commonService.handleObservableHttpError);

    return value;

  }
}
