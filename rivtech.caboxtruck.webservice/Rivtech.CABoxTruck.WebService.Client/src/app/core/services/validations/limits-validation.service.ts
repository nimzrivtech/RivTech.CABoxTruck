import { Injectable } from '@angular/core';
import { LimitsData } from '../../../modules/submission/data/limits/limits.data';
import FormUtils from '../../../shared/utilities/form.utils';

@Injectable()
export class LimitsValidationService {
  limitsSection: boolean;

  get LimitsPageForm() {
    return this.limitsData.limitsForm.limitsPageForm;
  }
  constructor(public limitsData: LimitsData) { }

  checkLimitsPage() {
    this.limitsSection = this.limitsData.checkLimitsSection();
  }
}
