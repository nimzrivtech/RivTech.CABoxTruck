import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PolicyBillingComponent } from './policy-billing.component';
import { RouterModule, Routes } from '@angular/router';
import { BillingSummaryComponent } from './billing-summary/billing-summary.component';
import { SharedModule } from '../../../../../../shared/shared.module';
import { InstallmentScheduleComponent } from './installment-schedule/installment-schedule.component';
import { PaymentsComponent } from './payments/payments.component';
import { FeesComponent } from './fees/fees.component';
import { TaxesComponent } from './taxes/taxes.component';
// import { RelatedDocumentsComponent } from './payments/related-documents/related-documents.component';
// import { PaymentDetailsModalComponent } from './payments/payment-details-modal/payment-details-modal.component';
// import { DocumentUploadModalComponent } from './payments/related-documents/document-upload-modal/document-upload-modal.component';
import { FeeModalComponent } from './fees/fee-modal/fee-modal.component';
// import { RecurringPaymentEnrollmentComponent } from './recurring-payment-enrollment/recurring-payment-enrollment.component';
import { NgxMaskModule } from 'ngx-mask';
// import { AdjustmentModalComponent } from './payments/adjustment-modal/adjustment-modal.component';
// import { PaymentConfirmationModalComponent } from './payments/payment-confirmation-modal/payment-confirmation-modal.component';
// import { WriteOffModalComponent } from './payments/write-off-modal/write-off-modal.component';
// import { TransferPaymentModalComponent } from './payments/transfer-payment-modal/transfer-payment-modal.component';
// import { EscheatConfirmationModalComponent } from './payments/escheat-confirmation-modal/escheat-confirmation-modal.component';
// import { ChangePaymentPlanModalComponent } from '../../components/policy-details/policy-billing/billing-summary/change-payment-plan-modal/change-payment-plan-modal.component';
// import { AddPaymentAccountModalComponent } from './recurring-payment-enrollment/add-payment-account-modal/add-payment-account-modal.component';

const routes: Routes = [
  { path: '', component: PolicyBillingComponent }
];

@NgModule({
  declarations: [PolicyBillingComponent,
    BillingSummaryComponent,
    InstallmentScheduleComponent,
    PaymentsComponent,
    FeesComponent,
    TaxesComponent,
    // PaymentDetailsModalComponent,
    // RelatedDocumentsComponent,
    // DocumentUploadModalComponent,
    FeeModalComponent,
    // RecurringPaymentEnrollmentComponent,
    // AdjustmentModalComponent,
    // PaymentConfirmationModalComponent,
    // WriteOffModalComponent,
    // TransferPaymentModalComponent,
    // EscheatConfirmationModalComponent,
    //ChangePaymentPlanModalComponent,
    // AddPaymentAccountModalComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes),
    NgxMaskModule.forRoot(),
  ],
  exports: [PolicyBillingComponent],
  entryComponents: [
    // PaymentDetailsModalComponent,
    // DocumentUploadModalComponent,
    FeeModalComponent,
    // AdjustmentModalComponent,
    // PaymentConfirmationModalComponent,
    // WriteOffModalComponent,
    // TransferPaymentModalComponent,
    // EscheatConfirmationModalComponent,
    //ChangePaymentPlanModalComponent,
    // AddPaymentAccountModalComponent
  ]
})
export class PolicyBillingModule { }
