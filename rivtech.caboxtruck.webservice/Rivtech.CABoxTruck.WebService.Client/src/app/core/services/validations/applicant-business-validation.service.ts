import { Injectable } from '@angular/core';
import FormUtils from '../../../shared/utilities/form.utils';
import { ApplicantData } from '../../../modules/submission/data/applicant/applicant.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';

@Injectable()
export class ApplicantBusinessValidationService {
  businessDetailsSection: boolean;
  businessDetailsSectionBinding: boolean;

  get BusinessDetailsForm() {
    return this.applicantData.applicantForms.businessDetails;
  }
  constructor(public applicantData: ApplicantData,
    public submissionData: SubmissionData,) { }

  checkBusinessDetailsPage() {
    this.businessDetailsSection = this.checkBusinessDetailsSection();
  }

  checkBusinessDetailsPageBinding() {
    this.businessDetailsSectionBinding = this.checkBusinessDetailsSectionBinding();
  }

  checkBusinessDetailsSection() {
    this.BusinessDetailsForm.markAllAsTouched();
    // return FormUtils.validateForm(this.BusinessDetailsForm);
    var isValid = FormUtils.validateFormControl(this.BusinessDetailsForm.controls['yearBusinessInput']) 
      && FormUtils.validateFormControl(this.BusinessDetailsForm.controls['newVenturePreviousExperienceDropdown']) 
      && FormUtils.validateFormControl(this.BusinessDetailsForm.controls['yearEstablishedInput']) 
      && FormUtils.validateFormControl(this.BusinessDetailsForm.controls['firstYearUnderCurrentInput']) 
      && FormUtils.validateFormControl(this.BusinessDetailsForm.controls['yearUnderCurrentInput']) 
      && FormUtils.validateFormControl(this.BusinessDetailsForm.controls['businessTypeDropdown']); 
      // && (FormUtils.validateFormControl(this.BusinessDetailsForm.controls['mainUseDropdown']) && this.BusinessDetailsForm.controls['mainUseDropdown'].value != 0)
      // && ((this.submissionData.riskDetail.raterVersionId < 7 || 
      //     (typeof this.submissionData.riskDetail.raterVersionId == 'undefined' || (this.submissionData.riskDetail.raterVersionId > 6 && (FormUtils.validateFormControl(this.BusinessDetailsForm.controls['secondaryBusinessGroupDropdown'])))))
      //     && this.BusinessDetailsForm.controls['secondaryBusinessGroupDropdown'].value != 0)
      // && FormUtils.validateFormControl(this.BusinessDetailsForm.controls['accountCategoryDropdown']) 
      // && (FormUtils.validateFormControl(this.BusinessDetailsForm.controls['accountCategoryUWDropdown']) && this.BusinessDetailsForm.controls['accountCategoryUWDropdown'].value != 0)
      // && (FormUtils.validateFormControl(this.BusinessDetailsForm.controls['mainGVW']) && this.BusinessDetailsForm.controls['mainGVW'].value != 0);

    return isValid;
  }

  checkBusinessDetailsSectionBinding() {
    this.BusinessDetailsForm.markAllAsTouched();
    if (FormUtils.validateFormControl(this.BusinessDetailsForm.controls['federalIdInput'])) {
      return true;
    }

    return false;
  }
}
