import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../../../core/services/common.service';
import { RiskHistoryListDTO } from '../../../shared/models/submission/coverages/riskHistoryDto';

@Injectable({
  providedIn: 'root'
})
export class CoverageHistoryService {

    private baseUrl: string;
    private genericUrl: string;

    constructor(private http: HttpClient, private commonService: CommonService) {
        this.genericUrl = environment.GenericServiceUrl;
        this.baseUrl = environment.ApiUrl.replace('/api', '');
    }

    getCoverageHistory(id: string): Observable<RiskHistoryListDTO> {
        const url_ = this.baseUrl + `/api/CoverageHistory/${id}`;
        if (id === undefined || id === null) {
            throw new Error('The parameter "id" must be defined.');
        }

        return this.http.get(url_)
            .map(data => {
                return data as RiskHistoryListDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }

    saveCoverageHistory(body: RiskHistoryListDTO | undefined): Observable<RiskHistoryListDTO> {
        const url_ = this.baseUrl + '/api/CoverageHistory';

        return this.http.post(url_, body)
            .map(data => {
                return data as RiskHistoryListDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }
}
