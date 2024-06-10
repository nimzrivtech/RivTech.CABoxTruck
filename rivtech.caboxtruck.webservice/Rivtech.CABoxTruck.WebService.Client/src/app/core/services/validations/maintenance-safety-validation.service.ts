import { Injectable } from '@angular/core';
import FormUtils from '../../../shared/utilities/form.utils';
import { RiskSpecificsData } from '../../../modules/submission/data/risk-specifics.data';
import { SubmissionData } from '../../../modules/submission/data/submission.data';

@Injectable()
export class MaintenanceSafetyValidationService {
  maintenanceSection: boolean;
  safetySection: boolean;

  get MaintenanceForm() {
    return this.riskSpecificData.riskSpecificsForms.maintenanceForm;
  }
  get SafetyForm() {
    return this.riskSpecificData.riskSpecificsForms.safetyDevicesForm;
  }
  constructor(public riskSpecificData: RiskSpecificsData, public submissionData: SubmissionData) { }

  checkMaintenanceSafetyPage() {
    this.safetySection = this.checkSafetySection();
    this.maintenanceSection = true;
    if (this.submissionData?.riskDetail?.vehicles?.length > 5) {
      this.maintenanceSection = this.checkMaintenanceSection();
    }
  }

  checkMaintenanceSection() {
    if (!this.riskSpecificData.isInitialMaintenanceAndSatefy) {
      this.MaintenanceForm.markAllAsTouched();
      return FormUtils.validateForm(this.MaintenanceForm);
    } else {
      return true;
    }
  }

  checkSafetySection() {
    if (!this.riskSpecificData.isInitialMaintenanceAndSatefy) {
      this.SafetyForm.markAllAsTouched();
      return FormUtils.validateForm(this.SafetyForm);
    } else {
      return true;
    }
  }
}
