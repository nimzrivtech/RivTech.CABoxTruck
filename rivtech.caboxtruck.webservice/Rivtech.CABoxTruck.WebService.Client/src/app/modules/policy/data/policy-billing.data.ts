/* eslint-disable no-restricted-imports */
import { EventEmitter, Injectable } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BaseClass } from '../../../shared/base-class';
import { PolicyBillingLabelsConstants } from '../../../../app/shared/constants/policy-billing.labels.constants';
// import { DocumentUpload } from '../../../../app/shared/models/policy/document-upload.model';
import { PaymentMethod } from '../../../../app/shared/enum/payment-method.enum';
import { BillingService } from '../../../../app/core/services/billing/billing.service';
import { BillingSummaryDTO } from '../../../../app/shared/models/policy/billing/billing-summary.dto';
import { InstallmentInvoiceDTO } from '../../../../app/shared/models/policy/billing/installment-invoice.dto';
import { PaymentViewDTO } from '../../../../app/shared/models/policy/billing/payment-view.dto';
import { PagerService } from '../../../../app/core/services/pager.service';
import { takeUntil } from 'rxjs/operators';
import { LvAccountType, LvCreditCardType, LvPaymentMethod, LvPayPlanOptions } from '../../../../app/shared/constants/billing.options.constants';
import JsUtils from '../../../../app/shared/utilities/js.utils';
import { BillingMapper } from '../../../../app/shared/models/policy/billing-mapper.model';
import FormUtils from '../../../../app/shared/utilities/form.utils';
import { CreditCardType } from '../../../../app/shared/enum/credit-card-type-enum';
import { TransactionFeeDTO } from '../../../../app/shared/models/policy/billing/transaction-fee.dto';
import { TransactionTaxDTO } from '../../../../app/shared/models/policy/billing/transaction-tax.dto';
import { TablePaginationState } from '../../../../app/shared/models/table-pagination.state';
import Utils from '../../../shared/utilities/utils';
import NotifUtils from '../../../shared/utilities/notif-utils';
// import { PaymentProfileRequest } from '../../../shared/models/data/dto/billing/payment-profile-request.dto';
import { PaymentProfileResponse } from '../../../shared/models/policy/billing/payment-profile-response.dto';
// import { PaymentAccountRequest } from '../../../shared/models/data/dto/billing/payment-account-request.dto';
import { PaymentAccountResponse } from '../../../shared/models/policy/billing/payment-account-response.dto';
import { Subject } from 'rxjs/Rx';
import { RegexConstants } from '../../../shared/constants/regex.constants';
import { MakePaymentDTO } from '../../../../app/shared/models/policy/billing/make-payment.dto';
// import { SummaryData } from '../../../../app/modules/submission/data/summary.data';
import { GeneralValidationService } from '../../../../app/core/services/submission/validations/general-validation.service';
import { RiskDTO } from '@app/shared/models/risk/riskDto';
import { PolicySummaryData } from './policy-summary.data';
import { LvRiskStatus } from '@app/shared/constants/risk-status';

@Injectable(
  { providedIn: 'root' }
)
export class PolicyBillingData extends BaseClass {

  public postPaymentForm: FormGroup;
  public postInstallmentScheduleForm: FormGroup;
  public paymentDetailsForm: FormGroup;
  public changePaymentPlanForm: FormGroup;
  public addPaymentAccountForm: FormGroup;
  public accountHolderForm: FormGroup;
  public billingLabelConstants = PolicyBillingLabelsConstants;

  // public paymentDocumentsList: DocumentUpload[] = [];

  public summary: BillingSummaryDTO;
  public installmentSchedules: InstallmentInvoiceDTO[];
  public paymentList: PaymentViewDTO[];
  public transactionFees: TransactionFeeDTO[];
  public transactionTaxes: TransactionTaxDTO[];

  public initialPaymentPlanId: string;
  public selectedPaymentPlanId: string;

  public data: RiskDTO;

  makePaymentData: MakePaymentDTO;

  LvPayPlanOptions = LvPayPlanOptions;
  LvCreditCardType = LvCreditCardType;
  LvAccountType = LvAccountType;
  LvPaymentMethod = LvPaymentMethod;

  paymentListPager: any = {};
  paymentListLoading: boolean;
  paymentListPagedItems: PaymentViewDTO[];

  feeListPager: any = {};
  feeListLoading: boolean;
  feeListPagedItems: TransactionFeeDTO[];

  taxListPager: any = {};
  taxListLoading: boolean;
  taxListPagedItems: TransactionTaxDTO[];

