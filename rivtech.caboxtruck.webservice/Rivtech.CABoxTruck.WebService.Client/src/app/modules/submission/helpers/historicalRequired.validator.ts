import { AbstractControl, FormGroup } from '@angular/forms';

export function historicalRequired(controlName: string) {
    return (formGroup: FormGroup) => {
        const control = formGroup.controls[controlName];

        if (control.errors) {
            // return if another validator has already found an error on the matchingControl
            return;
        }
        const arr = control.value;
        const invalid0 = arr[0] === null || arr[0] === '' ? true : false;
        if (invalid0) {
            control.setErrors({ invalid0: invalid0 });
        } else {
            control.setErrors(null);
        }
    };
}

export function historicalFieldRequired(control: AbstractControl) {
    const arr = control.value;

    const invalid0 = arr[0] === null || arr[0] === '' || arr[0] === 0 || arr[0] === '0' ? true : false;
    if (invalid0) {
        return { invalid0: invalid0 };
    } else {
        return null;
    }
}

export function historicalFieldNoZeroInputRequired(control: AbstractControl) {
    const arr = control.value;

    const invalid0 = arr[0] === null || arr[0] === '' ? true : false;
    if (invalid0) {
        return { invalid0: invalid0 };
    } else {
        return null;
    }
}

export function lessThanStartInvalidYear(controlStart: string, controlEnd: string) {
    return (formGroup: FormGroup) => {
        const endcontrol = formGroup.controls[controlEnd];
        const startControl = formGroup.controls[controlStart];

        const arrEnd = endcontrol.value;
        const arrStart = startControl.value;
        const invalidYear0 = (arrEnd[0] !== undefined && arrStart[0] !== undefined) && arrEnd[0]?.singleDate?.jsDate <= arrStart[0]?.singleDate?.jsDate ? true : false;
        const invalidYear1 = (arrEnd[1] !== undefined && arrStart[1] !== undefined) && arrEnd[1]?.singleDate?.jsDate <= arrStart[1]?.singleDate?.jsDate ? true : false;
        const invalidYear2 = (arrEnd[2] !== undefined && arrStart[2] !== undefined) && arrEnd[2]?.singleDate?.jsDate <= arrStart[2]?.singleDate?.jsDate ? true : false;
        const invalidYear3 = (arrEnd[3] !== undefined && arrStart[3] !== undefined) && arrEnd[3]?.singleDate?.jsDate <= arrStart[3]?.singleDate?.jsDate ? true : false;
        const invalidYear4 = (arrEnd[4] !== undefined && arrStart[4] !== undefined) && arrEnd[4]?.singleDate?.jsDate <= arrStart[4]?.singleDate?.jsDate ? true : false;
        let errorList: any = {};
        if (endcontrol.errors) {
            errorList = endcontrol.errors;
        }
        if (invalidYear0 || invalidYear1 || invalidYear2 || invalidYear3 || invalidYear4) {
            errorList.invalidYear0 = invalidYear0;
            errorList.invalidYear1 = invalidYear1;
            errorList.invalidYear2 = invalidYear2;
            errorList.invalidYear3 = invalidYear3;
            errorList.invalidYear4 = invalidYear4;

            endcontrol.setErrors(errorList);
        } else {
            if (endcontrol.errors) {
                endcontrol.setErrors({ invalid0: errorList.invalid0 });
            }
        }
    };
}

