import { Component, OnInit, ViewChild } from '@angular/core';
import { DotInfoService } from '../../../../../../../core/services/submission/risk-specifics/dot-info.service';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { DotInfoDto } from '../../../../../../../shared/models/submission/riskspecifics/dot-info.dto';
import { IChameleonIssuesDto } from '../../../../../../../shared/models/submission/riskspecifics/chameleon-issues.dto';
import { ISaferRatingDto } from '../../../../../../../shared/models/submission/riskspecifics/safer-rating.dto';
import { ITrafficLightRatingDto } from '../../../../../../../shared/models/submission/riskspecifics/traffic-light-rating.dto';
import FormUtils from '../../../../../../../shared/utilities/form.utils';
import { DotInfoData } from '../../../../../../../modules/submission/data/riskspecifics/dot-info.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { NavigationService } from '../../../../../../../core/services/navigation/navigation.service';
import { PathConstants } from '../../../../../../../shared/constants/path.constants';

@Component({
  selector: 'app-dot-information',
  templateUrl: './dot-information.component.html',
  styleUrls: ['./dot-information.component.scss']
})
export class DotInformationComponent implements OnInit {

  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;

  chameleonIssuesOptions: IChameleonIssuesDto[];
  saferRatingOptions: ISaferRatingDto[];
  trafficLightRatingOptions: ITrafficLightRatingDto[];

  constructor(private navigationService: NavigationService,
    public dotInfoData: DotInfoData,
    private dotInfoPageService: DotInfoService,
    public submissionData: SubmissionData) { }

  ngOnInit() {
    if (this.submissionData?.riskDetail?.dotInfo) {
      this.dotInfoData.initiateFormGroup(this.submissionData.riskDetail.dotInfo);
      this.switchValidator(this.fc['isThereAnyPolicyLevelAccidents'].value, 'numberOfPolicyLevelAccidents');
      this.dotInfoData.dotInfoForm.markAllAsTouched();
    } else {
      this.dotInfoData.initiateFormGroup(new DotInfoDto());
    }

    this.dotInfoPageService.getDotInfoPageOptions().subscribe(dotInfoPageOptions => {
      if (dotInfoPageOptions?.chameleonIssuesOptions) {
        this.chameleonIssuesOptions = dotInfoPageOptions.chameleonIssuesOptions;
        const chameleonIssuesId = this.submissionData?.riskDetail?.dotInfo?.chameleonIssues?.id ?? 1;
        const savedChameleonIssues = this.chameleonIssuesOptions.find(x => x.id === chameleonIssuesId);
        this.dotInfoData.dotInfoForm.controls.chameleonIssues.setValue(savedChameleonIssues);

      }
      if (dotInfoPageOptions?.saferRatingOptions) {
        this.saferRatingOptions = dotInfoPageOptions.saferRatingOptions;
        const saferRatingId = this.submissionData?.riskDetail?.dotInfo?.currentSaferRating?.id ?? 4;
        const savedSaferRating = this.saferRatingOptions.find(x => x.id === saferRatingId);
        this.dotInfoData.dotInfoForm.controls.currentSaferRating.setValue(savedSaferRating);
      }
      if (dotInfoPageOptions?.trafficLightRatingOptions) {
        this.trafficLightRatingOptions = dotInfoPageOptions.trafficLightRatingOptions;
        const issCabRatingId = this.submissionData?.riskDetail?.dotInfo?.issCabRating?.id ?? 4;
        const savedIssCabRating = this.trafficLightRatingOptions.find(x => x.id === issCabRatingId);
        this.dotInfoData.dotInfoForm.controls.issCabRating.setValue(savedIssCabRating);
      }
    });

    this.setFieldInputReset();
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.dotInfoData.dotInfoForm; }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.DriverInfo}`);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.MaintenanceSafety}`);
        break;
    }
  }

  private setFieldInputReset() {
    this.fc['doesApplicantPlanToStayInterstate'].valueChanges.subscribe(value => {
      this.reset(value, 'doesApplicantHaveInsterstateAuthority', true);
    });
    this.fc['isThereAnyPolicyLevelAccidents'].valueChanges.subscribe(value => {
      this.reset(value, 'numberOfPolicyLevelAccidents', 0);
    });
  }

  reset(isChecked: boolean, targetControlName: string, defaultValue?: any) {
    if (!isChecked) {
      this.fc[targetControlName].setValue(defaultValue);
    }
  }

  switchValidator(isChecked: boolean, targetControlName: string) {
    if (isChecked) {
      FormUtils.addRequiredValidator(this.fg, targetControlName);
    } else {
      FormUtils.clearValidator(this.fg, targetControlName);
    }
  }
}