  public paymentPlan: BillingMapper[] = [
    { code: 'PPO0', billingCode: 'Full Pay' },
    { code: 'PPO1', billingCode: 'Two Pay' },
    { code: 'PPO2', billingCode: 'Four Pay' },
    { code: 'PPO3', billingCode: 'Eight Pay' },
    { code: 'PPO4', billingCode: 'Mortgagee' },
  ];

  public paymentPlanChangePlan: BillingMapper[] = [
    { code: 'PPO0', billingCode: 'Full' },
    { code: 'PPO1', billingCode: 'Two' },
    { code: 'PPO2', billingCode: 'Four' },
    { code: 'PPO3', billingCode: 'Eight' },
  ];

  public feeTypes = [
    { code: 'NSFF', description: 'NSF Fee' },
    { code: 'IF', description: 'Installment Fee' },
  ];

  public paymentMethods: BillingMapper[] = [
    { code: 'PM0', billingCode: 'C' },
    { code: 'PM1', billingCode: 'CC' },
    { code: 'PM2', billingCode: 'E' },
    { code: 'PM3', billingCode: 'RCC' },
    { code: 'PM4', billingCode: 'RE' },
  ];

  public paymentMethodsPostPayment = [
    { code: 'PM0', description: 'Check', forPostPayment: true, billingCode: 'CHQ', recurringMethod: false },
    { code: 'PM1', description: 'Credit Card', forPostPayment: true, billingCode: 'CC', recurringMethod: false },
    { code: 'PM5', description: 'EFT', forPostPayment: true, billingCode: 'EFT', recurringMethod: false },
    { code: 'PM4', description: 'Recurring eCheck', forPostPayment: true, billingCode: 'RCHQ', recurringMethod: true },
    { code: 'PM3', description: 'Recurring Credit Card', forPostPayment: true, billingCode: 'RCC', recurringMethod: true },
  ];

  public accountTypes: BillingMapper[] = [
    { code: 'AT0', billingCode: 'checking' },
    { code: 'AT1', billingCode: 'savings' },
  ];

  public recurringPaymentMethods = [
    { display: 'Credit Card', code: 'PM3', isDisabled: false, instrumentTypeId: 'RCC' },
    { display: 'E-Check', code: 'PM4', isDisabled: false, instrumentTypeId: 'RCHQ' }
  ];

  public paymentAccounts: PaymentAccountResponse[] = [];
  recurringSectionDetails: PaymentProfileResponse = null;
  paymentProfile: PaymentProfileResponse = null;
  readonly cardCodeCvvLength: number = 3;
  readonly cardCodeCidLength: number = 4;

  cardCodeLabel: string = this.billingLabelConstants.creditCard.cardCodeCVV;
  cardCodeLength: number = this.cardCodeCvvLength;

  paymentProfileNotifier = new Subject<{ value: any; isCompleted: boolean; isError: boolean }>();
  paymentAccountNotifier = new Subject<{ value: any; isCompleted: boolean; isError: boolean }>();
  paymentAccountListNotifier = new Subject<{ value: any; isCompleted: boolean; isError: boolean }>();
  hasAccountHolder: boolean = false;

  isEnrolledToAutopay: boolean;

  public paymentMethodChangedEvent: EventEmitter<any> = new EventEmitter<any>();

  constructor(private fb: FormBuilder,
    public billingService: BillingService,
    private pagerService: PagerService,
    private summaryData: PolicySummaryData,
    private validationService: GeneralValidationService) {
    super();
  }

  initiateFormFields(): void {
    this.initPostPaymentForm();
    this.initPostInstallmentScheduleForm();
    // this.initChangePaymentPlanForm();
    this.initPaymentAccountForm();
    this.initPaymentDetailsForm();
    // this.initAccountHolderForm();
  }

  initPostPaymentForm(): void {
    this.postPaymentForm = this.fb.group({
      postDate: new FormControl(null, [Validators.required]),
      amount: new FormControl(null, [Validators.required, Validators.min(0.01)]),
      paymentMethod: new FormControl('', [Validators.required]),
      comments: new FormControl(''),
      email: new FormControl(''),

      // agreeEnrollAutoPay: new FormControl(false),
      // agreeOneTimePayment: new FormControl(false),

      check: this.initCheckForm(),
      // creditCard: this.initCreditCardForm(),
      // eft: this.initEftForm(),
    });

    this.postPaymentForm.get('paymentMethod').valueChanges.subscribe(_ => {
      this.paymentMethodChangedEvent.next(this.postPaymentForm.get('paymentMethod').value);
      this.clearAndSetValidators();
    });

    // this.onCardCodeValueChange(this.postPaymentForm);
  }

