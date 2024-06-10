import { FormGroup } from '@angular/forms';
import { Injectable } from '@angular/core';
import { takeUntil } from 'rxjs/operators';

import { BaseClass } from '../../../shared/base-class';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { ZipCodeService } from '../../../core/services/management/zip-code.service';
import { ErrorMessageConstant } from '../../../shared/constants/error-message.constants';

@Injectable()
export class ZipCodeData extends BaseClass {
    constructor(private zipCodeService: ZipCodeService) {
        super();
    }

    cityZipCodeData: any;
    cityList: any = [];

    getZipCode(zipCode, formGroup, formControlNames, isAdd?) { // , section?
        this.zipCodeService.getZipCode(zipCode).pipe(takeUntil(this.stop$)).subscribe(result => {
            this.cityZipCodeData = JSON.parse(result.toString());
            if (this.cityZipCodeData.length > 0) {
                try {
                    this.setList(this.cityZipCodeData, formGroup, formControlNames, isAdd); //, section
                } catch (e) { }
            } else {
                this.resetForm(formGroup, formControlNames);
                NotifUtils.showError(ErrorMessageConstant.zipCodeNotFoundContactUWErrorMessage);
            }
        }, error => {
            console.log(error);
            this.resetForm(formGroup, formControlNames);
            NotifUtils.showError(ErrorMessageConstant.zipCodeNotFoundContactUWErrorMessage);
        });
    }

    setList(data, formGroup, formControlNames, isAdd?) { //, section?
        this.cityList = [];
        this.cityList = this.getCityList(data);
        if (this.cityList.length > 0) {
            this.autoPopulateValues(data, formGroup, formControlNames, this.cityList, isAdd);
        }
    }

    getCityList(data) {
        const cityList = [];
        data.map(val => {
            const city = val.city;
            cityList.push(city);
        });

        return cityList;
    }

    autoPopulateValues(data, formGroup: FormGroup, formControls, cityList?, isAdd?) {
        formControls.map(formControl => {
            if (formControl.toLowerCase().includes('city')) {
                if (cityList.length === 1) {
                    formGroup.get('address').get(formControl).setValue(data[0].city);
                } else {
                    if (isAdd) { formGroup.get('address').get(formControl).setValue(''); }
                }
            } else if (formControl.toLowerCase().includes('state')) {
                formGroup.get('address').get(formControl).setValue(data[0].stateCode);
            }
        });
    }

    resetForm(formGroup: FormGroup, formControls) {
        formControls.map(formControl => {
            formGroup.get('address').get(formControl).setValue('');
        });
    }
}