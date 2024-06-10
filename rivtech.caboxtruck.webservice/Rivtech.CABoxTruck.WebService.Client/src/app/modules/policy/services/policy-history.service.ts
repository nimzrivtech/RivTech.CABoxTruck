import { HttpClient } from '@angular/common/http';
import { CommonService } from '@app/core/services/common.service';
import { PolicyHistoryDTO } from '@app/shared/models/policy/PolicyHistoryDto';
import { environment } from '../../../../environments/environment';
import { catchError, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class PolicyHistoryService {
    private siteServiceUrl: string = environment.ApiUrl.replace('/api', '');

    constructor(private http: HttpClient, private commonService: CommonService) { }

    getEndorsementHistory(riskId: string): Observable<PolicyHistoryDTO[]> {
        const _url = this.siteServiceUrl + `/api/PolicyHistory/${riskId}`;

        return this.http.get(_url)
                .pipe(
                    map(data => {
                        return data as PolicyHistoryDTO[];
                    }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }
}