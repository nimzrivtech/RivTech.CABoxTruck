import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable, throwError } from 'rxjs';
import { formatDate } from '@angular/common';
import { EndorsementProcessStatusLog } from '@app/modules/policy/data/endorsement-process-status-log';
import { catchError, map } from 'rxjs/operators';
import { EndorsementProcessQueueDTO } from '@app/shared/models/policy/Issuance/endorsement-process-queue-dto';

@Injectable({
  providedIn: 'root'
})
export class EndorsementService {
  private localServiceUrl: string = environment.ApiUrl.replace('/api', '');

  constructor(private http: HttpClient, private commonService: CommonService) {
  }

  GetEndorsementPremiumChanges$(riskDetailId: string): Observable<any> {
    return this.http.get(`${this.localServiceUrl}/api/Endorsement/PremiumChanges/${riskDetailId}`).pipe(
      catchError((err) => {
        console.error(err);
        return throwError("Error occured while loading premium changes.");
      })
    );
  }

  updateEffectiveDate(riskDetailId: string, effectiveDate: string): Observable<any> {
    //const effDate = formatDate(effectiveDate, 'yyyy-MM-dd', 'en');
    return this.http.put<string>(`${this.localServiceUrl}/api/Endorsement/UpdateEffectiveDate/${riskDetailId}?effectiveDate=${effectiveDate}`, null)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateEffectiveDateByButton(riskDetailId: string, effectiveDate: string): Observable<any> {
    //const effDate = formatDate(effectiveDate, 'yyyy-MM-dd', 'en');
    return this.http.put<string>(`${this.localServiceUrl}/api/Endorsement/UpdateEffectiveDateByButton/${riskDetailId}?effectiveDate=${effectiveDate}`, null)
      .catch(this.commonService.handleObservableHttpError);
  }

  GeneratePolicyNumber(riskDetailId: string): Observable<any> {
    return this.http.post(`${this.localServiceUrl}/api/Endorsement/GeneratePolicyNumber/${riskDetailId}`, null, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  ProcessEndorsement(riskId: string, effectiveDate: string): Observable<any> {
    return this.http.post(`${this.localServiceUrl}/api/Endorsement/ProcessEndorsement/${riskId}/${effectiveDate}`, null)
      .catch(this.commonService.handleObservableHttpError);
  }

  QueueProcessEndorsement$(endorsementProcessQueueDTO: EndorsementProcessQueueDTO): Observable<any> {
    return this.http.post(`${this.localServiceUrl}/api/Endorsement/QueueProcess`, endorsementProcessQueueDTO)
      .catch(this.commonService.handleObservableHttpError);
  }

  dequeue$(riskId: string) {
   return this.http.delete(`${this.localServiceUrl}/api/Endorsement/DequeueProcess/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  GetEndorsementProcessQueue(riskId: string) : Observable<any> {
    return this.http.get(`${this.localServiceUrl}/api/Endorsement/GetProcessQueue/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }


  retrieveStatusLogs$(riskId: string) : Observable<EndorsementProcessStatusLog[]> {
    return this.http.get(`${this.localServiceUrl}/api/Endorsement/StatusLogs/${riskId}`)
      .pipe(
        map(data => data as EndorsementProcessStatusLog[])
      )
      .catch(this.commonService.handleObservableHttpError);
  }

  clearStatusLogs$(riskId: string) {
    return this.http.delete(`${this.localServiceUrl}/api/Endorsement/ClearStatusLogs/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }
 
  hasEffectiveDatePrompted$(riskId: string) : Observable<boolean> {
    return this.http.get(`${this.localServiceUrl}/api/Endorsement/EffectiveDatePrompted/${riskId}`).pipe(
      map(data => data as boolean)
    ).catch(this.commonService.handleObservableHttpError);
  }

  updateEffectiveDatePrompted$(riskId: string, prompted: boolean) {
    return this.http.put(`${this.localServiceUrl}/api/Endorsement/UpdateEffectiveDatePrompted/${riskId}`, {
      prompted: prompted
    }).catch(this.commonService.handleObservableHttpError);
  }

}
