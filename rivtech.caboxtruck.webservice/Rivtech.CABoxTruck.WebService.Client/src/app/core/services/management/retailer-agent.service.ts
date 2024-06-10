import { mergeMap as _observableMergeMap, catchError as _observableCatch, catchError } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { AgencyDTO } from '../../../shared/models/management/agency-management/agencyDto';
import { CommonService } from '../common.service';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class RetailerAgentService {
    private http: HttpClient;
    private baseUrl: string;
    private commonService: CommonService;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    _agencies: AgencyDTO[];

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(CommonService) commonService: CommonService, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = environment.ClientManagementUrl;
        this.commonService = commonService;
    }

    getAllAgents(id): Observable<any> {
        return this.http.get(`${environment.ApiUrl}/retailerAgent/byretailer/${id}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getAgentInformation(agentId: string): Observable<any> {
        return this.http.get(`${environment.ApiUrl}/retailerAgent/${agentId}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    putAgent(details: any, id): Observable<any> {
        return this.http.put(`${environment.ApiUrl}/retailerAgent/${id}`, details)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    postAgent(details: any): Observable<any> {
        return this.http.post(`${environment.ApiUrl}/retailerAgent`, details)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    deleteAgent(id: string): Observable<any> {
        return this.http.delete(`${environment.ApiUrl}/retailerAgent/${id}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }
}
