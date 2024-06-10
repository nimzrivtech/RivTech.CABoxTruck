import { Injectable } from '@angular/core';
import { FieldConfig } from '../../../../shared/models/dynamic/field.interface';
import { Validators } from '@angular/forms';

const CHECKBOX: string = "checkbox";
const INPUT: string = "input";
const SELECT: string = "select";
const MULTISELECT: string = "multiselect";
const TEXTAREA: string = "textarea";
const INPUT_TYPE_TEXT: string = "text";
const RADIOBUTTON: string = "radiobutton";
const YES: string = "YES";
const NO: string = "NO";
const NA: string = "N/A";

@Injectable()
export class MaintenanceQuestionFieldConfig {
    FieldConfigs = MAINTENANCE_QUESTION_FIELDS;

    constructor() { }
}

export const MAINTENANCE_QUESTION_FIELDS: FieldConfig[] = [
    {
        type: SELECT,
        label: 'How often do you hold safety meetings?',
        name: 'safetyMeetings',
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ],
        options: [
          {label: 'Weekly', value: 'Weekly'},
          {label: 'Bi-Weekly', value: 'Bi-Weekly'},
          {label: 'Monthly', value: 'Monthly'},
          {label: 'Quarterly', value: 'Quarterly'},
          {label: 'Semi-Annually', value: 'Semi-Annually'},
          {label: 'Annually', value: 'Annually'},
          {label: 'Does Not Occur', value: 'Does Not Occur'},
        ]
    },
    {
        type: INPUT,
        label: 'Who is in charge of claims?',
        name: 'personInCharge',        
        placeholder: '',
        inputType: INPUT_TYPE_TEXT
    },
    {
        type: MULTISELECT,
        label: 'Garaging type',
        placeholder: "Please Select",
        name: 'garagingType',
        dropdownList: [
          {label: 'Indoor', value: 'Indoor'},
          {label: 'Outdoor', value: 'Outdoor'},
          {label: 'Fenced', value: 'Fenced'},
          {label: 'Lighted', value: 'Lighted'},
          {label: 'Security Guard', value: 'Security Guard'},
        ]
    },
    {
        type: TEXTAREA,
        label: 'Describe company safety programs',
        name: 'safetyProgramDesc',
        placeholder: ''
    },
    {
        type: TEXTAREA,
        label: 'Describe any safety award/incentive programs',
        name: 'incentinveAwardDesc',
        placeholder: ''
    },
    {
        type: TEXTAREA,
        label: 'Describe how and when drivers are evaluated',
        name: 'driverEvaluationDesc',
        placeholder: ''
    },
    {
        type: TEXTAREA,
        label: 'Describe Driver Disciplinary plan',
        name: 'driverDisciplinaryPlanDesc',
        placeholder: ''
    },
    {
        type: TEXTAREA,
        label: 'How is FMCSR compliance monitored?',
        name: 'FMSRcompliance',
        placeholder: ''
    },
    {
        type: MULTISELECT,
        label: 'Your Vehicle Maintenence Program includes',
        placeholder: "Please Select",
        name: 'vehicleMain',
        dropdownList: [
          {label: 'A service record for each vehicle', value: 'A service record for each vehicle'},
          {label: 'Controled and frequent inspections', value: 'Controled and frequent inspections'},
          {label: 'Vehicle daily condition reports', value: 'Vehicle daily condition reports'},
        ]
    },
    {
        type: RADIOBUTTON,
        label: 'Is your maintenance program managed by your company?',
        name: 'isMaintenanceProgramManagedByCompany',
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
        label: 'Do you provide complete maintenance on all vehicles?',
        name: 'provideCompleteMaintenanceOnVehicles',
        value:  YES,
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
        label: 'Are Driver Files available for review?',
        name: 'driverFilesAvailableForReview',
        value:  YES,
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
        label: 'Are Accident Files available for review?',
        name: 'accidentFilesAvailableForReview',
        value:  YES,
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
        label: 'Will all claims be reported directly to NARS?',
        name: 'allClaimsReportedDirectlyToNARS',
        value:  YES,
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ]
    },
    {
        type: RADIOBUTTON,
        label: 'Does road supervision include Recording Devices?',
        name: 'roadSupervisionIncludeRecordingDevices',
        value:  YES,
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ]
    },
    {
        type: RADIOBUTTON,
        label: 'Does road supervision include Radio Dispatch?',
        name: 'roadSupervisionIncludeRadioDispatch',
        value: YES,
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ]
    },
    {
        type: MULTISELECT,
        label: 'Select those which apply ',
        placeholder: 'Please Select',
        name: 'selectThoseWhichApply',
        dropdownList: [
          {label: 'Written maintenance program',value: 'Written maintenance program'},
          {label: 'Written driver-training program',value: 'Written driver-training program'},
          {label: 'Written safety program',value: 'Written safety program'},
          {label: 'Written accident reporting procedures',value: 'Written accident reporting procedures'},
        ],
        selectedItems: []
    } 
];
