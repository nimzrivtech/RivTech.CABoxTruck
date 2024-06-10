import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { environment } from "../../../../environments/environment";
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Menu } from '../../../shared/models/user-management/menu';
import { ApplicationRoles } from '../../../shared/models/user-management/application-roles';

@Injectable({
    providedIn: 'root'
})
export class ApplicationService {

    constructor(public http: HttpClient,
        private commonService: CommonService) { }

    getApplicationRoles(applicationId): Observable<ApplicationRoles[]> {
        return this.http.get(`${environment.IdentityServiceUrl}/api/Applications/${applicationId}/roles`)
            .catch(this.commonService.handleObservableHttpError);
    }

    getApplicationMenus(applicationId): Observable<Menu[]>{
        return this.http.get(`${environment.IdentityServiceUrl}/api/Applications/${applicationId}/menus`)
            .catch(this.commonService.handleObservableHttpError);
    }
}