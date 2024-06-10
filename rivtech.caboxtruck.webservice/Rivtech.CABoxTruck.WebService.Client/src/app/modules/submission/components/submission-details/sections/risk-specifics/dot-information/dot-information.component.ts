import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { DotInfoService } from '../../../../../../../core/services/submission/risk-specifics/dot-info.service';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { DotInfoDto } from '../../../../../../../shared/models/submission/riskspecifics/dot-info.dto';
import { DotInfoData } from '../../../../../data/riskspecifics/dot-info.data';
import { SubmissionData } from '../../../../../data/submission.data';
import { Location } from '@angular/common';
import { IChameleonIssuesDto } from '../../../../../../../shared/models/submission/riskspecifics/chameleon-issues.dto';
import { ISaferRatingDto } from '../../../../../../../shared/models/submission/riskspecifics/safer-rating.dto';
import { ITrafficLightRatingDto } from '../../../../../../../shared/models/submission/riskspecifics/traffic-light-rating.dto';
import { DotInfoValidationService } from '../../../../../../../core/services/validations/dot-information-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';
import FormUtils from '../../../../../../../shared/utilities/form.utils';

@Component({
  selector: 'app-dot-information',
  templateUrl: './dot-information.component.html',
  styleUrls: ['./dot-information.component.scss']
})
export class DotInformationComponent implements OnInit {

  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;

  chameleonIssuesOptions: IChameleonIssuesDto[];
  saferRatingOptions: ISaferRatingDto[];
  trafficLightRatingOptions: ITrafficLightRatingDto[];

  constructor(private router: Router,
    private menuOverrideService: MenuOverrideService,
    private location: Location,
    public dotInfoData: DotInfoData,
    private dotInfoPageService: DotInfoService,
    public dotInfoValidation: DotInfoValidationService,
    public formValidation: SubmissionNavValidateService,
    public submissionData: SubmissionData, private route: ActivatedRoute) { }

  ngOnInit() {
    if (this.submissionData?.riskDetail?.dotInfo) {
      this.dotInfoData.initiateFormGroup(this.submissionData.riskDetail.dotInfo);
      this.switchValidator(this.fc['isThereAnyPolicyLevelAccidents'].value, 'numberOfPolicyLevelAccidents');
      this.dotInfoData.dotInfoForm.markAllAsTouched();
      this.loadDotInfo();
    } else {
      this.submissionData.getCurrentServerDateTime2().subscribe((data) => {
        this.dotInfoData.currentServerDateTime = data;
        this.dotInfoData.initiateFormGroup(new DotInfoDto());
        this.loadDotInfo();
      });
    }

    this.setFieldInputReset();
  }

  loadDotInfo() {
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
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.dotInfoData.dotInfoForm; }

  public onClick(clickType?: any) {
    this.nextBack.location = this.location;
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([this.nextBack.getPrev()]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        this.dotInfoData.dotInfoForm.markAllAsTouched();
        this.router.navigate([this.nextBack.getNext()]);
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
