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
export class CoverageQuestionsFieldConfig {
  FieldConfigs = COVERAGE_QUESTIONS_FIELDS;

  constructor() { }
}

export const COVERAGE_QUESTIONS_FIELDS: FieldConfig[] = [
  {
    type: CHECKBOX,
    label: 'Continuous coverage for past 3 years?',
    name: 'continuesCoverage',
    value: true,
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
    label: 'What was the farthest destination traveled to in the past 12 months?',
    placeholder: '',
    inputType: 'text',
    name: 'farDestinationTravelled',
  },
  {
    type: RADIOBUTTON,
    label: 'Any destination in Mexico planned for the upcoming 12 months?',
    name: 'destMexico',
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
    type: CHECKBOX,
    label: 'Any NYC 5 boroughs exposures?',
    name: 'exposures',
    value: true,
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
        name: 'exposures',
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
