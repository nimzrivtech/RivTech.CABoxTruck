import { Injectable } from '@angular/core';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { RadiusOfOperationsData } from '../../../modules/submission/data/riskspecifics/radius-of-operations.data';
import FormUtils from '../../../shared/utilities/form.utils';

@Injectable()
export class DestinationInfoValidationService {
  radiusSection: boolean;
  destinationInfoSection: boolean;

  get RadiusOfOperationsForm() {
    return this.radiusOfOperationData.radiusOfOperationsForm;
  }
  constructor(public radiusOfOperationData: RadiusOfOperationsData, public submissionData: SubmissionData) { }

  checkRadiusOfOperationPage() {
    this.radiusSection = this.checkRadiusOfOperationSection() && this.checkDestinationInfoSection();
  }

  checkRadiusOfOperationSection() {
    this.RadiusOfOperationsForm.markAllAsTouched();
    return FormUtils.validateForm(this.RadiusOfOperationsForm);
  }

  checkDestinationInfoSection() {
    let sum = this.submissionData.riskDetail.riskSpecificDestinations?.reduce((acc, cur) => acc + Number(cur.percentageOfTravelActual), 0);
    sum = Math.round(sum * 100) / 100;
    return sum == 100;
  }
}
