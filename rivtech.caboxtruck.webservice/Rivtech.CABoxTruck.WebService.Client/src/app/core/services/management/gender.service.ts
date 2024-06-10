import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf, BehaviorSubject } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { blobToText, throwException } from '../../../core/services/api-utils.service';
import { environment } from '../../../../environments/environment';
import { GenderDTO } from '../../../shared/models/management/marital-gender/genderDto';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class GenderService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    _gender: GenderDTO[];

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = environment.GenericServiceUrl.replace('/api', '');
    }

    // Nswag Generated functions below:
    /**
     * @return Success
     */
    getAll(): Observable<GenderDTO[]> {
        let url_ = this.baseUrl + '/api/Genders';
        url_ = url_.replace(/[?&]$/, '');

        const options_: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                Accept: 'text/plain'
            })
        };

        return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetAll(<any>response_);
                } catch (e) {
                    return <Observable<GenderDTO[]>><any>_observableThrow(e);
                }
            } else {
                return <Observable<GenderDTO[]>><any>_observableThrow(response_);
            }
        }));
    }

    protected processGetAll(response: HttpResponseBase): Observable<GenderDTO[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (const item of resultData200) {
                    result200.push(GenderDTO.fromJS(item));
                }
            }
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<GenderDTO[]>(<any>null);
    }

    /**
     * @param body (optional) 
     * @return Success
     */
    post(body: GenderDTO | undefined): Observable<GenderDTO> {
        let url_ = this.baseUrl + '/api/Genders';
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
            return this.processPost(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processPost(<any>response_);
                } catch (e) {
                    return <Observable<GenderDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<GenderDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processPost(response: HttpResponseBase): Observable<GenderDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            const resultData200 = _responseText === '' ? null : _responseText;
            result200 = GenderDTO.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<GenderDTO>(<any>null);
    }

    /**
     * @return Success
     */
    get(id: number): Observable<GenderDTO> {
        let url_ = this.baseUrl + '/api/Genders/{id}';
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
            return this.processGet(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGet(<any>response_);
                } catch (e) {
                    return <Observable<GenderDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<GenderDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processGet(response: HttpResponseBase): Observable<GenderDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = GenderDTO.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<GenderDTO>(<any>null);
    }

    /**
     * @param body (optional)
     * @return Success
     */
    update(id: number, body: GenderDTO | undefined): Observable<GenderDTO> {
        let url_ = this.baseUrl + '/api/Genders/{id}';
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
            return this.processUpdate(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processUpdate(<any>response_);
                } catch (e) {
                    return <Observable<GenderDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<GenderDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processUpdate(response: HttpResponseBase): Observable<GenderDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            const resultData200 = _responseText === '' ? null : _responseText;
            result200 = GenderDTO.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<GenderDTO>(<any>null);
    }

    /**
     * @return Success
     */
    delete(id: number): Observable<GenderDTO> {
        let url_ = this.baseUrl + '/api/Genders/{id}';
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
            return this.processDelete(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processDelete(<any>response_);
                } catch (e) {
                    return <Observable<GenderDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<GenderDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processDelete(response: HttpResponseBase): Observable<GenderDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            const resultData200 = _responseText === '' ? null : _responseText;
            result200 = GenderDTO.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<GenderDTO>(<any>null);
    }
}