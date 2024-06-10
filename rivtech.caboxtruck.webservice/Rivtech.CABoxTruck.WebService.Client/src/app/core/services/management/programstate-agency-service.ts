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
import { ProgramStateAgentDTO } from '../../../shared/models/management/agent-management/programStateAgentDto';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class ProgramStateAgencyService {
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

    getAllProgramStateAgency(): Observable<any> {
        return this.http.get(`${environment.ClientManagementUrl}/api/ProgramStateAgency/program/${environment.ApplicationId}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }
}