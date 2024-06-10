import { Injectable } from '@angular/core';
import { FieldConfig } from '../../../../shared/models/dynamic/field.interface';
import {Validators} from '@angular/forms';

const CHECKBOX: string = "checkbox";
const DATE: string = "date";
const INPUT: string = "input";
const INPUT_TYPE_NUMBER: string = "number";
const INPUT_TYPE_TEXT: string = "text";
const SIZE_SMALL: string = "S";

@Injectable()
export class RiskSpecificsGeneralInfoFieldConfig {
    FieldConfigs = GENERAL_INFO_FIELDS;

    constructor() { }
}

export const GENERAL_INFO_FIELDS: FieldConfig[] = [
    // {
    //     type: DATE,
    //     label: 'Requested Quote Date',
    //     name: 'requestedQuoteDate',
    //     validations: [
    //         {
    //           name: 'required',
    //           validator: Validators.required,
    //           message: 'Requested Quote Date Required'
    //         }
    //     ]
    // },
    // {
    //     type: DATE,
    //     label: 'Effective Date',
    //     name: 'effectiveDate',        
    //     validations: [
    //         {
    //           name: 'required',
    //           validator: Validators.required,
    //           message: 'Effective Date Required'
    //         }
    //     ]
    // },    
    // {
    //     type: DATE,
    //     label: 'Expiration Date',
    //     name: 'expirationDate',        
    //     validations: [
    //         {
    //           name: 'required',
    //           validator: Validators.required,
    //           message: 'Expiration Date Required'
    //         }
    //     ]
    // },    
    {
        type: CHECKBOX,
        label: 'Is this a new venture?',
        name: 'isNewVenture',
        value: true,        
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Is this a new venture? Required'
            }
        ]
    },
    {
        type: INPUT,
        label: 'Hours of Service',
        name: 'hoursOfService',        
        placeholder: '0',      
        appendLabel: 'hr (s)',
        inputType: INPUT_TYPE_NUMBER,
        size: SIZE_SMALL,
        maxValue: 24
    },
    {
        type: INPUT,
        label: 'Days of Service',
        name: 'daysOfService',        
        placeholder: '0',    
        appendLabel: 'day (s)',
        inputType: INPUT_TYPE_NUMBER,
        size: SIZE_SMALL,
        maxValue: 7
    },
    {
        type: INPUT,
        label: 'How many shifts per day',
        name: 'shiftsPerDay',        
        placeholder: '0',
        inputType: INPUT_TYPE_NUMBER,
        size: SIZE_SMALL,
        maxValue: 100
    }
    // },
    // {
    //     type: INPUT,
    //     label: 'Radius of Operation',
    //     name: '',        
    //     placeholder: '0',
    //     prependLabel: '0 - 50',
    //     appendLabel: '%',
    //     inputType: INPUT_TYPE_NUMBER
    // },    
    // {
    //     type: INPUT,
    //     label: '',
    //     name: 'Radius of Operation',        
    //     placeholder: '0',
    //     prependLabel: '51 - 100',
    //     appendLabel: '%',
    //     inputType: INPUT_TYPE_NUMBER
    // },   
    // {
    //     type: INPUT,
    //     label: '',
    //     name: 'Radius of Operation',        
    //     placeholder: '0',
    //     prependLabel: '200+',
    //     appendLabel: '%',
    //     inputType: INPUT_TYPE_NUMBER
    // },
    // {
    //     type: CHECKBOX,
    //     label: 'Any operational exposure over a 300 mile radius?',
    //     name: 'operationalExposureOver300MileRadius'
    // },
    // {
    //     type: INPUT,
    //     label: 'Percentage of vehicles that are owner-operator',
    //     name: '',        
    //     placeholder: '0',
    //     appendLabel: '%',
    //     inputType: INPUT_TYPE_NUMBER
    // }
];
