import { Injectable } from '@angular/core';
import FormUtils from '../../../shared/utilities/form.utils';
import { ApplicantData } from '../../../modules/submission/data/applicant/applicant.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';

@Injectable()
export class ApplicantNameaddressValidationService {
  nameAndAddressSection: boolean;
  nameAndAddressSectionBinding: boolean;

  get NameAndAddressForm() {
    return this.applicantData.applicantForms.nameAndAddressForm;
  }
  get NameAndAddressBillingInfoForm() {
    return this.applicantData.applicantForms.billingInfoForm;
  }
  constructor(public applicantData: ApplicantData, public submissionData: SubmissionData) { }

  checkNameAndAddressPage() {
    this.nameAndAddressSection = this.checkNameAndAddressSection();
  }

  checkNameAndAddressPageBinding() {
    this.nameAndAddressSectionBinding = this.checkNameAndAddressSectionBinding() && this.checkNameAndAddressBillingInfoBinding();
  }

  checkNameAndAddressSection() {
    this.NameAndAddressForm.markAllAsTouched();
    // return FormUtils.validateForm(this.NameAndAddressForm);
    if (FormUtils.validateFormControl(this.NameAndAddressForm.controls['businessNameInput']) &&
    FormUtils.validateFormControl(this.NameAndAddressForm.controls['businessAddressInput']) &&
    FormUtils.validateFormControl(this.NameAndAddressForm.controls['previousPolicyNumber']) &&
    FormUtils.validateFormControl(this.NameAndAddressForm.controls['businessCityDropdown']) &&
    FormUtils.validateFormControl(this.NameAndAddressForm.controls['businessStateInput']) &&
    FormUtils.validateFormControl(this.NameAndAddressForm.controls['businessZipCodeInput']) &&
    FormUtils.validateFormControl(this.NameAndAddressForm.controls['cityOperations']) &&
    FormUtils.validateFormControl(this.NameAndAddressForm.controls['stateOperations']) &&
    FormUtils.validateFormControl(this.NameAndAddressForm.controls['zipCodeOperations'])) {
      return true;
    }

    return false;
  }

  checkNameAndAddressSectionBinding() {
    this.NameAndAddressForm.markAllAsTouched();
    if (FormUtils.validateFormControl(this.NameAndAddressForm.controls['businessPrincipalInput']) &&
      FormUtils.validateFormControl(this.NameAndAddressForm.controls['emailInput']) &&
      FormUtils.validateFormControl(this.NameAndAddressForm.controls['phoneExtInput'])) {
      return true;
    }

    return false;
  }

  checkNameAndAddressBillingInfoBinding() {
    if (this.submissionData.isQuoteIssued || this.submissionData.isIssued) {
      this.NameAndAddressBillingInfoForm.markAllAsTouched();
      if (FormUtils.validateFormControl(this.NameAndAddressBillingInfoForm.controls['contactName']) &&
        FormUtils.validateFormControl(this.NameAndAddressBillingInfoForm.controls['phone']) &&
        FormUtils.validateFormControl(this.NameAndAddressBillingInfoForm.controls['email'])) {
        return true;
      }

      return false;
    }
    return true;
  }
}

