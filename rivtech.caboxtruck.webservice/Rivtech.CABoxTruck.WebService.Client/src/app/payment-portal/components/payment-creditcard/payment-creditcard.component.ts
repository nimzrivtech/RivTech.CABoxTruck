import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {CreditCardConstants} from 'app/shared/constants/payment-portal-specifics/creditcard.constants';
import {MonthConstants} from 'app/shared/constants/payment-portal-specifics/month.constants';
import Utils from '../../../shared/utilities/utils';
import { ZipCodeData } from 'app/modules/submission/data/zipcode.data';
import NotifUtils from 'app/shared/utilities/notif-utils';
import { PageSections } from 'app/shared/enum/page-sections.enum';
import FormUtils from 'app/shared/utilities/form.utils';

@Component({
  selector: 'app-payment-creditcard',
  templateUrl: './payment-creditcard.component.html',
  styleUrls: ['./payment-creditcard.component.css']
})
export class PaymentCreditcardComponent implements OnInit {
  @Output() submitCreditCardForm: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();
  @Output() isValid: EventEmitter<boolean> = new EventEmitter<boolean>();
  creditCardType = CreditCardConstants;
  months = [];
  years = [];
  currentYear = new Date().getFullYear();
  creditCardForm: FormGroup;
  // address: Address[];
  disableCityField: boolean = true;
  public FormUtils = FormUtils;

  constructor(
    private fb: FormBuilder,
    public zipCodeData: ZipCodeData,
    // private addressSvc: AddressService
    ) { }

  ngOnInit() {
    this.creditCardForm = this.fb.group({
      firstName: this.fb.control('', Validators.required),
      lastName: this.fb.control('', Validators.required),
      address: this.fb.control('', Validators.required),
      email: this.fb.control('', Validators.required),
      zip: this.fb.control('', Validators.required),
      city: this.fb.control('', Validators.required),
      state: this.fb.control('', Validators.required),
      cvv: this.fb.control('', Validators.required),
      creditCardTypeId: this.fb.control('', Validators.required),
      creditCardNumber: this.fb.control('', Validators.required),
      expirationMonth: this.fb.control('', Validators.required),
      expirationYear: this.fb.control('', Validators.required),
      singleCity: this.fb.control('')
    });

    this.checkFormChanges();

    for (let i = 1; i < 20; i++) {
      this.years.push(this.currentYear++);
    }

    for (const month in MonthConstants) {
      this.months.push(MonthConstants[month]);
    }

  }

  checkFormChanges() {
    this.submitCreditCardForm.emit(this.creditCardForm.value);
    this.isValid.emit(this.creditCardForm.valid);

    this.creditCardForm.valueChanges.subscribe(form => {
      this.submitCreditCardForm.emit(form);
      this.isValid.emit(this.creditCardForm.valid);
    });
  }

  getAddressByZipCode(zipCode: string) {
    this.disableCityField = false;
    this.creditCardForm.get('city').enable();
    const formControlNames = ['state', 'city', 'zip'];
    this.zipCodeData.ccCityList = [];
    Utils.blockUI();
    if (zipCode !== '') {
      this.zipCodeData.getZipCodeV2(zipCode, this.creditCardForm, formControlNames, PageSections.CreditCardPayment);
    } else {
      this.FormUtils.resetFields(this.creditCardForm as FormGroup, formControlNames);
      this.zipCodeData.ccCityList = [];

      Utils.unblockUI();
      NotifUtils.showError('Zip code not found, contact Underwriting');
    }
  }
}
