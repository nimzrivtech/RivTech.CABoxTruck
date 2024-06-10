import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../common.service';
import { Observable } from 'rxjs';
import { Risk } from '../../../shared/models/risk/risk';
import { SafetyDevice } from '@app/shared/models/submission/riskspecifics/safetyDeviceDto';

@Injectable({
  providedIn: 'root'
})

export class RiskSpecificSafetyDeviceService {
  private baseUrl: string;
  private genericUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.genericUrl = environment.GenericServiceUrl;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  getSafetyDevice(riskDetailId: string) {
    return this.http.get(new URL(`/api/RiskSpecificSafetyDevice/GetRiskSpecificSafetyDevice/${riskDetailId}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  saveSafetyDevice(safetyDevices: SafetyDevice[], riskDetailId: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/RiskSpecificSafetyDevice/SaveRiskSpecificSafetyDevice/${riskDetailId}`, safetyDevices)
      .catch(this.commonService.handleObservableHttpError);
  }
}
