import { HttpBackend, HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { InstallmentInvoiceDTO } from '../../../shared/models/policy/billing/installment-invoice.dto';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { BillingSummaryDTO } from '../../../shared/models/policy/billing/billing-summary.dto';
import { PaymentRequestDTO } from '../../../shared/models/policy/billing/payment-request.dto';
import { PaymentDTO } from '../../../shared/models/policy/billing/payment.dto';
import { CommonService } from '../common.service';
import { GetChangePayPlanResponseDTO } from '../../../shared/models/policy/billing/change-payplan/get-change-payplan-response.dto';
import { ChangePayPlanRequestDTO } from '../../../shared/models/policy/billing/change-payplan/change-payplan-request.dto';
import { PaymentViewDTO } from '../../../shared/models/policy/billing/payment-view.dto';
import { PaymentDetailsViewDTO } from '../../../shared/models/policy/billing/payment-details-view.dto';
import { SuspendedPaymentRequestDTO } from '../../../shared/models/policy/billing/suspended-payment-request.dto';
import { SuspendedPaymentResponseDTO } from '../../../shared/models/policy/billing/suspended-payment-response.dto';
// import { SuspendedPaymentDetailsDTO } from '../../../shared/models/policy/suspended-payment/suspended-payment-detail.dto';
// import { PostSuspendedPaymentDTO } from '../../../shared/models/policy/suspended-payment/post-suspended-payment.dto';
// import { VoidSuspendedPaymentDTO } from '../../../shared/models/policy/suspended-payment/void-suspended-payment.dto';
import { UpdateReturnDetailsDTO } from '../../../shared/models/policy/billing/update-return-suspended-payment.dto';
// import { ReturnSuspendedPaymentDTO } from '../../../shared/models/policy/suspended-payment/return-suspended-payment.dto';
import { ReversePaymentDTO } from '../../../shared/models/policy/billing/reverse-payment.dto';
import { RefundRequestDTO } from 'app/shared/models/policy/billing/suspended-payment/refund.dto';
import { PaymentSuspendedDocDTO } from '../../../shared/models/policy/billing/payment-suspended-doc.dto';
import { PaymentRelatedDocDTO } from '../../../shared/models/policy/billing/payment-related-doc.dto';
import { ReverseSuspendedPaymentDTO } from '../../../shared/models/policy/billing/reverse-suspended-payment.dto';
import { TransactionFeeRequestDTO } from '../../../shared/models/policy/billing/transaction-fee-request.dto';
import { VoidTransactionFeeRequestDTO } from '../../../shared/models/policy/billing/void-transaction-fee-request.dto';
import { TransactionFeeDTO } from '../../../shared/models/policy/billing/transaction-fee.dto';
import { TransactionTaxDTO } from '../../../shared/models/policy/billing/transaction-tax.dto';
import { TransferPaymentDTO } from '../../../shared/models/policy/billing/transfer-payment.dto';
import { PaymentProfileRequest } from '../../../shared/models/policy/billing/payment-profile-request.dto';
import { PaymentProfileResponse } from '../../../shared/models/policy/billing/payment-profile-response.dto';
import { PaymentAccountRequest } from '../../../shared/models/policy/billing/payment-account-request.dto';
import { PaymentAccountResponse } from '../../../shared/models/policy/billing/payment-account-response.dto';
import { CopyPaymentRelatedDocDTO } from '../../../shared/models/policy/billing/copy-payment-related-doc.dto';
// import { RefundRequestListDTO } from '../../../shared/models/policy/refund/refund-request-list.dto';
import { AllowedPaymentRangeDTO } from '../../../shared/models/policy/billing/allowed-payment-range.dto';
// import { UpdateRefundRequestDTO } from '../../../shared/models/policy/refund/save-refund-request.dto';
import { UpdateRiskRequestDTO } from '../../../shared/models/policy/billing/update-risk-request.dto';
import { TransactionSummaryDTO } from '../../../shared/models/policy/billing/transaction-summary.dto';
import { AddEditInstallmentScheduleRequestDTO } from 'app/shared/models/policy/billing/add-edit-installment-schedule-request.dto';

@Injectable({
  providedIn: 'root'
})
export class BillingService {

  constructor(private http: HttpClient, private httpbackend: HttpBackend, private commonService: CommonService) { }

  getSummary(riskId: string): Observable<BillingSummaryDTO> {
    return this.http.get<BillingSummaryDTO>(`${environment.BillingServiceUrl}/api/Billing/summary/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getInstallmentInvoice(riskId: string): Observable<InstallmentInvoiceDTO[]> {
    return this.http.get<InstallmentInvoiceDTO[]>(`${environment.BillingServiceUrl}/api/Billing/installment-and-invoice/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  postPaymentRequest(paymentRequest: PaymentRequestDTO): Observable<PaymentDTO> {
    paymentRequest.appId = environment.BillingAppId;

    return this.http.post<PaymentDTO>(`${environment.BillingServiceUrl}/api/Payment/PostBasic`, paymentRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  postPaymentRequestv2(paymentRequest: PaymentRequestDTO): Observable<PaymentDTO> {
    let httpback = new HttpClient(this.httpbackend);
    paymentRequest.appId = environment.BillingAppId;

    return httpback.post<PaymentDTO>(`${environment.BillingServiceUrl}/api/Payment/PostBasic`, paymentRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  prebindPaymentRequest(paymentRequest: PaymentRequestDTO) {
    let httpback = new HttpClient(this.httpbackend);
    paymentRequest.appId = environment.BillingAppId;

    return httpback.post(`${environment.BillingServiceUrl}/api/Payment/prebindpay`, paymentRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  payToReinstate(paymentRequest: PaymentRequestDTO): Observable<any> {
    paymentRequest.appId = environment.BillingAppId;

    return this.http.post(`${environment.BillingServiceUrl}/api/Payment/PayToReinstate`, paymentRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  payBalance(paymentRequest: PaymentRequestDTO): Observable<PaymentDTO> {
    paymentRequest.appId = environment.BillingAppId;

    return this.http.post(`${environment.BillingServiceUrl}/api/Payment/PayBalance`, paymentRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  getPaymentByRiskId(riskId: string): Observable<PaymentViewDTO[]> {
    return this.http.get<PaymentViewDTO[]>(`${environment.BillingServiceUrl}/api/Payment/GetAllView/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getPaymentDetails(riskId: string, paymentId: string): Observable<PaymentDetailsViewDTO> {
    return this.http.get<PaymentDetailsViewDTO>(`${environment.BillingServiceUrl}/api/Payment/details/${riskId}/${paymentId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  postCreateSuspendedPayment(suspendedPaymentRequest: SuspendedPaymentRequestDTO): Observable<SuspendedPaymentResponseDTO> {
    return this.http.post<PaymentDTO>(`${environment.BillingServiceUrl}/api/SuspendedPayment/CreateSuspendedPayment`, suspendedPaymentRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  getChangePayplan(riskId: string): Observable<GetChangePayPlanResponseDTO> {
    return this.http.get<GetChangePayPlanResponseDTO>(`${environment.BillingServiceUrl}/api/billing/change-payment-plan/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  postChangePayplan(payplanRequest: ChangePayPlanRequestDTO): Observable<any> {
    payplanRequest.appId = environment.BillingAppId;

    return this.http.post<any>(`${environment.ApiUrl}/Billing/change-payment-plan/`, payplanRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  // getSuspendedPaymentDetail(suspendedPaymentId: string): Observable<SuspendedPaymentDetailsDTO> {
  //   return this.http.get<PaymentDetailsViewDTO>(`${environment.BillingServiceUrl}/api/SuspendedPayment/GetDetailView/${suspendedPaymentId}`)
  //     .catch(this.commonService.handleObservableHttpError);
  // }

  // getAllSuspendedPayments(statusId: string): Observable<SuspendedPaymentDetailsDTO> {
  //   return this.http.get<SuspendedPaymentDetailsDTO>(`${environment.BillingServiceUrl}/api/SuspendedPayment/GetAllView/${statusId}`)
  //     .catch(this.commonService.handleObservableHttpError);
  // }

  // postSuspendedPayment(postRequest: PostSuspendedPaymentDTO): Observable<any> {
  //   postRequest.appId = environment.BillingAppId;

  //   return this.http.post<any>(`${environment.BillingServiceUrl}/api/SuspendedPayment/PostSuspendedPayment`, postRequest)
  //     .catch(this.commonService.handleObservableHttpError);
  // }

  // voidSuspendedPayment(postRequest: VoidSuspendedPaymentDTO): Observable<any> {
  //   return this.http.post<any>(`${environment.BillingServiceUrl}/api/SuspendedPayment/VoidSuspendedPayment`, postRequest)
  //     .catch(this.commonService.handleObservableHttpError);
  // }

  // returnSuspendedPayment(req: ReturnSuspendedPaymentDTO): Observable<any> {
  //   return this.http.post<any>(`${environment.BillingServiceUrl}/api/SuspendedPayment/ReturnSuspendedPayment`, req)
  //     .catch(this.commonService.handleObservableHttpError);
  // }

  // getAllReturnedSuspendedPayments(): Observable<SuspendedPaymentDetailsDTO> {
  //   return this.http.get<SuspendedPaymentDetailsDTO>(`${environment.BillingServiceUrl}/api/SuspendedPayment/GetReturnedView`)
  //     .catch(this.commonService.handleObservableHttpError);
  // }

  putReturnDetail(putRequest: UpdateReturnDetailsDTO): Observable<SuspendedPaymentResponseDTO> {
    return this.http.put<any>(`${environment.BillingServiceUrl}/api/SuspendedPayment/UpdateReturnDetail`, putRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  reversePayment(req: ReversePaymentDTO): Observable<any> {
    return this.http.post<any>(`${environment.BillingServiceUrl}/api/Payment/reverse`, req)
      .catch(this.commonService.handleObservableHttpError);
  }

  refundPayment(req: RefundRequestDTO): Observable<any> {
    return this.http.post<any>(`${environment.BillingServiceUrl}/api/refund`, req)
      .catch(this.commonService.handleObservableHttpError);
  }

  addEditInstallmentScheduleFuture(riskId: string, req: AddEditInstallmentScheduleRequestDTO): Observable<any> {
    return this.http.post<any>(`${environment.BillingServiceUrl}/api/installmentschedule/add-edit/${riskId}`, req)
      .catch(this.commonService.handleObservableHttpError);
  }

  listSuspendedPaymentDoc(suspendedPaymentId: string): Observable<PaymentSuspendedDocDTO[]> {
    return this.http.get(`${environment.ApiUrl}/Billing/paymentSuspendedDoc?suspendedPaymentId=${suspendedPaymentId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  addSuspendedPaymentDoc(docuRequests: PaymentSuspendedDocDTO[]): Observable<string[]> {
    const formData = new FormData();
    docuRequests.forEach((model, index) => {
      Object.keys(model).forEach(key => {
        formData.append(`PaymentSuspendedDocs[${index}].` + key, model[key]);
      });
    });

    return this.http.post(`${environment.ApiUrl}/Billing/paymentsuspendedDoc/list`, formData)
      .catch(this.commonService.handleObservableHttpError);
  }

  deleteSuspendedPaymentDoc(documentId: string): Observable<any> {
    return this.http.delete(`${environment.ApiUrl}/Billing/paymentSuspendedDoc/${documentId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  listPaymentRelatedDoc(paymentId: string): Observable<PaymentRelatedDocDTO[]> {
    return this.http.get(`${environment.ApiUrl}/Billing/paymentRelatedDoc?paymentId=${paymentId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  addPaymentRelatedDoc(model: PaymentRelatedDocDTO): Observable<any> {
    const formData = new FormData();
    Object.keys(model).forEach(key => formData.append(key, model[key]));

    return this.http.post(`${environment.ApiUrl}/Billing/paymentRelatedDoc`, formData)
      .catch(this.commonService.handleObservableHttpError);
  }

  deletePaymentRelatedDoc(documentId: string): Observable<any> {
    return this.http.delete(`${environment.ApiUrl}/Billing/paymentRelatedDoc/${documentId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  reverseSuspendedPayment(req: ReverseSuspendedPaymentDTO): Observable<any> {
    return this.http.post(`${environment.BillingServiceUrl}/api/SuspendedPayment/ReverseSuspendedPayment`, req)
      .catch(this.commonService.handleObservableHttpError);
  }

  getTransactionFees(riskId: string): Observable<TransactionFeeDTO[]> {
    return this.http.get<TransactionFeeDTO[]>(`${environment.BillingServiceUrl}/api/Transaction/fee/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getTransactionTaxes(riskId: string): Observable<TransactionTaxDTO[]> {
    return this.http.get<TransactionTaxDTO[]>(`${environment.BillingServiceUrl}/api/Transaction/tax/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  addTransactionFee(transactionFeeRequest: TransactionFeeRequestDTO): Observable<any> {
    return this.http.post(`${environment.BillingServiceUrl}/api/Transaction/fee`, transactionFeeRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  voidTransactionFee(voidFeeRequest: VoidTransactionFeeRequestDTO): Observable<any> {
    return this.http.post(`${environment.BillingServiceUrl}/api/Transaction/fee/void`, voidFeeRequest, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  transferPayment(req: TransferPaymentDTO): Observable<any> {
    return this.http.post(`${environment.BillingServiceUrl}/api/Payment/transfer`, req)
      .catch(this.commonService.handleObservableHttpError);
  }

  postPaymentProfile(paymentProfileRequest: PaymentProfileRequest): Observable<PaymentProfileResponse> {
    paymentProfileRequest.appId = environment.BillingAppId;
    if (paymentProfileRequest.paymentAccount) {
      paymentProfileRequest.paymentAccount.appId = environment.BillingAppId;
    }

    return this.http.post(`${environment.BillingServiceUrl}/api/PaymentProfile/CreateProfile`, paymentProfileRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  putPaymentProfile(paymentProfileRequest: PaymentProfileRequest): Observable<PaymentProfileResponse> {
    paymentProfileRequest.appId = environment.BillingAppId;
    return this.http.put(`${environment.BillingServiceUrl}/api/PaymentProfile/UpdateProfile/`, paymentProfileRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  postPaymentAccount(paymentProfileRequest: PaymentAccountRequest): Observable<PaymentAccountResponse> {
    paymentProfileRequest.appId = environment.BillingAppId;
    return this.http.post(`${environment.BillingServiceUrl}/api/PaymentAccount/CreateAccount`, paymentProfileRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  putPaymentAccount(paymentProfileRequest: PaymentAccountRequest): Observable<PaymentAccountResponse> {
    return this.http.put(`${environment.BillingServiceUrl}/api/PaymentAccount/UpdateAccount`, paymentProfileRequest)
      .catch(this.commonService.handleObservableHttpError);
  }

  deletePaymentAccount(paymentProfileRequest: any): Observable<PaymentAccountResponse> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: paymentProfileRequest
    };
    return this.http.delete(`${environment.BillingServiceUrl}/api/PaymentAccount/DeleteAccount`, options)
      .catch(this.commonService.handleObservableHttpError);
  }

  getPaymentAccounts(riskId: string): Observable<PaymentAccountResponse[]> {
    return this.http.get(`${environment.BillingServiceUrl}/api/PaymentAccount/GetAccount/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getRecurringPaymentDetails(riskId: string): Observable<PaymentProfileResponse> {
    return this.http.get(`${environment.BillingServiceUrl}/api/PaymentProfile/GetProfile/${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  copyPaymentRelatedDoc(req: CopyPaymentRelatedDocDTO): Observable<any> {
    return this.http.post(`${environment.ApiUrl}/Billing/CopyRiskBillingPaymentDocument`, req)
      .catch(this.commonService.handleObservableHttpError);
  }

  getInvoiceDocUrl(invoiceId: string): Observable<string> {
    return this.http.get(`${environment.ApiUrl}/Billing/InvoiceDoc?invoiceId=${invoiceId}`, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }

  getAllowedPaymentRange(riskId: string): Observable<AllowedPaymentRangeDTO> {
    return this.http.get(`${environment.BillingServiceUrl}/api/Payment/allowed-range?riskId=${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  getTransactionSummary(riskId: string): Observable<TransactionSummaryDTO> {
    return this.http.get(`${environment.BillingServiceUrl}/api/Transaction/summary?riskId=${riskId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  // #region ====> REFUND REQUEST
  // getRefundRequestList(): Observable<RefundRequestListDTO> {
  //   return this.http.get(`${environment.ApiUrl}/Billing/RefundRequest`)
  //     .catch(this.commonService.handleObservableHttpError);
  // }

  // postRefundRequest(refundRequestPayload?: UpdateRefundRequestDTO): Observable<RefundRequestListDTO> {
  //   return this.http.post(`${environment.BillingServiceUrl}/api/RefundRequest/refundrequest`, refundRequestPayload)
  //     .catch(this.commonService.handleObservableHttpError);
  // }

  // putRefundRequest(refundRequestPayload?: UpdateRefundRequestDTO): Observable<any> {
  //   return this.http.put(`${environment.BillingServiceUrl}/api/RefundRequest`, refundRequestPayload)
  //     .catch(this.commonService.handleObservableHttpError);
  // }

  deleteRefundRequest(refundRequestId?: string): Observable<any> {
    return this.http.delete(`${environment.BillingServiceUrl}/api/RefundRequest/${refundRequestId}`)
      .catch(this.commonService.handleObservableHttpError);
  }

  putPolicyOnBind(putRequest: UpdateRiskRequestDTO): Observable<any> {
    return this.http.put(`${environment.BillingServiceUrl}/api/Risk`, putRequest)
      .catch(this.commonService.handleObservableHttpError);
  }
  // #endregion
}
