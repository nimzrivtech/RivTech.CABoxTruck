import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { IProgramStateRequestDTO } from '../../../shared/models/management/agency-management/programStateAgencyDto';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../common.service';

@Injectable({
  providedIn: 'root'
})
export class ProgramstateDetailService {
  private http: HttpClient;
  private baseUrl: string;
  private commonService: CommonService;

  constructor(@Inject(HttpClient) http: HttpClient, @Inject(CommonService) commonService: CommonService) {
    this.http = http;
    this.baseUrl = environment.ClientManagementUrl;
    this.commonService = commonService;
  }

  getProgramStatesWithAgencyId(agencyId: string) {
    return this.http.get(`${this.baseUrl}/api/ProgramStateDetail/${agencyId}`)
        .pipe(
            catchError(this.commonService.handleObservableHttpError)
        );
  }

  postProgramStateDetails(payload: IProgramStateRequestDTO): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/ProgramStateDetail`, payload).map(data => {
      return data;
    }).catch(this.commonService.handleObservableHttpError);
  }

  postProgramStateStatusDetails(payload: IProgramStateRequestDTO): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/ProgramStateDetail/updateStatus/${payload.id}`, payload, { responseType: 'text' }).map(data => {
        return data;
    }).catch(this.commonService.handleObservableHttpError);
  }

  deleteProgramStateStatusDetails(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/api/ProgramStateDetail/${id}`, { responseType: 'text'}).map(data => {
        return data;
    }).catch(this.commonService.handleObservableHttpError);
  }
}
