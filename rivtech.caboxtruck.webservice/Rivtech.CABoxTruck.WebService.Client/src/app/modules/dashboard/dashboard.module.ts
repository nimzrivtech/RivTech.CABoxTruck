import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe, PercentPipe } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts';
import { BsDropdownModule, BsModalService, ButtonsModule } from 'ngx-bootstrap';
import { AppBreadcrumbModule } from '@coreui/angular';
import { PolicySectionComponent } from './components/dashboard/policy-section/policy-section.component';
import { QuestionNotifSectionComponent } from './components/dashboard/question-notif-section/question-notif-section.component';
import { NewsInfoSectionComponent } from './components/dashboard/news-info-section/news-info-section.component';
import { DownloadMaterialsSectionComponent } from './components/dashboard/download-materials-section/download-materials-section.component';
import { StatusCounterComponent } from './components/status-counter/status-counter.component';
import { BillingSectionComponent } from './components/dashboard/billing-section/billing-section.component';
import { ReportsSectionComponent } from './components/dashboard/reports-section/reports-section.component';
import { NotesSectionComponent } from './components/dashboard/notes-section/notes-section.component';
import { ManagementSectionComponent } from './components/dashboard/management-section/management-section.component';
import { AgentDashboardComponent } from './pages/agent-dashboard/agent-dashboard.component';
import { UwDashboardComponent } from './pages/uw-dashboard/uw-dashboard.component';
import { QuoteSectionComponent } from './components/dashboard/quote-section/quote-section.component';
import { QuickReferenceSectionComponent } from './components/dashboard/quick-reference-section/quick-reference-section.component';
import { HelpSectionComponent } from './components/dashboard/help-section/help-section.component';
import { ProcessEndorsementModalComponent } from './components/dashboard/process-endorsement-modal/process-endorsement-modal.component';
import {ModalModule} from 'ngx-bootstrap/modal';
import {DashboardData} from './data/dashboard.data';
import {SharedModule} from '../../shared/shared.module';
import {BlacklistedDriverModule} from '../blacklisted-driver/blacklisted-driver.module';
import { AccountsReceivableComponent } from './components/reports/accounts-receivable/accounts-receivable.component';
import { DMVModalComponent } from './components/reports/dmv/dmv-modal.component';
import { AccountReceivableReportData } from './data/reports/accounts-receivable.data';
import { DMVReportData } from './data/reports/dmv.data';
import { PaymentPostingModalComponent } from './components/reports/payment-posting/payment-posting-modal.component';
import { PaymentPostingReportData } from './data/reports/payment-posting.data';
import { BrokerCommissionModalComponent } from './components/reports/broker-commission/broker-commission-modal.component';
import { BrokerCommissionReportData } from './data/reports/broker-commission.data';
import { PreBindPaymentModalComponent } from './components/reports/prebind-payment/prebind-payment-modal.component';
import { PreBindPaymentReportData } from './data/reports/prebind-payment.data';
import { BordereauxModalComponent } from './components/reports/bordereaux/bordereaux-modal.component';
import { BordereauxReportData } from './data/reports/bordereaux.data';
import { PremiumEarnedTaxModalComponent } from './components/reports/premium-earned-tax/premium-earned-tax-modal.component';
import { PremiumEarnedTaxReportData } from './data/reports/premium-earned-tax.data';
import { SubmissionQuoteModalComponent } from './components/reports/submission-quote/submission-quote-modal.component';
import { SubmissionQuoteReportData } from './data/reports/submission-quote.data';
import { InvoiceDetailModalComponent } from './components/reports/invoice-detail/invoice-detail-modal.component';
import { InvoiceDetailReportData } from './data/reports/invoice-detail.data';
import { RenewalSectionComponent } from './components/dashboard/renewal-section/renewal-section.component';
import { AgedReceivablesComponent } from './components/reports/aged-receivables/aged-receivables.component';
import { AgedReceivablesData } from './data/reports/aged-receivables.data';

@NgModule({
  declarations: [DashboardComponent,
    PolicySectionComponent,
    QuestionNotifSectionComponent,
    NewsInfoSectionComponent,
    DownloadMaterialsSectionComponent,
    StatusCounterComponent,
    BillingSectionComponent,
    ReportsSectionComponent,
    NotesSectionComponent,
    ManagementSectionComponent,
    AgentDashboardComponent,
    UwDashboardComponent,
    QuoteSectionComponent,
    QuickReferenceSectionComponent,
    HelpSectionComponent,
    ProcessEndorsementModalComponent,
    AccountsReceivableComponent,
    PaymentPostingModalComponent,
    PreBindPaymentModalComponent,
    BrokerCommissionModalComponent,
    DMVModalComponent,
    BordereauxModalComponent,
    PremiumEarnedTaxModalComponent,
    SubmissionQuoteModalComponent,
    InvoiceDetailModalComponent,
    RenewalSectionComponent,
    AgedReceivablesComponent],
  imports: [
    CommonModule,
    SharedModule,
    DashboardRoutingModule,
    BlacklistedDriverModule,
    FormsModule,
    DashboardRoutingModule,
    ChartsModule,
    BsDropdownModule,
    AppBreadcrumbModule,
    ModalModule.forRoot(),
    ButtonsModule.forRoot()
  ],
  providers: [
    BsModalService,
    DashboardData,
    CurrencyPipe,
    PercentPipe,
    AccountReceivableReportData,
    AgedReceivablesData,
    PaymentPostingReportData,
    PreBindPaymentReportData,
    BrokerCommissionReportData,
    DMVReportData,
    BordereauxReportData,
    PremiumEarnedTaxReportData,
    SubmissionQuoteReportData,
    InvoiceDetailReportData
  ],
  entryComponents: [
    ProcessEndorsementModalComponent,
    AccountsReceivableComponent,
    PaymentPostingModalComponent,
    PreBindPaymentModalComponent,
    BrokerCommissionModalComponent,
    DMVModalComponent,
    BordereauxModalComponent,
    PremiumEarnedTaxModalComponent,
    SubmissionQuoteModalComponent,
    InvoiceDetailModalComponent,
    AgedReceivablesComponent
    // ModalBackdropComponent
  ],
})
export class DashboardModule { }
