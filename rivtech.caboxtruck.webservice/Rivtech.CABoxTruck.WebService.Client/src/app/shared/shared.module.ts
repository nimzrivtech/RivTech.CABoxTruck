import { NgModule } from '@angular/core';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { CheckboxComponent } from './components/dynamic/checkbox/checkbox.component';
import { DateComponent } from './components/dynamic/date/date.component';
import { InputComponent } from './components/dynamic/input/input.component';
import { SelectComponent } from './components/dynamic/select/select.component';
import { DynamicFormComponent } from './components/dynamic/dynamic-form/dynamic-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DynamicFieldDirective } from './components/dynamic/dynamic-field.directive';
import { RadiobuttonComponent } from './components/dynamic/radiobutton/radiobutton.component';
import { TextAreaComponent } from './components/dynamic/text-area/text-area.component';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { NgxCurrencyModule } from 'ngx-currency';
import { MultipleSelectComponent } from './components/dynamic/multiselect/multiselect.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { OrderModule } from 'ngx-order-pipe';
import { ConfirmationModalComponent } from './components/confirmation-modal/confirmation-modal.component';
import { ToggleCollapseComponent } from './components/toggle-collapse/toggle-collapse.component';
import { ToggleSwitchComponent } from './components/toggle-switch/toggle-switch.component';
import { ValidationErrorComponent } from './components/dynamic/validation-error/validation-error.component';
import { NumberOnlyDirective } from './directives/number-only.directive';
import { SortDirective } from './directives/sort.directive';
import { TablePaginationComponent } from './components/table-pagination/table-pagination.component';
import { DatePickerComponent } from './components/dynamic/date-picker/date-picker.component';
import { AngularMyDatePickerModule } from 'angular-mydatepicker';
import { MaskDateDirective } from './directives/mask-date.directive';
import { MaskDateMMYYYYDirective } from './directives/mask-date-mm-yyyy.directive';

import { BsModalRef, TooltipModule } from 'ngx-bootstrap';
import { CollapsibleCardHeaderComponent } from './components/collapsible-card-header/collapsible-card-header.component';
import { NextBackBtnComponent } from './components/next-back-btn/next-back-btn.component';
import { DriverFilterPipe } from './components/customPipes/driver-filter.pipe';
import { StateIdToCodePipe } from './components/customPipes/state-id-to-code.pipe';
import { PolicyEditButtonComponent } from './components/policy-edit-button/policy-edit-button.component';
import { SelectItemIdToLabelPipe } from './components/customPipes/select-item-id-to-label.pipe';

