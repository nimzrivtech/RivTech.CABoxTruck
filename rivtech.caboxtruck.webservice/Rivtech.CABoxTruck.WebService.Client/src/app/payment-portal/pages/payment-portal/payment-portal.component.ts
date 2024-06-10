import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { InstrumentConstants } from 'app/shared/constants/payment-portal-specifics/instrument.constants';
import { PaymentPortalService } from 'app/core/services/payment-portal/payment-portal.service';
import Utils from '../../../shared/utilities/utils';
import { PaymentPortalRequest } from 'app/shared/models/policy/billing/payment-portal-request';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogueComponent } from 'app/payment-portal/components/confirmation-dialogue/confirmation-dialogue.component';
import { take, takeUntil } from 'rxjs/operators';
import { AddressType } from 'app/shared/constants/name-and-address.options.constants';
import { BillingService } from '../../../../app/core/services/billing/billing.service';
import { PaymentRequestDTO } from 'app/shared/models/policy/billing/payment-request.dto';
import { PaymentSummaryDTO } from 'app/shared/models/policy/billing/payment-summary.dto';
import { BillingDetailDTO } from 'app/shared/models/policy/billing/billing-detail.dto';
import { CreditCardDTO } from 'app/shared/models/policy/billing/credit-card.dto';
import { BankAccountDTO } from 'app/shared/models/policy/billing/bank-account.dto';
import { BillingAddressDTO } from 'app/shared/models/policy/billing/billing-address.dto';
import { PaymentMethod } from 'app/shared/enum/payment-method.enum';
import { Observable } from 'rxjs';
import { PaymentDTO } from 'app/shared/models/policy/billing/payment.dto';
import { BaseClass } from 'app/shared/base-class';
import { DatePipe } from '@angular/common';
import {AccountAchTypeConstants} from 'app/shared/constants/payment-portal-specifics/accountAchType.constants';
import { SummaryData } from '@app/modules/submission/data/summary.data';
import { PaymentConfirmationRequestDTO } from 'app/shared/models/payment-portal/payment-confirmation.dto';
import { BoxtruckAzureFunctionsService } from '../../../../app/core/services/boxtruck-azure-functions/boxtruck-azure-functions.service';
import { UserService } from '@app/core/services/management/user.service';
import { PreBindPaymentConfirmationRequestDTO } from '@app/shared/models/payment-portal/prebind-payment-confirmation.dto';

@Component({
  selector: 'app-payment-portal',
  templateUrl: './payment-portal.component.html',
  styleUrls: ['./payment-portal.component.css']
})
export class PaymentPortalComponent extends BaseClass implements OnInit {
  policyNumber: string;
  billingZipCode: string;
  paymentForm: FormGroup;
  insuredName: string;
  currentAmountDue: number;
  payoffAmount: number;
  riskId: string;
  isPolicyFound = false;
  errors = [];
  isPreBind: boolean = false;
  preBindInsuredName: string;
  validAccessCode: string = 'Pay Quote';
  isValidAccessCode: boolean = false;
  underwriter: string;
  isCurrentAmountDueNegative: boolean = false;
  isPaymentAmountNegative: boolean = false;

  PaymentMethod = PaymentMethod;
  instrumentType = InstrumentConstants;
  datePipe: DatePipe;
  accountTypes = AccountAchTypeConstants;

  constructor(
    private fb: FormBuilder,
    private paymentPortalService: PaymentPortalService,
    public billingService: BillingService,
    private dialog: MatDialog,
    public summaryData: SummaryData,
    public boxtruckAzureFunctionsService: BoxtruckAzureFunctionsService,
    private userService: UserService,
    ) { 
      super();
    }
  Object = Object;
  isValid = false;
  isSubFormValid = false;