export function currentPriorInvalid() {
    return (formGroup: FormGroup) => {
        const startDatesControl = formGroup.controls['policyTermStartDate'];
        const endDatesControl = formGroup.controls['policyTermEndDate'];
        // const lossRunDatesControl = formGroup.controls['lossRunDateForEachTerm'];

        const startDates = startDatesControl?.value;
        const endDates = endDatesControl?.value;
        // const lossRunDates = lossRunDatesControl?.value;

        const errorListTermStart = startDatesControl.errors ? startDatesControl.errors : {};
        const errorListTermEnd = endDatesControl.errors ? endDatesControl.errors : {};
        // const errorListLossRun = lossRunDatesControl.errors ? lossRunDatesControl.errors : {};

        for (let i = 1; i <= 4; i++) {
            if (startDates[i]?.singleDate?.jsDate >= startDates[0]?.singleDate?.jsDate) {
                errorListTermStart[`invalidPrior${i}`] = true;
            }
            if (!errorListTermEnd[`invalidYear${i}`] && endDates[i]?.singleDate?.jsDate >= endDates[0]?.singleDate?.jsDate) {
                errorListTermEnd[`invalidPrior${i}`] = true;
            }
            // if (lossRunDates[i]?.singleDate?.jsDate >= lossRunDates[0]?.singleDate?.jsDate) {
            //     errorListLossRun[`invalidPrior${i}`] = true;
            // }
        }

        startDatesControl.setErrors(Object.keys(errorListTermStart).length > 0 ? errorListTermStart : null);
        endDatesControl.setErrors(Object.keys(errorListTermEnd).length > 0 ? errorListTermEnd : null);
        // lossRunDatesControl.setErrors(Object.keys(errorListLossRun).length > 0 ? errorListLossRun : null);
    };
}

export function lessThan6MonthsStartInvalidYear(controlEnd: string, coverageForms?: any) {

    const isNotAfter6Months = (order: number, arrEnd: any, arrStart: any): boolean => {
        if (arrEnd[order] === undefined || arrStart[order] === undefined) { return false; }

        const policyTermStartDate = arrStart[order]?.singleDate?.jsDate;
        if (!policyTermStartDate) { return false; }

        const startDateAfter6Months = new Date(policyTermStartDate);
        startDateAfter6Months.setMonth(startDateAfter6Months.getMonth() + 6); // After 6 months validation

        return arrEnd[order]?.singleDate?.jsDate < startDateAfter6Months;
    };


    return (formGroup: FormGroup) => {
        const arrStart = coverageForms ? coverageForms.historicalCoverageForm.controls['policyTermStartDate']?.value :
            formGroup.controls['policyTermStartDate']?.value;

        const endcontrol = formGroup.controls[controlEnd];
        const errorList = endcontrol.errors ? endcontrol.errors : {};

        for (let i = 0; i <= 4; i++) {
            const invalidYear = isNotAfter6Months(i, endcontrol.value, arrStart);
            if (invalidYear) {
                errorList[`invalidYear${i}`] = invalidYear;
            } else {
                delete errorList[`invalidYear${i}`];
            }
        }

        endcontrol.setErrors(Object.keys(errorList).length > 0 ? errorList : null);
    };
}

export function policyTermValidator(currentField: string, brokerData?: any) {
    return (formGroup: FormGroup) => {

        const dateValue = brokerData?.formInfo?.controls['effectiveDate']?.value;
        const effectiveDate = dateValue?.singleDate ? new Date(dateValue.singleDate?.jsDate?.toDateString()) : new Date(dateValue);

        const control = formGroup.controls[currentField];
        const invalidTerm0 = dateValue && control.value[0]?.singleDate?.jsDate > effectiveDate;
        const invalidTerm1 = dateValue && control.value[1]?.singleDate?.jsDate > effectiveDate;
        const invalidTerm2 = dateValue && control.value[2]?.singleDate?.jsDate > effectiveDate;
        const invalidTerm3 = dateValue && control.value[3]?.singleDate?.jsDate > effectiveDate;
        const invalidTerm4 = dateValue && control.value[4]?.singleDate?.jsDate > effectiveDate;

        let errorList: any = {};
        if (control.errors) {
            errorList = control.errors;
        }
        if (invalidTerm0 || invalidTerm1 || invalidTerm2 || invalidTerm3 || invalidTerm4) {
            errorList.invalidTerm0 = invalidTerm0;
            errorList.invalidTerm1 = invalidTerm1;
            errorList.invalidTerm2 = invalidTerm2;
            errorList.invalidTerm3 = invalidTerm3;
            errorList.invalidTerm4 = invalidTerm4;

            control.setErrors(errorList);
        }
    };

}