  initPostInstallmentScheduleForm(): void {
    this.postInstallmentScheduleForm = this.fb.group({
      invoiceDate: new FormControl(null, [Validators.required]),
      dueDate: new FormControl(null, [Validators.required])
    });
  }

  clearAndSetValidators() {
    this.clearPaymentMethodValidators();

    // this.validationService.clearValidatorFormControl(this.postPaymentForm, 'agreeEnrollAutoPay');
    // this.validationService.clearValidatorFormControl(this.postPaymentForm, 'agreeOneTimePayment');

    if (this.postPaymentForm.get('paymentMethod').value === PaymentMethod.CashCheck) {
      this.setValidatorCheckPayment(this.postPaymentForm);
    } else if (this.postPaymentForm.get('paymentMethod').value === PaymentMethod.CreditCard || this.postPaymentForm.get('paymentMethod').value === PaymentMethod.RecurringCreditCard) {
      this.setValidatorCreditCardPayment(this.postPaymentForm);
    } else if (this.postPaymentForm.get('paymentMethod').value === PaymentMethod.EFT || this.postPaymentForm.get('paymentMethod').value === PaymentMethod.RecurringECheck) {
      this.setValidatorEftPayment(this.postPaymentForm);
    }
  }

  initChangePaymentPlanForm(): void {
    this.changePaymentPlanForm = this.fb.group({
      newPlan: new FormControl('', [Validators.required]),
      paymentMethod: new FormControl('', [Validators.required]),
      paymentRequired: new FormControl(null),

      amount: new FormControl(null, [Validators.required]),

      agreeEnrollAutoPay: new FormControl(false),
      agreeOneTimePayment: new FormControl(false),

      creditCard: this.initCreditCardForm(),
      eft: this.initEftForm()
    });

    this.changePaymentPlanForm.get('paymentRequired').valueChanges.subscribe(_ => {
      if (this.changePaymentPlanForm.get('paymentRequired').value === 0) {
        this.changePaymentPlanForm.get('paymentMethod').setValue(null);

        this.validationService.clearValidatorFormControl(this.changePaymentPlanForm, 'amount');
        this.validationService.clearValidatorFormControl(this.changePaymentPlanForm, 'agreeEnrollAutoPay');
        this.validationService.clearValidatorFormControl(this.changePaymentPlanForm, 'agreeOneTimePayment');
      } else {
        this.validationService.resetValidatorFormControl(this.changePaymentPlanForm, 'amount', [Validators.required]);
      }
    });

    this.changePaymentPlanForm.get('paymentMethod').valueChanges.subscribe(_ => {
      this.clearChangePaymentMethodValidators();

      if (this.changePaymentPlanForm.get('paymentMethod').value === PaymentMethod.CreditCard) {
        this.setValidatorCreditCardPayment(this.changePaymentPlanForm);
      } else if (this.changePaymentPlanForm.get('paymentMethod').value === PaymentMethod.EFT) {
        this.setValidatorEftPayment(this.changePaymentPlanForm);
      }
    });

    this.onCardCodeValueChange(this.changePaymentPlanForm);
  }

  initPaymentDetailsForm(): void {
    this.paymentDetailsForm = this.fb.group({
      postDate: new FormControl(null),
      amount: new FormControl(null),
      postedBy: new FormControl(),
      premium: new FormControl(),
      tax: new FormControl(),
      fee: new FormControl(),
      type: new FormControl(),
      paymentMethod: new FormControl(''),
      reference: new FormControl(),
      referenceType: new FormControl(),

      reversalType: new FormControl(),
      reversalDate: new FormControl(),
      reversalProcessedBy: new FormControl(),

      clearDate: new FormControl(),
      escheatDate: new FormControl(),

      comments: new FormControl(''),
      status: new FormControl(),
    });
  }

  initPaymentAccountForm(): void {
    this.addPaymentAccountForm = this.fb.group({
      paymentMethod: new FormControl('', [Validators.required]),
      creditCard: this.initCreditCardForm(),
      eft: this.initEftForm()
    });

    this.addPaymentAccountForm.get('paymentMethod').valueChanges.subscribe(_ => {
      this.clearPaymentMethodValidatorsRecurringEnrollment();

      if (this.addPaymentAccountForm.get('paymentMethod').value === PaymentMethod.RecurringCreditCard) {
        this.setValidatorCreditCardPayment(this.addPaymentAccountForm);
        this.addPaymentAccountForm.get('creditCard').updateValueAndValidity();
      } else if (this.addPaymentAccountForm.get('paymentMethod').value === PaymentMethod.RecurringECheck) {
        this.setValidatorEftPayment(this.addPaymentAccountForm);
        this.addPaymentAccountForm.get('eft').updateValueAndValidity();
      }
    });
    this.onCardCodeValueChange(this.addPaymentAccountForm);
  }

