import { Injectable } from '@angular/core';
import { DotInfoData } from '../../../modules/submission/data/riskspecifics/dot-info.data';
import FormUtils from '../../../shared/utilities/form.utils';

@Injectable()
export class DotInfoValidationService {
  dotInfoSection: boolean;

  get DotInfoForm() {
    return this.dotInfoData.dotInfoForm;
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.dotInfoData.dotInfoForm; }

  constructor(public dotInfoData: DotInfoData) { }

  checkDotInfoPage() {
    this.dotInfoSection = this.checkDotInfoSection();
  }

  checkDotInfoSection() {
    if (this.fc['isThereAnyPolicyLevelAccidents'].value) {
      FormUtils.addRequiredValidator(this.fg, 'numberOfPolicyLevelAccidents');
    }
    this.DotInfoForm.markAllAsTouched();
    return FormUtils.validateForm(this.DotInfoForm);
  }
}

