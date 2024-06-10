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
export class ProgramStateSubAgencyService {
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

    getProgramSubStateAgencyWithAgencyId(agencyId: string) {
        return this.http.get(`${environment.ClientManagementUrl}/api/ProgramStateSubAgency/program/${environment.ApplicationId}/${agencyId}`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    getProgramSubStateAgencyWithAgencyAndSubId() {
        //still in development
        return this.http.get(`${environment.ClientManagementUrl}/api/ProgramStateSubAgency/program/${environment.ApplicationId}/2242247D-E005-41E4-2AD7-08D92A6E9E79/CD928D9B-D4CC-4B65-9C95-08D9355C2884`)
            .pipe(
                catchError(this.commonService.handleObservableHttpError)
            );
    }


}