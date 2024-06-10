import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class CustomValidators {
  static removeSpaces(control: AbstractControl) {
    if (control && control.value && !control.value.replace(/\s/g, '').length) {
        control.setValue('');
    }
    return null;
  }

  static spaceValidator(control: AbstractControl) {
    if (control && control.value && !control.value.replace(/\s/g, '').length) {
        control.setValue('');
        return { required: true };
    } else {
        return null;
    }
  }

  static hasNoValue(control: AbstractControl) {
    return control?.value?.toString()?.trim() === '' ? { required: true } : null;
  }

  static requiredFileFormat(type: string[]) {
    return function(control: AbstractControl) {
      const file = control.value;
      if (file instanceof File) {
        const extension = file?.name?.split('.')?.pop();
        if (!type.includes(extension.toLowerCase())) {
          return {
            requiredFileType: true
          };
        }
        return null;
      }

      return null;
    };
  }

  static zeroValueValidator(control: AbstractControl) {
    if (control && control.value && (control.value === 0 || control.value === '0')) {
        return { required: true };
    } else {
        return null;
    }
  }

  static phoneNumberValidator(control: AbstractControl) {
    const regex = /^\d{10}$/;
    const zerosReg = /[1-9]/g;
    if (control && control.value) {
      const phoneMatch = control.value.match(regex);
      if (!phoneMatch || !zerosReg.test(phoneMatch)) {
        return { pattern: true };
      } else {
        return null;
      }
    } else {
      return null;
    }
  }

  static federalIdNumberValidator(control: AbstractControl) {
    const regex = /^\d{9}$/;
    const zerosReg = /[1-9]/g;
    if (control && control.value) {
      const phoneMatch = control.value.match(regex);
      if (!phoneMatch || !zerosReg.test(phoneMatch)) {
        return { pattern: true };
      } else {
        return null;
      }
    } else {
      return null;
    }
  }


  static passwordValidator(error: ValidationErrors): ValidatorFn {
    let regex: RegExp;

    return (control: AbstractControl): { [key: string]: any } => {
      if (control && control.value) {
        if(error.hasOwnProperty('hasNumber')){
          regex = /\d/;
        }
        if(error.hasOwnProperty('hasCapitalCase')){
          regex = /[A-Z]/;
        }
        if(error.hasOwnProperty('hasSmallCase')){
          regex = /[a-z]/;
        }
        if(error.hasOwnProperty('hasSpecialCharacters')){
          regex = /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;
        }

        const valid = regex.test(control.value);
        return valid ? null : error;
      } else {
        return null;
      }
    };
  }
}
