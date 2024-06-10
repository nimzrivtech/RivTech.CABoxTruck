import { mergeMap as _observableMergeMap, catchError as _observableCatch, catchError } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { AgencyDTO } from '../../../shared/models/management/agency-management/agencyDto';
import { CommonService } from '../common.service';
import { ProgramStateAgentDTO } from '../../../shared/models/management/agent-management/programStateAgentDto';
import { ProgramStateResponseDTO } from '../../../shared/models/management/program-state-response.dto';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class ProgramStateAgentService {
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

    getAllProgramStateAgents(agencyId?: string, subagencyId?: string): Observable<ProgramStateAgentDTO[]> {
        let url = `${environment.ClientManagementUrl}/api/ProgramStateAgent/program/${environment.ApplicationId}`;
        if (agencyId != null && subagencyId != null) {
            url = `${environment.ClientManagementUrl}/api/ProgramStateAgent/program/${environment.ApplicationId}/agency/${agencyId}/subagency/${subagencyId}`;
        }
        if (agencyId != null && subagencyId == null) {
            url = `${environment.ClientManagementUrl}/api/ProgramStateAgent/program/${environment.ApplicationId}/agency/${agencyId}`;
        }

        return this.http.get(url)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getProgramStateAgents(agentId): Observable<ProgramStateAgentDTO> {
        return this.http.get(`${environment.ClientManagementUrl}/api/ProgramStateAgent/program/${environment.ApplicationId}/agent/${agentId}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getProgramStateAgentDetail(agentId): Observable<ProgramStateResponseDTO> {
        return this.http.get(`${environment.ClientManagementUrl}/api/ProgramStateAgentDetail/${environment.ApplicationId}/${agentId}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }
}