  ngOnInit() {
    this.datePipe = new DatePipe('en-US');
    
    this.paymentForm = this.fb.group({
      submissionId: this.fb.control(''),
      policyNumber: this.fb.control(''),
      billingZipCode: this.fb.control(''),
      insuredName: this.fb.control(''),
      currentAmountDue: this.fb.control(''),
      payoffAmount: this.fb.control(''),
      amount: this.fb.control('', [Validators.required, Validators.max(999999.99), Validators.min(1)]),
      premium: this.fb.control({ value: '', disabled: true }),
      tax: this.fb.control({ value: '', disabled: true }),
      fee: this.fb.control({ value: '', disabled: true }),
      instrumentId: this.fb.control('', Validators.required),
      isPreBindPayment: this.fb.control(''),
      accessCode: this.fb.control(''),
      preBindInsuredName: this.fb.control(''),
    });

    this.paymentForm.get('isPreBindPayment').valueChanges.subscribe(value => {
      if (value) {
        this.paymentForm.get('preBindInsuredName').setValidators(Validators.required);
        this.paymentForm.get('accessCode').setValidators(Validators.required);
      } else {
        this.paymentForm.get('preBindInsuredName').clearValidators();
        this.paymentForm.get('accessCode').clearValidators();
      }
      // resets form
      this.policyNumber = '';
      this.billingZipCode = '';
      this.preBindInsuredName = '';
      this.isValidAccessCode = false;
      this.paymentForm.controls['insuredName'].setValue('');
      this.paymentForm.controls['accessCode'].setValue('');
      this.paymentForm.controls['accessCode'].setErrors(null);
      this.paymentForm.controls['instrumentId'].setValue('');
      this.paymentForm.controls['amount'].setValue('');
      this.paymentForm.markAsUntouched();
    });

    this.paymentForm.valueChanges.subscribe(() => this.checkFormStatus());
  }

  checkFormStatus() {
    this.isValid = this.paymentForm.valid && this.isSubFormValid;
  }

  onSubmitForm(data) {
    this.getFormBasedOnInstrument(data);
    this.paymentForm.patchValue(data);
  }

  onIsValid(isFormValid: boolean) {
    this.isSubFormValid = isFormValid;
    this.checkFormStatus();
    this.clearErrors();
  }

  getFormBasedOnInstrument(forms) {
    Object.keys(forms).forEach(element => {
      this.addFormControl(element);
    });
  }

  clearErrors(){
    this.errors = [];
  }

  private addFormControl(formControlName: string) {
    this.paymentForm.addControl(formControlName, this.fb.control(''));
  }

  paymentConfirmationDialog() {
    if (this.isPreBind) {
      this.validateAccessCode();
      this.preBindPayment();
      return;
    }

    if(!this.isValid)
    {
      return;
    }

    const amount = this.paymentForm.value.amount;
    const paymentConfirmationDialogRef = this.dialog.open(ConfirmationDialogueComponent, {
      // role: 'alertdialog',
      width: '600px',
      data: {
        title: 'Confirm Payment',
        content: `You are making a payment of <strong>$${(+amount.toFixed(2)).toLocaleString()}</strong> 
                  on Policy <strong>${this.policyNumber}(${this.insuredName})</strong>. Click Confirm to proceed`,
        yesLabel: 'Confirm',
        noLabel: 'Cancel'
      },
    });

    paymentConfirmationDialogRef.afterClosed().subscribe(data => {
      if (data != true) return;
      this.confirmPayment();
    });
  }

  confirmPayment() {
    Utils.blockUI();
    const amount = this.paymentForm.value.amount;
    const paymentRequest: PaymentRequestDTO = this.getPaymentRequest();

    let paymentProcedure$: Observable<PaymentDTO>;

    paymentProcedure$ = this.billingService.postPaymentRequestv2(paymentRequest);

    paymentProcedure$.pipe(takeUntil(this.stop$)).subscribe(_ => {
      const paymentConfirmationRequestDTO: PaymentConfirmationRequestDTO = {
        insuredName: this.insuredName,
        policyNumber: this.policyNumber,
        paymentDate: this.datePipe.transform(new Date(), 'MM/dd/yyyy', 'EST'),
        paymentMethod: paymentRequest.paymentSummary.instrumentId === "EFT" ? "Electronic Fund Transfer" : "Credit Card",
        amount: paymentRequest.paymentSummary.amount,
        underwriter: this.underwriter,
        email: paymentRequest.paymentSummary.email
      };

      let paymentConfirmation$: Observable<boolean>;

      paymentConfirmation$ = this.boxtruckAzureFunctionsService.sendPaymentConfirmationEmail(paymentConfirmationRequestDTO);

      paymentConfirmation$.pipe(takeUntil(this.stop$)).subscribe(_ => {
      },
        err => {
        }
      );
      
      NotifUtils.showSuccess(`A payment of $${(+amount.toFixed(2)).toLocaleString()} was successfully added`, () => {
        this.paymentForm.reset();
        this.clearErrors();
        this.searchDetailByPolicyAndZipCode();
      });
    },
      err => {
        Utils.unblockUI();
        NotifUtils.showError(`Something went wrong, ${err.error?.message}`);
      }
    );
  }

