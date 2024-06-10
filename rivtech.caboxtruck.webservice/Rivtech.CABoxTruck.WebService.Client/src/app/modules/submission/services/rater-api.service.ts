import { Injectable } from '@angular/core';
import { HttpBackend, HttpClient, HttpParams } from '@angular/common/http';
import { CommonService } from '../../../core/services/common.service';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { formatDate } from '@angular/common';
import { AuthService } from '@app/core/services/auth.service';
import { map } from 'rxjs/operators';
import { ExperienceRaterDto, RaterVersion } from '../data/rater-api.data';

@Injectable({
  providedIn: 'root'
})
export class RaterApiService {

  private raterUrl: string;
  private baseUrl: string;

  constructor(
    private http: HttpClient, 
    private commonService: CommonService, 
    private httpbackend: HttpBackend,
    private authService: AuthService
  ) {
    this.raterUrl = environment.ApiUrl.replace('/api', '');
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  test(): Observable<any> {
    return this.http.get(new URL(`/api/ExperienceRater/testconnection`, this.baseUrl).href, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  upload(obj: any, id: string): Observable<any> {

    this.http = new HttpClient(this.httpbackend);
    return this.http.post(new URL(`/api/ExperienceRater/upload/${id}`, this.baseUrl).href, obj, { reportProgress: true, observe: 'events', responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  upload2(obj: any, id: string, callbackUploadStart?: Function, callbackUploadProgress?: Function, callbackUploadComplete?: Function): Promise<unknown> {

    const promise = new Promise((resolve, reject) => {
      try {
        let xhr = new XMLHttpRequest();
        xhr.open('POST', new URL(`/api/ExperienceRater/upload/${id}`, this.baseUrl).href, true);
        xhr.responseType = 'text';

        if (callbackUploadStart) {
          xhr.addEventListener('loadstart', callbackUploadStart.bind(this), false);
        }

        if (callbackUploadProgress) {
          xhr.upload.addEventListener('progress', callbackUploadProgress.bind(this), false);
        }

        if (callbackUploadComplete) {
          xhr.addEventListener('load', callbackUploadComplete.bind(this), false);
        }

        xhr.onreadystatechange = function (response) {
          if (xhr.readyState === 4) {
            if (xhr.status === 200) {
              resolve(xhr.response);
            } else {
              reject(xhr.response);
            }
          }
        };

        let token = null;
        if (this.authService.isLoggedIn()) {
            const auth = this.authService.getAuth();
            token = auth ? auth.token : null;
        }
        if (token) {
          xhr.setRequestHeader('Authorization', `Bearer ${token}`);
        } 


        xhr.send(obj);
      } catch {
        reject(null);
      }
    });
    return promise;
  }

  // download(id: string, inputValues:any): Observable<any> {
  //   return this.http.post(new URL(`/api/ExperienceRater/download/${id}`, this.raterUrl).href, inputValues, { responseType: 'blob' })
  //     .catch(this.commonService.handleObservableHttpError);
  // }

  download(id: string, inputValues: any): Observable<any> {
    return this.http.post(new URL(`/api/ExperienceRater/download/${id}`, this.baseUrl).href, inputValues, { reportProgress: true, observe: 'events', responseType: 'blob' })
      .catch(this.commonService.handleObservableHttpError);
  }

  download2(id: string, raterVersionId: number, inputValues: any, callbackStart?: Function, callbackDownload?: Function): Promise<unknown> {
    // native way of calling api but modified as asyncronous call.
    const promise = new Promise((resolve, reject) => {
      try {
        let xhr = new XMLHttpRequest();
        xhr.open('POST', new URL(`/api/ExperienceRater/download/${id}?raterVersionId=${raterVersionId}`, this.baseUrl).href, true);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.responseType = 'blob';

        let token = null;
        if (this.authService.isLoggedIn()) {
            const auth = this.authService.getAuth();
            token = auth ? auth.token : null;
        }
        if (token) {
          xhr.setRequestHeader('Authorization', `Bearer ${token}`);
        } 

        if (callbackStart) {
          xhr.addEventListener('loadstart', callbackStart.bind(this), false);
        }

        if (callbackDownload) {
          xhr.addEventListener('progress', callbackDownload.bind(this), false);
        }

        xhr.onreadystatechange = function (response) {
          if (xhr.readyState === 4) {
            if (xhr.status === 200) {
              resolve(xhr.response);
            } else {
              reject(xhr.response);
            }
          }
        };
        xhr.send(JSON.stringify(inputValues));
      } catch {
        reject(null);
      }
    });
    return promise;
  }

  downloadPremiumRater(submissionNumber: string, inputValues: any, callbackStart?: Function, callbackDownload?: Function): Promise<unknown> {
    // native way of calling api but modified as asyncronous call.
    const promise = new Promise((resolve, reject) => {
      try {
        let xhr = new XMLHttpRequest();
        xhr.open('POST', new URL(`/api/PremiumRater/download`, this.baseUrl).href, true);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.responseType = 'blob';

        let token = null;
        if (this.authService.isLoggedIn()) {
            const auth = this.authService.getAuth();
            token = auth ? auth.token : null;
        }
        if (token) {
          xhr.setRequestHeader('Authorization', `Bearer ${token}`);
        } 

        if (callbackStart) {
          xhr.addEventListener('loadstart', callbackStart.bind(this), false);
        }

        if (callbackDownload) {
          xhr.addEventListener('progress', callbackDownload.bind(this), false);
        }

        xhr.onreadystatechange = function (response) {
          if (xhr.readyState === 4) {
            if (xhr.status === 200) {
              resolve(xhr.response);
            } else {
              reject(xhr.response);
            }
          }
        };
        xhr.send(JSON.stringify(inputValues));
      } catch {
        reject(null);
      }
    });
    return promise;
  }

  downloadExperienceRater(submissionNumber: string, raterVersionId: number, riskId: string, inputValues: any, callbackStart?: Function, callbackDownload?: Function): Promise<unknown> {
    // native way of calling api but modified as asyncronous call.
    const promise = new Promise((resolve, reject) => {
      try {
        let xhr = new XMLHttpRequest();
        xhr.open('POST', new URL(`/api/ExperienceRater/downloadlatest/${riskId}?raterVersionId=${raterVersionId}`, this.baseUrl).href, true);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.responseType = 'blob';

        let token = null;
        if (this.authService.isLoggedIn()) {
            const auth = this.authService.getAuth();
            token = auth ? auth.token : null;
        }
        if (token) {
          xhr.setRequestHeader('Authorization', `Bearer ${token}`);
        } 

        if (callbackStart) {
          xhr.addEventListener('loadstart', callbackStart.bind(this), false);
        }

        if (callbackDownload) {
          xhr.addEventListener('progress', callbackDownload.bind(this), false);
        }

        xhr.onreadystatechange = function (response) {
          if (xhr.readyState === 4) {
            if (xhr.status === 200) {
              resolve(xhr.response);
            } else {
              reject(xhr.response);
            }
          }
        };
        xhr.send(JSON.stringify(inputValues));
      } catch {
        reject(null);
      }
    });
    return promise;
  }

  testPremium(): Observable<any> {
    return this.http.get(new URL(`/api/PremiumRater/testconnection`, this.baseUrl).href, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  create(id: string, raterVersionId: number): Observable<any> {
    return this.http.post(new URL(`/api/ExperienceRater/create/${id}?raterVersionId=${raterVersionId}`, this.baseUrl).href, true)
      .catch(this.commonService.handleObservableHttpError);
  }

  preLoadPremium(id: string, riskId: string, fromPolicy?: boolean): Observable<any> {
    let params = new HttpParams()
      .set('riskDetailId', riskId);


    if(!!fromPolicy)
      params = params.set('fromPolicy', "true");

    return this.http.post(new URL(`/api/PremiumRater/preload/${id}?${params.toString()}`, this.baseUrl).href, true)
      .catch(this.commonService.handleObservableHttpError);
  }

  unloadPremium(id: string, riskDetailId: string, ispageunload: boolean = false, fromPolicy: boolean): Observable<any> {
    return this.http.post(new URL(`/api/PremiumRater/unload/${id}?riskDetailId=${riskDetailId}&ispageunload=${ispageunload}&fromPolicy=${fromPolicy}`, this.baseUrl).href, true)
      .catch(this.commonService.handleObservableHttpError);
  }

  calculatePremium(params: { inputData: any, fromPolicy?: boolean, effectiveDate?: Date, forIssuance?:boolean}): Observable<any> {
    var url = "/api/PremiumRater/calculatePremium?";
    if(!!params.fromPolicy)
      url += `&fromPolicy=${params.fromPolicy}`;
    if(!!params.effectiveDate)
      url += `&effectiveDate=${formatDate(params.effectiveDate ?? Date.now(), "yyyy-MM-dd", "en-us")}`;
    if(params.forIssuance != null)
      url += `&forIssuance=${params.forIssuance}`;

    console.log(url);
    return this.http.post(new URL(url, this.baseUrl).href, params.inputData, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  loadRatingFactors(riskDetailId: any) {
    let url = new URL(`/api/PremiumRater/GetRatingFactors/${riskDetailId}`, this.baseUrl).href;
    return this.http.get(url)
      .catch(this.commonService.handleObservableHttpError);
  }

  updateRatingFactors(riskDetailId: string, ratingFactors: any) {
    let url = new URL(`/api/PremiumRater/UpdateRatingFactors/${riskDetailId}`, this.baseUrl).href;
    return this.http.put(url, ratingFactors, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  getRaterVersion$(riskDetailId: string): Observable<RaterVersion> {
    let url = new URL(`/api/PremiumRater/GetRaterVersion/${riskDetailId}`, this.baseUrl).href;
    return this.http.get(url).pipe(
      map(data => data as RaterVersion)
    ).catch(this.commonService.handleObservableHttpError);
  }

  getRaterVersionOfEffectiveDate$(effectiveDate: Date) : Observable<RaterVersion> {
    var dateParam = formatDate(effectiveDate, 'yyyy-MM-dd', 'en-US');
    let url = new URL(`/api/PremiumRater/GetRaterVersionOfEffectiveDate/${dateParam}`, this.baseUrl).href;
    return this.http.get(url).pipe(
      map(data => data as RaterVersion)
    ).catch(this.commonService.handleObservableHttpError);
  }

  initializeRaterVersion$(riskId) : Observable<any> {
    let url = new URL(`/api/PremiumRater/InitializeVersion/${riskId}`, this.baseUrl).href;
    return this.http.put(url, null)
      .catch(this.commonService.handleObservableHttpError);
  }

  confirmXpRaterUpdate$(riskId: string) {
    let url = new URL(`/api/ExperienceRater/ConfirmVersionUpdate/${riskId}`, this.baseUrl).href;
    return this.http.put(url, null)
      .catch(this.commonService.handleObservableHttpError);
  }

  getExperienceRaterDto$(riskId: string) : Observable<ExperienceRaterDto> {
    let url = new URL(`/api/ExperienceRater/GetDto/${riskId}`, this.baseUrl).href;
    return this.http.get(url).pipe(
      map(data => data as ExperienceRaterDto)
    ).catch(this.commonService.handleObservableHttpError);

  }

  allowExpRaterDownload(riskId: string) : Observable<boolean> {
    let url = new URL(`/api/ExperienceRater/AllowExpRaterDownload/${riskId}`, this.baseUrl).href;
    return this.http.get(url).pipe(
      map(data => data as boolean)
    ).catch(this.commonService.handleObservableHttpError);

  }
}
