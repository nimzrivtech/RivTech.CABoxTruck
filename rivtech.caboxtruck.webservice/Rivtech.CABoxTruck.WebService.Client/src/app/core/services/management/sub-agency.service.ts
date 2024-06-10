import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { blobToText, throwException } from '../../../core/services/api-utils.service';
import { environment } from '../../../../environments/environment';
import { SubAgencyDTO } from '../../../shared/models/management/agency-management/sub-agencyDto';
import { SaveSubAgencyDTO } from '../../../shared/models/management/agency-management/save-sub-agencyDto';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class SubAgencyService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    _subAgencies: SubAgencyDTO[];

    constructor(@Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = environment.ClientManagementUrl;
    }

    // Nswag Generated functions below:

    /**
     * @return Success
     */
    getAllSubAgencies(): Observable<SubAgencyDTO[]> {
        let url_ = this.baseUrl + '/api/SubAgencies';
        url_ = url_.replace(/[?&]$/, '');

        const options_: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                Accept: 'text/plain'
            })
        };

        return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processSubAgenciesAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processSubAgenciesAll(<any>response_);
                } catch (e) {
                    return <Observable<SubAgencyDTO[]>><any>_observableThrow(e);
                }
            } else {
                return <Observable<SubAgencyDTO[]>><any>_observableThrow(response_);
            }
        }));
    }

    protected processSubAgenciesAll(response: HttpResponseBase): Observable<SubAgencyDTO[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
                if (Array.isArray(resultData200)) {
                    result200 = [] as any;
                    for (const item of resultData200) {
                        result200.push(SubAgencyDTO.fromJS(item));
                    }
                }
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<SubAgencyDTO[]>(<any>null);
    }

    /**
     * @param body (optional)
     * @return Success
     */
    createSubAgencies(body: SaveSubAgencyDTO | undefined): Observable<string> {
        let url_ = this.baseUrl + '/api/SubAgencies';
        url_ = url_.replace(/[?&]$/, '');

        const content_ = JSON.stringify(body);

        const options_: any = {
            body: content_,
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                Accept: 'text/plain'
            })
        };

        return this.http.request('post', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processSubAgencies(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processSubAgencies(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else {
                return <Observable<string>><any>_observableThrow(response_);
            }
        }));
    }

    protected processSubAgencies(response: HttpResponseBase): Observable<string> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<string>(<any>null);
    }

    /**
     * @return Success
     */
    findSubAgencies(id: string): Observable<SubAgencyDTO> {
        let url_ = this.baseUrl + '/api/SubAgencies/{id}';
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
            return this.processSubAgencies2(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processSubAgencies2(<any>response_);
                } catch (e) {
                    return <Observable<SubAgencyDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<SubAgencyDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processSubAgencies2(response: HttpResponseBase): Observable<SubAgencyDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = SubAgencyDTO.fromJS(resultData200);
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<SubAgencyDTO>(<any>null);
    }

    /**
     * @param body (optional)
     * @return Success
     */
    updateSubAgencies(id: string, body: SaveSubAgencyDTO | undefined): Observable<string> {
        let url_ = this.baseUrl + '/api/SubAgencies/{id}';
        if (id === undefined || id === null) {
            throw new Error('The parameter "id" must be defined.');
        }
        url_ = url_.replace('{id}', encodeURIComponent('' + id));
        url_ = url_.replace(/[?&]$/, '');

        const content_ = JSON.stringify(body);

        const options_: any = {
            body: content_,
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                Accept: 'text/plain'
            })
        };

        return this.http.request('put', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processSubAgencies3(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processSubAgencies3(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else {
                return <Observable<string>><any>_observableThrow(response_);
            }
        }));
    }

    protected processSubAgencies3(response: HttpResponseBase): Observable<string> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<string>(<any>null);
    }

    /**
     * @return Success
     */
    deleteSubAgencies(id: string): Observable<string> {
        let url_ = this.baseUrl + '/api/SubAgencies/{id}';
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

        return this.http.request('delete', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processSubAgencies4(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processSubAgencies4(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else {
                return <Observable<string>><any>_observableThrow(response_);
            }
        }));
    }

    protected processSubAgencies4(response: HttpResponseBase): Observable<string> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : _responseText; //JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<string>(<any>null);
    }
}