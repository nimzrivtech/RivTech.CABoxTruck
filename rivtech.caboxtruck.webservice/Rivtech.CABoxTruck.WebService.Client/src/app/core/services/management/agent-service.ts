import { mergeMap as _observableMergeMap, catchError as _observableCatch, catchError } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { AgencyDTO } from '../../../shared/models/management/agency-management/agencyDto';
import { CommonService } from '../common.service';
import { AgenciesResponseDTO } from '../../../shared/models/data/dto/agent/agencies-response.dto';
import { SubAgenciesResponseDTO } from '../../..//shared/models/data/dto/agent/subagencies-response.dto';
import { AgentUserResponseDTO } from '../../../shared/models/data/dto/agent/agentuser-response.dto';
import { AgenciesSubAgenciesResponseDTO } from '../../../shared/models/data/dto/agent/agencies-subagencies.response.dto';
import { AllAgenciesSubAgenciesResponseDTO } from '../../../shared/models/data/dto/agent/all-agencies-subagencies.response.dto';
import { AgentDTO } from '../../../shared/models/management/agent-management/agentDto';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class AgentService {
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

    // Agent Module START
    getAgentUserInformation(username: string): Observable<AgentUserResponseDTO> {
        return this.http.get(`${environment.ClientManagementUrl}/api/Agent/user/${environment.ApplicationId}/${username}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    postAgent(body) {
        return this.http.post(`${environment.ClientManagementUrl}/api/Agent`, body, { responseType: 'text' })
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    putAgent(body) {
        return this.http.put(`${environment.ClientManagementUrl}/api/Agent/${body.id}`, body, { responseType: 'text' })
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }
    // Agent Module END

    getAgenciesById(agencyId?: string): Observable<AgenciesResponseDTO> {
        return this.http.get(`${environment.ClientManagementUrl}/api/Agencies/getagencydetails/${agencyId}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getSubAgenciesById(subAgencyId?: string): Observable<SubAgenciesResponseDTO> {
        return this.http.get(`${environment.ClientManagementUrl}/api/SubAgencies/getsubagencydetails/${subAgencyId}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getAgencySubAgencies(agencyId: string, subAgencyId: string): Observable<AgenciesSubAgenciesResponseDTO> {
        const programId = environment.ApplicationId;
        return this.http.get(`${environment.ClientManagementUrl}/api/ProgramStateSubAgency/program/${programId}/${agencyId}/${subAgencyId}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getAllAgenciesSubagencies(agentId: string[]): Observable<AllAgenciesSubAgenciesResponseDTO> {
        return this.http.post(`${environment.ClientManagementUrl}/api/Agent/getAllAgenciesSubagencies`, agentId)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getAgentInformation(agentId: string): Observable<AgentUserResponseDTO> {
        return this.http.get(`${environment.ClientManagementUrl}/api/Agent/${agentId}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getAllActiveLicenseAgent(subAgencyId: string): Observable<AgentDTO[]> {
        return this.http.get(`${environment.ClientManagementUrl}/api/Agent/getAllActiveLicenseAgent/${subAgencyId}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    deleteAgent(agentId: string): Observable<any> {
        return this.http.delete(`${environment.ClientManagementUrl}/api/Agent/${agentId}`, {responseType: 'text'})
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }
}