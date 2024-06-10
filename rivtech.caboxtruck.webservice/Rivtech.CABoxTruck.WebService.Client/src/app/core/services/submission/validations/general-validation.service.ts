import { Injectable } from '@angular/core';
import { InvalidFormControl } from '../../../../shared/models/dynamic/invalid-control.interface';
import * as _ from 'lodash';

@Injectable({
  providedIn: 'root'
})
export class GeneralValidationService {
  invalidFormControlList: InvalidFormControl[] = [];
  invalidFormControlErrorMessage: string;
  invalidForms: Array<string> = [];

  constructor() { }

  clearValidatorFormControl(formGroup?, controlName?): void {
    formGroup.get(controlName).clearValidators();
    formGroup.get(controlName).updateValueAndValidity();
  }

  resetValidatorFormControl(formGroup?, controlName?, validator?): void {
    formGroup.get(controlName).setValidators(validator);
    formGroup.get(controlName).updateValueAndValidity();
  }

  findInvalidControls(formGroup?) {
    const invalid = [];
    const controls = formGroup.controls;
    for (const name in controls) {
        if (controls[name].invalid) {
            invalid.push(name);
            this.invalidForms.push(name);
        } else {
          // condition to enable/disable Issue Button
          _.remove(this.invalidForms, (n: string) => n === name && !controls[name].invalid);
        }
    }
    return invalid;
  }

  mapToInvalidFormControlList(invalidList?, page?, section?): void {
     if (invalidList.length > 0) {
      invalidList.map(item => {
        // Error Message
        const displayError = `Invalid Field: <b>${page}</b> page at <b>${section}</b> section and <b>${item}</b> field name`;
        this.invalidFormControlErrorMessage = `${this.invalidFormControlErrorMessage} ${displayError} <br><br> `;
        // List of Form Controls that are invalid
        const invalidItem: InvalidFormControl = {
          formControlName: item,
          page: page,
          section: section,
        };
        this.invalidFormControlList.push(invalidItem);
      });
    }
  }

}
