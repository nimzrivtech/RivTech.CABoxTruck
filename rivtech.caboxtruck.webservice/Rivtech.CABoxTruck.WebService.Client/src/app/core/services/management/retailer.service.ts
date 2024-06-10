import { mergeMap as _observableMergeMap, catchError as _observableCatch, catchError } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { AgencyDTO } from '../../../shared/models/management/agency-management/agencyDto';
import { CommonService } from '../common.service';
import { AgencyDetailsResponseDTO } from '@app/shared/models/management/agency-management/agency-details.response.dto';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class RetailerService {
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    _agencies: AgencyDTO[];

    constructor(
        private http: HttpClient, 
        private commonService: CommonService,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        this.baseUrl = environment.ClientManagementUrl;
    }

    getAllRetailers(): Observable<any> {
        return this.http.get(`${environment.ApiUrl}/retailer`)
            .pipe(catchError(this.commonService.handleObservableHttpError));
    }

    getAllRetailersByAgency(agencyId: string): Observable<any> {
        return this.http.get(`${environment.ApiUrl}/retailer/agency/${agencyId}`)
            .pipe(catchError(this.commonService.handleObservableHttpError));
    }

    getRetailerDetails(retailerId: string): Observable<AgencyDetailsResponseDTO> {
        return this.http.get(`${environment.ApiUrl}/retailer/${retailerId}`)
            .catch(this.commonService.handleObservableHttpError);
    }

    putRetailerDetails(details: any, retailerId): Observable<any> {
        return this.http.put(`${environment.ApiUrl}/retailer/${retailerId}`, details)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    postRetailerDetails(details: any): Observable<any> {
        return this.http.post(`${environment.ApiUrl}/retailer`, details)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    deleteRetailer(id: string): Observable<any> {
        return this.http.delete(`${environment.ApiUrl}/retailer/${id}`)
            .pipe(catchError(this.commonService.handleObservableHttpError));
    }

}
