import { EventEmitter } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { PolicyBillingLabelsConstants } from '../../../../../../../../../../app/shared/constants/policy-billing.labels.constants';
import { PaymentMethod } from '../../../../../../../../../../app/shared/enum/payment-method.enum';
import { ErrorMessageConstant } from '../../../../../../../../../../app/shared/constants/error-message.constants';
import { BsModalRef } from 'ngx-bootstrap';
import { PolicyBillingData } from '../../../../../../../../../../app/modules/policy/data/policy-billing.data';
import { AuthService } from '../../../../../../../../../../app/core/services/auth.service';
import { switchMap, takeUntil } from 'rxjs/operators';
import { BaseClass } from '../../../../../../../../../../app/shared/base-class';
import { BillingService } from '../../../../../../../../../../app/core/services/billing/billing.service';
import { PaymentRequestDTO } from '../../../../../../../../../../app/shared/models/policy/billing/payment-request.dto';
import { PaymentSummaryDTO } from '../../../../../../../../../../app/shared/models/policy/billing/payment-summary.dto';
import { BillingDetailDTO } from '../../../../../../../../../../app/shared/models/policy/billing/billing-detail.dto';
import { CreditCardDTO } from '../../../../../../../../../../app/shared/models/policy/billing/credit-card.dto';
import { BankAccountDTO } from '../../../../../../../../../../app/shared/models/policy/billing/bank-account.dto';
import { BillingAddressDTO } from '../../../../../../../../../../app/shared/models/policy/billing/billing-address.dto';
import { DatePipe } from '@angular/common';
import Utils from '../../../../../../../../../../app/shared/utilities/utils';
import NotifUtils from '../../../../../../../../../../app/shared/utilities/notif-utils';
import { AbstractControl, Validators } from '@angular/forms';
import { CurrencyMaskInputMode } from 'ngx-currency';
import { RefundRequestDTO } from 'app/shared/models/policy/billing/suspended-payment/refund.dto';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
// import { OfacService } from 'app/core/services/submission/third-party/ofac/ofac.service';
import { ZipCodeData } from 'app/modules/submission/data/zipcode.data';
import { AllowedPaymentRangeDTO } from '../../../../../../../../../shared/models/policy/billing/allowed-payment-range.dto';
import { PaymentProcedure } from '../../../../../../../../../../app/shared/enum/payment-procedure.enum';
import { Observable } from 'rxjs';
import { PaymentDTO } from '../../../../../../../../../../app/shared/models/policy/billing/payment.dto';
import { GenericLabelConstants } from '../../../../../../../../../../app/shared/constants/generic.labels.constants';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
// import { PolicyService } from '../../../../../../../../../../app/core/services/submission/policy.service';
// import { PolicyNotesData } from '../../../../../../../app/modules/policy/data/policy-notes.data';

@Component({
  selector: 'app-post-payment-modal',
  templateUrl: './post-payment-modal.component.html',
  styleUrls: ['./post-payment-modal.component.scss']
})
export class PostPaymentModalComponent extends BaseClass implements OnInit {

  public event: EventEmitter<any> = new EventEmitter<any>();
  public errorMessageConstant = ErrorMessageConstant;
  public PostPaymentLabelConstants = PolicyBillingLabelsConstants.postPayment;
  public GenericLabel = GenericLabelConstants;
  
  currencySettings: any = {
    align: 'left',
    allowNegative: false,
    precision: 2,
    nullable: false
  };

  public minimumPaymentLimitMessage: string;
  public maximumPaymentLimitMessage: string;

  PaymentMethod = PaymentMethod;
  paymentProcedure: PaymentProcedure = PaymentProcedure.PostBasic;

  modalRef: BsModalRef | null;

  isRefund: boolean = false;
  isInternal: boolean;
  paymentMethods: any;
  datePipe: DatePipe;
  currencyInputMode = CurrencyMaskInputMode.NATURAL;
  datePickerDateOption: IAngularMyDpOptions;
  allowedPaymentRange: AllowedPaymentRangeDTO;

