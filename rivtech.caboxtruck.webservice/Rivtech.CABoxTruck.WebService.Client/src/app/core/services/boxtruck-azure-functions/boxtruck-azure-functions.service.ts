import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { CommonService } from '../common.service';
import { PaymentConfirmationRequestDTO } from 'app/shared/models/payment-portal/payment-confirmation.dto';
import { PreBindPaymentConfirmationRequestDTO } from '@app/shared/models/payment-portal/prebind-payment-confirmation.dto';

@Injectable({
  providedIn: 'root'
})
export class BoxtruckAzureFunctionsService {

  constructor(private http: HttpClient, private commonService: CommonService) { }

  sendPaymentConfirmationEmail(paymentConfirmationRequest: PaymentConfirmationRequestDTO): Observable<boolean> {
    return this.http.post<boolean>(`${environment.BoxtruckAzureFunction.BaseUrl}/api/PaymentConfirmationFunction?${environment.BoxtruckAzureFunction.PaymentConfirmationCode}`, paymentConfirmationRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  sendPreBindPaymentConfirmationEmail(paymentConfirmationRequest: PreBindPaymentConfirmationRequestDTO): Observable<boolean> {
    return this.http.post<boolean>(`${environment.BoxtruckAzureFunction.BaseUrl}/api/PreBindPaymentConfirmationFunction?${environment.BoxtruckAzureFunction.PaymentConfirmationCode}`, paymentConfirmationRequest)
      .catch(this.commonService.handleObservableHttpError);
  }
}
