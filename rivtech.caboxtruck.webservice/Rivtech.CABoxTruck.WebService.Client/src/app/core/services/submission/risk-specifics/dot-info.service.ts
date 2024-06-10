import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment';
import { DotInfoPageOptionsDto } from '../../../../shared/models/submission/riskspecifics/dot-info-page-options.dto';
import { IDotInfoDto } from '../../../../shared/models/submission/riskspecifics/dot-info.dto';
import { CommonService } from '../../common.service';

@Injectable({
  providedIn: 'root'
})

export class DotInfoService {
  private baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.baseUrl = environment.ApiUrl;
  }

  updateDotInfo(riskDetailId: string, dotInfoData: IDotInfoDto): Observable<string> {
    return this.http.put(`${this.baseUrl}/riskspecificdotinfo/${riskDetailId}`, dotInfoData, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  getDotInfoPageOptions(): Observable<DotInfoPageOptionsDto> {
    return this.http.get(`${this.baseUrl}/riskspecificdotinfo/options`)
                      .pipe(
                        map((data) => {
                          return data as DotInfoPageOptionsDto;
                        }),
                        catchError(this.commonService.handleObservableHttpError)
                      );
  }
}
