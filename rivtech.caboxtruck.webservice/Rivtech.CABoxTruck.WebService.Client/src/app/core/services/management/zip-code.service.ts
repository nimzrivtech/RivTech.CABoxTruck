import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf, BehaviorSubject } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { blobToText, throwException } from '../api-utils.service';
import { ZipCodeDTO } from '../../../shared/models/management/zipcodeDto';
import { MaritalStatusDTO } from '../../../shared/models/management/marital-gender/maritalDto';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class ZipCodeService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    _maritalstatus: MaritalStatusDTO[];

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = environment.GenericServiceUrl.replace('/api', '');
    }

    // Nswag Generated functions below:
    /**
     * @return Success
     */
   /**
     * @return Success
     */
    getZipCode(zipCode: string | null): Observable<ZipCodeDTO> {
        let url_ = this.baseUrl + '/api/ZipCodes/{zipCode}';
        if (zipCode === undefined || zipCode === null) {
            throw new Error('The parameter "zipCode" must be defined.');
        }
        url_ = url_.replace('{zipCode}', encodeURIComponent('' + zipCode));
        url_ = url_.replace(/[?&]$/, '');

        const options_: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                Accept: 'text/plain'
            })
        };

        return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processgetZipCode(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processgetZipCode(<any>response_);
                } catch (e) {
                    return <Observable<ZipCodeDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<ZipCodeDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processgetZipCode(response: HttpResponseBase): Observable<ZipCodeDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            const resultData200 = _responseText === '' ? null : _responseText;
            result200 = resultData200; //ZipCodeDTO.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<ZipCodeDTO>(<any>null);
    }

    /**
     * @return Success
     */
    getZipCodeById(id: number): Observable<ZipCodeDTO> {
        let url_ = this.baseUrl + '/api/ZipCodes/GetByid/{id}';
        if (id === undefined || id === null) {
            throw new Error('The parameter "id" must be defined.');
        }
        url_ = url_.replace('{id}', encodeURIComponent('' + id));
        url_ = url_.replace(/[?&]$/, '');

        const options_: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                Accept: 'text/plain'
            })
        };

        return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetZipCodeById(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetZipCodeById(<any>response_);
                } catch (e) {
                    return <Observable<ZipCodeDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<ZipCodeDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processGetZipCodeById(response: HttpResponseBase): Observable<ZipCodeDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = ZipCodeDTO.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<ZipCodeDTO>(<any>null);
    }
}