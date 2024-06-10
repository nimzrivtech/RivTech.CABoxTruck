import { Injectable } from '@angular/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService, private httpbackend: HttpBackend) {
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  get(riskId: any) {
    return this.http.get(new URL(`/api/QuoteReport/GetQuoteReport/${riskId}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

}