import { CustomTableComponent } from './components/custom-table/custom-table.component';
import { NameCasePipe } from './custom pipes/name-pipe';
import { PaymentAgreementComponent } from '../../app/modules/policy/components/policy-details/sections/policy-billing/payments/post-payments/payment-agreement/payment-agreement.component';
import { PostPaymentModalComponent } from '../../app/modules/policy/components/policy-details/sections/policy-billing/payments/post-payments/post-payment-modal/post-payment-modal.component';
import { PostPaymentCheckComponent } from '../../app/modules/policy/components/policy-details/sections/policy-billing/payments/post-payments/post-payment-check/post-payment-check.component';
import { EntityNameComponent } from '../../app/modules/policy/components/policy-details/sections/policy-billing/payments/post-payments/entity-name/entity-name.component';
import { WriteOffModalComponent } from '../../app/modules/policy/components/policy-details/sections/policy-billing/payments/write-off-modal/write-off-modal.component';
import { RatingModule } from '@app/modules/rating/rating.module';
import { PaymentDetailsModalComponent } from '../../app/modules/policy/components/policy-details/sections/policy-billing/payments/payment-details-modal/payment-details-modal.component';
import { PaymentConfirmationModalComponent } from '../../app/modules/policy/components/policy-details/sections/policy-billing/payments/payment-confirmation-modal/payment-confirmation-modal.component';
import { TransferPaymentModalComponent } from '../../app/modules/policy/components/policy-details/sections/policy-billing/payments/transfer-payment-modal/transfer-payment-modal.component';
import { InstallmentScheduleModalComponent } from '../../app/modules/policy/components/policy-details/sections/policy-billing/installment-schedule/installment-schedule-modal/installment-schedule-modal.component';
import { PolicyCancellationComponent } from '@app/modules/policy/components/policy-details/policy-summary/policy-header-modals/policy-cancellation/policy-cancellation.component';
import { GenericAddressComponent } from './components/generic-address/generic-address.component';
import { SearchFieldComponent } from './components/dynamic/search-field/search-field.component';
import { MgtNextBackButtonComponent } from './components/dynamic/mgt-next-back-button/mgt-next-back-button.component';
import { PreventDoubleClickDirective } from './directives/prevent-double-click.directive';
import { PaymentPortalService } from 'app/core/services/payment-portal/payment-portal.service';
import { PolicyCancellationNoticeComponent } from '@app/modules/policy/components/policy-details/policy-summary/policy-header-modals/policy-cancellation-notice/policy-cancellation-notice.component';
import { TablePaginationModdedComponent } from './components/table-pagination-mod/table-pagination-mod.component';
import { ReadOnlyDirective } from './directives/read-only.directive';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [DateComponent,
    InputComponent,
    SelectComponent,
    DynamicFormComponent,
    CheckboxComponent,
    DynamicFieldDirective,
    RadiobuttonComponent,
    TextAreaComponent,
    MultipleSelectComponent,
    ConfirmationModalComponent,
    NextBackBtnComponent,
    ToggleCollapseComponent,
    ToggleSwitchComponent,
    ValidationErrorComponent,
    NumberOnlyDirective,
    ReadOnlyDirective,
    SortDirective,
    TablePaginationComponent,
    TablePaginationModdedComponent,
    DatePickerComponent,
    MaskDateDirective,
    MaskDateMMYYYYDirective,
    CollapsibleCardHeaderComponent,
    StateIdToCodePipe,
    DriverFilterPipe,
    CustomTableComponent,
    PolicyEditButtonComponent,
    //NameCasePipe,
    SelectItemIdToLabelPipe,
    PaymentAgreementComponent,
    PostPaymentModalComponent,
    PostPaymentCheckComponent,
    EntityNameComponent,
    WriteOffModalComponent,
    PaymentDetailsModalComponent,
    PaymentConfirmationModalComponent,
    TransferPaymentModalComponent,
    InstallmentScheduleModalComponent,
    PolicyCancellationComponent,
    GenericAddressComponent,
    SearchFieldComponent,
    MgtNextBackButtonComponent,
    PreventDoubleClickDirective,
    PolicyCancellationNoticeComponent
  ],
  imports: [
    SweetAlert2Module,
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
    NgxMaskModule.forRoot(maskConfig),
    NgxCurrencyModule,
    NgMultiSelectDropDownModule.forRoot(),
    OrderModule,
    AngularMyDatePickerModule,
    TooltipModule.forRoot(),
    RatingModule
  ],
  entryComponents: [
    DynamicFormComponent,
    CheckboxComponent,
    DateComponent,
    InputComponent,
    SelectComponent,
    RadiobuttonComponent,
    TextAreaComponent,
    MultipleSelectComponent,
    ConfirmationModalComponent,
    PostPaymentModalComponent,
    WriteOffModalComponent,
    PaymentDetailsModalComponent,
    PaymentConfirmationModalComponent,
    PolicyCancellationComponent,
    TransferPaymentModalComponent,
    InstallmentScheduleModalComponent,
    PolicyCancellationNoticeComponent
  ],
  exports: [
    SweetAlert2Module,
    DynamicFieldDirective,
    DynamicFormComponent,
    NgxCurrencyModule,
    ReactiveFormsModule,
    FormsModule,
    OrderModule,
    CommonModule,
    NgxMaskModule,
    NextBackBtnComponent,
    ToggleCollapseComponent,
    ToggleSwitchComponent,
    ValidationErrorComponent,
    NumberOnlyDirective,
    ReadOnlyDirective,
    SortDirective,
    TablePaginationComponent,
    TablePaginationModdedComponent,
    DatePickerComponent,
    MaskDateDirective,
    MaskDateMMYYYYDirective,
    TooltipModule,
    CollapsibleCardHeaderComponent,
    StateIdToCodePipe,
    DriverFilterPipe,
    PolicyEditButtonComponent,
    SelectItemIdToLabelPipe,
    CustomTableComponent,
    PaymentAgreementComponent,
    PostPaymentCheckComponent,
    EntityNameComponent,
    PaymentDetailsModalComponent,
    PaymentConfirmationModalComponent,
    TransferPaymentModalComponent,
    InstallmentScheduleModalComponent,
    GenericAddressComponent,
    SearchFieldComponent,
    MgtNextBackButtonComponent,
    PreventDoubleClickDirective
  ],
  providers: [NameCasePipe, BsModalRef, PaymentPortalService]
})
export class SharedModule { }
