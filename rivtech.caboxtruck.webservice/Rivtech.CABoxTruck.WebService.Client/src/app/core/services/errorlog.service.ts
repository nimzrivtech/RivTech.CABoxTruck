import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ErrorLogDTO } from '../../shared/models/submission/errorlogDto';
import { CommonService } from './common.service';

@Injectable({
  providedIn: 'root'
})
export class ErrorlogService {
  private baseUrl: string;
  constructor(private http: HttpClient, private commonService: CommonService) {
    this.baseUrl = environment.ApiUrl;
   }

  saveErrorLog(errorLog: ErrorLogDTO): Observable<any> {
    const url_ = this.baseUrl + `/ErrorLog`;
    return this.http.post(url_, errorLog).catch(this.commonService.handleObservableHttpError);
  }
}
