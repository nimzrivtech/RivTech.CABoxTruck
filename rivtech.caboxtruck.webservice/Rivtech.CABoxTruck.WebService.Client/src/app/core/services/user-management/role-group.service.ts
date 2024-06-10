import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { Observable } from 'rxjs';
import { RoleGroup } from '../../../shared/models/user-management/role-group';
import { environment } from "../../../../environments/environment";
import { map, catchError } from 'rxjs/operators';
import { Menu } from '../../../shared/models/menu';


@Injectable({
  providedIn: 'root'
})
export class RoleGroupService {

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  getList(): Observable<RoleGroup[]> {
    return this.http.get(`${environment.GenericServiceUrl}/api/role-groups`)
      .pipe(
        map(data => {
          return data as RoleGroup;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  getMenusByRoleGroupId(id: number){
    return this.http.get(`${environment.GenericServiceUrl}/api/role-groups/${id}/menus`)
      .pipe(
        map(data => {
          return data as Menu;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
    }

    getRoleGroups() {
        return this.http.get(`${environment.IdentityServiceUrl}/api/RoleGroups`)
            .catch(this.commonService.handleObservableHttpError)
    }
}
