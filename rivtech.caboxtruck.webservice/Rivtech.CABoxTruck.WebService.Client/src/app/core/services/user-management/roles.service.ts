import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { environment } from "../../../../environments/environment";
import { UserRoleInfo } from '../../../shared/models/user-management/user-role-info';
import { Observable } from 'rxjs';
import { RolePages } from '../../../shared/models/user-management/role-pages';
import { ApplicationRoles as ApplicationRole } from '../../../shared/models/user-management/application-roles';

@Injectable({
  providedIn: 'root'
})
export class RolesService {

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  getRolePagesByRoleId(roleId): Observable<any[]> {
    return this.http.get(`${environment.IdentityServiceUrl}/api/Roles/${roleId}/pages`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getRoleById(roleId): Observable<ApplicationRole> {
    return this.http.get(`${environment.GenericServiceUrl}/api/Roles/${roleId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getCurrentUserRoleInfo(username): Observable<UserRoleInfo> {
    return this.http.get(`${environment.IdentityServiceUrl}/api/Users/${username}/role-info`)
      .catch(this.commonService.handleObservableHttpError);
  }
}
