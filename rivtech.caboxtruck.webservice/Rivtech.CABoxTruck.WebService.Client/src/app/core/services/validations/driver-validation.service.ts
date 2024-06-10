import { Injectable } from '@angular/core';
import FormUtils from '../../../shared/utilities/form.utils';
import { DriverData } from '../../../modules/submission/data/driver/driver.data';
import { SubmissionData } from '../../../modules/submission/data/submission.data';

@Injectable()
export class DriverValidationService {
  driverSection: boolean;

  constructor(public submissionData: SubmissionData, public driverData: DriverData) { }

  checkDriverPage() { // Checking for submission
    this.driverSection = this.checkDriverSection();
  }

  checkDriverSection() {
    this.driverData.formDriverHeader.markAllAsTouched();
    // put validation  requirements to display badge.
    //if (FormUtils.validateForm(this.driverData.formDriverHeader) && this.submissionData.riskDetail?.drivers?.length > 0) {
    if (this.submissionData.riskDetail?.drivers?.length > 0 && !this.submissionData.riskDetail?.drivers?.find(x => !x.isValidated)) {
      const drivers = this.submissionData.riskDetail?.drivers.filter(x => x.options !== null && x.options !== undefined);
      if (drivers.length === 0) {
        console.log('No selected options for drivers');
        return false;
      }

      const bindOption = this.submissionData.riskDetail?.binding?.bindOptionId ?? "1";
      if (bindOption !== null && bindOption !== undefined) {
        const driversBind = this.submissionData.riskDetail?.drivers.filter(x => x.options !== null && x.options.indexOf(bindOption) !== -1);
        if (driversBind.length === 0) {
          console.log('No drivers selected for bind option', bindOption);
          return false;
        }
      }

      return true;
    }
    return false;
  }

  checkPolicyDriverSection() {
    const drivers = this.submissionData.riskDetail?.drivers ?? [];
    const hasDrivers = drivers?.length > 0;
    const hasNoInvalidDriver = hasDrivers && !drivers?.find(x => !x.isValidated);

    const hasDriverWithPassRating = hasDrivers && drivers.filter(driver =>
              driver.deletedDate === null
              && driver.options?.length > 0 
              && driver.options?.split(",")?.length > 0)?.length > 0;
    console.log(drivers);
    if(!hasDriverWithPassRating)
      console.warn("No driver selected with pass rating.")

    this.driverSection = (hasDrivers && hasNoInvalidDriver && hasDriverWithPassRating);
  }
}
