import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GeneralLiabilityConstants } from '../../../../../../../../shared/constants/risk-specifics/general-liability-cargo.label.constants';
import { RiskSpecificsData } from '../../../../../../../../modules/submission/data/risk-specifics.data';
import { BaseComponent } from '../../../../../../../../shared/base-component';
import { GenericLabelConstants } from '../../../../../../../../shared/constants/generic.labels.constants';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import FormUtils from '../../../../../../../../shared/utilities/form.utils';

@Component({
  selector: 'app-general-liability-section',
  templateUrl: './general-liability-section.component.html',
  styleUrls: ['./general-liability-section.component.scss']
})

export class GeneralLiabilitySectionComponent extends BaseComponent implements OnInit {
  hideMe: boolean = false;

  constructor(
    public riskSpecificsData: RiskSpecificsData,
    public router: Router,
    public submissionData: SubmissionData
  ) {
    super();
  }
  get fg() { return this.riskSpecificsData.riskSpecificsForms.generalLiabilityForm; }
  get fc() { return this.fg.controls; }
  get fn() { return 'generalLiabilityForm'; }

  public GeneralLiabilityConstants = GeneralLiabilityConstants;
  public GenericLabelConstants = GenericLabelConstants;

  ngOnInit() {
    if (this.submissionData?.riskDetail?.generalLiabilityCargo) {
      this.fg.markAllAsTouched();
    }
    this.switchValidator(this.fc['haveOperationsOtherThanTrucking'].value, 'haveOperationsOtherThanTruckingExplain');
    this.switchValidator(this.fc['hasApplicantHadGeneralLiabilityLoss'].value, 'hasApplicantHadGeneralLiabilityLossExplain');
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  switchValidator(isChecked: boolean, targetControlName: string) {
/*     // Reserved for Quote validation
    if (isChecked) {
      FormUtils.addRequiredValidator(this.fg, targetControlName);
    } else {
      FormUtils.clearValidator(this.fg, targetControlName);
    } */
    if (!isChecked) {
      this.fc[targetControlName].setValue(null);
    }
  }

}
