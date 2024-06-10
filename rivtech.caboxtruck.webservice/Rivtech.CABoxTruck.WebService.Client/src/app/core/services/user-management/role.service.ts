import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { environment } from "../../../../environments/environment";
import { PathConstants } from '../../../shared/constants/path.constants';
import { map, catchError } from 'rxjs/operators';
import { CommonService } from '../common.service';
import { Role } from '../../../shared/models/user-management/role';


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
    role.applicationId = environment.ApplicationId;

    return this.http.post(`${environment.GenericServiceUrl}/api/Roles`, role).pipe(
      map(data => {
        return data;
      })
    ).catch(this.commonService.handleObservableHttpError);
  }

}