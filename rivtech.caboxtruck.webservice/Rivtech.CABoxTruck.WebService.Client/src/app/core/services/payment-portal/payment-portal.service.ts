import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Injectable()
export class PaymentPortalService {
  constructor(private http: HttpClient) { }

  getSomething<T>(serviceName: string) {
    return this.http.get(`${environment.ApiUrl}/${serviceName}`)
      .toPromise()
      .then(data => data as T[])
      .catch(this.handleError);
  }

  addSomething<T>(serviceName: string, objEntity: T) {
    return this.http.post(`${environment.BillingServiceUrl}/${serviceName}`, objEntity)
      .toPromise()
      .then(data => data as T)
      .catch(this.handleError);
  }

  handleError(error: any): Promise<any> {
    if (console && console.error) console.error('An error occurred', error);
    return Promise.reject(error.status || error.message || error);
  }
}
