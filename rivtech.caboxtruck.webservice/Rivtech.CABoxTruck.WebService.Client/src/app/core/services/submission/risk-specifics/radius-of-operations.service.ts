import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { IRadiusOfOperationsDto } from '../../../../shared/models/submission/riskspecifics/radius-of-operations.dto';
import { CommonService } from '../../common.service';

@Injectable({
  providedIn: 'root'
})

export class RadiusOfOperationsService {
  private baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.baseUrl = environment.ApiUrl;
  }

  updateRadiusOfOperations(radiusOfOperations: IRadiusOfOperationsDto): Observable<string> {
    return this.http.put(`${this.baseUrl}/riskspecificradiusofoperation/risk/${radiusOfOperations.riskDetailId}`, radiusOfOperations, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }
}
