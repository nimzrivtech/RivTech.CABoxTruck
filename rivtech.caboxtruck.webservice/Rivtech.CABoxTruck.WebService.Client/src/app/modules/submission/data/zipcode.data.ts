import { Injectable } from '@angular/core';
import { LvZipCodeDTO } from 'app/shared/models/policy/billing/payments/lv-zipcode.dto';
import { ZipcodeService } from 'app/core/services/generics/zipcode.service';
import { BaseClass } from '../../../shared/base-class';
import Utils from '../../../shared/utilities/utils';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { FormGroup } from '@angular/forms';
import { PageSections } from '../../../shared/enum/page-sections.enum';
import { take, takeUntil } from 'rxjs/operators';
import FormUtils from '../../../shared/utilities/form.utils';
import { Subject } from 'rxjs';
import { ExcludedZipCodeList } from 'app/shared/constants/excluded.zipcode.list';
import { PolicySummaryData } from 'app/modules/policy/data/policy-summary.data';
// import { FormsData } from 'app/modules/submission/data/forms/forms.data';

@Injectable({
  providedIn: 'root'
})
export class ZipCodeData extends BaseClass {
  constructor(
    private zipCodeService: ZipcodeService,
    private policySummaryData: PolicySummaryData,
    // private formsData: FormsData
  ) {
    super();
  }
  public FormUtils = FormUtils;

  stateCode: string;

  zipCodeData: any;
  zipCodeList: any = [];
  qqCityList: any = [];
  qqCountyList: any = [];
  applicantInsuredCityList: any = [];
  applicantInsuredqqCountyList: any = [];
  coApplicantCityList: any = [];
  coApplicantCountyList: any = [];
  interestCityList: any = [];
  billingCityList: any = [];
  billingCountyList: any = [];
  checkCityList: any = [];
  ccCityList: any = [];
  eftCityList: any = [];
  supendedPaymentCityList: any = [];
  addSupendedPaymentCityList: any = [];
  selectedCity: string = '';
  cityObservable = new Subject<{ value: string; onFetch: boolean }>();
  multipleCities: boolean = false;
  zipCodeOnInitChange: boolean = true;
  excludedZipCodeList = ExcludedZipCodeList;

  getZipCode(zipCode?, formGroup?, formControlNames?, section?, isAdd?) {
    this.zipCodeService.getZipCodes(zipCode).pipe(take(1)).subscribe(result => {
      this.zipCodeData = <LvZipCodeDTO>result;

      if (this.zipCodeData.length > 0) {
        try {
          this.setList(this.zipCodeData, formGroup, formControlNames, section, isAdd);
        } catch (e) { }
        Utils.unblockUI();
      } else {
        this.FormUtils.resetFields(formGroup, formControlNames);
        NotifUtils.showError('Zip code not found, contact Underwriting');
      }
    }, error => {
      console.log(error);
      Utils.unblockUI();
      this.FormUtils.resetFields(formGroup, formControlNames);
      NotifUtils.showError('Zip code not found, contact Underwriting');
    });
  }

  setList(zipCodeData?, formGroup?, formControlNames?, section?, isAdd?) {
    switch (section) {
      case PageSections.QuickQuoteApplicant:
        this.qqCityList = [];
        this.qqCityList = this.getCityList(zipCodeData);
        if (this.qqCityList.length > 0) {
          this.autoPopulateValues(zipCodeData, formGroup, formControlNames, this.qqCityList);
        }
        break;
      case PageSections.ApplicantInformation:
        this.applicantInsuredCityList = [];
        this.applicantInsuredCityList = this.getCityList(zipCodeData);
        if (this.applicantInsuredCityList.length > 0) {
          this.autoPopulateValues(zipCodeData, formGroup, formControlNames, this.applicantInsuredCityList);
        }
        break;
      case PageSections.CoApplicant:
        this.coApplicantCityList = [];
        this.coApplicantCityList = this.getCityList(zipCodeData);
        if (this.coApplicantCityList.length > 0) {
          this.autoPopulateValues(zipCodeData, formGroup, formControlNames, this.coApplicantCityList, false, true);
        }
        break;
      case PageSections.Interest:
        this.interestCityList = [];
        this.interestCityList = this.getCityList(zipCodeData);
        if (this.interestCityList.length > 0) {
          this.autoPopulateValues(zipCodeData, formGroup, formControlNames, this.interestCityList, isAdd);
        }
        break;
      case PageSections.Billing:
        this.billingCityList = [];
        this.billingCityList = this.getCityList(zipCodeData);
        if (this.billingCityList.length > 0) {
          this.autoPopulateValues(zipCodeData, formGroup, formControlNames, this.billingCityList);
        }
        break;
      case PageSections.CreditCardPayment:
        this.ccCityList = [];
        this.ccCityList = this.getCityList(zipCodeData);
        if (this.ccCityList.length > 0) {
          this.autoPopulateValues(zipCodeData, formGroup, formControlNames, this.ccCityList);
        }
        break;
      case PageSections.EFTPayment:
        this.eftCityList = [];
        this.eftCityList = this.getCityList(zipCodeData);
        if (this.eftCityList.length > 0) {
          this.autoPopulateValues(zipCodeData, formGroup, formControlNames, this.eftCityList);
        }
        break;
      case PageSections.CheckPayment:
        this.checkCityList = [];
        this.checkCityList = this.getCityList(zipCodeData);
        if (this.checkCityList.length > 0) {
          this.autoPopulateValues(zipCodeData, formGroup, formControlNames, this.checkCityList);
        }
        break;
      case PageSections.SuspendedPayment:
        this.supendedPaymentCityList = [];
        this.supendedPaymentCityList = this.getCityList(zipCodeData);
        if (this.supendedPaymentCityList.length > 0) {
          this.autoPopulateValues(zipCodeData, formGroup, formControlNames, this.supendedPaymentCityList);
        }
        break;
      case PageSections.AddSuspendedPayment:
        this.addSupendedPaymentCityList = [];
        this.addSupendedPaymentCityList = this.getCityList(zipCodeData);
        if (this.addSupendedPaymentCityList.length > 0) {
          this.autoPopulateValues(zipCodeData, formGroup, formControlNames, this.addSupendedPaymentCityList, isAdd);
        }
        break;
    }
  }

