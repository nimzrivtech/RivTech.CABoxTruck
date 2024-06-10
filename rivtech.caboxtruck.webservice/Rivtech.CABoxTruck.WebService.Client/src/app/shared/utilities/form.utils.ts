import { Injectable } from "@angular/core";
import { FormGroup, ValidationErrors, AbstractControl, Validators, FormControl, FormArray } from '@angular/forms';

@Injectable()
export default class FormUtils {
    constructor() { }

    static formsAreDirty(jsonForms: any): boolean {
        if (!jsonForms) return false;

        for (let key in jsonForms) {
            if (jsonForms.hasOwnProperty(key) && jsonForms[key] instanceof FormGroup) {
                let isDirty: boolean = (jsonForms[key] as FormGroup).dirty;

                if (isDirty) return true;
            }
        }

        return false;
    }

    public static matchValues(
        matchTo: string // name of the control to match to
    ): (AbstractControl) => ValidationErrors | null {
        return (control: AbstractControl): ValidationErrors | null => {
            return !!control.parent &&
                !!control.parent.value &&
                control.value === control.parent.controls[matchTo].value
                ? null
                : { isMatching: false };
        };
    }

    static clearValidator(form: FormGroup, controlName: string) {
        let field = form.controls[controlName];
        field.clearValidators();
        field.updateValueAndValidity();    
    }

    static addRequiredValidator(form: FormGroup, controlName: string) {
        let control = form.controls[controlName];
        control.setValidators([Validators.required, Validators.min(1)]);
        control.markAsUntouched();
        control.markAsPristine();
        control.updateValueAndValidity();
    }

    static validateForm(formGroup: FormGroup) {
        return formGroup.status === 'DISABLED' ? true :
          formGroup.valid;
      }

    static validateFormControl(formControl: AbstractControl) {
        return formControl.status === 'DISABLED' ? true :
        formControl.valid;
      }

    static clearValidatorsByFormGroup(control: AbstractControl): void {
    Object.keys((control as FormArray).controls).forEach(key => {
        control['controls'][key].clearValidators();
        control['controls'][key].updateValueAndValidity();
    });
    }

    static resetFields(formGroup?: FormGroup, formControls?) {
        formControls.map(formControl => {
          formGroup.get(formControl).setValue('');
        });
    }

    static isControlValid(controlId: string | AbstractControl, formGroup: FormGroup): boolean {
        const control = (controlId instanceof AbstractControl) ? controlId :
          formGroup.get(controlId);
    
        return ((control.touched || control.dirty) && !control.valid && control.status.toUpperCase() !== 'DISABLED');
      }

}
