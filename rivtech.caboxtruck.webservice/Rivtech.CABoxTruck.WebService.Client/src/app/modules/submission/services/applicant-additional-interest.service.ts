import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { BlanketCoverageDto } from '../../../shared/models/submission/applicant/BlanketCoverageDto';
import { RiskAdditionalInterestDTO , RiskAddnlInterestListRequestDTO, IRiskAddnlInterestListResultDTO} from '@app/shared/models/submission/RiskAdditionalInterestDto';
import { catchError, map } from 'rxjs/operators';
import * as moment from 'moment';
import { formatDate } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ApplicantAdditionalInterestService {
  private genericServiceUrl: string = environment.GenericServiceUrl;
  private siteServiceUrl: string = environment.ApiUrl.replace('/api', '');


  constructor(private http: HttpClient, private commonService: CommonService) { }

  getAdditionalInterestTypes(): Observable<any> {
    return this.http.get(`${this.genericServiceUrl}/api/AdditionalInterestType`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getAdditionalInterests(riskDetailId: string, fromEndorsement: boolean = false) {
    const url_ = this.siteServiceUrl + `/api/ApplicantAdditionalInterest/GetByRisk/${riskDetailId}?fromEndorsement=${fromEndorsement}`;

    return this.http.get(url_)
            .pipe(map(data => {
              return data as RiskAdditionalInterestDTO[];
            }),
            catchError(this.commonService.handleObservableHttpError)
          );
  }

  post(additionalInterest: any): Observable<any> {
    return this.http.post(`${this.siteServiceUrl}/api/ApplicantAdditionalInterest`, additionalInterest)
      .catch(this.commonService.handleObservableHttpError);
  }

  put(additionalInterest: any): Observable<any> {
    return this.http.put<string>(`${this.siteServiceUrl}/api/ApplicantAdditionalInterest/${additionalInterest.id}`, additionalInterest)
      .catch(this.commonService.handleObservableHttpError);
  }

  delete(id: any, fromEndorsement: boolean = false, expirationDate?: Date): Observable<any> {
    let url = `${this.siteServiceUrl}/api/ApplicantAdditionalInterest/${id}?fromEndorsement=${fromEndorsement}`;
    if (expirationDate) {
      const effectiveDate = formatDate(expirationDate, 'yyyy-MM-dd', 'en');
      url = `${url}&expirationDate=${effectiveDate}`;
    }
    return this.http.delete(url)
      .catch(this.commonService.handleObservableHttpError);
  }

  putBlanketCoverage(blanketCoverage: BlanketCoverageDto): Observable<any> {
    return this.http.put<string>(`${this.siteServiceUrl}/api/ApplicantAdditionalInterest/BlanketCoverage/${blanketCoverage.riskDetailId}`, blanketCoverage)
      .catch(this.commonService.handleObservableHttpError);
  }

  getAdditionalInterestsWithPaging(payload: RiskAddnlInterestListRequestDTO): Observable<IRiskAddnlInterestListResultDTO> {
    return this.http.post(`${this.siteServiceUrl}/api/ApplicantAdditionalInterest/AddtlInterestList`,payload)
            .pipe(map(data => {
              return data as IRiskAddnlInterestListResultDTO[];
            }),
            catchError(this.commonService.handleObservableHttpError)
          );
  }

}
