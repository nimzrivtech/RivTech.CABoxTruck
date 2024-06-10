import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from '@app/core/services/common.service';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class PolicyCancellationService {
    private siteServiceUrl: string = environment.ApiUrl.replace('/api', '');

    constructor(private http: HttpClient, private commonService: CommonService) { }

    getCancellationReasons(): Observable<any> {
        const _url = this.siteServiceUrl + `/api/KeyValue/generic/CancellationReasons`;

        return this.http.get(_url)
                .pipe(
                    map(data => {
                        return data as any;
                    }),
                catchError(this.commonService.handleObservableHttpError)
            );
    }

    CancelPolicy(cancellationDetails: any): Observable<any> {
        return this.http.post(new URL(`/api/RiskBind/CancelPolicy`, this.siteServiceUrl).href, cancellationDetails)
            .catch(this.commonService.handleObservableHttpError);
    }

    ManualCancellationNotice(cancellationNoticeDetails: any): Observable<any> {
        return this.http.post(new URL(`/api/RiskBind/ManualCancellationNotice`, this.siteServiceUrl).href, cancellationNoticeDetails)
            .catch(this.commonService.handleObservableHttpError);
    }
}
