import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { RiskCoverageDTO, RiskCoverageListDTO } from '../../../shared/models/submission/limits/riskCoverageDto';
import { Observable } from 'rxjs/Rx';
import { Guid } from 'guid-typescript';

@Injectable({
    providedIn: 'root'
})
export class QuoteOptionsService {


    private baseUrl: string;

    constructor(private http: HttpClient, private commonService: CommonService) {
        this.baseUrl = environment.ApiUrl.replace('/api', '');
    }

    // Base API
    getQuoteOptions(id: string): Observable<RiskCoverageListDTO> {
        const url_ = this.baseUrl + `/api/QuoteOption/${id}`;
        if (id === undefined || id === null) {
            throw new Error('The parameter "id" must be defined.');
        }

        return this.http.get(url_)
            .map(data => {
                return data as RiskCoverageListDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }

    getPremium(id: string): Observable<RiskCoverageListDTO> {
        const url_ = this.baseUrl + `/api/QuoteOption/GetPremium/${id}`;
        if (id === undefined || id === null) {
            throw new Error('The parameter "id" must be defined.');
        }

        return this.http.get(url_)
            .map(data => {
                return data as RiskCoverageListDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }

    saveQuoteOptions(body: RiskCoverageListDTO | undefined): Observable<RiskCoverageListDTO> {
        const url_ = this.baseUrl + `/api/QuoteOption`;

        return this.http.post(url_, body)
            .map(data => {
                return data as RiskCoverageListDTO;
            })
            .catch(this.commonService.handleObservableHttpError);
    }

    updatePaymentOption(riskDetailId: string, option: number) {
        const url_ = this.baseUrl + `/api/QuoteOption/UpdatePaymentOption/${riskDetailId}?option=${option}`;
        return this.http.put(url_, null).catch(this.commonService.handleObservableHttpError);
    }

    copyOption(riskDetailId: string, optionToCopy: number) {
        const url_ = this.baseUrl + `/api/QuoteOption/CopyOption/${riskDetailId}?optionToCopy=${optionToCopy}`;
        return this.http.post(url_, null).catch(this.commonService.handleObservableHttpError);
    }

    deleteOption(riskDetailId: string, option: RiskCoverageDTO) : Observable<any>{
        console.log(option);
        const url_ = this.baseUrl + `/api/QuoteOption/DeleteOption/${riskDetailId}?optionId=${option.id}`;
        return this.http.delete(url_).catch(this.commonService.handleObservableHttpError);
    }
}
