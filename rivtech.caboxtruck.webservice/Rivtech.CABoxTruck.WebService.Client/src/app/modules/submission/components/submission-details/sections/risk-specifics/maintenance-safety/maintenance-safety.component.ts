import { Component, OnInit, ViewChild } from '@angular/core';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { RiskSpecificsData } from '../../../../../../../modules/submission/data/risk-specifics.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { BaseComponent } from '../../../../../../../shared/base-component';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { RiskSpecificMaintenanceSafetyService } from '../../../../../../../modules/submission/services/riskspecific-maintenance-safety.service';
import { MaintenanceSafetyValidationService } from '../../../../../../../core/services/validations/maintenance-safety-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';

@Component({
  selector: 'app-maintenance-safety',
  templateUrl: './maintenance-safety.component.html',
  styleUrls: ['./maintenance-safety.component.scss']
})
export class MaintenanceSafetyComponent extends BaseComponent implements OnInit {
  nextButtonClicked: boolean = false;

  constructor (
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    public riskSpecificsData: RiskSpecificsData,
    public submissionData: SubmissionData,
    public maintenanceSafetyService: RiskSpecificMaintenanceSafetyService,
    public maintenanceSafetyValidation: MaintenanceSafetyValidationService,
    public formValidation: SubmissionNavValidateService,
    public location: Location
  ) {
    super();
  }

  get maintenanceForm() { return this.riskSpecificsData.riskSpecificsForms.maintenanceForm; }
  get safetyDevicesForm() { return this.riskSpecificsData.riskSpecificsForms.safetyDevicesForm; }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;
  ngOnInit() {
  }

  get scrollPage() {
    return false || this.submissionData?.riskDetail?.vehicles?.length > 5 || this.riskSpecificsData.riskSpecificsForms.safetyDevicesForm.controls['safetyDevice'].value?.length > 4;
  }

  onClick(clickType?: any) {
    this.nextBack.location = this.location;
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([this.nextBack.getPrev()]);
        break;
      case ClickTypes.Next:
        this.maintenanceForm.markAllAsTouched();
        this.safetyDevicesForm.markAllAsTouched();
        this.menuOverrideService.overrideSideBarBehavior();
        this.router.navigate([this.nextBack.getNext()]);
        break;
    }
  }
}
