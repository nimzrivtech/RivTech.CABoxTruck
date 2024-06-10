import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from '../../../core/services/common.service';
import { NameAndAddressDTO } from '../../../shared/models/submission/applicant/applicant-name-address';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { EntityAddressDTO } from '@app/shared/models/submission/entityAddressDto';
import { AddressDTO } from '@app/shared/models/submission/addressDto';

@Injectable({
  providedIn: 'root'
})
export class ApplicantNameAddressService {
  private readonly baseUrl: string;
  private genericUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.http = http;
    this.genericUrl = environment.GenericServiceUrl;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  getNameAndAddress(id: string): Observable<NameAndAddressDTO> {
    const url_ = this.baseUrl + `/api/ApplicantNameAndAddress/${id}`;
    if (id === undefined || id === null) {
      throw new Error('The parameter "id" must be defined.');
    }

    return this.http.get(url_)
      .pipe(
        map(data => {
          return data as NameAndAddressDTO;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  getAddress(id: string): Observable<AddressDTO> {
    const url_ = this.baseUrl + `/api/ApplicantNameAndAddress/GetAddress/${id}`;
    if (id === undefined || id === null) {
      throw new Error('The parameter "id" must be defined.');
    }

    return this.http.get(url_)
      .pipe(
        map(data => {
          return data as AddressDTO;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  saveNameAndAddress(body: NameAndAddressDTO | undefined): Observable<NameAndAddressDTO> {
    const url_ = this.baseUrl + '/api/ApplicantNameAndAddress';

    return this.http.post(url_, body)
      .map(data => {
        return data as NameAndAddressDTO;
      })
      .catch(this.commonService.handleObservableHttpError);
  }

  updateNameAndAddress(body: NameAndAddressDTO | undefined): Observable<NameAndAddressDTO> {
    const url_ = this.baseUrl + '/api/ApplicantNameAndAddress';

    return this.http.put(url_, body)
      .map(data => {
        return data as NameAndAddressDTO;
      })
      .catch(this.commonService.handleObservableHttpError);
  }

  getFeeDetailsByState(state: string): Observable<any> {
    const url = this.baseUrl + `/api/ApplicantNameAndAddress/GetFeeDetailsByState/${state}`;
    return this.http.get(url);
  }

  updateNameAndAddressMidterm(body: NameAndAddressDTO | undefined): Observable<NameAndAddressDTO> {
    const url_ = this.baseUrl + '/api/ApplicantNameAndAddress/UpdateNameAndAddressMidterm';

    return this.http.put(url_, body)
      .map(data => {
        return data as NameAndAddressDTO;
      })
      .catch(this.commonService.handleObservableHttpError);
  }

  removeAddressMidterm(id: string): Observable<any> {
    const url_ = this.baseUrl + `/api/ApplicantNameAndAddress/${id}`;
    return this.http.delete(url_).catch(this.commonService.handleObservableHttpError);
  }

  insertEntityAddress(body: EntityAddressDTO | undefined): Observable<EntityAddressDTO> {
    const url_ = this.baseUrl + `/api/ApplicantNameAndAddress/InsertEntityAddress`;
    return this.http.post(url_, body)
      .map(data => {
        return data as EntityAddressDTO;
      })
      .catch(this.commonService.handleObservableHttpError);
  }
}
