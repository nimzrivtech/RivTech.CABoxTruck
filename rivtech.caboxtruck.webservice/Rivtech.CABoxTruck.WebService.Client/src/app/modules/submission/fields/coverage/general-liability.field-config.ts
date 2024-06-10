import {Injectable} from '@angular/core';
import {FieldConfig} from '../../../../shared/models/dynamic/field.interface';
import {Validators} from '@angular/forms';

const CHECKBOX: string = "checkbox";
const RADIOBUTTON: string = "radiobutton";
const INPUT: string = "input";
const TEXTAREA: string = "textarea";
const YES: string = "YES";
const NO: string = "NO";
const NA: string = "N/A";
const SIZE_SMALL: string = "S";
const SIZE_MEDIUM: string = "M";
const MAX_VAL_LESS_MILLION: number = 999999999;
const MAX_VAL_LESS_HUNDRED: number = 99;

@Injectable()
export class GeneralLiabilityFieldConfig {
  FieldConfigs = GENERAL_LIABILITY_FIELDS;

  constructor() { }
}

export const GENERAL_LIABILITY_FIELDS: FieldConfig[] = [
  {
    type: RADIOBUTTON,
    label: 'Are you contractually required to carry General Liability insurance?',
    name: 'reqGlInsurance',
    value: NO,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ],
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: RADIOBUTTON,
    label: 'Do you have any operations other than trucking?',
    name: 'otherTrucking',
    value: NO,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ],
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: TEXTAREA,
    label: 'Explain ',
    placeholder: '',
    name: 'explain',
    parentField: {
        name: 'otherTrucking',
        value: YES
    },
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: RADIOBUTTON,
    label: 'Are any operations at a storage lot or impound yard?',
    name: 'storageImpound',
    value: NO,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ],
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: RADIOBUTTON,
    label: 'Any storage of goods?',
    name: 'storageGoods',
    value: NO,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ],
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: RADIOBUTTON,
    label: 'Any warehousing?',
    name: 'warehousing',
    value: NO,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ],
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: RADIOBUTTON,
    label: 'Any storage of vehicles for others?',
    name: 'storageVehicleOthers',
    value: NO,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ],
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: RADIOBUTTON,
    label: 'Any leasing space to others?',
    name: 'leasingSpace',
    value: NO,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ],
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: RADIOBUTTON,
    label: 'Any freight forwarding?',
    name: 'freightForwarding',
    value: NO,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ],
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: RADIOBUTTON,
    label: 'Any storage of fuels and/or chemicals?',
    name: 'fuelChemical',
    value: NO,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ],
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: RADIOBUTTON,
    label: 'Has applicant ever had a general liability loss?',
    name: 'generalLiabilityLoss',
    value: NO,
    options: [
      { label: YES, value: YES },
      { label: NO, value: NO },
      { label: NA, value: NA }
    ],
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: TEXTAREA,
    label: 'Explain ',
    placeholder: '',
    name: 'explain',
    parentField: {
        name: 'generalLiabilityLoss',
        value: YES
    },
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: INPUT,
    label: 'GL Annual Payroll',
    placeholder: '0',
    inputType: 'number',
    name: 'glAnnualPayroll',
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ],
    size: SIZE_MEDIUM,
    maxValue: MAX_VAL_LESS_MILLION
  },
  {
    type: INPUT,
    label: 'Number of Additional Insured?',
    placeholder: '0',
    inputType: 'number',
    name: 'additlInsured',
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ],
    size: SIZE_MEDIUM,
    maxValue: MAX_VAL_LESS_HUNDRED
  },
  {
    type: INPUT,
    label: 'How many require Waiver of Subrogation?',
    placeholder: '0',
    inputType: 'number',
    name: 'waiverSubrogation',
    size: SIZE_MEDIUM,
    maxValue: MAX_VAL_LESS_HUNDRED
  },
  {
    type: INPUT,
    label: 'How many require Primary Noncontributory Endorsement?',
    placeholder: '0',
    inputType: 'number',
    name: 'waiverSubrogation',
    size: SIZE_MEDIUM,
    maxValue: MAX_VAL_LESS_HUNDRED
  },
];
