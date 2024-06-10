import { FormGroup } from '@angular/forms';

// custom validator to check that two fields match
export function validateEmptySelect(controlName: string) {
  return (formGroup: FormGroup) => {
          const control = formGroup.controls[controlName];

          if (control.errors && !control.errors.invalidSelected) {
              // return if another validator has already found an error on the matchingControl
              return;
          }
          if (control.value == undefined || control.value == "" || control.value == null || control.value == "null") {
              control.setErrors({ validateEmptySelect: true });
          }else{
              control.setErrors(null);
          }
  }
}
