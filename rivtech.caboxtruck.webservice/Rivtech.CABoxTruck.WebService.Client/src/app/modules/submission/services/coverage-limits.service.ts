import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../../../core/services/common.service';
import { RiskCoverageDTO, SaveRiskCoverageDTO } from '../../../shared/models/submission/limits/riskCoverageDto';
import { LimitsDTO } from '../../../shared/models/submission/limits/limitsDto';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LimitsService {
    private baseUrl: string;
    readonly RATER_VERSION_SUPPORT = 7;
    constructor(private http: HttpClient, private commonService: CommonService) {
        this.baseUrl = environment.ApiUrl.replace('/api', '');
    }

    getLimits(stateCode, raterVersionId): Observable<LimitsDTO> {
        raterVersionId = raterVersionId ?? this.RATER_VERSION_SUPPORT;
        const url_ = this.baseUrl + `/api/limit/getlimits/${stateCode}/${raterVersionId}`;
        if (stateCode === undefined || stateCode === null) {
            throw new Error('The parameter "stateCode" must be defined.');
        }

        return this.http.get(url_)
            .pipe(
                map(data => {
                    return data as LimitsDTO;
                }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    // Base API
    getRiskCoverageLimit(id: string): Observable<RiskCoverageDTO> {
        const url_ = this.baseUrl + `/api/Limit/${id}`;
        if (id === undefined || id === null) {
            throw new Error('The parameter "id" must be defined.');
        }

        return this.http.get(url_)
            .map(data => {
                return data as RiskCoverageDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }

    saveRiskCoverageLimit(body: SaveRiskCoverageDTO | undefined): Observable<RiskCoverageDTO> {
        const url_ = this.baseUrl + `/api/Limit`;

        return this.http.post(url_, body)
            .map(data => {
                return data as RiskCoverageDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }

    setDefaultRiskCoverageLimit(body: SaveRiskCoverageDTO | undefined): Observable<RiskCoverageDTO> {
        const url_ = this.baseUrl + `/api/Limit/setdefaults`;

        return this.http.post(url_, body).catch(this.commonService.handleObservableHttpError);
    }
}
