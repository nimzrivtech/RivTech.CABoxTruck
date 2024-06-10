import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
// { LvZipCodeDTO } from '../../../../shared/models/data/dto/quick-quote/lv-zipcode.dto';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ZipcodesService {

  baseUrl: string;
  genericUrl: string;

  constructor(private http: HttpClient) {
    // this.baseUrl = environment.ApiUrl; TODO: Uncomment
    this.baseUrl = environment.ApiUrl;
    this.genericUrl = environment.GenericServiceUrl;
  }

  getZipCode(zipCode: string) {
    const url = `${this.baseUrl}/lv/zipcode`;
    const options = {
      params: new HttpParams({
        fromString: `zipCode=${zipCode}`
      })
    };
    return this.get(url, options);
  }

  getGenericZipCode(zipCode: string) {
    const url = `${this.genericUrl}/api/ZipCodes/${zipCode}`;
    const options = { };
    return this.get(url, options);
  }

  getZipCodeWithState(zipCode: string, stateCode?: string) {
    const url = `${this.baseUrl}/Lv/zipCode`;
    const options = {
      params: new HttpParams({
        fromString: `zipCode=${zipCode}&stateCode=${stateCode}`
      })
    };
    return this.get(url, options);
  }

  getAllZipCodeByState(stateCode: string) {
    const url = `${this.baseUrl}/lv/zipCode`;
    const options = {
      params: new HttpParams({
        fromString: `stateCode=${stateCode}`
      })
    };
    return this.get(url, options);
  }

  get(url, options?) {
    return this.http.get(url, options);
  }
}