  initAccountHolderForm(): void {
    this.accountHolderForm = this.fb.group({
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      phoneNumber: new FormControl('', [Validators.pattern(RegexConstants.phoneValidation)]),
      email: new FormControl('', [Validators.required, Validators.email]),
      agreeEnrollAutoPay: new FormControl(false, [Validators.requiredTrue])
    });
  }

  private onCardCodeValueChange(currForm: FormGroup): void {
    currForm.get('creditCard').get('creditCardTypeId').valueChanges.subscribe(typeId => {
      if (typeId === CreditCardType.Amex) {
        this.cardCodeLabel = this.billingLabelConstants.creditCard.cardCodeCID;
        this.cardCodeLength = this.cardCodeCidLength;
      } else {
        if (currForm.get('creditCard').get('cardCode').value?.length > this.cardCodeCvvLength) {
          currForm.get('creditCard').get('cardCode').setValue('');
        }

        this.cardCodeLabel = this.billingLabelConstants.creditCard.cardCodeCVV;
        this.cardCodeLength = this.cardCodeCvvLength;
      }
      currForm.updateValueAndValidity();
    });
  }

  initCheckForm(): FormGroup {
    return this.fb.group({
      isIndividual: new FormControl(true),
      name: new FormControl(),
      firstName: new FormControl(),
      lastName: new FormControl(),
      suffix: new FormControl(),
      address: new FormControl(),
      city: new FormControl(),
      state: new FormControl(),
      zip: new FormControl(),
      checkNumber: new FormControl(),
    });
  }

  initCreditCardForm(): FormGroup {
    return this.fb.group({
      firstName: new FormControl(),
      lastName: new FormControl(),
      address: new FormControl(),
      zip: new FormControl(),
      city: new FormControl(),
      state: new FormControl(),
      creditCardTypeId: new FormControl(),
      cardNumber: new FormControl(),
      expirationMonth: new FormControl(),
      expirationYear: new FormControl(),
      cardCode: new FormControl(),
      email: new FormControl()
    });
  }

  initEftForm(): FormGroup {
    return this.fb.group({
      accountType: new FormControl(),
      routingNumber: new FormControl(),
      accountNumber: new FormControl(),
      bankName: new FormControl(),
      nameOnAccount: new FormControl(),

      firstName: new FormControl(),
      lastName: new FormControl(),
      address: new FormControl(),
      zip: new FormControl(),
      city: new FormControl(),
      state: new FormControl(),

      email: new FormControl(),
    });
  }

  setValidatorCheckPayment(form: AbstractControl): void {
    const checkPaymentForm = form.get('check');

    // if (checkPaymentForm.get('isIndividual').value) {
    //   this.resetValidator(checkPaymentForm, 'firstName', [Validators.required]);
    //   this.resetValidator(checkPaymentForm, 'lastName', [Validators.required]);
    // } else {
    //   this.resetValidator(checkPaymentForm, 'name', [Validators.required]);
    // }

    this.resetValidator(checkPaymentForm, 'name', [Validators.required]);
    this.resetValidator(checkPaymentForm, 'address', [Validators.required]);
    this.resetValidator(checkPaymentForm, 'city', [Validators.required]);
    this.resetValidator(checkPaymentForm, 'zip', [Validators.required]);
  }

  setValidatorCreditCardPayment(form: AbstractControl): void {
    const ccPaymentForm = form.get('creditCard');
    this.resetValidator(ccPaymentForm, 'firstName', [Validators.required]);
    this.resetValidator(ccPaymentForm, 'lastName', [Validators.required]);
    this.resetValidator(ccPaymentForm, 'address', [Validators.required]);
    this.resetValidator(ccPaymentForm, 'zip', [Validators.required]);
    this.resetValidator(ccPaymentForm, 'city', [Validators.required]);
    this.resetValidator(ccPaymentForm, 'creditCardTypeId', [Validators.required]);
    this.resetValidator(ccPaymentForm, 'cardNumber', [Validators.required]);
    this.resetValidator(ccPaymentForm, 'expirationMonth', [Validators.required]);
    this.resetValidator(ccPaymentForm, 'expirationYear', [Validators.required]);
    this.resetValidator(ccPaymentForm, 'cardCode', [Validators.required, Validators.minLength(3)]);
    this.resetValidator(ccPaymentForm, 'email', [Validators.email]);

    if (form.get('paymentMethod').value === PaymentMethod.RecurringCreditCard) {
      if (form.get('agreeEnrollAutoPay')) {
        this.resetValidator(form, 'agreeEnrollAutoPay', [Validators.requiredTrue]);
      }
    }

    if (form.get('paymentMethod').value === PaymentMethod.CreditCard) {
      if (form.get('agreeOneTimePayment')) {
        this.resetValidator(form, 'agreeOneTimePayment', [Validators.requiredTrue]);
      }
    }

    if (ccPaymentForm.get('email')) {
      this.resetValidator(ccPaymentForm, 'email', [Validators.email, Validators.required]);
    }
  }

