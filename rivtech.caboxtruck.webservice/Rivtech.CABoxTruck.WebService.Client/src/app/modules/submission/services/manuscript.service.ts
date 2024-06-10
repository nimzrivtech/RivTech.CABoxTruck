import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { ManuscriptDTO, ManuscriptListRequestDTO, IManuscriptListResultDTO } from '@app/shared/models/submission/manuscript/ManuscriptDto';
import * as moment from 'moment';
import { formatDate } from '@angular/common';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ManuscriptService {
  private siteServiceUrl: string = environment.ApiUrl.replace('/api', '');

  constructor(private http: HttpClient, private commonService: CommonService) { }

  get(id: string): Observable<any> {
    return this.http.get(`${this.siteServiceUrl}/api/Manuscripts/${id}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  post(manuscript: ManuscriptDTO): Observable<any> {
    return this.http.post(`${this.siteServiceUrl}/api/Manuscripts`, manuscript)
      .catch(this.commonService.handleObservableHttpError);
  }

  put(manuscript: any): Observable<any> {
    return this.http.put<string>(`${this.siteServiceUrl}/api/Manuscripts/${manuscript.id}`, manuscript)
      .catch(this.commonService.handleObservableHttpError);
  }

  putDates(manuscript: ManuscriptDTO): Observable<any> {
    return this.http.put(`${this.siteServiceUrl}/api/Manuscripts/Dates/${manuscript.riskDetailId}`, manuscript)
      .catch(this.commonService.handleObservableHttpError);
  }

  delete(id: string, fromEndorsement: boolean = false, expirationDate?: Date): Observable<any> {
    let url = `${this.siteServiceUrl}/api/Manuscripts/${id}?fromEndorsement=${fromEndorsement}`;
    if (expirationDate) {
      const effectiveDate = formatDate(expirationDate, 'yyyy-MM-dd', 'en');
      url = `${url}&expirationDate=${effectiveDate}`;
    }
    return this.http.delete(url)
      .catch(this.commonService.handleObservableHttpError);
  }

  getManuscriptWithPaging(payload: ManuscriptListRequestDTO): Observable<IManuscriptListResultDTO> {
    return this.http.post(`${this.siteServiceUrl}/api/Manuscripts/ManuscriptList`,payload)
            .pipe(map(data => {
              return data as IManuscriptListResultDTO[];
            }),
            catchError(this.commonService.handleObservableHttpError)
          );
  }

}
