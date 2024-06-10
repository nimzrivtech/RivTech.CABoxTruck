import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IDriverHiringCriteriaDto } from '../../../../shared/models/submission/riskspecifics/driver-hiring-criteria.dto';
import { SubmissionData } from '../submission.data';

@Injectable()
export class DriverHiringCriteriaData {
  driverHiringCriteriaForm: FormGroup;
  isInitial: boolean = true;

  constructor(private formBuilder: FormBuilder, private submissionData: SubmissionData) { }

  initiateFormGroup(obj: IDriverHiringCriteriaDto): FormGroup {
    this.driverHiringCriteriaForm = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      areDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years: [this.setInitialValue(obj.areDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years, true)],
      doDriversHave5YearsDrivingExperience: [this.setInitialValue(obj.doDriversHave5YearsDrivingExperience, true)],
      isAgreedToReportAllDriversToRivington: [this.setInitialValue(obj.isAgreedToReportAllDriversToRivington, true)],
      areFamilyMembersUnder21PrimaryDriversOfCompanyAuto: [this.setInitialValue(obj.areFamilyMembersUnder21PrimaryDriversOfCompanyAuto, true)],
      areDriversProperlyLicensedDotCompliant: [this.setInitialValue(obj.areDriversProperlyLicensedDotCompliant, true)],
      isDisciplinaryPlanDocumented: [this.setInitialValue(obj.isDisciplinaryPlanDocumented, true)],
      isThereDriverIncentiveProgram: [this.setInitialValue(obj.isThereDriverIncentiveProgram, true)],
      backGroundCheckIncludes: [this.setInitialValue(obj.backGroundCheckIncludes, null), Validators.required],
      driverTrainingIncludes: [this.setInitialValue(obj.driverTrainingIncludes, null)],
    }, {
    });

    return this.driverHiringCriteriaForm;
  }

  populateDrivingHiringCriteriaField() {
    if (this.submissionData.riskDetail?.driverHiringCriteria) {
      this.initiateFormGroup(this.submissionData.riskDetail?.driverHiringCriteria);
    }
  }


  public setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length === 0) ? val : obj;
  }

  initiateFormValues(fb: FormGroup) {
  }
}
