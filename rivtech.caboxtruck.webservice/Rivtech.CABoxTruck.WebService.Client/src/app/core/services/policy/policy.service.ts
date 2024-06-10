import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PolicyPremiumDetailsDTO } from '@app/modules/policy/components/policy-details/sections/policy-rating/policy-premium-details-dto';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { CommonService } from '../common.service';

@Injectable({
  providedIn: 'root'
})
export class PolicyService {

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  getPolicyPremiumDetails$(riskId: string, riskDetailId?: string): Observable<PolicyPremiumDetailsDTO> {
    let url = `${environment.ApiUrl}/PolicyPremiumDetails/details/${riskId}`;
    if(riskDetailId != null) {
      url += `/${riskDetailId}`;
    }

    return this.http.get(url)
      .pipe(
        map(data => data as PolicyPremiumDetailsDTO),
      ).catch(this.commonService.handleObservableHttpError);
  }
}
