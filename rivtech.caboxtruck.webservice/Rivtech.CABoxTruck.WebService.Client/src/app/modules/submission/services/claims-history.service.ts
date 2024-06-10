import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { ClaimsHistoryDto } from '../../../shared/models/submission/claimsHistory/claimsHistoryDto';

@Injectable({
  providedIn: 'root'
})
export class ClaimsHistoryService {
  private baseUrl: string;
  private genericUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.genericUrl = environment.GenericServiceUrl;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  getClaimsHistory(): Observable<any> {
    return this.http.get(new URL(`/api/claimshistory/GetAllInclude`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  put(obj: ClaimsHistoryDto): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/claimshistory/Put/${obj.id}`, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  post(obj: ClaimsHistoryDto[]): Observable<any> {
    return this.http.post(new URL(`/api/claimshistory`, this.baseUrl).href, obj)
      .catch(this.commonService.handleObservableHttpError);
  }


}
