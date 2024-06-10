import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import {CommonService} from '@app/core/services/common.service';

@Injectable({
  providedIn: 'root'
})



export class RiskBrokerServices {
  constructor(private http: HttpClient,
              private commonService: CommonService) {
  }

  post() {
    const risk = { };
    return this.http.post(`${environment.ApiUrl}`, risk)
      .map(data => {
        const response = data;
      })
      .catch(this.commonService.handleObservableHttpError);
  }
}
