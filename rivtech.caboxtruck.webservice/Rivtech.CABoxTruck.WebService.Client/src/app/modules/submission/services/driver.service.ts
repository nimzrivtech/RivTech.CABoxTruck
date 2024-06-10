import { Injectable } from '@angular/core';
import { HttpBackend, HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { DriverDto, DriverHeaderDto } from '../../../shared/models/submission/Driver/DriverDto';
import { formatDate } from '@angular/common';
import { catchError, map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class DriverService {
 
  private baseUrl: string;
  private genericUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService, private httpbackend: HttpBackend) {
    this.genericUrl = environment.GenericServiceUrl;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  getStates(): Observable<any> {
    return this.http.get(new URL('/api/State', this.genericUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getRiskDrivers(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Driver/GetByRiskInclude/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  getRiskExportDrivers(id: string, fromPolicy : boolean): Observable<DriverDto> {
    
    const urlpol_ = this.baseUrl + `/api/Driver/GetByRiskIncludeExport/${id}?fromPolicy=${fromPolicy}`;
      return this.http.get(fromPolicy? urlpol_: urlpol_)
      .pipe(
        map(data => {
          return data as DriverDto[];
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }
  
//  putExportReport(obj: DriverDto, fromEndorsement: boolean = false): Observable<any> {
//     return this.http.put(`${this.baseUrl}/api/Driver/${obj.id}/${fromEndorsement}`, obj)
//       .catch(this.commonService.handleObservableHttpError);
//   }

  get(id: string): Observable<any> {
    return this.http.get(new URL(`/api/Driver/GetInclude/${id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  put(obj: DriverDto, fromEndorsement: boolean = false): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Driver/${obj.id}/${fromEndorsement}`, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  PutListOptions(obj: DriverDto[]): Observable<any> {
    return this.http.put(`${this.baseUrl}/api/Driver/PutListOptions`, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  post(obj: DriverDto): Observable<any> {
    return this.http.post(new URL(`/api/Driver/Post/`, this.baseUrl).href, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  postDrivers(drivers: DriverDto[]): Observable<DriverDto[]> {
    return this.http.post(`${this.baseUrl}/api/Driver/ExcelDrivers`, drivers)
      .catch(this.commonService.handleObservableHttpError);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/api/Driver/Delete/${id}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  deleteAll(id:string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/api/Driver/DeleteAll/${id}`)
      .catch(this.commonService.handleObservableHttpError);
  } 
  addUpdateHeader(obj: DriverHeaderDto): Observable<any> {
    return this.http.post(new URL(`/api/Driver/DriverHeader/Update/`, this.baseUrl).href, obj)
      .catch(this.commonService.handleObservableHttpError);
  }

  downloadTemplate(): Observable<any> {
    //this.http = new HttpClient(this.httpbackend);

    return this.http.get(new URL(`/api/Driver/Template`, this.baseUrl).href, { reportProgress: true, observe: 'events', responseType: 'blob' })
      .catch(this.commonService.handleObservableHttpError);
  }

  uploadExcelFile(inputData?: any): Observable<DriverDto[]> {
    // this.http = new HttpClient(this.httpbackend);
    // const headers = new HttpHeaders();
    // headers.set('Content-Type', 'multipart/form-data;');

    return this.http.post(new URL(`/api/Driver/AddFromExcel`, this.baseUrl).href, inputData, {
      // headers: headers,
      reportProgress: true, observe: 'events', responseType: 'json'
    })
      .catch(this.commonService.handleObservableHttpError);
  }

  getPreviousDrivers(driver: DriverDto): Observable<any> {
    return this.http.get(new URL(`/api/Driver/GetPreviousDrivers/${driver.id}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  reinstate(id: string) {
    return this.http.put(`${this.baseUrl}/api/Driver/reinstate/${id}`, null)
      .catch(this.commonService.handleObservableHttpError);
  }

  getAllDriverData() {
    return this.http.get(new URL(`${this.baseUrl}/api/Driver/GetAllInclude`).href)
      .catch(this.commonService.handleObservableHttpError);
  }
  
  getDriversByLastNameAndLicenseNumber(lastName: string, licenseNumber: string): Observable<any> {
    return this.http.get(new URL(`/api/Driver/GetDriversByLastNameAndLicenseNumber/${lastName}/${licenseNumber}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateDriverExclusionEndorsementForm$(riskDetailId: string): Observable<any> {
    let url = new URL(`/api/Driver/updateDriverExclusionEndorsementForm/${riskDetailId}`, this.baseUrl);
    return this.http.put(url.href, null)
      .catch(this.commonService.handleObservableHttpError);
  }

  getDriverHeader$(riskDetailId: string) {
    return this.http.get(new URL(`/api/Driver/DriverHeader/${riskDetailId}`, this.baseUrl).href)
      .catch(this.commonService.handleObservableHttpError);
  }
 
}