  setValidatorEftPayment(form: AbstractControl): void {
    const eftPaymentForm = form.get('eft');
    this.resetValidator(eftPaymentForm, 'accountType', [Validators.required]);
    this.resetValidator(eftPaymentForm, 'routingNumber', [Validators.required]);
    this.resetValidator(eftPaymentForm, 'accountNumber', [Validators.required]);
    this.resetValidator(eftPaymentForm, 'firstName', [Validators.required]);
    this.resetValidator(eftPaymentForm, 'lastName', [Validators.required]);
    this.resetValidator(eftPaymentForm, 'address', [Validators.required]);
    this.resetValidator(eftPaymentForm, 'zip', [Validators.required]);
    this.resetValidator(eftPaymentForm, 'city', [Validators.required]);
    this.resetValidator(eftPaymentForm, 'email', [Validators.email]);
    this.resetValidator(eftPaymentForm, 'nameOnAccount', [Validators.required]);

    if (form.get('paymentMethod').value === PaymentMethod.RecurringECheck) {
      if (form.get('agreeEnrollAutoPay')) {
        this.resetValidator(form, 'agreeEnrollAutoPay', [Validators.requiredTrue]);
      }
    }

    if (form.get('paymentMethod').value === PaymentMethod.EFT || form.get('paymentMethod').value === PaymentMethod.ECheck) {
      if (form.get('agreeOneTimePayment')) {
        this.resetValidator(form, 'agreeOneTimePayment', [Validators.requiredTrue]);
      }
    }

    if (eftPaymentForm.get('email')) {
      this.resetValidator(eftPaymentForm, 'email', [Validators.email, Validators.required]);
    }
  }

  clearPaymentMethodValidators(): void {
    const checkPaymentForm = this.postPaymentForm.get('check');
    // const ccPaymentForm = this.postPaymentForm.get('creditCard');
    // const eftPaymentForm = this.postPaymentForm.get('eft');

    FormUtils.clearValidatorsByFormGroup(checkPaymentForm);
    // FormUtils.clearValidatorsByFormGroup(ccPaymentForm);
    // FormUtils.clearValidatorsByFormGroup(eftPaymentForm);
  }

  clearChangePaymentMethodValidators(): void {
    const ccPaymentForm = this.changePaymentPlanForm.get('creditCard');
    const eftPaymentForm = this.changePaymentPlanForm.get('eft');

    FormUtils.clearValidatorsByFormGroup(ccPaymentForm);
    FormUtils.clearValidatorsByFormGroup(eftPaymentForm);
  }

  clearPaymentMethodValidatorsRecurringEnrollment(): void {
    const ccPaymentForm = this.addPaymentAccountForm.get('creditCard');
    const eftPaymentForm = this.addPaymentAccountForm.get('eft');

    FormUtils.clearValidatorsByFormGroup(ccPaymentForm);
    FormUtils.clearValidatorsByFormGroup(eftPaymentForm);
  }

  mapDate(date?): string {
    const month = new Date(date).getMonth() + 1 > 9 ? new Date(date).getMonth() + 1 : '0' + (new Date(date).getMonth() + 1);
    const day = new Date(date).getDate() > 9 ? new Date(date).getDate() : '0' + new Date(date).getDate();
    const year = new Date(date).getFullYear();

    return `${month}/${day}/${year}`;
  }

  showSummary(parentRiskId: string): void {
    this.summary = null;
    this.billingService.getSummary(parentRiskId).subscribe(result => {
      if (result.paymentPlan && typeof result.totalPremium !== 'undefined') {

        // const payPlan: string = JsUtils.mergeByKey(this.paymentPlan, LvPayPlanOptions, 'code').filter(item => item.billingCode === result.paymentPlan)[0].code;
        // this.selectedPaymentPlanId = payPlan;
        // this.initialPaymentPlanId = payPlan;

        this.summary = result;
        this.summaryData.fees = result.billingSummaryDetails.find(x => x.description === 'Fee').written;
        this.summaryData.taxes = result.billingSummaryDetails.find(x => x.description === 'Tax').written;
      }
    });
  }

