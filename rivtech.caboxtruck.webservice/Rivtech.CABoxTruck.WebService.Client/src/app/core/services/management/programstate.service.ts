import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../common.service';

@Injectable({
  providedIn: 'root'
})
export class ProgramstateService {
  private http: HttpClient;
  private baseUrl: string;
  private commonService: CommonService;

  constructor(@Inject(HttpClient) http: HttpClient, @Inject(CommonService) commonService: CommonService) {
    this.http = http;
    this.baseUrl = environment.ClientManagementUrl;
    this.commonService = commonService;
  }

  getProgramStatesList() {
    return this.http.get(`${this.baseUrl}/api/ProgramState`)
        .pipe(
            catchError(this.commonService.handleObservableHttpError)
        );
  }
}
