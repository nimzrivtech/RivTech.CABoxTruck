import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GenericLabelConstants } from '../../../../../../../../shared/constants/generic.labels.constants';
import { CargoConstants } from '../../../../../../../../shared/constants/risk-specifics/general-liability-cargo.label.constants';
import { RiskSpecificsData } from '../../../../../../../../modules/submission/data/risk-specifics.data';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { BaseComponent } from '../../../../../../../../shared/base-component';
import FormUtils from '../../../../../../../../shared/utilities/form.utils';

@Component({
  selector: 'app-cargo-section',
  templateUrl: './cargo-section.component.html',
  styleUrls: ['./cargo-section.component.scss']
})
export class CargoSectionComponent  extends BaseComponent implements OnInit {
  hideMe: boolean = false;
  constructor(
    public submissionData: SubmissionData,
    public riskSpecificsData: RiskSpecificsData,
    public router: Router
  ) {
    super();
  }
  get fg() { return this.riskSpecificsData.riskSpecificsForms.cargoForm; }
  get fc() { return this.fg.controls; }
  get fn() { return 'cargoForm'; }

  public CargoConstants = CargoConstants;
  public GenericLabelConstants = GenericLabelConstants;

  ngOnInit() {
    if (this.submissionData?.riskDetail?.generalLiabilityCargo) {
      this.fg.markAllAsTouched();
    }
    this.switchValidator(this.fc['areCommoditiesStoredInTruckOvernight'].value, 'areCommoditiesStoredInTruckOvernightExplain');
    this.switchValidator(this.fc['hadCargoLoss'].value, 'hadCargoLossExplain');
  }

  switchValidator(isChecked: boolean, targetControlName: string) {
    // if (isChecked) {
    //   FormUtils.addRequiredValidator(this.fg, targetControlName);
    // } else {
    //   FormUtils.clearValidator(this.fg, targetControlName);
    // }
    if (!isChecked) {
      this.fc[targetControlName].setValue(null);
    }
  }

  ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }
}