  populateTransactionSummary(parentRiskId: string): void {
    this.billingService.getTransactionSummary(parentRiskId).pipe(takeUntil(this.stop$)).subscribe(result => {
      this.summaryData.billingPremiumRetrieve.next(result);
    });
  }

  listInstallmentInvoice(parentRiskId: string): void {
    this.installmentSchedules = [];
    this.billingService.getInstallmentInvoice(parentRiskId).subscribe(items => {
      this.installmentSchedules = items;
    });
  }

  listPaymentsByRiskId(parentRiskId: string): void {
    this.paymentList = [];
    this.billingService.getPaymentByRiskId(parentRiskId).pipe(takeUntil(this.stop$)).subscribe(items => {
      this.paymentList = items.sort((a, b) => +new Date(b.postDate) - +new Date(a.postDate));
      this.paymentListSetPage(1);
    });
  }

  listTransactionFeesByRiskId(parentRiskId: string): void {
    this.transactionFees = [];
    this.billingService.getTransactionFees(parentRiskId).pipe(takeUntil(this.stop$)).subscribe(items => {
      this.transactionFees = items.sort((a, b) => +new Date(b.addDate) - +new Date(a.addDate));
      this.feeListSetPage(1);
    });
  }

  listTransactionTaxesByRiskId(parentRiskId: string): void {
    this.transactionTaxes = [];
    this.billingService.getTransactionTaxes(parentRiskId).pipe(takeUntil(this.stop$)).subscribe(items => {
      this.transactionTaxes = items.sort((a, b) => +new Date(b.addDate) - +new Date(a.addDate));
      this.taxListSetPage(1);
    });
  }

  public populatePolicyBillingPage(data: RiskDTO): void {
    this.data = data;
    const riskId: string = data.riskId;
    this.populateBillingSections(riskId);
  }

  populateBillingSections(riskId: string): void {
    this.populateTransactionSummary(riskId);
    this.showSummary(riskId);
    this.listInstallmentInvoice(riskId);
    this.listPaymentsByRiskId(riskId);
    this.listTransactionFeesByRiskId(riskId);
    this.listTransactionTaxesByRiskId(riskId);
    this.getRecurringPaymentDetails(riskId);

    // if (!this.recurringSectionDetails) {
    //   this.setDefaultAccountHolderInfo();
    // }
  }

  paymentListSetPage(page: number): void {
    if (page < 1) {
      return;
    }

    this.paymentListPager = this.pagerService.getPager(this.paymentList.length, page);
    this.paymentListPagedItems = this.paymentList.slice(this.paymentListPager.startIndex, this.paymentListPager.endIndex + 1);
  }

  feeListSetPage(page: number): void {
    this.feeListPager = this.pagerService.getPager(this.transactionFees.length, page);
    this.feeListPagedItems = this.transactionFees.slice(this.feeListPager.startIndex, this.feeListPager.endIndex + 1);
  }

  taxListSetPage(page: number): void {
    this.taxListPager = this.pagerService.getPager(this.transactionTaxes.length, page);
    this.taxListPagedItems = this.transactionTaxes.slice(this.taxListPager.startIndex, this.taxListPager.endIndex + 1);
  }

  getAccountTypeBillingCode(code: string): string {
    return JsUtils.mergeByKey(this.accountTypes, LvAccountType, 'code').filter(item => item.code === code)[0].billingCode;
  }

  getCurrentPayPlan(): string {
    return LvPayPlanOptions.filter(item => item.code === this.initialPaymentPlanId)[0].description;
  }

  enableCityFields(): void {
    this.postPaymentForm.get('check').get('city').enable();
    // this.postPaymentForm.get('creditCard').get('city').enable();
    // this.postPaymentForm.get('eft').get('city').enable();
  }

  private resetValidator(formGroup?, controlName?, validator?): void {
    this.validationService.resetValidatorFormControl(formGroup, controlName, validator);
  }

