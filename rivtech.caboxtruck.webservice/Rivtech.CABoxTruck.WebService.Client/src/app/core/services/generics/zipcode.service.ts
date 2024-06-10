import { Injectable } from '@angular/core';
import { HttpBackend, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../common.service';
import { ICityStateZipDto } from '../../../shared/models/submission/riskspecifics/cityStateZip.dto';

@Injectable({
  providedIn: 'root'
})

export class ZipcodeService {
  private genericUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService, private httpbackend: HttpBackend ) {
    this.genericUrl = environment.GenericServiceUrl;
  }

  searchZipCodes(zipCode: string): Observable<any> {
    const url_ = this.genericUrl + `/api/ZipCodes/filter/${zipCode}`;
    if (zipCode === undefined || zipCode === null) {
      throw new Error('The parameter \'zipcode\' must be defined.');
    }
    return this.http.get(url_).catch(this.commonService.handleObservableHttpError);
  }

  getZipCodes(zipcode: number): Observable<any> {
    const url_ = this.genericUrl + `/api/ZipCodes/${zipcode}`;
    if (zipcode === undefined || zipcode === null) {
      throw new Error('The parameter "zipcode" must be defined.');
    }
    return this.http.get(url_).catch(this.commonService.handleObservableHttpError);
  }

  getZipCodeById(id: number): Observable<any> {
    const url_ = this.genericUrl + `/api/ZipCodes/GetByid/${id}`;
    if (id === undefined || id === null) {
      throw new Error('The parameter "id" must be defined.');
    }
    return this.http.get(url_).catch(this.commonService.handleObservableHttpError);
  }

  getZipCodeAll(): Observable<any> {
    return this.http.get(new URL(`/api/ZipCodes`, this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getZipCodesV2(zipcode: number): Observable<any> {
    let httpback = new HttpClient(this.httpbackend);
    const url_ = this.genericUrl + `/api/ZipCodes/v2/${zipcode}`;
    console.log('url_', url_);
    if (zipcode === undefined || zipcode === null) {
      throw new Error('The parameter "zipcode" must be defined.');
    }
    return httpback.post(url_, { 
      userName: environment.ClientAccess.uid, 
      secretKey: environment.ClientAccess.secretKey, 
      appCode: environment.ApplicationId })
      .catch(this.commonService.handleObservableHttpError);
  }
}
