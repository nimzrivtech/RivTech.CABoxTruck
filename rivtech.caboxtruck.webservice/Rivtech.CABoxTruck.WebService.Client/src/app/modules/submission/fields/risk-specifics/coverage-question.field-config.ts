import { Injectable } from '@angular/core';
import { FieldConfig } from '../../../../shared/models/dynamic/field.interface';
import { Validators } from '@angular/forms';

const CHECKBOX: string = "checkbox";
const TEXTAREA: string = "textarea";

@Injectable()
export class CoverageQuestionFieldConfig {
    FieldConfigs = COVERAGE_QUESTION_FIELDS;

    constructor() { }
}

export const COVERAGE_QUESTION_FIELDS: FieldConfig[] = [    
  {
    type: CHECKBOX,
    label: 'Are you contractually required to carry cargo insurance?',
    name: 'contractuallyRequiredToCarryCargoInsurance',
    value: false,
    validations: [
        {
          name: 'required',
          validator: Validators.required,
          message: 'Required'
        }
    ]
  },  
  {
    type: CHECKBOX,
    label: 'Are commodities ever stored in truck overnight?',
    name: 'commoditiesStoredInTruckOvernight',
    value: false,
    validations: [
        {
          name: 'required',
          validator: Validators.required,
          message: 'Required'
        }
    ]
  },     
  {
    type: TEXTAREA,
    label: 'Explain',
    name: 'explain',
    placeholder: '',
    parentField: {
        name: 'commoditiesStoredInTruckOvernight',
        value: true
    }
  },  
  {
    type: CHECKBOX,
    label: 'Do you haul any hazardous materials?',
    name: 'haulAnyHazardousMaterials',
    value: false, //none
  },  
  {
    type: CHECKBOX,
    label: 'Do you have any refrigerated units?',
    name: 'haveRefrigeratedUnits',
    value: false,
  },  
  {
    type: CHECKBOX,
    label: 'Do you require refrigeration breakdown coverage?',
    name: 'requireRefrigerationBreakdownCoverage',
    value: false,
  },  
  {
    type: CHECKBOX,
    label: 'Has applicant ever had a cargo loss?',
    name: 'applicantHadCargoLoss',
    value: false, //none
  },    
  {
    type: TEXTAREA,
    label: 'Explain',
    name: 'explain',
    placeholder: '',
    parentField: {
        name: 'applicantHadCargoLoss',
        value: true
    }
  }
];
