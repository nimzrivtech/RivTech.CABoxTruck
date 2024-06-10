import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../../../core/services/common.service';
import { RiskDTO } from '../../../shared/models/risk/riskDto';
import { Observable } from 'rxjs/Observable';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class SubmissionService {
  
  private baseUrl: string;
  private genericUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.genericUrl = environment.GenericServiceUrl;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  getRiskByIdAsync(id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/api/Risk/GetByRiskInclude/${id}`)
      .pipe(
        map(data => {
        return data as any;
        }),
        catchError(this.commonService.handleObservableHttpError)
    );
  }

  getRiskDetailStatusByIdAsync(id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/api/Risk/Status/${id}`, { responseType: 'text' })
      .pipe(
        map(data => {
        return data as any;
        }),
        catchError(this.commonService.handleObservableHttpError)
    );
  }

  put(obj: RiskDTO): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Risk/Put/${obj.id}`, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  post(obj: RiskDTO): Observable<any> {
    return this.http.post(new URL(`/api/Risk/Post/`, this.baseUrl).href, obj, { responseType: 'json' })
      .catch(this.commonService.handleObservableHttpError);
  }

  postBindAndIssue(obj: RiskDTO): Observable<any> {
    return this.http.post(new URL(`/api/RiskBind/bindIssue/`, this.baseUrl).href, obj, { responseType: 'json' })
      .catch(this.commonService.handleObservableHttpError);
  }

  getServerDateTime(): Observable<Date> {
    return this.http.get(`${this.baseUrl}/api/Risk/GetServerDateTime`, { responseType: 'text' })
      .pipe(
        map(data => {
          return new Date(data);
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  getLatestVersion$(riskid: string): Observable<string> {
    return this.http.get(`${this.baseUrl}/api/Risk/GetLatestRiskDetailVersion/${riskid}`).pipe(
        map(data => data as string)
      );
  }
}
