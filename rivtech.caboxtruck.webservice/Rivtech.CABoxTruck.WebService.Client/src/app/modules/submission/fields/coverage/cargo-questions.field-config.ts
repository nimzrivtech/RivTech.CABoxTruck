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
const MAX_VAL_YEARS: number = 100;

@Injectable()
export class CargoQuestionsFieldConfig {
  FieldConfigs = CARGO_QUESTIONS_FIELDS;

  constructor() { }
}

export const CARGO_QUESTIONS_FIELDS: FieldConfig[] = [
  {
    type: CHECKBOX,
    label: 'Are you contractually required to carry cargo insurance?',
    name: 'cargoInsurance',
    value: false,
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
  {
    type: CHECKBOX,
    label: 'Are commodities ever stored in truck overnight?',
    name: 'trucking',
    value: false,
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
    name: 'truckingExplain',
    parentField: {
        name: 'trucking',
        value: true
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
    type: CHECKBOX,
    label: 'Do you haul any hazardous materials?',
    name: 'hazardous',
    value: false,
  },
  {
    type: CHECKBOX,
    label: 'Do you have any refrigerated units?',
    name: 'refrigerated',
    value: false,
  },
  {
    type: CHECKBOX,
    label: 'Do you require refrigeration breakdown coverage?',
    name: 'refrigeratedBreakdown',
    value: false,
  },
  {
    type: CHECKBOX,
    label: 'Has applicant ever had a cargo loss?',
    name: 'cargoLoss',
    value: false,
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
    name: 'cargoLossExplain',
    parentField: {
        name: 'cargoLoss',
        value: true
    },
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      }
    ]
  },
];
