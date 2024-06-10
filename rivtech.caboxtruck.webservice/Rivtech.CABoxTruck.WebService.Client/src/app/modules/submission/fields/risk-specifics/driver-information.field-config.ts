import { Injectable } from '@angular/core';
import { FieldConfig } from '../../../../shared/models/dynamic/field.interface';
import { Validators } from '@angular/forms';

const CHECKBOX: string = "checkbox";
const INPUT: string = "input";
const SELECT: string = "select";
const TEXTAREA: string = "textarea";
const RADIOBUTTON: string = "radiobutton";
const INPUT_TYPE_NUMBER: string = "number";
const SIZE_SMALL: string = "S";
const SIZE_MEDIUM: string = "M";
const YES: string = "YES";
const NO: string = "NO";
const NA: string = "N/A";
const MAX_VAL_PERCENTAGE: number = 100;
const MAX_VAL_MONETARY: number = 999999999;
const MAX_VAL_NONMONETARY: number = 999999;

@Injectable()
export class DriverInfoFieldConfig {
    FieldConfigs = DRIVER_INFO_FIELDS;

    constructor() { }
}

export const DRIVER_INFO_FIELDS: FieldConfig[] = [     
    {
        type: INPUT,
        label: 'Total number of drivers',
        name: 'totalDrivers',
        placeholder: '0',
        inputType: INPUT_TYPE_NUMBER,
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ],
        size: SIZE_SMALL,
        maxValue: MAX_VAL_NONMONETARY
    }, 
    {
        type: INPUT,
        label: 'Number of drivers over 70 y.o.',
        name: 'driverOver70',
        placeholder: '0',
        inputType: INPUT_TYPE_NUMBER,
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ],
        size: SIZE_SMALL,
        maxValue: MAX_VAL_NONMONETARY
    },  
    {
        type: INPUT,
        label: 'Number of drivers under 24 y.o.',
        name: 'driverUnder25',
        placeholder: '0',
        inputType: INPUT_TYPE_NUMBER,
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ],
        size: SIZE_SMALL,
        maxValue: MAX_VAL_NONMONETARY
    }, 
    {
        type: SELECT,
        label: 'Have you ever driven for or been associated with any transportation operation before?',
        name: 'transportationOperation',
        options: [
          {label: 'New Venture - No Driving Experience', value: 'New Venture - No Driving Experience'},
          {label: 'New Venture - Owned Another Company', value: 'New Venture - Owned Another Company'},
          {label: 'New Venture - Owner Currently Owns Another Company', value: 'New Venture - Owner Currently Owns Another Company'},
          {label: 'New Venture - Owner Was Driver For Another Company', value: 'New Venture - Owner Was Driver For Another Company'},
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
        type: CHECKBOX,
        label: 'Is the business principal a driver on the policy?',
        name: 'isBusinessPrincipalDriverOnPolicy',
        value: true,
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
        label: 'Will vehicles be operated for personal use?',
        name: 'vehicleOperatedForPersonalUse',
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
        label: 'Are any volunteers used in your business?',
        name: 'volunteersUsedInBusiness',
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
        type: INPUT,
        label: 'Percentage of staff',
        name: 'percentageOfStaff',
        placeholder: '0',
        appendLabel: '%',
        inputType: INPUT_TYPE_NUMBER,
        size: SIZE_SMALL,
        maxValue: MAX_VAL_PERCENTAGE,
        parentField: {
            name: 'volunteersUsedInBusiness',
            value: YES
        }
    },
    {
        type: RADIOBUTTON,
        label: 'Do you hire from others for your use?',
        name: 'hireFromOthersForUse',
        value: NO,
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ],
    },  
    {
        type: INPUT,
        label: 'Annual cost of hire',
        name: 'annualCostOfHire',
        placeholder: '0',
        prependLabel: '$',
        inputType: INPUT_TYPE_NUMBER,
        size: SIZE_MEDIUM,
        maxValue: MAX_VAL_MONETARY,
        parentField: {
            name: 'hireFromOthersForUse',
            value: YES
        }
    }, 
    {
        type: RADIOBUTTON,
        label: 'Do you hire from others with a driver?',
        name: 'hireFromOthersWithDriver',
        value: NO,
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ]
    },  
    {
        type: INPUT,
        label: 'Annual cost of hire',
        name: 'annualCostOfHire  ',
        placeholder: '0',
        prependLabel: '$',
        inputType: INPUT_TYPE_NUMBER,
        size: SIZE_MEDIUM,
        maxValue: MAX_VAL_MONETARY,
        parentField: {
            name: 'hireFromOthersWithDriver',
            value: YES
        }
    }, 
    {
        type: RADIOBUTTON,
        label: 'Do you lease to others for their use?',
        name: 'leaseToOthersForUse',
        value: NO,
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ],
    }, 
    {
        type: INPUT,
        label: 'Annual Income Derived',
        name: 'annualIncomeDerivedUse',
        placeholder: '0',
        prependLabel: '$',
        inputType: INPUT_TYPE_NUMBER,
        size: SIZE_MEDIUM,
        maxValue: MAX_VAL_MONETARY,
        parentField: {
            name: 'leaseToOthersForUse',
            value: YES
        }
    },  
    {
        type: RADIOBUTTON,
        label: 'Do you lease to others without a driver?',
        name: 'leaseToOthersWithoutDriver',
        value: NO,
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ],
    }, 
    {
        type: INPUT,
        label: 'Annual Income Derived',
        name: 'annualIncomeDerivedLease',
        placeholder: '0',
        prependLabel: '$',
        inputType: INPUT_TYPE_NUMBER,
        size: SIZE_MEDIUM,
        maxValue: MAX_VAL_MONETARY,
        parentField: {
            name: 'leaseToOthersWithoutDriver',
            value: YES
        }
    },  
    {
        type: RADIOBUTTON,
        label: 'Is there assumed liability by contract/agreement?',
        name: 'assumedLiabilityByContractAgreement',
        value: NO,
        options: [
          { label: YES, value: YES },
          { label: NO, value: NO },
          { label: NA, value: NA }
        ],
    },   
    {
        type: RADIOBUTTON,
        label: 'Do drivers take vehicles home?',
        name: 'driversTakeVehiclesHome',
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
        label: 'Are the vehicles solely owned by the applicant?',
        name: 'vehiclesOwnedByApplicant',
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
        label: 'Will you be adding or deleting vehicles during the policy term?',
        name: 'addOrDeleteVehiclesDuringPolicyTerm',
        value: null, //none
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
        type: TEXTAREA,
        label: 'Describe',
        name: 'describe',
        placeholder: '',
        parentField: {
            name: 'addOrDeleteVehiclesDuringPolicyTerm',
            value: YES
        }
    },
    {
        type: INPUT,
        label: 'In the past year, how many drivers were hired?',        
        name: 'pastYearDriverHired',
        placeholder: '0',
        inputType: INPUT_TYPE_NUMBER,
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ],
        size: SIZE_SMALL,
        maxValue: MAX_VAL_NONMONETARY
    },    
    {
        type: INPUT,
        label: 'In the past year, how many drivers were terminated?',     
        name: 'pastYearDriverTerminated',
        placeholder: '0',
        inputType: INPUT_TYPE_NUMBER,
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ],
        size: SIZE_SMALL,
        maxValue: MAX_VAL_NONMONETARY
    }, 
    {
        type: SELECT,
        label: 'How often are current MVRs pulled?',
        name: 'currentMVRspulled',
        validations: [
            {
              name: 'required',
              validator: Validators.required,
              message: 'Required'
            }
        ],
        options: [
          {label: 'Quarterly', value: 'Quarterly'},
          {label: 'Semi-Annually', value: 'Semi-Annually'},
          {label: 'Annually', value: 'Annually'},
          {label: 'Bi-Annually', value: 'Bi-Annually'},
          {label: 'Not Pulled', value: 'Not Pulled'},
        ]
    },
    {
        type: RADIOBUTTON,
        label: 'Are drivers considered independent contractors?',
        name: 'driversConsideredIndependentContractors',
        value: null, //none
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
    }
];
