import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from '../../../core/services/common.service';
import { DashboardDTO } from '../../../shared/models/dashboard/dashboard';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { DashboardPolicySearchDTO } from '../../../shared/models/dashboard/dashboard-policy-search';
import { AccountsReceivableDTO, AccountsReceivableInputDTO } from '@app/shared/models/dashboard/arDto';
import { ReportManagementFilterDTO } from 'app/shared/models/reports/report-management-filter.dto';
import { BordereauxReportRequestDTO } from 'app/shared/models/reports/bordereaux-report-request.dto';
import { SubmissionQuoteReportRequestDTO } from 'app/shared/models/reports/submission-quote-report-request.dto';
import { PaymentPostingDTO, PaymentPostingInputDTO } from '@app/shared/models/dashboard/paymentPostingDto';
import { BrokerCommissionDTO, BrokerCommissionInputDTO } from '@app/shared/models/dashboard/brokerCommissionDto';
import { PreBindPaymentDTO, PreBindPaymentInputDTO } from '@app/shared/models/dashboard/prebindPaymentDto';
import { PremiumEarnedTaxReportRequestDTO } from 'app/shared/models/reports/premium-earned-tax-report-request.dto';
import { AgedReceivablesReportRequestDTO } from '@app/shared/models/reports/aged-receivables-report-request.dto';

@Injectable({
  providedIn: 'root'
})

export class ReportsService {
  private readonly baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.http = http;
    this.baseUrl = environment.ApiUrl.replace('/api', '');
  }

  searchAccountsReceivable(data: AccountsReceivableInputDTO): Observable<AccountsReceivableDTO[]> {
    const url_ = this.baseUrl + `/api/Reports/SearchAccountsReceivable`;
    return this.http.post(url_, data)
      .pipe(
        map(data => {
          return data as AccountsReceivableDTO[];
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  generatePaymentPosting(data: PaymentPostingInputDTO): Observable<PaymentPostingDTO[]> {
    const url_ = this.baseUrl + `/api/Reports/GeneratePaymentPosting`;
    return this.http.post(url_, data)
      .pipe(
        map(data => {
          return data as PaymentPostingDTO[];
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  generatePreBindPayments(data: PreBindPaymentInputDTO): Observable<PreBindPaymentDTO[]> {
    const url_ = this.baseUrl + `/api/Reports/GeneratePreBindPayments`;
    return this.http.post(url_, data)
      .pipe(
        map(data => {
          return data as PreBindPaymentDTO[];
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  generateBrokerCommission(data: BrokerCommissionInputDTO): Observable<BrokerCommissionDTO[]> {
    const url_ = this.baseUrl + `/api/Reports/GenerateBrokerCommission`;
    return this.http.post(url_, data)
      .pipe(
        map(data => {
          return data as BrokerCommissionDTO[];
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  getReport(paymentRequest: ReportManagementFilterDTO, isRaterV7?: boolean): Observable<any> {
    const url_ = this.baseUrl + `/api/Reports/GetReportStream`;
    const requestData = {
      paymentRequest: paymentRequest,
      isRaterV7: isRaterV7
    };
    return this.http.post(url_, requestData, {
      observe: 'response',
      responseType: 'blob'
    })
      .catch(this.commonService.handleObservableHttpError);
  }

  generateBordereauxReport(bordereauxReportRequestDTO: BordereauxReportRequestDTO): Observable<any> {
    const url_ = this.baseUrl + `/api/Reports/GenerateBordereauxReport`;
    return this.http.post(url_, bordereauxReportRequestDTO, {
      observe: 'response',
      responseType: 'blob'
    })
      .catch(this.commonService.handleObservableHttpError);
  }

  generateSubmissionQuoteReport(submissionQuoteReportRequestDTO: SubmissionQuoteReportRequestDTO): Observable<any> {
    const url_ = this.baseUrl + `/api/Reports/GenerateSubmissionQuoteReport`;
    return this.http.post(url_, submissionQuoteReportRequestDTO, {
      observe: 'response',
      responseType: 'blob'
    })
      .catch(this.commonService.handleObservableHttpError);
  }

  generatePremiumEarnedTaxReport(premiumEarnedTaxReportRequestDTO: PremiumEarnedTaxReportRequestDTO): Observable<any> {
    const url_ = this.baseUrl + `/api/Reports/GeneratePremiumEarnedTaxReport`;
    return this.http.post(url_, premiumEarnedTaxReportRequestDTO, {
      observe: 'response',
      responseType: 'blob'
    })
      .catch(this.commonService.handleObservableHttpError);
  }

  generatePaymentReceipt(riskId: string, paymentId: string): Observable<any> {
    const url_ = this.baseUrl + `/api/Reports/GeneratePaymentReceipt/${riskId}/${paymentId}`;
    return this.http.post(url_, null, {
      observe: 'response',
      responseType: 'blob'
    }).pipe(
      //map(data => data.body),
    ).catch(this.commonService.handleObservableHttpError);
  }

  generatePolicyStatement(riskId: string): Observable<any> {
    const url_ = this.baseUrl + `/api/Reports/GeneratePolicyStatement/${riskId}`;
    return this.http.post(url_, null, {
      observe: 'response',
      responseType: 'blob'
    }).pipe(
    ).catch(this.commonService.handleObservableHttpError);
  }

  generateAgedReceivablesReport(agedReceivablesReportRequest: AgedReceivablesReportRequestDTO): Observable<any> {
    const url_ = this.baseUrl + `/api/Reports/GenerateAgingReport`;
    return this.http.post(url_, agedReceivablesReportRequest, {
      observe: 'response',
      responseType: 'blob'
    }).catch(this.commonService.handleObservableHttpError);
  }
}

