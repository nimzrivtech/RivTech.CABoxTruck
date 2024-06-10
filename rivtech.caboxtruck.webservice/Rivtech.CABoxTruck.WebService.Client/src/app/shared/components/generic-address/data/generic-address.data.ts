import { BaseClass } from '../../../../shared/base-class';
import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ITableFormControl, ITableTr, ITableTh, ITableTd } from '../../../../shared/models/dynamic/table.interface';
import { GenericAddressTableConstants } from '../../../../shared/constants/generic-address.table.constants';
import { CustomValidators } from '../../../../shared/validators/custom.validator';
import NotifUtils from '../../../../shared/utilities/notif-utils';
import { takeUntil } from 'rxjs/operators';
import { ZipCodeService } from '../../../../core/services/management/zip-code.service';
import { EntityAddressDTO } from '../../../../shared/models/management/generic-address.model';
import { AddressTypeService } from '../../../../core/services/management/address-type.service';
import { AddressTypeDTO } from '../../../../shared/models/management/agency-management/address-typeDto';
import { ErrorMessageConstant } from '../../../constants/error-message.constants';

@Injectable({
    providedIn: 'root'
})
export class GenericAddressData extends BaseClass {
    GenericAddressTableConstants = GenericAddressTableConstants;
    genericFormAddress: FormGroup;
    addressTypes: AddressTypeDTO[];
    addressList: EntityAddressDTO[] = [];

    tableHeaders: ITableTh[] = [
        { value: this.GenericAddressTableConstants.addressType },
        { value: this.GenericAddressTableConstants.city },
        { value: this.GenericAddressTableConstants.state },
        { value: this.GenericAddressTableConstants.zipCode }
    ];
    addressTableRows: ITableTr[] = [];
    addresstableFormControls: ITableFormControl[] = [
        {
            name: 'field_1',
            type: 'string',
        },
        {
            name: 'field_2',
            type: 'string',
        },
        {
            name: 'field_3',
            type: 'string',
        },
    ];

    constructor(private fb: FormBuilder,
        private addressTypeService: AddressTypeService,
        private zipCodeService: ZipCodeService) {
        super();
    }

    initializeForms() {
        this.getAddressType();

        const regex = /[^0-9]*/g;
        this.genericFormAddress = this.fb.group({
            // id: [null, []],
            addressType: new FormControl(null, [Validators.required]),
            effectiveDate: new FormControl(null),
            expirationDate: new FormControl(null),
            address: this.fb.group({
                streetAddress1: new FormControl(''),
                streetAddress2: new FormControl(''),
                zipCode: new FormControl('', [Validators.required, Validators.pattern(regex), CustomValidators.hasNoValue]),
                state: new FormControl({ value: '', disabled: true }, [Validators.required]),
                city: new FormControl('', [Validators.required]),
                isGarageIndoor: new FormControl(false), //Remove all related to garage once API is done
                isGarageOutdoor: new FormControl(false),
                isGarageFenced: new FormControl(false),
                isGarageLighted: new FormControl(false),
                isGarageWithSecurityGuard: new FormControl(false)
            })
        });
    }

    getAddressType() {
        this.addressTypeService.addressTypesAll().takeUntil(this.stop$).subscribe((res) => {
            this.addressTypes = res.filter(x => x.isActive);
        });
    }

    fillForms(entityAddressModel: EntityAddressDTO) {
        this.getAddressType();

        const regex = /[^0-9]*/g;
        this.genericFormAddress = this.fb.group({
            // id: [entityAddressModel.id, []],
            addressType: new FormControl(entityAddressModel.addressTypeId, [Validators.required]),
            effectiveDate: new FormControl(entityAddressModel.effectiveDate),
            expirationDate: new FormControl(entityAddressModel.expirationDate),
            address: this.fb.group({
                streetAddress1: new FormControl(entityAddressModel.address[0].streetAddress1),
                streetAddress2: new FormControl(entityAddressModel.address[0].streetAddress2),
                zipCode: new FormControl(entityAddressModel.address[0].zipCode, [Validators.required, Validators.pattern(regex), CustomValidators.hasNoValue]),
                state: new FormControl({value: entityAddressModel.address[0].stateCode, disabled: true }, [Validators.required]),
                city: new FormControl(entityAddressModel.address[0].city, [Validators.required]),
                isGarageIndoor: new FormControl(entityAddressModel.address[0].isGarageIndoor),
                isGarageOutdoor: new FormControl(entityAddressModel.address[0].isGarageOutdoor),
                isGarageFenced: new FormControl(entityAddressModel.address[0].isGarageFenced),
                isGarageLighted: new FormControl(entityAddressModel.address[0].isGarageLighted),
                isGarageWithSecurityGuard: new FormControl(entityAddressModel.address[0].isGarageWithSecurityGuard)
            })
        });
    }

    addressTypeValue(id) {
        return this.addressTypes?.find(x => x.id === Number(id))?.name;
    }

    addTableItem(data: any): void {
        const tr: ITableTd[] = [];

        // Header
        const entityAddressFields: string[] = [
            'addressTypeId'
        ];
        entityAddressFields.forEach((item, index) => {
            // const display = Boolean(data[item]) ? String(data[item]) : '';
            const display = Boolean(data[item]) ? this.addressTypeValue(data[item]) : '';

            tr.push({
                id: index + 1,
                value: Boolean(data[item]) ? data[item] : '',
                display: display
            });
        });

        // Child
        if (data.address.length) {
            const addressFields: string[] = [
                'city',
                'state',
                'zipCode'
            ];

            const address = data.address[0];
            this.zipCodeService.getZipCode(String(address.zipCode)).pipe(takeUntil(this.stop$)).subscribe(result => {
                const cityZipCode: any = JSON.parse(result.toString());
                cityZipCode.forEach(add => {
                    if (add.city === address.city) {
                        addressFields.forEach((item, index) => {
                            switch (item) {
                                case 'city': {
                                    tr.push({
                                        id: index + 1,
                                        value: Boolean(add.city) ? address[add.city] : '',
                                        display: Boolean(add.city) ? String(add.city) : ''
                                    });
                                    break;
                                }
                                case 'state': {
                                    tr.push({
                                        id: index + 1,
                                        value: Boolean(add.stateFullName) ? add.stateFullName : '',
                                        display: Boolean(add.stateFullName) ? String(add.stateFullName) : ''
                                    });
                                    break;
                                }
                                case 'zipCode': {
                                    tr.push({
                                        id: index + 1,
                                        value: Boolean(add[item]) ? add[item] : '',
                                        display: Boolean(add[item]) ? String(add[item]) : ''
                                    });
                                    break;
                                }
                            }
                        });
                    }
                });
            }, error => {
                console.log(error);
                NotifUtils.showError(ErrorMessageConstant.zipCodeNotFoundContactUWErrorMessage);
            });
        }

        this.addressTableRows.push({
            id: (this.addressTableRows.length !== 0) ? this.addressTableRows.length + 1 : 1,
            tr: tr,
        });
    }
}