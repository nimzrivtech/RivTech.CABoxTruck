import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../../../core/services/common.service';
import { Observable } from 'rxjs';
import { MaintenanceSafetyDto } from '../../../shared/models/submission/riskspecifics/maintenance-safety/maintenance-safetyDto';

@Injectable({
  providedIn: 'root'
})

export class RiskSpecificMaintenanceSafetyService {
  private baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.http = http;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  saveRiskSpecificMaintenanceSafety(maintenanceSafety: MaintenanceSafetyDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/api/RiskSpecificMaintenanceSafety`, maintenanceSafety)
      .catch(this.commonService.handleObservableHttpError);
  }
}
