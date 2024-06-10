import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { FilingsInformationDto } from '../../../shared/models/submission/applicant/FilingsInformationDto';
import { map, catchError } from 'rxjs/operators';
import { FilingTypeDto } from '../../../shared/models/submission/applicant/FilingTypeDto';

@Injectable({
  providedIn: 'root'
})
export class ApplicantFilingsInformationService {
  private genericServiceUrl: string = environment.GenericServiceUrl;
  private localServiceUrl: string = environment.ApiUrl.replace('/api', '');

  constructor(private http: HttpClient, private commonService: CommonService) {
  }

  getFilingTypes(): Observable<FilingTypeDto[]> {
    return this.http.get(`${this.genericServiceUrl}/api/FilingType`)
      .catch(this.commonService.handleObservableHttpError);
  }

  get(riskDetailId: string): Observable<FilingsInformationDto> {
    return this.http.get(`${this.localServiceUrl}/api/FilingsInformation/${riskDetailId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  post(filingsInfo: FilingsInformationDto): Observable<any> {
    return this.http.post(`${this.localServiceUrl}/api/FilingsInformation`, filingsInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

  put(riskDetailId: string, filingsInfo: FilingsInformationDto): Observable<any> {
    return this.http.put(`${this.localServiceUrl}/api/FilingsInformation/${riskDetailId}`, filingsInfo)
      .catch(this.commonService.handleObservableHttpError);
  }

}
