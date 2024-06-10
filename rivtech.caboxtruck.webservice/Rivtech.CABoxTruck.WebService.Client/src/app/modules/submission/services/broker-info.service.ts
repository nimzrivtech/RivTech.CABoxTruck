import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { BrokerInfoDto } from '../../../shared/models/submission/brokerinfo/brokerInfoDto';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BrokerInfoService {
  private baseUrl: string;
  private genericUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.genericUrl = environment.GenericServiceUrl;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  getAgencyList(): Observable<any> {
    return this.http.get(new URL(`/api/Agency/GetAllInclude`, this.baseUrl).href)
                      .pipe(
                        map((data) => {
                          return data as any;
                        }),
                        catchError(this.commonService.handleObservableHttpError)
                      );
  }

  getAgencyListSyc(): Array<any> {
    const r = new XMLHttpRequest();
    r.open('GET', new URL(`/api/Agency/GetAllInclude`, this.baseUrl).href, false);
    r.send();
    return JSON.parse(r.responseText);
  }

  getAgentListAsync(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Agent/GetByAgencyIdInclude/${id}`, this.baseUrl).href)
                      .pipe(
                        map((data) => {
                          return data as any;
                        }),
                        catchError(this.commonService.handleObservableHttpError)
                      );
  }

  getAgentList(id: string): Observable<any> {
    const r = new XMLHttpRequest();
    r.open('GET', new URL(`/api/Agent/GetByAgencyIdInclude/${id}`, this.baseUrl).href, false);
    r.send();
    return JSON.parse(r.responseText);
  }

  getSubAgencyList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/SubAgency/GetByAgencyIdInclude/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getSubAgentList(id: string): Observable<any> {
    return this.http.get(new URL(`/api/SubAgent/GetByAgencyIdInclude/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getRiskBrokerInfo(id: string): Observable<any> {
    return this.http.get(new URL(`/api/BrokerInfo/GetByRisk/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  get(id: string): Observable<any> { // get the brokerinfo by its own id
    return this.http.get(new URL(`/api/BrokerInfo/Get/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  put(obj: BrokerInfoDto): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/BrokerInfo/Put/${obj.id}`, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  post(obj: BrokerInfoDto): Observable<any> {
    return this.http.post(new URL(`/api/BrokerInfo/Post/`, this.baseUrl).href, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  getReasonMoveList(): Observable<any> {
    return this.http.get(new URL(`/api/ReasonMove/GetAll`, this.baseUrl).href)
                      .pipe(
                        map((data) => {
                          return data as any;
                        }),
                        catchError(this.commonService.handleObservableHttpError)
                      );
  }

  getPolicyContacts(): Observable<any> {
    let url = new URL(`/api/BrokerInfo/GetPolicyContacts`, this.baseUrl);
    return this.http.get(url.href).pipe(
      catchError(this.commonService.handleObservableHttpError)
    );
  }
}
