import { FormGroup } from '@angular/forms';

// custom validator to check that two fields match
export function yearEstablishedInvalidYear(year: number, controlName: string) {
    return (formGroup: FormGroup) => {
            const control = formGroup.controls[controlName];

            if (control.errors && !control.errors.yearEstablishedInvalidYear && !control.errors.yearEstablishedMinYear) {
                control.setErrors(control.errors);
                // return if another validator has already found an error on the matchingControl
                return;
            }
            if (control.value !== undefined && (isNaN(control.value) || control.value == 0)) {
                control.setErrors({ required: true });
                return;
            }
            if (control.value !== undefined && (isNaN(control.value) || (control.value > 0 && control.value < 1900))) {
                control.setErrors({ yearEstablishedMinYear: true });
                return;
            }
            if (control.value !== undefined && (isNaN(control.value) || control.value > year)) {
                control.setErrors({ yearEstablishedInvalidYear: true });
            } else {
                control.setErrors(null);
            }
    };
}

export function firstYearUnderCurrentInvalidYear(year: number, controlName: string) {
    return (formGroup: FormGroup) => {
            const control = formGroup.controls[controlName];

            if (control.errors && !control.errors.invalidYear) {
                // return if another validator has already found an error on the matchingControl
                return;
            }
            if (control.value !== undefined && (isNaN(control.value) || control.value > year)) {
                control.setErrors({ firstYearUnderCurrentInvalidYear: true });
            } else {
                control.setErrors(null);
            }
    };
}

export function firstYearVsUnderCurrentInvalidYear(yearEstablished: string, firstYear: string) {
    return (formGroup: FormGroup) => {
            const yearEstablishedControl = formGroup.controls[yearEstablished];
            const firstYearControl = formGroup.controls[firstYear];

            if (firstYearControl.errors && !firstYearControl.errors.invalidYear) {
                // return if another validator has already found an error on the matchingControl
                return;
            }

            if (firstYearControl.value !== null && firstYearControl.value !== '' && firstYearControl.value !== undefined &&
                (isNaN(firstYearControl.value) || isNaN(yearEstablishedControl.value) || firstYearControl.value < yearEstablishedControl.value)) {
                firstYearControl.setErrors({ firstYearVsUnderCurrentInvalidYear: true });
            } else {
                firstYearControl.setErrors(null);
            }
    }
}