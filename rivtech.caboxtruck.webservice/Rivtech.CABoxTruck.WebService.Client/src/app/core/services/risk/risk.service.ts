import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../common.service';
import { Observable } from 'rxjs';
import { RiskDTO } from '../../../shared/models/risk/riskDto';

@Injectable({
  providedIn: 'root'
})

export class RiskService {
  private baseUrl: string;
  private genericUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.genericUrl = environment.GenericServiceUrl;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  fetchAllRisk(): Observable<any> {
    return this.http.get(new URL(`/api/Risk/GetRiskList/`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getRiskListByIdAsync(id: string) {
    return this.http.get(new URL(`/api/Risk/GetRiskListById/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getRiskListById(id: string): RiskDTO {
    const r = new XMLHttpRequest();
    r.open('GET', new URL(`/api/Risk/GetRiskListById/${id}`, this.baseUrl).href, false);
    r.send();
    return JSON.parse(r.responseText) as RiskDTO;
  }

  put(obj: RiskDTO): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Risk/PutRiskList/${obj.id}`, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateStatus(obj: RiskDTO): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Risk/Status/${obj.id}`, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateEndorsementStatus(id: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Risk/UpdateEndorsementStatus/${id}`, null, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  post(obj: RiskDTO): Observable<any> {
    return this.http.post(new URL(`/api/Risk/PostRiskList/`, this.baseUrl).href, obj)
      .catch(this.commonService.handleObservableHttpError);
  }
}
