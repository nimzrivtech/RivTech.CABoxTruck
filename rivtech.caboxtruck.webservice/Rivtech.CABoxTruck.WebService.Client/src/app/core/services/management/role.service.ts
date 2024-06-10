import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { map, catchError } from 'rxjs/operators';
import { CommonService } from '../common.service';
import { Role } from '../../../shared/models/management/role';
import { RoleGroup, UserType } from '../../../shared/models/management/user-type';
import { SearchDTO } from '../../../shared/models/management/search.dto';


@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  getList(): Observable<Role[]> {
    return this.http.get(`${environment.GenericServiceUrl}/api/roles`)
      .pipe(
        map(data => {
          return data as Role;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  get(id: number): Observable<Role> {
    return this.http.get(`${environment.GenericServiceUrl}/api/roles/${id}`)
      .pipe(
        map(data => {
          return data as Role;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  put(id, role): Observable<any> {
    return this.http.put(`${environment.GenericServiceUrl}/api/roles/${id}`, role)
      .catch(this.commonService.handleObservableHttpError);
  }

  post(role: Role) {
    // role.applicationId = environment.ApplicationId;

    return this.http.post(`${environment.GenericServiceUrl}/api/Roles`, role).pipe(
      map(data => {
        return data;
      })
    ).catch(this.commonService.handleObservableHttpError);
  }

  getUserTypeList(id: Array<number>): Observable<UserType[]> {
    return this.http.post(`${environment.GenericServiceUrl}/api/Roles/All`, id).pipe(
      map(data => {
        return data as UserType;
      })
    ).catch(this.commonService.handleObservableHttpError);
  }

  putUserType(id, role): Observable<any> {
    return this.http.put(`${environment.GenericServiceUrl}/api/roles/${id}`, role)
      .catch(this.commonService.handleObservableHttpError);
  }

  postUserType(role: Role) {

    return this.http.post(`${environment.GenericServiceUrl}/api/Roles`, role)
      .catch(this.commonService.handleObservableHttpError);
  }

  deleteUserType(id): Observable<any> {
    return this.http.delete(`${environment.GenericServiceUrl}/api/Roles/${id}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getAccessCategoriesList(): Observable<RoleGroup[]> {
    return this.http.get(`${environment.GenericServiceUrl}/api/role-groups`)
      .catch(this.commonService.handleObservableHttpError);
  }

  validateNameExistence(term: string, id: number): Observable<any> {
    return this.http.get(`${environment.GenericServiceUrl}/api/Roles/rolename/${term}/program/${id}`, { observe: 'response' })
      .catch(this.commonService.handleNotFoundHttpError);
  }

  searchUserType(search: SearchDTO): Observable<UserType[]> {
    return this.http.post(`${environment.GenericServiceUrl}/api/Roles/Search`, search)
      .catch(this.commonService.handleObservableHttpError);
  }
}
