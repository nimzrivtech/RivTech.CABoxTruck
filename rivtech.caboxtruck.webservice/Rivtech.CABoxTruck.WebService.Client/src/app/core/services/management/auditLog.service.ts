import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../common.service';
import { environment } from '../../../../environments/environment';
import { Observable, of } from 'rxjs';
//import { AuditLogDTO } from '../../../shared/models/management/auditlog.dto';  // Error in ng build --prod in PR

@Injectable({
  providedIn: 'root'
})
export class AuditLogService {

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  insertToAuditLog(payload: any): Observable<any> {
    /* return this.http.post(`${environment.ApiUrl}/AuditLog`, payload) // Not implement yet.
      .catch(this.commonService.handleObservableHttpError); */
      return of(payload); // NOT IMPLEMENTED YET
  }

}