  preBindPayment() {
    if (this.isValidAccessCode) return;
    
    const amount = this.paymentForm.value.amount;
    const preBindPaymentConfirmationDialogRef = this.dialog.open(ConfirmationDialogueComponent, {
      // role: 'alertdialog',
      width: '600px',
      data: {
        title: 'Confirm Payment',
        content: `You are making a payment of <strong>$${(+amount.toFixed(2)).toLocaleString()}</strong> 
                  for <strong>${this.preBindInsuredName}</strong>. Click Confirm to proceed.`,
        yesLabel: 'Confirm',
        noLabel: 'Cancel'
      },
    });

    preBindPaymentConfirmationDialogRef.afterClosed().subscribe(data => {
      if (data != true) return;
      this.confirmPreBindPayment();
    });
  }

  confirmPreBindPayment() {
    Utils.blockUI();
        
    const amount = this.paymentForm.value.amount;
    const paymentRequest: PaymentRequestDTO = this.getPaymentRequest();
    paymentRequest.paymentPortalRequest.insuredName = this.preBindInsuredName;

    this.billingService.prebindPaymentRequest(paymentRequest).pipe(takeUntil(this.stop$)).subscribe(_ => {
      const prebindpaymentConfirmationRequestDTO: PreBindPaymentConfirmationRequestDTO = {
        insuredName: this.preBindInsuredName,
        paymentDate: this.datePipe.transform(new Date(), 'MM/dd/yyyy', 'EST'),
        paymentMethod: paymentRequest.paymentSummary.instrumentId === "EFT" ? "Electronic Fund Transfer" : "Credit Card",
        paymentMethodShort: paymentRequest.paymentSummary.instrumentId,
        amount: paymentRequest.paymentSummary.amount,
        email: paymentRequest.paymentSummary.email
      };

      this.boxtruckAzureFunctionsService.sendPreBindPaymentConfirmationEmail(prebindpaymentConfirmationRequestDTO).pipe(takeUntil(this.stop$)).subscribe(_ => {
      },
        err => {
        }
      );
      
      NotifUtils.showSuccess(`A payment of $${(+amount.toFixed(2)).toLocaleString()} was successfully added`, () => {
        this.paymentForm.reset();
        this.clearErrors();
        this.searchDetailByPolicyAndZipCode();
      });
    },
      err => {
        Utils.unblockUI();
        NotifUtils.showError(`Something went wrong, ${err.error?.message}`);
      }
    );
  }

  savePayment() {
    this.paymentConfirmationDialog();
  }

  searchDetailByPolicyAndZipCode() {
    this.isPolicyFound = false;
    if (typeof this.policyNumber === "undefined" || typeof this.billingZipCode === "undefined" || this.policyNumber?.length === 0 || this.billingZipCode?.length === 0) {
      return;
    }
    Utils.blockUI();
    this.paymentPortalService.getSomething(`risk/SearchRiskByPolicyNumber/${this.policyNumber}/${this.billingZipCode}`).then(
      result => {
        Utils.unblockUI();
        if (result !== null) {
          this.billingService.getSummary(result?.riskId).subscribe(data => {
            if (data.paymentPlan && data.totalPremium) {
      
              this.paymentForm.controls['insuredName'].setValue(result.nameAndAddress.businessName);

              let balance: number = data?.balance ?? 0;

              if(data?.paymentPlan === "Premium Financed"){
                let totalCommission: number = 0;
                totalCommission = +(data?.totalCommissionBilled - data?.totalCommissionPaid).toFixed(2);
                balance = balance + totalCommission;
              }

              if(balance < 0)
              {
                this.paymentForm.controls['currentAmountDue'].setValue(balance * -1);
                this.currentAmountDue = +balance.toFixed(2) * -1;
                this.isCurrentAmountDueNegative = true;
              }
              else
              {
                this.paymentForm.controls['currentAmountDue'].setValue(balance);
                this.currentAmountDue = +balance.toFixed(2);
              }
              
              if(data?.payoffAmount < 0)
              {
                this.paymentForm.controls['payoffAmount'].setValue(data?.payoffAmount * -1);
                this.payoffAmount = +data?.payoffAmount.toFixed(2) * -1;
                this.isPaymentAmountNegative = true;
              }
              else
              {
                this.paymentForm.controls['payoffAmount'].setValue(data?.payoffAmount);
                this.payoffAmount = +data?.payoffAmount.toFixed(2);
              }
              
              this.insuredName = result.nameAndAddress.businessName;
              this.riskId = result.riskId;
              this.isPolicyFound = true;
              
              // this.userService.getUserInfoById(result.createdBy).subscribe((data) => {
              //   this.underwriter = `${data?.firstName} ${data?.lastName}`;
              // });
            }
          }, (error) => {
            Utils.unblockUI();
            if (error.status == 429) {
              NotifUtils.showError(error.error);
            } else {
              NotifUtils.showError('There was an error on retrieving payment details. Please contact administrator');
            }
          });
          return;
        } 
        
        Utils.unblockUI();
        NotifUtils.showError('The policy does not exist or is not yet issued');
      }, (e) => {
        Utils.unblockUI();
        if (e == 429) {
          NotifUtils.showError("The request limit has been reached. Please wait for a minute to proceed.");
        } else {
          NotifUtils.showError('There was an error on retrieving policy details. Please contact administrator');
        }
      });
  }

