import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from '../../../core/services/common.service';
import { DashboardDTO } from '../../../shared/models/dashboard/dashboard';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { DashboardPolicySearchDTO } from '../../../shared/models/dashboard/dashboard-policy-search';

@Injectable({
  providedIn: 'root'
})

export class DashboardService {
  private readonly baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.http = http;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  getDashboardCount(): Observable<DashboardDTO> {
    const url_ = this.baseUrl + `/api/Dashboard/GetDashboardCount`;
    return this.http.get(url_)
      .pipe(
        map(data => {
          return data as DashboardDTO;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  getRiskByPolicyNumber(policyNumber: string) {
    const url_ = this.baseUrl + `/api/Dashboard/GetRiskByPolicyNumber?policyNumber=${policyNumber}`;
    return this.http.get(url_);
  }
}
