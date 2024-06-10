import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { environment } from '../../../../environments/environment';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Menu } from '../../../shared/models/management/menu';
import { ApplicationRoles } from '../../../shared/models/management/application-roles';
import { LineOfBusinessDTO } from '../../../shared/models/management/line-of-business-dto';

@Injectable({
    providedIn: 'root'
})
export class ApplicationService {

    constructor(public http: HttpClient,
        private commonService: CommonService) { }

    getApplicationRoles(applicationId): Observable<ApplicationRoles[]> {
        return this.http.get(`${environment.IdentityServiceUrl}/api/Programs/${applicationId}/roles`)
            .catch(this.commonService.handleObservableHttpError);
    }

    getApplicationMenus(applicationId): Observable<Menu[]> {
        return this.http.get(`${environment.IdentityServiceUrl}/api/Programs/${applicationId}/menus`)
            .catch(this.commonService.handleObservableHttpError);
    }

    getUserApplications(): Observable<LineOfBusinessDTO[]> {
        return this.http.get(`${environment.ClientManagementUrl}/api/Program/programstateuser`)
            .catch(this.commonService.handleObservableHttpError);
    }
}
