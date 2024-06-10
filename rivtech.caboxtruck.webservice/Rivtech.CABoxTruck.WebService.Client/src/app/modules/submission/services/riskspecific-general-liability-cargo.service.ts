import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../../../core/services/common.service';
import { Observable } from 'rxjs';
import { GeneralLiabilityCargoDto } from '../../../shared/models/submission/riskspecifics/general-liability-cargoDto';

@Injectable({
  providedIn: 'root'
})

export class RiskSpecificGeneralLiabilityCargoService {
  private baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.http = http;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  getSafetyDevice(riskDetailId: string) {
    return this.http.get(new URL(`/api/RiskSpecificSafetyDevice/GetRiskSpecificSafetyDevice/${riskDetailId}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  saveGeneralLiabilityCargo(generalLiabilityCargo: GeneralLiabilityCargoDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/RiskSpecificGeneralLiabilityCargo`, generalLiabilityCargo)
      .catch(this.commonService.handleObservableHttpError);
  }

  deleteGeneralLiabilityCargo(riskDetailId: string, sectionIds: number[]): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/RiskSpecificGeneralLiabilityCargo/${riskDetailId}`, sectionIds)
      .catch(this.commonService.handleObservableHttpError);
  }
}
