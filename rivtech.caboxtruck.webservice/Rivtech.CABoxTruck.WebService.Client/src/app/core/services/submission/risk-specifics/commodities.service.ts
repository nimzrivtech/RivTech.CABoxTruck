import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { CommonService } from '../../common.service';

@Injectable({
  providedIn: 'root'
})

export class CommoditiesService {
  private baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.baseUrl = environment.ApiUrl;
  }

  insert(commodity) {
    return this.http.post(`${this.baseUrl}/riskspecificcommoditieshauled`, commodity)
      .catch(this.commonService.handleObservableHttpError);
  }

  update(commodity) {
    return this.http.put(`${this.baseUrl}/riskspecificcommoditieshauled/${commodity.id}`, commodity)
      .catch(this.commonService.handleObservableHttpError);
  }

  delete(id: string) {
    return this.http.delete(`${this.baseUrl}/riskspecificcommoditieshauled/${id}`)
      .catch(this.commonService.handleObservableHttpError);
  }

}
