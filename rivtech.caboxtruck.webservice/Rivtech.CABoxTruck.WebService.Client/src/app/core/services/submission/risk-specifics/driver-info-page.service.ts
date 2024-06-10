import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment';
import { DriverInfoPageOptionsDto } from '../../../../shared/models/submission/riskspecifics/driver-info-page-options.dto';
import { IDriverInfoPageDto } from '../../../../shared/models/submission/riskspecifics/driver-info-page.dto';
import { CommonService } from '../../common.service';

@Injectable({
  providedIn: 'root'
})

export class DriverInfoPageService {
  private baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.baseUrl = environment.ApiUrl;
  }

  updateDriverInfoPage(riskDetailId: string, driverInfoPageData: IDriverInfoPageDto): Observable<string> {
    return this.http.put(`${this.baseUrl}/riskspecificdriverinfopage/${riskDetailId}`, driverInfoPageData, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  getDriverInfoPageOptions(): Observable<DriverInfoPageOptionsDto> {
    return this.http.get(`${this.baseUrl}/riskspecificdriverinfopage/options`)
                      .pipe(
                        map((data) => {
                          return data as DriverInfoPageOptionsDto;
                        }),
                        catchError(this.commonService.handleObservableHttpError)
                      );
  }
}
