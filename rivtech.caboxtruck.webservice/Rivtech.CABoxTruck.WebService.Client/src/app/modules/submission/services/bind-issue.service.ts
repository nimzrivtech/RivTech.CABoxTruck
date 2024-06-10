import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { RiskDTO } from '@app/shared/models/risk/riskDto';

@Injectable({
  providedIn: 'root'
})
export class BindIssueService {
  private localServiceUrl: string = environment.ApiUrl.replace('/api', '');

  constructor(private http: HttpClient, private commonService: CommonService) {
  }

  Issue(riskDetailId: string): Observable<any> {
    return this.http.get(`${this.localServiceUrl}/api/RiskBind/Issue/${riskDetailId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  Reset(riskDetailId: string): Observable<any> {
    return this.http.get(`${this.localServiceUrl}/api/RiskBind/Reset/${riskDetailId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  GetPolicyChanges(riskDetailId: string): Observable<any> {
    return this.http.get(`${this.localServiceUrl}/api/RiskBind/GetPolicyChanges/${riskDetailId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  IssueEndorsement(policyChanges: any): Observable<any> {
    return this.http.post(new URL(`/api/RiskBind/IssueEndorsement`, this.localServiceUrl).href, policyChanges)
      .catch(this.commonService.handleObservableHttpError);
  }

  ReinstatePolicy(riskDetailId: string): Observable<any> {
    return this.http.get(`${this.localServiceUrl}/api/RiskBind/ReinstatePolicy/${riskDetailId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  RewritePolicy(riskDetailId: string): Observable<any> {
    return this.http.get(`${this.localServiceUrl}/api/RiskBind/RewritePolicy/${riskDetailId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  CopySubmission(riskDetailId: string): Observable<any> {
    return this.http.get(`${this.localServiceUrl}/api/RiskBind/CopySubmission/${riskDetailId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  RescindPolicy(riskDetailId: string): Observable<any> {
    return this.http.get(`${this.localServiceUrl}/api/RiskBind/RescindPolicy/${riskDetailId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

}
