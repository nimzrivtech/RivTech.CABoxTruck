import { FormGroup } from '@angular/forms';

export function mcs90Validator(brokerData: any) {
    return (formGroup: FormGroup) => {

        const dateReceivedControl = formGroup.controls['dateReceived'];
        const effectiveDateControl = formGroup.controls['effectiveDate'];
        const yearControl = formGroup.controls['policyDateYear'];
        const monthControl = formGroup.controls['policyDateMonth'];
        const dayControl = formGroup.controls['policyDateDay'];

        const dateValue1 = brokerData?.formInfo?.controls['effectiveDate']?.value;
        const effectiveDate = dateValue1?.singleDate ? new Date(dateValue1.singleDate?.jsDate?.toDateString()) : new Date(dateValue1);

        const dateValue2 = brokerData?.formInfo?.controls['expirationDate']?.value;
        const expirationDate = dateValue2?.singleDate ? new Date(dateValue2.singleDate?.jsDate?.toDateString()) : new Date(dateValue2);


        if (dateReceivedControl.value?.singleDate?.jsDate < effectiveDate) {
            dateReceivedControl.setErrors({ 'beforeEffective': true });
        }

        if (dateReceivedControl.value?.singleDate?.jsDate > expirationDate) {
            dateReceivedControl.setErrors({ 'afterExpiration': true });
        }

        if (yearControl?.value > expirationDate?.getFullYear()) {
            yearControl.setErrors({ 'afterExpirationYear': true });
        }

        if (yearControl?.value < 1900) {
            yearControl.setErrors({ 'invalidYear': true });
        }

        if (effectiveDateControl.value?.singleDate?.jsDate < effectiveDate) {
            effectiveDateControl.setErrors({ 'beforeEffective': true });
        }

        dayControl.setErrors(null);
        const monthValue = (new Date('1 ' + monthControl?.value + ' 1900')).getMonth();
        const lastDayOfMonth = (new Date(yearControl?.value, monthValue + 1, 0)).getDate();

        if (!isNaN(lastDayOfMonth) && dayControl?.value > lastDayOfMonth) {
            dayControl.setErrors({ 'invalidLastDay': true });
        }

    };
}