  constructor(
    public bsModalRef: BsModalRef,
    public billingData: PolicyBillingData,
    private authService: AuthService,
    private billingService: BillingService,
    // private ofacService: OfacService,
    private zipCodeData: ZipCodeData,
    // private policyService: PolicyService,
    // private policyNotesData: PolicyNotesData
    private submissionData: SubmissionData
  ) {
    super();
  }

  ngOnInit() {
    this.datePipe = new DatePipe('en-US');

    this.authService.userType.pipe(takeUntil(this.stop$)).subscribe(userType => {
      this.isInternal = this.isInternalUser(userType);

      this.getPaymentMethods();
    });

    this.initForm();
    
  
    this.minimumPaymentLimitMessage = this.errorMessageConstant.billingErrorMessage.makePaymentAmountMinErrorMessage.replace('{0}', '$' + this.allowedPaymentRange?.minimumPaymentAmount.toString());
    this.maximumPaymentLimitMessage = this.errorMessageConstant.billingErrorMessage.makePaymentAmountMaxErrorMessage.replace('{0}', '$' + this.allowedPaymentRange?.maximumPaymentAmount.toString());
  }

  initForm(): void {
    this.datePickerDateOption = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };

    this.billingData.enableCityFields();

    this.zipCodeData.checkCityList = [];
    this.zipCodeData.ccCityList = [];
    this.zipCodeData.eftCityList = [];

