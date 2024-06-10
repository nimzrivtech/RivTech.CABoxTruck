import { Component, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { RiskSpecificsData } from '../../../../../../../modules/submission/data/risk-specifics.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { BaseComponent } from '../../../../../../../shared/base-component';
import { Location } from '@angular/common';
import { RiskSpecificMaintenanceSafetyService } from '../../../../../../../modules/submission/services/riskspecific-maintenance-safety.service';
import { MaintenanceSafetyValidationService } from '../../../../../../../core/services/validations/maintenance-safety-validation.service';
import { PathConstants } from '@app/shared/constants/path.constants';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { NextBackBtnComponent } from '@app/shared/components/next-back-btn/next-back-btn.component';
import { RiskDTO } from '@app/shared/models/risk/riskDto';

@Component({
  selector: 'app-maintenance-safety',
  templateUrl: './maintenance-safety.component.html',
  styleUrls: ['./maintenance-safety.component.scss']
})
export class MaintenanceSafetyComponent extends BaseComponent implements OnInit {
  nextButtonClicked: boolean = false;

  constructor (
    private navigationService: NavigationService,
    public riskSpecificsData: RiskSpecificsData,
    public submissionData: SubmissionData,
    public maintenanceSafetyService: RiskSpecificMaintenanceSafetyService,
    public location: Location
  ) {
    super();
  }

  get maintenanceForm() { return this.riskSpecificsData.riskSpecificsForms.maintenanceForm; }
  get safetyDevicesForm() { return this.riskSpecificsData.riskSpecificsForms.safetyDevicesForm; }

  ngOnInit() {
  }

  get scrollPage() {
    return false || this.submissionData?.riskDetail?.vehicles?.length > 5 || this.riskSpecificsData.riskSpecificsForms.safetyDevicesForm.controls['safetyDevice'].value?.length > 4;
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.DotInfo}`);
        break;
      case ClickTypes.Next:
        if (this.riskSpecificsData.hiddenNavItems.includes('General Liability & Cargo')) {
          this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.UWQuestions}`);
        } else {
          this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo}`);
        }
        break;
    }
  }
}
