import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { BillingErrorConstants } from './validator-error-constants/billing-error.constants';

export class BillingValidators {
  static amountCannotBeZeroValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const key: any = {};
      if (control && control.value === 0) {
        key[BillingErrorConstants.amountCannotBeZeroErrorMessage.key] = true;
        return key;
      }
      return null;
    };
  }
}