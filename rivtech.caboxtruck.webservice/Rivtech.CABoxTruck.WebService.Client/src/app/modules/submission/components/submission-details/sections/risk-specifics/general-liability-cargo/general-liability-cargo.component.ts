import { Component, OnInit, ViewChild } from '@angular/core';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { RiskSpecificsData } from '../../../../../../../modules/submission/data/risk-specifics.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { RiskSpecificGeneralLiabilityCargoService } from '../../../../../services/riskspecific-general-liability-cargo.service';
import { BaseComponent } from '../../../../../../../shared/base-component';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { GeneralLiabilityCargoValidationService } from '../../../../../../../core/services/validations/general-liability-cargo-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';

@Component({
  selector: 'app-general-liability-cargo',
  templateUrl: './general-liability-cargo.component.html',
  styleUrls: ['./general-liability-cargo.component.scss']
})

export class GeneralLiabilityCargoComponent extends BaseComponent implements OnInit {

  constructor (
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    public riskSpecificsData: RiskSpecificsData,
    public submissionData: SubmissionData,
    public generalLiabilityCargoService: RiskSpecificGeneralLiabilityCargoService,
    public location: Location,
    public generalLiabilityCargoValidation: GeneralLiabilityCargoValidationService,
    public formValidation: SubmissionNavValidateService,
  ) {
    super();
  }

  get cargoForm() { return this.riskSpecificsData.riskSpecificsForms.cargoForm; }
  get generalLiabilityForm() { return this.riskSpecificsData.riskSpecificsForms.generalLiabilityForm; }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;
  ngOnInit() {
    this.riskSpecificsData.isInitialGeneralLiabAndCargo = false;
  }

  onClick(clickType?: any) {
    this.nextBack.location = this.location;
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([this.nextBack.getPrev()]);
        break;
      case ClickTypes.Next:
        this.cargoForm.markAllAsTouched();
        this.generalLiabilityForm.markAllAsTouched();
        this.menuOverrideService.overrideSideBarBehavior();
        this.router.navigate([this.nextBack.getNext()]);
        break;
    }
  }

  get scrollPage() {
    return false || this.riskSpecificsData.commoditiesHauled?.length > 3 || (this.riskSpecificsData?.hasGLCoverage && this.riskSpecificsData?.shouldShowCargoSection);
  }
}
