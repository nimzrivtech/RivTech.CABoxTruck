import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf, BehaviorSubject } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { blobToText, throwException } from '../../../core/services/api-utils.service';
import { environment } from '../../../../environments/environment';
import { AgencyDTO } from '../../../shared/models/management/agency-management/agencyDto';
import { AddressTypeDTO } from '../../../shared/models/management/agency-management/address-typeDto';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class AddressTypeService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    _agencies: AgencyDTO[];

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = environment.GenericServiceUrl;
    }

    // Nswag Generated functions below:

    /**
     * @return Success
     */
    addressTypesAll(): Observable<AddressTypeDTO[]> {
        let url_ = this.baseUrl + '/api/AddressType';
        url_ = url_.replace(/[?&]$/, '');

        const options_: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                Accept: 'text/plain'
            })
        };

        return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processAddressTypesAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAddressTypesAll(<any>response_);
                } catch (e) {
                    return <Observable<AddressTypeDTO[]>><any>_observableThrow(e);
                }
            } else {
                return <Observable<AddressTypeDTO[]>><any>_observableThrow(response_);
            }
        }));
    }

    protected processAddressTypesAll(response: HttpResponseBase): Observable<AddressTypeDTO[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            const resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (const item of resultData200) {
                    result200.push(AddressTypeDTO.fromJS(item));
                }
            }
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<AddressTypeDTO[]>(<any>null);
    }
}
