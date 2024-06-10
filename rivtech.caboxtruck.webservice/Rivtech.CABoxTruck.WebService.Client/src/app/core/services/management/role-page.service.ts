import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { map, catchError } from 'rxjs/operators';
import { RolePage } from '../../../shared/models/management/role-page';

@Injectable({
  providedIn: 'root'
})
export class RolePageService {

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  getByRoleId(roleId: number): Observable<RolePage[]> {
    return this.http.get(`${environment.GenericServiceUrl}/api/roles/${roleId}/pages`)
      .pipe(
        map(data => {
          return data as RolePage;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  put(roleId: number, rolePages): Observable<any> {
    return this.http.put(`${environment.GenericServiceUrl}/api/Roles/${roleId}/pages`, rolePages);
  }
}
