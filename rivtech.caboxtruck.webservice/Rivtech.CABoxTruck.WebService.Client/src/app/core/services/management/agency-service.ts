import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf, BehaviorSubject } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { blobToText, throwException } from '../../../core/services/api-utils.service';
import { environment } from '../../../../environments/environment';
import { AgencyDTO } from '../../../shared/models/management/agency-management/agencyDto';
import { SubAgencyDTO } from '../../../shared/models/management/agency-management/sub-agencyDto';
import { SaveAgencyDTO } from '../../../shared/models/management/agency-management/save-agency-Dto';
import { AgencySubAgencyDTO } from '../../../shared/models/management/agency-management/agencySubAgencyDto';
import { SaveAgencySubAgencyDTO } from '../../../shared/models/management/agency-management/save-agency-sub-agencyDto';
import { SaveSubAgencyDTO } from '../../../shared/models/management/agency-management/save-sub-agencyDto';
import { SearchDTO } from '../../../shared/models/management/search.dto';
import { CommonService } from '../common.service';
import { AgencyRequestDTO } from '../../../shared/models/management/agency-management/agency.request.dto';
import { AgencyDetailsResponseDTO } from '../../../shared/models/management/agency-management/agency-details.response.dto';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class AgencyService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    _agencies: AgencyDTO[];

    constructor(private commonService: CommonService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = environment.ClientManagementUrl;
    }

    // Nswag Generated functions below:

   /**
    * @return Success
    */
    agenciesAll(): Observable<AgencyDTO[]> {
        const url_ = this.baseUrl + `/api/Agencies`;
        return this.http.get(url_)
            .map(data => {
                return data as AgencyDTO[];
            });
    }

    /**
     * @param body (optional)
     * @return Success
     */
    agencies(body: SaveAgencyDTO | undefined): Observable<string> {
        let url_ = this.baseUrl + '/api/Agencies';
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
            return this.processAgencies(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAgencies(<any>response_);
                } catch (e) {
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else {
                return <Observable<string>><any>_observableThrow(response_);
            }
        }));
    }

    protected processAgencies(response: HttpResponseBase): Observable<string> {
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


    /**
     * @param body (optional)
     * @return Success
     */
    update(id: string, body: SaveAgencyDTO | undefined): Observable<string> {
        let url_ = this.baseUrl + '/api/Agencies/{id}';
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
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else {
                return <Observable<string>><any>_observableThrow(response_);
            }
        }));
    }

    protected processUpdate(response: HttpResponseBase): Observable<string> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : _responseText;//JSON.parse(_responseText, this.jsonParseReviver);
                result200 = SaveAgencyDTO.fromJS(resultData200);
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
    find(id: string): Observable<AgencyDTO> {
        let url_ = this.baseUrl + '/api/Agencies/{id}';
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
            return this.processAgencies2(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processAgencies2(<any>response_);
                } catch (e) {
                    return <Observable<AgencyDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<AgencyDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processAgencies2(response: HttpResponseBase): Observable<AgencyDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = AgencyDTO.fromJS(resultData200);
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<AgencyDTO>(<any>null);
    }

    /**
     * @return Success
     */
    delete(id: string): Observable<string> {
        let url_ = this.baseUrl + '/api/Agencies/{id}';
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
                    return <Observable<string>><any>_observableThrow(e);
                }
            } else {
                return <Observable<string>><any>_observableThrow(response_);
            }
        }));
    }

    protected processDelete(response: HttpResponseBase): Observable<string> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : _responseText; //JSON.parse(_responseText, this.jsonParseReviver);
                result200 = AgencyDTO.fromJS(resultData200);
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<string>(<any>null);
    }

    // Sub Agencies

    /**
     * @return Success
     */
    subAgenciesAll(agencyId: string): Observable<AgencySubAgencyDTO[]> {
        let url_ = this.baseUrl + '/api/Agencies/{agencyId}/sub-agencies';
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
            return this.processSubAgenciesAll(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processSubAgenciesAll(<any>response_);
                } catch (e) {
                    return <Observable<AgencySubAgencyDTO[]>><any>_observableThrow(e);
                }
            } else {
                return <Observable<AgencySubAgencyDTO[]>><any>_observableThrow(response_);
            }
        }));
    }

    protected processSubAgenciesAll(response: HttpResponseBase): Observable<AgencySubAgencyDTO[]> {
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
                        result200.push(AgencySubAgencyDTO.fromJS(item));
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
    addAgencySubAgency(agencyId: string, body: SaveAgencySubAgencyDTO | undefined): Observable<AgencySubAgencyDTO> {
        let url_ = this.baseUrl + '/api/Agencies/{agencyId}/sub-agencies';
        if (agencyId === undefined || agencyId === null) {
            throw new Error('The parameter "agencyId" must be defined.');
        }
        url_ = url_.replace('{agencyId}', encodeURIComponent('' + agencyId));
        url_ = url_.replace(/[?&]$/, '');

        const content_ = JSON.stringify(body);

        const options_: any = {
            body: content_,
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Accept': 'text/plain'
            })
        };

        return this.http.request('post', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processaddAgencySubAgency(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processaddAgencySubAgency(<any>response_);
                } catch (e) {
                    return <Observable<AgencySubAgencyDTO>><any>_observableThrow(e);
                }
            } else {
                return <Observable<AgencySubAgencyDTO>><any>_observableThrow(response_);
            }
        }));
    }

    protected processaddAgencySubAgency(response: HttpResponseBase): Observable<AgencySubAgencyDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : _responseText;//JSON.parse(_responseText, this.jsonParseReviver);
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
     * @return Success
     */
    getSubAgency(agencyId: number, subAgencyId: number): Observable<SubAgencyDTO> {
        let url_ = this.baseUrl + '/api/Agencies/{agencyId}/sub-agencies/{subAgencyId}';
        if (agencyId === undefined || agencyId === null) {throw new Error('The parameter \'agencyId\' must be defined.');}
        url_ = url_.replace('{agencyId}', encodeURIComponent('' + agencyId));
        if (subAgencyId === undefined || subAgencyId === null) {throw new Error('The parameter \'subAgencyId\' must be defined.');}
        url_ = url_.replace('{subAgencyId}', encodeURIComponent('' + subAgencyId));
        url_ = url_.replace(/[?&]$/, '');

        const options_: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                'Accept': 'text/plain'
            })
        };

        return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processGetSubAgency(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetSubAgency(<any>response_);
                } catch (e) {
                    return <Observable<SubAgencyDTO>><any>_observableThrow(e);
                }
            } else {return <Observable<SubAgencyDTO>><any>_observableThrow(response_);}
        }));
    }

    protected processGetSubAgency(response: HttpResponseBase): Observable<SubAgencyDTO> {
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
    updateSubAgencies(agencyId: string, subAgencyId: string, body: SaveAgencySubAgencyDTO | undefined): Observable<SaveSubAgencyDTO> {
        let url_ = this.baseUrl + '/api/Agencies/{agencyId}/sub-agencies/{subAgencyId}';
        if (agencyId === undefined || agencyId === null) {throw new Error('The parameter \'agencyId\' must be defined.');}
        url_ = url_.replace('{agencyId}', encodeURIComponent('' + agencyId));
        if (subAgencyId === undefined || subAgencyId === null) {throw new Error('The parameter \'subAgencyId\' must be defined.');}
        url_ = url_.replace('{subAgencyId}', encodeURIComponent('' + subAgencyId));
        url_ = url_.replace(/[?&]$/, '');

        const content_ = JSON.stringify(body);

        const options_: any = {
            body: content_,
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Accept': 'text/plain'
            })
        };

        return this.http.request('put', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processUpdateSubAgencies(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processUpdateSubAgencies(<any>response_);
                } catch (e) {
                    return <Observable<SaveSubAgencyDTO>><any>_observableThrow(e);
                }
            } else {return <Observable<SaveSubAgencyDTO>><any>_observableThrow(response_);}
        }));
    }

    protected processUpdateSubAgencies(response: HttpResponseBase): Observable<SaveSubAgencyDTO> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                let result200: any = null;
                const resultData200 = _responseText === '' ? null : _responseText; //JSON.parse(_responseText, this.jsonParseReviver);
                result200 = SaveSubAgencyDTO.fromJS(resultData200);
                return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<SaveSubAgencyDTO>(<any>null);
    }

    /**
     * @return Success
     */
    deleteSubAgency(agencyId: number, subAgencyId: number): Observable<void> {
        let url_ = this.baseUrl + '/api/Agencies/{agencyId}/sub-agencies/{subAgencyId}';
        if (agencyId === undefined || agencyId === null) {throw new Error('The parameter \'agencyId\' must be defined.');}
        url_ = url_.replace('{agencyId}', encodeURIComponent('' + agencyId));
        if (subAgencyId === undefined || subAgencyId === null) {throw new Error('The parameter \'subAgencyId\' must be defined.');}
        url_ = url_.replace('{subAgencyId}', encodeURIComponent('' + subAgencyId));
        url_ = url_.replace(/[?&]$/, '');

        const options_: any = {
            observe: 'response',
            responseType: 'blob',
            headers: new HttpHeaders({
            })
        };

        return this.http.request('delete', url_, options_).pipe(_observableMergeMap((response_: any) => {
            return this.processDeleteSubAgency(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processDeleteSubAgency(<any>response_);
                } catch (e) {
                    return <Observable<void>><any>_observableThrow(e);
                }
            } else {return <Observable<void>><any>_observableThrow(response_);}
        }));
    }

    protected processDeleteSubAgency(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
                (<any>response).error instanceof Blob ? (<any>response).error : undefined;

        const _headers: any = {}; if (response.headers) { for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return _observableOf<void>(<any>null);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
                return throwException('An unexpected server error occurred.', status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(<any>null);
    }

    postSearchList_old(search: SearchDTO): Observable<AgencyDTO[]> {
        return this.http.post(`${this.baseUrl}/api/Agencies/Search`, search).map(data => {
            return data as AgencyDTO[];
        }).catch(this.commonService.handleObservableHttpError);
    }

    postSearchList(search: SearchDTO): Observable<any> {
        return this.http.post(`${this.baseUrl}/api/ProgramStateAgency/Search`, search).map(data => {
            return data as AgencyDTO[];
        }).catch(this.commonService.handleObservableHttpError);
    }

    postSearchAgentList(search: SearchDTO): Observable<any> {
        return this.http.post(`${this.baseUrl}/api/ProgramStateAgent/Search`, search).map(data => {
            return data as any;
        }).catch(this.commonService.handleObservableHttpError);
    }

    postAgencyDetails(payload: AgencyRequestDTO): Observable<string> {
      return this.http.post(`${this.baseUrl}/api/Agencies/saveagencydetails`, payload, {responseType: 'text'}).map(data => {
            return data;
      }).catch(this.commonService.handleObservableHttpError);
    }

    putAgencyDetails(payload: AgencyRequestDTO, agencyId: string): Observable<string> {
        return this.http.put(`${this.baseUrl}/api/Agencies/updateagencydetails/${agencyId}`, payload, {responseType: 'text'}).map(data => {
              return data;
        }).catch(this.commonService.handleObservableHttpError);
    }

    deleteAgencyDetails(agencyId: string): Observable<string> {
        return this.http.delete(`${this.baseUrl}/api/Agencies/${agencyId}`, { responseType: 'text' }).map(data => {
            return data;
        }).catch(this.commonService.handleObservableHttpError);
    }

    getAgencyDetails(agencyId: string): Observable<AgencyDetailsResponseDTO> {
      return this.http.get(`${this.baseUrl}/api/Agencies/getagencydetails/${agencyId}`)
        .catch(this.commonService.handleObservableHttpError);
    }

    getRegionalSalesManagerList(): Observable<any> {
        /* return this.http.get(`${this.baseUrl}/regionalmanagers/${environment.ApplicationId}`)
          .catch(this.commonService.handleObservableHttpError); */

        // Workaround to get Regional Sales Manager data while Environment AppId = 4;
        return this.http.get(`${this.baseUrl}/regionalmanagers/1`)
            .catch(this.commonService.handleObservableHttpError);
    }

    getCommissionGroup(): Observable<any> {
      return this.http.get(`${this.baseUrl}/api/CommissionGroup`)
        .catch(this.commonService.handleObservableHttpError);
    }
}