    this.populateFields();
  }

  populateFields(): void {
    this.billingData.postPaymentForm.reset();

    if (this.isRefund) {
      this.billingData.postPaymentForm.get('paymentMethod').setValue('PM0');
      this.billingData.postPaymentForm.get('paymentMethod').disable();
      // this.billingData.postPaymentForm.get('amount').setValidators([Validators.required, Validators.min(0.01), Validators.max(this.billingData.summary.totalPremium)]);
      this.billingData.postPaymentForm.get('amount').updateValueAndValidity();
    } else {
      this.billingData.postPaymentForm.get('paymentMethod').enable();
      this.billingData.postPaymentForm.get('paymentMethod').setValue('');
      // this.billingData.postPaymentForm.get('amount').setValidators([Validators.required,
      // Validators.min(this.allowedPaymentRange.minimumPaymentAmount),
      // Validators.max(this.allowedPaymentRange.maximumPaymentAmount)]);
      this.billingData.postPaymentForm.get('amount').updateValueAndValidity();
    }

    this.billingData.postPaymentForm.get('postDate').setValue({ isRange: false, singleDate: { jsDate: new Date(this.submissionData.currentServerDateTime) } });

    let balance: number = this.billingData.summary.balance;
    const pastDueAmount: number = this.billingData.summary.pastDueAmount;
    let totalCommission: number = 0;

    if(this.billingData.summary.paymentPlan === "Premium Financed"){
      totalCommission = +(this.billingData.summary.totalCommissionBilled - this.billingData.summary.totalCommissionPaid).toFixed(2);
      balance = balance + totalCommission;
    }

    if (!this.isRefund) {

      let defaultAmountToPay: number;
      switch (this.paymentProcedure) {
        case PaymentProcedure.PostBasic:
          defaultAmountToPay = balance;
          break;
        case PaymentProcedure.PayBalance:
          defaultAmountToPay = balance;
          break;
        case PaymentProcedure.PayToReinstate:
          defaultAmountToPay = pastDueAmount;
          break;
        default:
          defaultAmountToPay = balance;
      }

      this.billingData.postPaymentForm.get('amount').setValue(defaultAmountToPay > 0 ? defaultAmountToPay : 0);

      this.billingData.postPaymentForm.get('check').get('isIndividual').setValue(true);
    }
  }

  hideModal(): void {
    this.bsModalRef.hide();
  }

  onPostPayment(): void {
    if (this.isRefund) {
      //this.processOfacSearch();
      this.processRefund();
    } else {
      this.processPostPayment();
    }
  }

  private processPostPayment(): void {
    const paymentRequest: PaymentRequestDTO = this.getPaymentRequest();

    let paymentProcedure$: Observable<PaymentDTO>;

    switch (this.paymentProcedure) {
      case PaymentProcedure.PostBasic:
        paymentProcedure$ = this.billingService.postPaymentRequest(paymentRequest);
        break;
      case PaymentProcedure.PayBalance:
        paymentProcedure$ = this.billingService.payBalance(paymentRequest);
        break;
      case PaymentProcedure.PayToReinstate:
        paymentProcedure$ = this.billingService.payToReinstate(paymentRequest);
        break;
      default:
        paymentProcedure$ = this.billingService.postPaymentRequest(paymentRequest);
    }

    Utils.blockUI();
    paymentProcedure$.pipe(takeUntil(this.stop$)).subscribe(_ => {
      this.billingData.showSummary(paymentRequest.riskId);
      this.billingData.listInstallmentInvoice(paymentRequest.riskId);
      this.billingData.listPaymentsByRiskId(paymentRequest.riskId);
      this.billingData.getRecurringPaymentDetails(paymentRequest.riskId);

      this.bsModalRef.hide();
      Utils.unblockUI();
      NotifUtils.showSuccess(PolicyBillingLabelsConstants.successPayment);
    },
      err => {
        Utils.unblockUI();
        NotifUtils.showMultiLineError(err.error?.message);
      }
    );
  }

  private processRefund(): void {
    const billingMethod = this.billingData.postPaymentForm.get('paymentMethod').value;
    const instrumentType: string = this.billingData.paymentMethodsPostPayment.filter(x => x.code === billingMethod)[0].billingCode;

    //NOTE: Check method is the only available method for refund as of the moment
    const cashForm = this.billingData.postPaymentForm.get('check');
    const billingAddress = this.getBillingAddress(cashForm);

    const refundRequest: RefundRequestDTO = {
      riskId: this.billingData.data.riskId,
      riskBindId: this.billingData.data.binding.id,
      instrumentId: instrumentType,
      amount: this.billingData.postPaymentForm.get('amount').value,
      comments: this.billingData.postPaymentForm.get('comments').value,
      payeeInfo: {
        name: cashForm.get('isIndividual').value ? `${billingAddress.firstName} ${billingAddress.lastName} ${cashForm.get('suffix').value}` : cashForm.get('name').value,
        address: billingAddress.address,
        city: billingAddress.city,
        state: cashForm.get('state').value,
        zipCode: billingAddress.zip
      }
    };

    Utils.blockUI();
    this.billingService.refundPayment(refundRequest)
      .pipe(
        takeUntil(this.stop$),
        //switchMap(_ => this.policyService.getPolicyNotes(this.billingData.data.riskId))
      ).subscribe(notes => {
        this.billingData.populateBillingSections(refundRequest.riskId);
        //this.policyNotesData.populateNotes(notes);

        this.bsModalRef.hide();
        Utils.unblockUI();
        NotifUtils.showSuccess(PolicyBillingLabelsConstants.successRefund);
      },
        err => {
          Utils.unblockUI();
          NotifUtils.showMultiLineError(err.error?.message);
        }
      );
  }

  getPaymentMethods(): void {
    const paymentMethods = this.billingData.paymentMethodsPostPayment;
    // if (!this.isInternal && !this.isRefund) {
    //   this.paymentMethods = paymentMethods.filter(x => x.code !== PaymentMethod.CashCheck);
    // } else {
    //   this.paymentMethods = paymentMethods;
    // }

    // const recurringPayments: string[] = [PaymentMethod.RecurringCreditCard, PaymentMethod.RecurringECheck];
    // if (this.billingData.isEnrolledToAutopay) {
    //   this.paymentMethods = paymentMethods.filter(x => !recurringPayments.includes(x.code));
    // }
    this.paymentMethods = paymentMethods.filter(x => x.code === PaymentMethod.CashCheck);
  }

  public isInternalUser(userType: string): boolean {
    return userType.toLocaleLowerCase() === 'internal';
  }

  private getPaymentRequest(): PaymentRequestDTO {

    const billingMethod = this.billingData.postPaymentForm.get('paymentMethod').value;
    const instrumentType: string = this.billingData.paymentMethodsPostPayment.filter(x => x.code === billingMethod)[0].billingCode;

    const paymentSummary: PaymentSummaryDTO = {
      amount: +this.billingData.postPaymentForm.get('amount').value.toFixed(2),
      effectiveDate: this.datePipe.transform(this.billingData.postPaymentForm.get('postDate').value.singleDate.jsDate, 'yyyy-MM-dd'),
      instrumentId: instrumentType,
    };

    if (billingMethod === PaymentMethod.RecurringCreditCard || billingMethod === PaymentMethod.RecurringECheck) {
      paymentSummary.isRecurringPayment = this.billingData.postPaymentForm.get('agreeEnrollAutoPay').value;
    }

    const billingDetail: BillingDetailDTO = new BillingDetailDTO();

    if (billingMethod === PaymentMethod.CreditCard || billingMethod === PaymentMethod.RecurringCreditCard) {
      const ccForm = this.billingData.postPaymentForm.get('creditCard');

      const creditCardDetail: CreditCardDTO = {
        cardCode: ccForm.get('cardCode').value,
        cardNumber: ccForm.get('cardNumber').value,
        expirationDate: `${ccForm.get('expirationYear').value}-${('00' + ccForm.get('expirationMonth').value).slice(-2)}`,
      };

      billingDetail.creditCard = creditCardDetail;
      billingDetail.billingAddress = this.getBillingAddress(ccForm);

      paymentSummary.email = ccForm.get('email').value;
    } else if (billingMethod === PaymentMethod.EFT || billingMethod === PaymentMethod.RecurringECheck) {
      const eftForm = this.billingData.postPaymentForm.get('eft');

      const accountType: string = eftForm.get('accountType').value;
      const accountTypeBilling: string = this.billingData.getAccountTypeBillingCode(accountType);

      const bankAccount: BankAccountDTO = {
        accountNumber: eftForm.get('accountNumber').value,
        accountType: accountTypeBilling,
        nameOnAccount: eftForm.get('nameOnAccount').value,
        routingNumber: eftForm.get('routingNumber').value,
      };

      billingDetail.bankAccount = bankAccount;
      billingDetail.billingAddress = this.getBillingAddress(eftForm);

      paymentSummary.email = eftForm.get('email').value;
    } else if (billingMethod === PaymentMethod.CashCheck) {
      const cashForm = this.billingData.postPaymentForm.get('check');
      billingDetail.billingAddress = this.getBillingAddress(cashForm);
    }

    const paymentRequest: PaymentRequestDTO = {
      riskId: this.billingData.data.riskId,
      paymentSummary: paymentSummary,
      billingDetail: billingDetail,
    };

    return paymentRequest;
  }

  private getBillingAddress(form: AbstractControl): BillingAddressDTO {
    const billingAddress: BillingAddressDTO = {
      firstName: form.get('firstName').value,
      lastName: form.get('lastName').value,
      address: form.get('address').value,
      city: form.get('city').value,
      zip: form.get('zip').value,
      state: form.get('state').value,
      checkNumber: form.get('checkNumber').value
    };

    return billingAddress;
  }


  get isAmountZero(): boolean {
    return this.billingData.postPaymentForm.get('amount').value === 0;
  }

  // processOfacSearch(): void {
  //   const checkForm = this.billingData.postPaymentForm.get('check');

  //   this.ofacService.runOfacOnRefund(
  //     checkForm.get('isIndividual').value ? checkForm.get('firstName').value : checkForm.get('name').value,
  //     checkForm.get('isIndividual').value ? checkForm.get('lastName').value : '',
  //     checkForm.get('isIndividual').value ? checkForm.get('suffix').value : ''
  //   );
  // }
}
