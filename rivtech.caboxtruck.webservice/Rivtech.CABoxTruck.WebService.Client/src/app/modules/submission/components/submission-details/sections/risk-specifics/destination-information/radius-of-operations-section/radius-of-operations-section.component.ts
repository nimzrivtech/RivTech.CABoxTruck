import { Component, Input, OnInit } from '@angular/core';
import { RiskSpecificsConstants } from '../../../../../../../../shared/constants/risk-specifics/risk-specifics.constants';
import { BaseComponent } from '../../../../../../../../shared/base-component';
import { RadiusOfOperationsDto } from '../../../../../../../../shared/models/submission/riskspecifics/radius-of-operations.dto';
import { RadiusOfOperationsData } from '../../../../../../data/riskspecifics/radius-of-operations.data';
import { SubmissionData } from '../../../../../../data/submission.data';

@Component({
  selector: 'app-radius-of-operations-section',
  templateUrl: './radius-of-operations-section.component.html',
  styleUrls: ['./radius-of-operations-section.component.scss']
})
export class RadiusOfOperationsSectionComponent extends BaseComponent  implements OnInit {

  @Input() mexicoDestination: boolean = false;
  @Input() nyc5Boroughs: boolean = false;
  maskOptions = RiskSpecificsConstants.maskOptions.percentage;
  constructor(public radiusOfOperationsData: RadiusOfOperationsData, public submissionData: SubmissionData) {
    super();
  }

  ngOnInit() {
    if (this.submissionData?.riskDetail?.radiusOfOperations) {
      this.radiusOfOperationsData.initiateFormGroup(this.submissionData.riskDetail.radiusOfOperations);
      this.fg.markAllAsTouched();
    } else {
      this.radiusOfOperationsData.initiateFormGroup(new RadiusOfOperationsDto());
      // if retrieved from submission list but has no saved values yet, then trigger validations
      if (this.submissionData.validationList.destinationInfo === false) { this.fg.markAllAsTouched(); }
    }
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.radiusOfOperationsData.radiusOfOperationsForm; }

  ngOnChanges() {
    if (this.fg?.controls) {
      if (!this.mexicoDestination){
        this.fc['anyDestinationToMexicoPlanned'].setValue(false);
      }
      if (!this.nyc5Boroughs) {
        this.fc['anyNyc5BoroughsExposure'].setValue(false);
        this.fc['exposureDescription'].setValue('');
      }
    }
  }

  onNyExposureChanged() {
    this.fc['exposureDescription'].setValue(null);
  }

}
