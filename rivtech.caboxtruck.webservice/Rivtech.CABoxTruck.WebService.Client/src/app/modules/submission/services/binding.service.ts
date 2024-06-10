import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpBackend, HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../../../core/services/common.service';
import { BindingDto } from '../../../shared/models/submission/binding/BindingDto';
import { BindingRequirementsDto } from '../../../shared/models/submission/binding/BindingRequirementsDto';
import { catchError, map } from 'rxjs/operators';
import { QuoteConditionRequestDTO } from '@app/shared/models/submission/binding/QuoteConditionRequestDTO';

@Injectable({
  providedIn: 'root'
})
export class BindingService {
  private baseUrl: string;
  private genericUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService, private httpbackend: HttpBackend) {
    this.baseUrl = environment.ApiUrl.replace('/api', '');
    this.genericUrl = environment.GenericServiceUrl;
  }

  getBindOption(): Observable<any> {
    const url_ = this.baseUrl + `/api/KeyValue/generic/BindOption`;
    return this.http.get(url_)
      .pipe(
        map(data => {
          return data as any;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  getpaymentTypesList(): Observable<any> {
    const url_ = this.baseUrl + `/api/KeyValue/generic/PaymentTypes`;
    return this.http.get(url_)
                    .pipe(
                      map((data) => {
                        return data as any;
                      }),
                      catchError(this.commonService.handleObservableHttpError)
                    );
  }

  getCreditedOffices(): Observable<any> {
    const url_ = this.baseUrl + `/api/KeyValue/generic/CreditedOffice`;
    return this.http.get(url_)
                    .pipe(
                      map((data) => {
                        return data as any;
                      }),
                      catchError(this.commonService.handleObservableHttpError)
                    );
  }

  getBindStatus(): Observable<any> {
    const url_ = this.baseUrl + `/api/KeyValue/generic/BindStatus`;
    return this.http.get(url_)
                    .pipe(
                      map((data) => {
                        return data as any;
                      }),
                      catchError(this.commonService.handleObservableHttpError)
                    );
  }

  getSubjectivities(): Observable<any> {
    const url_ = this.baseUrl + `/api/KeyValue/generic/Subjectivities`;
    return this.http.get(url_)
                    .pipe(
                      map((data) => {
                        return data as any;
                      }),
                      catchError(this.commonService.handleObservableHttpError)
                    );
  }

  getSurplusLineDefault(creditId: string, state: string): Observable<any> {
    let params = new HttpParams()
      .set('state', state);
    if (creditId) {
      params = params.set('creditId', creditId);
    }

    const url_ = this.baseUrl + `/api/Binding/getSLADefault?${params.toString()}`;
    return this.http.get(url_).catch(this.commonService.handleObservableHttpError);
  }

  getbindRequirementsOption(): Observable<any> {
    const url_ = this.baseUrl + `/api/KeyValue/generic/BindRequirementsOption`;
    return this.http.get(url_)
                    .pipe(
                      map((data) => {
                        return data as any;
                      }),
                      catchError(this.commonService.handleObservableHttpError)
                    )
  }

  getQuoteConditionsOption(): Observable<any> {
    const url_ = this.baseUrl + `/api/KeyValue/generic/QuoteConditionsOption`;
    return this.http.get(url_)
                    .pipe(
                      map((data) => {
                        return data as any;
                      }),
                      catchError(this.commonService.handleObservableHttpError)
                    );
  }

  getFileUploadByRefId(id: string): Observable<any> {
    const url_ = this.baseUrl + `/api/FileUpload/GetFileUploadByRefId/${id}`;
    return this.http.get(url_).catch(this.commonService.handleObservableHttpError);
  }

  getFileUploadByRiskDetailId(id: string): Observable<any> {
    const url_ = this.baseUrl + `/api/FileUpload/GetFileUploadByRiskDetailId/${id}`;
    return this.http.get(url_).catch(this.commonService.handleObservableHttpError);
  }

  getQuoteHasRecord(id: string): Observable<any> {
    const url_ = this.baseUrl + `/api/Binding/Quote/hasRecord/${id}`;
    return this.http.get(url_).catch(this.commonService.handleObservableHttpError);
  }

  getQuoteConditionOptionIsExist(id: string, quoteConditionOptionIds: Array<number>): Observable<any> {
    const url_ = this.baseUrl + `/api/Binding/Quote/IsQuoteConditionOptionExist/${id}`;
    return this.http.post(url_, quoteConditionOptionIds).catch(this.commonService.handleObservableHttpError);
  }

  addUpdateBinding(obj: BindingDto): Observable<any> {
    return this.http.post(new URL(`/api/Binding/AddUpdate`, this.baseUrl).href, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  addUpdateBindingRequirements(obj: BindingRequirementsDto): Observable<any> {
    return this.http.post(new URL(`/api/Binding/Requirements/AddUpdate`, this.baseUrl).href, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  addUpdateBindingRequirementsWithFile(file: any): Observable<any> {
    // this.http = new HttpClient(this.httpbackend);
    return this.http.post(new URL(`/api/Binding/Requirements/AddUpdateWithFile`, this.baseUrl).href, file)
      .catch(this.commonService.handleObservableHttpError);
  }

  addUpdateQuoteConditionsWithFile(file: any): Observable<any> {
    // this.http = new HttpClient(this.httpbackend);
    return this.http.post(new URL(`/api/Binding/Quote/AddUpdateQuoteWithFile`, this.baseUrl).href, file)
      .catch(this.commonService.handleObservableHttpError);
  }

  get(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Binding/Requirements/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(new URL(`/api/Binding/Requirements/Delete/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  deleteQuote(id: string): Observable<any> {
    return this.http.delete(new URL(`/api/Binding/Quote/DeleteQuote/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getStates(): Observable<any> {
    return this.http.get(new URL('/api/State', this.genericUrl).href)
                    .pipe(
                      map((data) => {
                        return data as any;
                      }),
                      catchError(this.commonService.handleObservableHttpError)
                    );
  }

  getQuoteConditionOptions(quoteConditionRequest: QuoteConditionRequestDTO): Observable<any> {
    const url_ = this.baseUrl + `/api/Binding/Quote/SetQuoteConditionOptions`;
    return this.http.post(url_, quoteConditionRequest).catch(this.commonService.handleObservableHttpError);
  }
}