  getCityList(zipCode) {
    const cityList = [];
    zipCode.map(val => {
      const city = val.city;
      cityList.push(city);
    });

    return cityList;
  }

  autoPopulateValues(zipData?, formGroup?: FormGroup, formControls?, cityList?, isAdd = true, isFromCoApplicant = false) {
    formControls.map(formControl => {
      if (formControl.toLowerCase().includes('city')) {
        if (isFromCoApplicant) {
          this.updateFormControlValidity(formGroup, formControl);
        }
        if (cityList.length === 1) {
          formGroup.get(formControl).setValue(zipData[0].city);
          formGroup.get(formControl).disable();

          if (isFromCoApplicant) {
            this.coApplicantCitySetter(false, zipData[0].city);
          }
        } else {
          if (isAdd) {
            formGroup.get(formControl).setValue('');
          }

          if (isFromCoApplicant) {
            this.coApplicantCitySetter(true, this.selectedCity);
          }
        }
      } else if (formControl.toLowerCase().includes('state')) {
        formGroup.get(formControl).setValue(zipData[0].stateCode);
      } else if (formControl.toLowerCase().includes('county')) {
        formGroup.get(formControl).setValue(zipData[0].county);
      }
    });
  }

  updateFormControlValidity(group: FormGroup, control: string): void {
    /**
     * This method is for formcontrols that
     * do not update its validity in realtime
     */
    // if (!this.formsData.isPolicyUI ||
    //   (this.formsData.isPolicyUI && this.policySummaryData.canEditPolicy)) {
    //   group.get(control).markAsDirty();
    //   group.get(control).enable();
    //   group.get(control).markAsTouched();
    // }
    
    group.get(control).markAsDirty();
    group.get(control).enable();
    group.get(control).markAsTouched();
  }

  coApplicantCitySetter(isMultipleCities: boolean, city: string, onSelect: boolean = false): void {
    this.multipleCities = isMultipleCities;

    if ((!onSelect && ((this.multipleCities && this.zipCodeOnInitChange) || !this.multipleCities)) || onSelect) {
      this.cityObservable.next({ value: city, onFetch: false });
    } else if (!onSelect && this.multipleCities && !this.zipCodeOnInitChange) {
      this.cityObservable.next({ value: '', onFetch: false });
    }
  }

  getZipCodeV2(zipCode?, formGroup?, formControlNames?, section?, isAdd?) {
    this.zipCodeService.getZipCodesV2(zipCode).pipe(take(1)).subscribe(result => {
      this.zipCodeData = <LvZipCodeDTO>result;

      if (this.zipCodeData.length > 0) {
        try {
          this.setList(this.zipCodeData, formGroup, formControlNames, section, isAdd);
        } catch (e) { }
        Utils.unblockUI();
      } else {
        this.FormUtils.resetFields(formGroup, formControlNames);
        NotifUtils.showError('Zip code not found, contact Underwriting');
      }
    }, error => {
      console.log(error);
      Utils.unblockUI();
      this.FormUtils.resetFields(formGroup, formControlNames);
      NotifUtils.showError('Zip code not found, contact Underwriting');
    });
  }
}
