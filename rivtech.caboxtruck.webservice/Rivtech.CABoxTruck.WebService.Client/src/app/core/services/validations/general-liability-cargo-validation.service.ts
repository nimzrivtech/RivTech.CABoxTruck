import { Injectable } from '@angular/core';
import { RiskSpecificsData } from '../../../modules/submission/data/risk-specifics.data';
import FormUtils from '../../../shared/utilities/form.utils';

@Injectable()
export class GeneralLiabilityCargoValidationService {
  generalLiabilitySection: boolean;
  cargoSection: boolean;

  get GeneralLiabilityForm() {
    return this.riskSpecificData.riskSpecificsForms.generalLiabilityForm;
  }
  get CargoForm() {
    return this.riskSpecificData.riskSpecificsForms.cargoForm;
  }
  constructor(public riskSpecificData: RiskSpecificsData) { }

  checkGeneralLiabilityCargoPage() {
    if (!this.riskSpecificData.isInitialGeneralLiabAndCargo) {
      this.generalLiabilitySection = this.checkGeneralLiabilitySection();
      this.cargoSection = this.checkCargoSection();
    }
  }

  checkGeneralLiabilityCargoPolicyPage() {
    this.generalLiabilitySection = this.checkGeneralLiabilitySection();
    this.cargoSection = this.checkCargoSection();
  }

  checkGeneralLiabilitySection() {
    this.GeneralLiabilityForm.markAllAsTouched();
    return !this.riskSpecificData.hasGLCoverage || FormUtils.validateForm(this.GeneralLiabilityForm);
  }

  checkCargoSection() {
    this.CargoForm.markAllAsTouched();
    if (!this.riskSpecificData.hasGLCargoCoverage && !this.riskSpecificData.hasGLRefrigerationCoverage) { return true; }
    let sum = this.riskSpecificData.commoditiesHauled.reduce((acc, cur) => acc + Number(cur.percentageOfCarry), 0);
    sum = Math.round(sum * 100) / 100;
    return FormUtils.validateForm(this.CargoForm) && this.riskSpecificData.commoditiesHauled.length > 0 && sum == 100;
  }
}

