import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EntityService {

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  getEntityByUsername(programId: number, username: string){
    const uname = encodeURIComponent(username);
    return this.http.get(`${environment.ClientManagementUrl}/api/Entity/${programId}/${uname}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  postEntity(entity) {
    return this.http.post(`${environment.ClientManagementUrl}/api/Entity/programstate`, entity, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  putEntity(entity){
    return this.http.put(`${environment.ClientManagementUrl}/api/Entity/update/${entity.id}`, entity, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }
}