  //#region Recurring Payment Enrollment Section
  // addPaymentProfile(request: PaymentProfileRequest): void {
  //   Utils.blockUI();
  //   this.paymentProfileNotifier.next({ value: null, isCompleted: false, isError: false });
  //   this.billingService.postPaymentProfile(request).subscribe((result: PaymentProfileResponse) => {
  //     this.paymentProfileNotifier.next({ value: result, isCompleted: true, isError: false });
  //     this.populateAccountHolderSection(result);
  //     this.getRecurringPaymentDetails(result.riskId);
  //     Utils.unblockUI();
  //   }, err => {
  //     this.paymentProfileNotifier.next({ value: err.error, isCompleted: true, isError: true });
  //     Utils.unblockUI();
  //     NotifUtils.showError(JSON.stringify(err.error?.message));
  //   });
  // }

  // updatePaymentProfile(request: PaymentProfileRequest): void {
  //   Utils.blockUI();
  //   this.paymentProfileNotifier.next({ value: null, isCompleted: false, isError: false });
  //   this.billingService.putPaymentProfile(request).subscribe((result: PaymentProfileResponse) => {
  //     this.paymentProfileNotifier.next({ value: result, isCompleted: true, isError: false });
  //     this.populateAccountHolderSection(result);
  //     this.getRecurringPaymentDetails(result.riskId);
  //     Utils.unblockUI();
  //   }, err => {
  //     this.paymentProfileNotifier.next({ value: err.error, isCompleted: true, isError: true });
  //     Utils.unblockUI();
  //     NotifUtils.showError(JSON.stringify(err.error?.message));
  //   });
  // }

  populateAccountHolderSection(profile: PaymentProfileResponse): void {
    this.accountHolderForm.get('firstName').setValue(profile.firstName);
    this.accountHolderForm.get('lastName').setValue(profile.lastName);
    this.accountHolderForm.get('phoneNumber').setValue(profile.phoneNumber);
    this.accountHolderForm.get('email').setValue(profile.email);
    this.accountHolderForm.get('agreeEnrollAutoPay').setValue(profile.isRecurringPayment);
    this.paymentProfile = profile;
    this.hasAccountHolder = true;
  }

  // paymentAccountRequestMapper(isAdd: boolean, isEdit: boolean, item: PaymentAccountResponse = null): PaymentAccountRequest {
  //   const methodType = this.addPaymentAccountForm.get('paymentMethod').value;
  //   const creditCard = (this.addPaymentAccountForm.get('creditCard') as FormGroup).controls;
  //   const eftForm = (this.addPaymentAccountForm.get('eft') as FormGroup).controls;
  //   const zeroLeading = '0';
  //   let payload: PaymentAccountRequest = null;

  //   if (isAdd) {
  //     const selectedPaymentMethod = this.paymentMethodsPostPayment.find(method => method.code === methodType).code;

  //     let billingAddressForm: { [key: string]: AbstractControl };
  //     if (selectedPaymentMethod === PaymentMethod.RecurringCreditCard) {
  //       billingAddressForm = creditCard;
  //     } else if (selectedPaymentMethod === PaymentMethod.RecurringECheck) {
  //       billingAddressForm = eftForm;
  //     }

  //     payload = {
  //       riskId: this.data.risks[0].parentRiskId,
  //       instrumentTypeId: this.paymentMethodsPostPayment.find(method => method.code === methodType).billingCode,
  //       isDefault: true,
  //       billingDetail: {
  //         creditCard: (selectedPaymentMethod === PaymentMethod.RecurringCreditCard) ? {
  //           cardCode: creditCard.cardCode.value,
  //           cardNumber: creditCard.cardNumber.value,
  //           expirationDate: `${creditCard.expirationYear.value}-${Number(creditCard.expirationMonth.value) < 10 ? zeroLeading.concat(creditCard.expirationMonth.value) : creditCard.expirationMonth.value}`
  //         } : null,
  //         bankAccount: (selectedPaymentMethod === PaymentMethod.RecurringECheck) ? {
  //           accountNumber: eftForm.accountNumber.value,
  //           accountType: this.getAccountTypeBillingCode(eftForm.accountType.value),
  //           nameOnAccount: eftForm.nameOnAccount.value,
  //           routingNumber: eftForm.routingNumber.value,
  //         } : null,
  //         billingAddress: {
  //           firstName: billingAddressForm.firstName.value,
  //           lastName: billingAddressForm.lastName.value,
  //           address: billingAddressForm.address.value,
  //           city: billingAddressForm.city.value,
  //           zip: billingAddressForm.zip.value
  //         },
  //       }
  //     };
  //   } else {
  //     payload = {
  //       id: item.id,
  //       riskId: this.data.risks[0].parentRiskId
  //     };
  //     if (isEdit) {
  //       payload.isDefault = item.isDefault;
  //     }
  //   }
  //   return payload;
  // }

