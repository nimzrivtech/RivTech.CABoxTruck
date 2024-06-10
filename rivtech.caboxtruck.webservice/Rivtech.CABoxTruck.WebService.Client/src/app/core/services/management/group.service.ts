import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { GroupDTO } from '../../../shared/models/management/group-dto';
import { SearchDTO } from '../../../shared/models/management/search.dto';

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  getUserGroups(applicationId): Observable<GroupDTO[]> {
    return this.http.get(`${environment.ClientManagementUrl}/api/Group/GetGroups/${applicationId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  saveGroup(body: GroupDTO | undefined): Observable<GroupDTO> {
    const url_ = `${environment.ClientManagementUrl}/api/Group`;
    return this.http.post(url_, body).catch(this.commonService.handleObservableHttpError);
  }

  searchUserGroup(search: SearchDTO): Observable<GroupDTO[]> {
    return this.http.post(`${environment.ClientManagementUrl}/api/Group/Search`, search)
      .catch(this.commonService.handleObservableHttpError);
  }
}
