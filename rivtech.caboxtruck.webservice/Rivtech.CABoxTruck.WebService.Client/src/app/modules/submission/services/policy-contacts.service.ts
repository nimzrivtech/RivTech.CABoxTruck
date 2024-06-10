import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { RiskPolicyContactDTO } from '@app/shared/models/submission/RiskPolicyContactDto';
import { catchError, map } from 'rxjs/operators';
import { PolicyContactsDTO } from '@app/shared/models/submission/PolicyContactsDto';

@Injectable({
  providedIn: 'root'
})
export class PolicyContactsService {
  private siteServiceUrl: string = environment.ApiUrl.replace('/api', '');

  constructor(private http: HttpClient, private commonService: CommonService) { }

  get(id: string): Observable<any> {
    return this.http.get(`${this.siteServiceUrl}/api/PolicyContact/${id}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getAll(riskDetailId: string): Observable<any> {
    return this.http.get(`${this.siteServiceUrl}/api/PolicyContact/GetByRiskDetailId/${riskDetailId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  post(policyContact: RiskPolicyContactDTO): Observable<any> {
    return this.http.post(`${this.siteServiceUrl}/api/PolicyContact`, policyContact)
      .catch(this.commonService.handleObservableHttpError);
  }

  put(policyContact: any): Observable<any> {
    return this.http.put<string>(`${this.siteServiceUrl}/api/PolicyContact/${policyContact.id}`, policyContact)
      .catch(this.commonService.handleObservableHttpError);
  }

  delete(id: string, fromEndorsement: boolean = false): Observable<any> {
    const url = `${this.siteServiceUrl}/api/PolicyContact/${id}?fromEndorsement=${fromEndorsement}`;
    return this.http.delete(url)
      .catch(this.commonService.handleObservableHttpError);
  }

  getAllAssignedToPolicyContacts(): Observable<Array<PolicyContactsDTO>> {
    return this.http.get(`${this.siteServiceUrl}/api/PolicyContact/GetAllAssignedToPolicyContacts`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getPolicyContactTypes(): Observable<any> {
    const _url = this.siteServiceUrl + `/api/KeyValue/generic/PolicyContactTypes`;
    return this.http.get(_url)
            .pipe(
                map(data => {
                    return data as any;
                }),
            catchError(this.commonService.handleObservableHttpError)
        );
  }
}
