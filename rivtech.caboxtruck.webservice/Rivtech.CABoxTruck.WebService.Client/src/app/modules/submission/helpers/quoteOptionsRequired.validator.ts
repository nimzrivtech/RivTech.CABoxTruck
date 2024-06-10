import { AbstractControl } from '@angular/forms';

export function oneColumnRequired(control: AbstractControl) {
    const arr = control.value;
    const req1 = arr === null || arr === '' ? true : false;
    if (req1) {
        return { req1: req1 };
    } else {
        return null;
    }
}

export function twoColumnRequired(control: AbstractControl) {
    const arr = control.value;
    const req1 = arr === null || arr === '' ? true : false;
    const req2 = arr === null || arr === '' ? true : false;
    if (req1 || req2) {
        return { req1: req1, req2: req2 };
    } else {
        return null;
    }
}

export function threeColumnRequired(control: AbstractControl) {
    const arr = control.value;
    const req1 = arr === null || arr === '' ? true : false;
    const req2 = arr === null || arr === '' ? true : false;
    const req3 = arr === null || arr === '' ? true : false;
    if (req1 || req2 || req3) {
        return { req1: req1, req2: req2, req3: req3 };
    } else {
        return null;
    }
}

export function fourColumnRequired(control: AbstractControl) {
    const arr = control.value;
    const req1 = arr === null || arr === '' ? true : false;
    const req2 = arr === null || arr === '' ? true : false;
    const req3 = arr === null || arr === '' ? true : false;
    const req4 = arr === null || arr === '' ? true : false;
    if (req1 || req2 || req3 || req4) {
        return { req1: req1, req2: req2, req3: req3, req4: req4 };
    } else {
        return null;
    }
}