  validateAmount(event) {
    if (event.which == 69 || event.which == 187 || event.which == 189) {
      event.preventDefault();
    }
  }

  validateAccessCode() {
    const accessCode = this.paymentForm.value.accessCode;
    if (accessCode != this.validAccessCode) { 
      this.isValidAccessCode = true;
      this.paymentForm.controls['accessCode'].setErrors({'invalid': true});
    } else {
      this.isValidAccessCode = false;
      this.paymentForm.controls['accessCode'].setErrors(null);
    }
  }

  private getPaymentRequest(): PaymentRequestDTO {
    const paymentToIssue = this.paymentForm.getRawValue() as PaymentPortalRequest;
    const instrumentId = this.paymentForm.value.instrumentId;
    paymentToIssue.riskId = this.riskId;
    paymentToIssue.submissionId = null;
    paymentToIssue.premium = null;
    paymentToIssue.fee = null;
    paymentToIssue.tax = null;

    paymentToIssue.zipCode = this.paymentForm.value.zip;
    paymentToIssue.creditCardNumber = this.paymentForm.value.creditCardNumber;
    paymentToIssue.expirationMonth = +this.paymentForm.value.expirationMonth;
    paymentToIssue.expirationYear = +this.paymentForm.value.expirationYear;
    paymentToIssue.creditCardFee = +this.paymentForm.value.creditCardFee;
    paymentToIssue.totalChangeAmount = +this.paymentForm.value.totalChangeAmount;
    paymentToIssue.creditCardTypeId = +this.paymentForm.value.creditCardTypeId;
    paymentToIssue.accountType = +this.paymentForm.value.accountType;
    paymentToIssue.accountUsageType = +this.paymentForm.value.accountUsageType;
    paymentToIssue.accessCode = this.paymentForm.value.accessCode;

    const paymentSummary: PaymentSummaryDTO = {
      amount: this.paymentForm.value.amount,
      effectiveDate: this.datePipe.transform(new Date(), 'yyyy-MM-dd', 'EST'),
      instrumentId: instrumentId,
    };

    const billingDetail: BillingDetailDTO = new BillingDetailDTO();

    if (instrumentId === this.instrumentType.CreditCard.id) {
      const creditCardDetail: CreditCardDTO = {
        creditCardTypeId: this.paymentForm.value.creditCardTypeId,
        cardCode: this.paymentForm.value.cvv,
        cardNumber: this.paymentForm.value.creditCardNumber,
        expirationDate: `${this.paymentForm.value.expirationYear}-${('00' + this.paymentForm.value.expirationMonth).slice(-2)}`,
      };

      billingDetail.creditCard = creditCardDetail;
      billingDetail.billingAddress = this.getBillingAddress(this.paymentForm);

    } 
    else if (instrumentId === this.instrumentType.EFT.id) {
      let accountTypeDescription = '';

      switch(this.paymentForm.value.accountType) { 
        case this.accountTypes.Checking.id: { 
          accountTypeDescription = this.accountTypes.Checking.name
          break; 
        } 
        case this.accountTypes.Savings.id: { 
          accountTypeDescription = this.accountTypes.Savings.name
          break; 
        } 
        default: { 
          accountTypeDescription = this.accountTypes.Unknown.name
          break; 
        } 
     }

      const bankAccount: BankAccountDTO = {
        payer: this.paymentForm.value.payer,
        accountNumber: this.paymentForm.value.accountNumber,
        accountType: accountTypeDescription.toLowerCase(),
        nameOnAccount: this.paymentForm.value.payer,
        routingNumber: this.paymentForm.value.routingNumber,
      };

      billingDetail.bankAccount = bankAccount;
      paymentSummary.comment = this.paymentForm.value.comments
    }
    paymentSummary.email = this.paymentForm.value.email;

    const paymentRequest: PaymentRequestDTO = {
      riskId: this.riskId,
      paymentSummary: paymentSummary,
      billingDetail: billingDetail,
      paymentPortalRequest: paymentToIssue,
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
      state: form.get('state').value
    };

    return billingAddress;
  }
}