  // addPaymentAccount(): void {
  //   Utils.blockUI();
  //   this.paymentAccountNotifier.next({ value: null, isCompleted: false, isError: false });
  //   const request = this.paymentAccountRequestMapper(true, false);

  //   this.billingService.postPaymentAccount(request).subscribe((result: PaymentAccountResponse) => {
  //     this.paymentAccountNotifier.next({ value: result, isCompleted: true, isError: false });
  //     this.getRecurringPaymentDetails(request.riskId);
  //     this.paymentAccountListNotifier.subscribe(acctList => {
  //       if (acctList.isCompleted) {
  //         Utils.unblockUI();
  //       }
  //     });
  //   }, err => {
  //     this.paymentAccountNotifier.next({ value: err.error, isCompleted: true, isError: true });
  //     Utils.unblockUI();
  //     NotifUtils.showError(JSON.stringify(err.error?.message));
  //   });
  // }

  // updatePaymentAccount(account): void {
  //   Utils.blockUI();
  //   this.paymentAccountNotifier.next({ value: null, isCompleted: false, isError: false });
  //   const request = this.paymentAccountRequestMapper(false, true, account);

  //   this.billingService.putPaymentAccount(request).subscribe((result: PaymentAccountResponse) => {
  //     this.paymentAccountNotifier.next({ value: result, isCompleted: true, isError: false });
  //     this.getRecurringPaymentDetails(request.riskId);
  //     this.paymentAccountListNotifier.subscribe(acctList => {
  //       if (acctList.isCompleted) {
  //         Utils.unblockUI();
  //       }
  //     });
  //   }, err => {
  //     this.paymentAccountNotifier.next({ value: err.error, isCompleted: true, isError: true });
  //     Utils.unblockUI();
  //     NotifUtils.showError(JSON.stringify(err.error?.message));
  //   });
  // }

  // deletePaymentAccount(account): void {
  //   Utils.blockUI();
  //   this.paymentAccountNotifier.next({ value: null, isCompleted: false, isError: false });
  //   const request = this.paymentAccountRequestMapper(false, false, account);

  //   this.billingService.deletePaymentAccount(request).subscribe((result: PaymentAccountResponse) => {
  //     this.paymentAccountNotifier.next({ value: result, isCompleted: true, isError: false });

  //     this.getRecurringPaymentDetails(request.riskId);
  //     this.paymentAccountListNotifier.subscribe(acctList => {
  //       if (acctList.isCompleted) {
  //         Utils.unblockUI();
  //       }
  //     });
  //   }, err => {
  //     this.paymentAccountNotifier.next({ value: err.error, isCompleted: true, isError: true });
  //     Utils.unblockUI();
  //     NotifUtils.showError(JSON.stringify(err.error?.message));
  //   });
  // }

  getRecurringPaymentDetails(riskId: string): void {
    this.resetRecurringPaymentDetails();

    this.paymentAccountListNotifier.next({ value: null, isCompleted: false, isError: false });
    this.billingService.getRecurringPaymentDetails(riskId).subscribe((result: PaymentProfileResponse) => {
      this.paymentAccountListNotifier.next({ value: result, isCompleted: true, isError: false });
      if (result) {
        this.isEnrolledToAutopay = result.isRecurringPayment;
        this.recurringSectionDetails = result;
        this.populateAccountHolderSection(result);
        this.paymentAccounts = result?.paymentAccounts.length > 0 ? result.paymentAccounts.map(val => {
          val.accountTypeDisplay = this.recurringPaymentMethods.find(method => method.instrumentTypeId === val.instrumentTypeId).display;
          return val;
        }) : [];
      }
    }, err => {
      this.paymentAccountListNotifier.next({ value: err.error, isCompleted: true, isError: true });
      NotifUtils.showError(JSON.stringify(err.error?.message));
    });
  }

  resetRecurringPaymentDetails(): void {
    this.hasAccountHolder = false;
    this.isEnrolledToAutopay = false;
    this.paymentAccounts = [];
    this.recurringSectionDetails = null;
    this.paymentProfile = null;
  }

  get IsPolicyNotActive(): boolean {
    return this.summaryData.policyStatus === 'Canceled' || this.summaryData.policyStatus === 'Rewrite';
  }

  // setDefaultAccountHolderInfo(): void {
  //   this.accountHolderForm.get('firstName').setValue(this.data.firstName);
  //   this.accountHolderForm.get('lastName').setValue(this.data.lastName);
  //   this.accountHolderForm.get('phoneNumber').setValue(this.data.mobilePhone);
  //   this.accountHolderForm.get('email').setValue(this.data.personalEmailAddress);
  // }
  //#endregion
}
