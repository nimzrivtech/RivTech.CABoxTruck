import { Component, OnInit } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { RiskSpecificsData } from '../../../../../../../modules/submission/data/risk-specifics.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { BaseComponent } from '../../../../../../../shared/base-component';
import { RiskSpecificGeneralLiabilityCargoService } from '../../../../../../../modules/submission/services/riskspecific-general-liability-cargo.service';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { PathConstants } from '@app/shared/constants/path.constants';

@Component({
  selector: 'app-general-liability-cargo',
  templateUrl: './general-liability-cargo.component.html',
  styleUrls: ['./general-liability-cargo.component.scss']
})

export class GeneralLiabilityCargoComponent extends BaseComponent implements OnInit {

  constructor (
    private navigationService: NavigationService,
    public riskSpecificsData: RiskSpecificsData,
    public submissionData: SubmissionData,
    public generalLiabilityCargoService: RiskSpecificGeneralLiabilityCargoService
  ) {
    super();
  }

  get cargoForm() { return this.riskSpecificsData.riskSpecificsForms.cargoForm; }
  get generalLiabilityForm() { return this.riskSpecificsData.riskSpecificsForms.generalLiabilityForm; }

  ngOnInit() {
    this.riskSpecificsData.isInitialGeneralLiabAndCargo = false;
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.MaintenanceSafety}`);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.UWQuestions}`);
        break;
    }
  }

  get scrollPage() {
    return false || this.riskSpecificsData.commoditiesHauled?.length > 3 || (this.riskSpecificsData?.hasGLCoverage && this.riskSpecificsData?.shouldShowCargoSection);
  }
}
