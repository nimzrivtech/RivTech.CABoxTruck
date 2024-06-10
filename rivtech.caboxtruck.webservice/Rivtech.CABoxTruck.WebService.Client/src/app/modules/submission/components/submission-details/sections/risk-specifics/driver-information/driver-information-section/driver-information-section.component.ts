import { Component, OnInit, ViewChild } from '@angular/core';
import { DriverInfoPageService } from '../../../../../../../../core/services/submission/risk-specifics/driver-info-page.service';
import { BaseComponent } from '../../../../../../../../shared/base-component';
import { ToggleCollapseComponent } from '../../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { DriverInfoDto } from '../../../../../../../../shared/models/submission/riskspecifics/driver-info.dto';
import { IMvrPullingFrequencyDto } from '../../../../../../../../shared/models/submission/riskspecifics/mvr-pulling-frequency.dto';
import { DriverInfoData } from '../../../../../../data/riskspecifics/driver-info.data';
import { SubmissionData } from '../../../../../../data/submission.data';
import FormUtils from '../../../../../../../../shared/utilities/form.utils';

@Component({
  selector: 'app-driver-information-section',
  templateUrl: './driver-information-section-static.component.html',
  styleUrls: ['./driver-information-section.component.scss']
})
export class DriverInformationSectionComponent extends BaseComponent implements OnInit {

  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  mvrPullingFrequencyOptions: IMvrPullingFrequencyDto[];

  constructor(public driverInfoData: DriverInfoData, public submissionData: SubmissionData,
    private driverInfoPageService: DriverInfoPageService) {
    super();
  }

  numberOptions = Array.from(Array(16).keys());
  symbols189selected: boolean;
  ngOnInit() {
    if (this.submissionData?.riskDetail?.driverInfo) {
      this.driverInfoData.initiateFormGroup(this.submissionData.riskDetail.driverInfo);
      this.switchValidator(this.fc['isHiringFromOthersWithoutDriver'].value, 'annualCostOfHireWithoutDriver');
      this.driverInfoData.driverInfoForm.markAllAsTouched();
    } else {
      this.driverInfoData.initiateFormGroup(new DriverInfoDto());
    }

    this.driverInfoPageService.getDriverInfoPageOptions().subscribe(driverInfoPageOptions => {
      if (driverInfoPageOptions?.mvrPullingFrequencyOptions) {
        this.mvrPullingFrequencyOptions = driverInfoPageOptions.mvrPullingFrequencyOptions;
        if (this.submissionData?.riskDetail?.driverInfo?.mvrPullingFrequency?.id) {
          const savedMvrPullingFrequency = this.mvrPullingFrequencyOptions.find(x => x.id === this.submissionData.riskDetail.driverInfo.mvrPullingFrequency.id);
          this.driverInfoData.driverInfoForm.controls.mvrPullingFrequency.setValue(savedMvrPullingFrequency);
        }
      }
    });
    this.setBusinessRules();
    this.setFieldInputReset();
    // this.switchValidator(this.fc['areVolunteersUsedInBusiness'].value, 'percentageOfStaff');
    // this.switchValidator(this.fc['isHiringFromOthers'].value, 'annualCostOfHire');
    // this.switchValidator(this.fc['isHiringFromOthersWithoutDriver'].value, 'annualCostOfHireWithoutDriver');
    // this.switchValidator(this.fc['isLeasingToOthers'].value, 'annualIncomeDerivedFromLease');
    // this.switchValidator(this.fc['isLeasingToOthersWithoutDriver'].value, 'annualIncomeDerivedFromLeaseWithoutDriver');
    // this.switchValidator(this.fc['willBeAddingDeletingVehiclesDuringTerm'].value, 'addingDeletingVehiclesDuringTermDescription');
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.driverInfoData.driverInfoForm; }

  private setBusinessRules() {

    let coverage = this.submissionData.riskDetail?.riskCoverage;
    if (!coverage) {
      const riskCoverages = this.submissionData.riskDetail?.riskCoverages;
      if (riskCoverages) {
        coverage = riskCoverages[0];
      }
    }

    const symbols189 = [1, 2, 3, 4, 11, 12]; // Refer to limits.data symbolList;
    this.symbols189selected = symbols189.includes(coverage?.alSymbolId);
    if (!this.symbols189selected) {
      this.fc.isHiringFromOthers.setValue(false);
      this.fc.isHiringFromOthersWithoutDriver.setValue(false);
      this.fc.isLeasingToOthers.setValue(false);
      this.fc.isLeasingToOthersWithoutDriver.setValue(false);
    }
  }

  private setFieldInputReset() {
    this.fc['areVolunteersUsedInBusiness'].valueChanges.subscribe(value => {
      this.reset(value, 'percentageOfStaff', 0);
    });
    this.fc['isHiringFromOthers'].valueChanges.subscribe(value => {
      this.reset(value, 'annualCostOfHire', 0);
      this.reset(value, 'isHiringFromOthersWithoutDriver', false);
    });
    this.fc['isHiringFromOthersWithoutDriver'].valueChanges.subscribe(value => {
      this.reset(value, 'annualCostOfHireWithoutDriver', 0);
    });
    this.fc['isLeasingToOthers'].valueChanges.subscribe(value => {
      this.reset(value, 'annualIncomeDerivedFromLease', 0);
      this.reset(value, 'isLeasingToOthersWithoutDriver', false);
    });
    this.fc['isLeasingToOthersWithoutDriver'].valueChanges.subscribe(value => {
      this.reset(value, 'annualIncomeDerivedFromLeaseWithoutDriver', 0);
    });
    this.fc['willBeAddingDeletingVehiclesDuringTerm'].valueChanges.subscribe(value => {
      this.reset(value, 'addingDeletingVehiclesDuringTermDescription');
    });
  }

  switchValidator(isChecked: boolean, targetControlName: string) {
    if (isChecked) {
      FormUtils.addRequiredValidator(this.fg, targetControlName);
    } else {
      FormUtils.clearValidator(this.fg, targetControlName);
    }
  }

  reset(isChecked: boolean, targetControlName: string, defaultValue?: any) {
    if (!isChecked) {
      this.fc[targetControlName].setValue(defaultValue);
    }
  }
}
