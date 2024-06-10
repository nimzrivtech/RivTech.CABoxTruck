import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf, BehaviorSubject } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { blobToText, throwException } from '../../../core/services/api-utils.service';
import { environment } from '../../../../environments/environment';
import { SaveAgencySubAgencyDTO } from '../../../shared/models/management/agency-management/save-agency-sub-agencyDto';
import { AgencySubAgencyDTO } from '../../../shared/models/management/agency-management/agencySubAgencyDto';
import { CommonService } from '../common.service';
import { AgencyRequestDTO } from '../../../shared/models/management/agency-management/agency.request.dto';
import { AgencyDetailsResponseDTO } from '../../../shared/models/management/agency-management/agency-details.response.dto';
import { SearchDTO } from '../../../shared/models/management/search.dto';
import { AgencyDTO } from '../../../shared/models/management/agency-management/agencyDto';
import { UploadAgentsThruSubAgencyDTO } from '../../../shared/models/management/uploadAgentsThruSubAgency.dto';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class AgencySubAgencyService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    _agencySubAgencies: AgencySubAgencyDTO[];

    constructor(private commonService: CommonService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string,) {
        this.http = http;
        this.baseUrl = environment.ClientManagementUrl;
    }

    // Nswag Generated functions below:

    /**
     * @return Success
     */
    getAgencySubAgenciesAll(): Observable<AgencySubAgencyDTO[]> {
        let url_ = this.baseUrl + '/api/AgencySubAgencies';
        url_ = url_.replace(/[?&]$/, '');

        const options_: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                Accept: 'text/plain'
            })
        };

        return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processAgencySubAgenciesAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAgencySubAgenciesAll(<any>response_);
                } catch (e) {
                    return <Observable<AgencySubAgencyDTO[]>><any>_observableThrow(e);
                }
            } else {
                return <Observable<AgencySubAgencyDTO[]>><any>_observableThrow(response_);
            }
        }));
    }

    protected processAgencySubAgenciesAll(response: HttpResponseBase): Observable<AgencySubAgencyDTO[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                if (Array.isArray(resultData200)) {
                    result200 = [] as any;
                    for (const item of resultData200) {
                        result200!.push(AgencySubAgencyDTO.fromJS(item));
                    }
                }
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<AgencySubAgencyDTO[]>(<any>null);
    }

    /**
     * @param body (optional) 
     * @return Success
     */
    createAgencySubAgencies(body: SaveAgencySubAgencyDTO | undefined): Observable<string> {
        let url_ = this.baseUrl + '/api/AgencySubAgencies';
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
            return this.processAgencySubAgencies(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAgencySubAgencies(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else {
                return <Observable<string>><any>_observableThrow(response_);
            }
        }));
    }

    protected processAgencySubAgencies(response: HttpResponseBase): Observable<string> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : _responseText;
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
    findAgencySubAgenciesById(id: string): Observable<AgencySubAgencyDTO> {
        let url_ = this.baseUrl + '/api/AgencySubAgencies/{id}';
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
            return this.processAgencySubAgencies2(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAgencySubAgencies2(<any>response_);
                } catch (e) {
                    return <Observable<AgencySubAgencyDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<AgencySubAgencyDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processAgencySubAgencies2(response: HttpResponseBase): Observable<AgencySubAgencyDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = AgencySubAgencyDTO.fromJS(resultData200);
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<AgencySubAgencyDTO>(<any>null);
    }

    /**
     * @param body (optional)
     * @return Success
     */
    updateAgencySubAgencies(id: string, body: SaveAgencySubAgencyDTO | undefined): Observable<string> {
        let url_ = this.baseUrl + '/api/AgencySubAgencies/{id}';
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
            return this.processAgencySubAgencies3(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAgencySubAgencies3(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else {
                return <Observable<string>><any>_observableThrow(response_);
            }
        }));
    }

    protected processAgencySubAgencies3(response: HttpResponseBase): Observable<string> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : _responseText;
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
    deleteAgencySubAgencies(id: string): Observable<string> {
        let url_ = this.baseUrl + '/api/AgencySubAgencies/{id}';
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
            return this.processAgencySubAgencies4(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAgencySubAgencies4(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else {
                return <Observable<string>><any>_observableThrow(response_);
            }
        }));
    }

    protected processAgencySubAgencies4(response: HttpResponseBase): Observable<string> {
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
    getAgencySubAgenciesByAgencyId(agencyId: string): Observable<AgencySubAgencyDTO[]> {
        let url_ = this.baseUrl + '/api/AgencySubAgencies/{agencyId}/agencies';
        if (agencyId === undefined || agencyId === null) {
            throw new Error('The parameter "agencyId" must be defined.');
        }
        url_ = url_.replace('{agencyId}', encodeURIComponent('' + agencyId));
        url_ = url_.replace(/[?&]$/, '');

        const options_: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                Accept: 'text/plain'
            })
        };

        return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processAgencies(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAgencies(<any>response_);
                } catch (e) {
                    return <Observable<AgencySubAgencyDTO[]>><any>_observableThrow(e);
                }
            } else {
                return <Observable<AgencySubAgencyDTO[]>><any>_observableThrow(response_);
            }
        }));
    }

    protected processAgencies(response: HttpResponseBase): Observable<AgencySubAgencyDTO[]> {
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
                        result200!.push(AgencySubAgencyDTO.fromJS(item));
                    }
                }
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<AgencySubAgencyDTO[]>(<any>null);
    }

    /**
     * @return Success
     */
    findAgencySubAgency(agencyId: string, subAgencyId: string): Observable<AgencySubAgencyDTO> {
        let url_ = this.baseUrl + '/api/AgencySubAgencies/{agencyId}/agencies/{subAgencyId}/sub-agencies';
        if (agencyId === undefined || agencyId === null) {
            throw new Error('The parameter "agencyId" must be defined.');
        }
        url_ = url_.replace('{agencyId}', encodeURIComponent('' + agencyId));
        if (subAgencyId === undefined || subAgencyId === null) {
            throw new Error('The parameter "subAgencyId" must be defined.');
        }
        url_ = url_.replace('{subAgencyId}', encodeURIComponent('' + subAgencyId));
        url_ = url_.replace(/[?&]$/, '');

        const options_: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                Accept: 'text/plain'
            })
        };

        return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processSubAgencies(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processSubAgencies(<any>response_);
                } catch (e) {
                    return <Observable<AgencySubAgencyDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<AgencySubAgencyDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processSubAgencies(response: HttpResponseBase): Observable<AgencySubAgencyDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = AgencySubAgencyDTO.fromJS(resultData200);
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<AgencySubAgencyDTO>(<any>null);
    }

    postSubAgencyDetails(payload: AgencyRequestDTO): Observable<string> {
        return this.http.post(`${this.baseUrl}/api/SubAgencies/savesubagencydetails`, payload, { responseType: 'text' }).map(data => {
            return data;
        }).catch(this.commonService.handleObservableHttpError);
    }

    getSubAgencyDetails(subAgencyId: string): Observable<AgencyDetailsResponseDTO> {
        return this.http.get(`${this.baseUrl}/api/SubAgencies/getsubagencydetails/${subAgencyId}`)
            .catch(this.commonService.handleObservableHttpError);
    }

    putSubAgencyDetails(payload: AgencyRequestDTO, subAgencyId: string): Observable<string> {
        return this.http.put(`${this.baseUrl}/api/SubAgencies/updatesubagencydetails/${subAgencyId}`, payload, { responseType: 'text' }).map(data => {
            return data;
        }).catch(this.commonService.handleObservableHttpError);
    }

    deleteSubAgencyDetails(id: string): Observable<string> {
        return this.http.delete(`${this.baseUrl}/api/SubAgencies/${id}`, { responseType: 'text' }).map(data => {
            return data;
        }).catch(this.commonService.handleObservableHttpError);
    }

    postSearchSubAgencyList(search: SearchDTO): Observable<any> {
        return this.http.post(`${this.baseUrl}/api/ProgramStateSubAgency/Search`, search).map(data => {
            return data as AgencyDTO[];
        }).catch(this.commonService.handleObservableHttpError);
    }

    postSearchProgramStateModalList(search: SearchDTO): Observable<any> {
        return this.http.post(`${this.baseUrl}/api/ProgramState/Search`, search).map(data => {
            return data as AgencyDTO[];
        }).catch(this.commonService.handleObservableHttpError);
    }

    postUploadAgentsThruSubAgency(uploadFile: UploadAgentsThruSubAgencyDTO): Observable<any> {
      const formData = new FormData();
      formData.append('program_id',uploadFile?.program_Id?.toString()?.trim());
      formData.append('subagency_id',uploadFile?.subagency_Id?.toString()?.trim());
      formData.append('File',uploadFile?.file);
      formData.append('agency_id',uploadFile?.agency_Id?.toString()?.trim());

      return this.http.post(`${this?.baseUrl}/api/Agent/upload`, formData).map(data => {
          return data;
      });
  }
}