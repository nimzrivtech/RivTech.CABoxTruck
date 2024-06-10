import { Injectable } from '@angular/core';
import { FieldConfig } from '../../../../shared/models/dynamic/field.interface';
import { Validators } from '@angular/forms';

const CHECKBOX: string = "checkbox";
const RADIOBUTTON: string = "radiobutton";
const YES: string = "YES";
const NO: string = "NO";
const NA: string = "N/A";
const SELECT: string = "select";
const MULTISELECT: string = "multiselect";

@Injectable()
export class DriverHiringCriteriaFieldConfig {
    FieldConfigs = DRIVER_HIRING_CRITERIA_FIELDS;

    constructor() { }
}

export const DRIVER_HIRING_CRITERIA_FIELDS: FieldConfig[] = [
    {
        type: RADIOBUTTON,
        label: 'Have all drivers been driving a similar vehicle commercially for 2+ years?',
        name: 'driversDriveSimilarVehicleCommerciallyForMoreThan2Years',
        value: '',//none
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ],
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ]
    },
    {
        type: RADIOBUTTON,
        label: 'Do all drivers have at least 5 years U.S. driving experience?',
        name: 'driversHave5YearsDrivingExperience',
        value: '',//none
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ],
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ]
    },
    {
        type: RADIOBUTTON,
        label: 'Do you agree to report all drivers to Rivington?',
        name: 'agreeToReportAllDriversToRivington',
        value: YES,
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ],
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ]
    },
    {
        type: RADIOBUTTON,
        label: 'Are any family members under 21 primary drivers of a company auto?',
        name: 'familyMembersUnder21PrimaryDriversOfCompanyAuto',
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
              message: 'Required'
            }
        ]
    },
    {
        type: RADIOBUTTON,
        label: 'Are all drivers properly licensed and DOT Compliant?',
        name: 'driversProperlyLicensedDOTcompliant',
        value: YES,
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ],
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ]
    },
    {
        type: RADIOBUTTON,
        label: 'Is disciplinary plan documented for all drivers?',
        name: 'isDisciplinaryPlanDocumentedDrivers',
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
              message: 'Required'
            }
        ]
    },
    {
        type: RADIOBUTTON,
        label: 'Is there a driver incentive program?',
        name: 'isThereDriverIncentiveProgram',
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
              message: 'Required'
            }
        ]
    },
    {
        type: MULTISELECT,
        label: 'Background check includes ',
        placeholder: "Please Select",
        name: 'backgroundCheckIncludes',
        dropdownList: [
          {label: 'Written Application:', value: 'Written Application:'},
          {label: 'Road Test:', value: 'Road Test:'},
          {label: 'Written Test:', value: 'Written Test:'},
          {label: 'Full Medical:', value: 'Full Medical:'},
          {label: 'Drug Testing:', value: 'Drug Testing:'},
          {label: 'Current MVR:', value: 'Current MVR:'},
          {label: 'Reference Checks:', value: 'Reference Checks:'},
          {label: 'Criminal Background Check:', value: 'Criminal Background Check:'},
        ]
    },
    {
        type: SELECT,
        label: 'Driver training includes ',
        name: 'driverTrainingIncludes',
        options: [
          {label: 'Company rules and policies', value: 'Company rules and policies'},
          {label: 'Vehicle Inspection Procedures', value: 'Vehicle Inspection Procedures'},
          {label: 'Equipment Familiarization', value: 'Equipment Familiarization'},
          {label: 'Route familiarization', value: 'Route familiarization'},
          {label: 'Emergency procedures', value: 'Emergency procedures'},
          {label: 'Accident reporting procedures', value: 'Accident reporting procedures'},
          {label: 'Observation Period', value: 'Observation Period'},
        ]
    }
];
