import { Injectable } from '@angular/core';
import { SubmissionData } from '../../../modules/submission/data/submission.data';
import { DriverHiringCriteriaData } from '../../../modules/submission/data/riskspecifics/driver-hiring-criteria.data';
import { DriverInfoData } from '../../../modules/submission/data/riskspecifics/driver-info.data';
import FormUtils from '../../../shared/utilities/form.utils';

@Injectable()
export class DriverInfoValidationService {
  driverInfoSection: boolean;
  driverHiringSection: boolean;

  get DriverInfoForm() {
    return this.driverInfoData.driverInfoForm;
  }
  get DriverHiringForm() {
    return this.driverHiringData.driverHiringCriteriaForm;
  }
  get fc() { return this.fg.controls; }
  get fg() { return this.driverInfoData.driverInfoForm; }

  constructor(public driverInfoData: DriverInfoData, public driverHiringData: DriverHiringCriteriaData, private submissionData: SubmissionData) { }

  checkDriverInfoPage() {
    const currentDriversCount = this.submissionData.riskDetail?.drivers?.filter(driver => driver.isValidated).length;
    this.driverInfoSection = this.checkDriverInfoSection();
    this.driverHiringSection = true;
    if (currentDriversCount > 2 && !this.driverHiringData.isInitial) {
      this.driverHiringSection = this.checkDriverHiringSection();
    }
  }

  checkDriverInfoSection() {
    if (this.fc['isHiringFromOthersWithoutDriver'].value) {
      FormUtils.addRequiredValidator(this.fg, 'annualCostOfHireWithoutDriver');
    }
    this.DriverInfoForm.markAllAsTouched();
    return FormUtils.validateForm(this.DriverInfoForm);
  }

  checkDriverHiringSection() {
    this.DriverHiringForm.markAllAsTouched();
    return FormUtils.validateForm(this.DriverHiringForm);
  }
}
