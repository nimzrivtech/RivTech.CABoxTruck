import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { map, catchError } from 'rxjs/operators';
import { Menu } from '../../../shared/models/menu';
import { RoleGroup } from '../../../shared/models/management/role-group';
import { MenuCategory } from '../../../shared/models/menuCategory';
import { MenuSection } from '../../../shared/models/menuSection';
import { UserMenuSection } from '../../../shared/models/userMenuSection';


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

  getMenusByRoleGroupId(id: number) {
    return this.http.get(`${environment.GenericServiceUrl}/api/role-groups/${id}/menus`)
      .pipe(
        map(data => {
          return data as Menu;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  getMenusByRoleGroupIdProgramId(id: number, programId: number) {
    return this.http.get(`${environment.GenericServiceUrl}/api/role-groups/${id}/projectmenus/${programId}`)
        .catch(this.commonService.handleObservableHttpError);
  }

  getRoleGroups() {
    return this.http.get(`${environment.IdentityServiceUrl}/api/RoleGroups`)
      .catch(this.commonService.handleObservableHttpError);
  }

  // Menu Category
  getMenuCategoryProgramId(programId: number) {
    return this.http.get(`${environment.GenericServiceUrl}/api/MenuCategory/${programId}`)
      .pipe(
        map(data => {
          return data as MenuCategory;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  getMenuSectionProgramId(programId: number) {
    return this.http.get(`${environment.GenericServiceUrl}/api/MenuSection/${programId}`)
      .pipe(
        map(data => {
          return data as MenuSection;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  getUserMenuSection(userName: string, programId: number) {
    return this.http.get(`${environment.GenericServiceUrl}/api/UserMenuSection/${userName}/${programId}`)
    .pipe(
      map(data => {
        return data as UserMenuSection;
      }),
      catchError(this.commonService.handleObservableHttpError)
    );
  }

  postUserMenuSection(data: UserMenuSection[]) {

    return this.http.post(`${environment.GenericServiceUrl}/api/UserMenuSection`, data)
      .catch(this.commonService.handleObservableHttpError);
  }
}
