import { Injectable, Input } from '@angular/core';
import { FieldConfig } from '../../../../shared/models/dynamic/field.interface';
import { Placeholder } from '@angular/compiler/src/i18n/i18n_ast';
import { Validators } from '@angular/forms';

const CHECKBOX: string = "checkbox";
const INPUT: string = "input";
const TEXTAREA: string = "textarea";

@Injectable()
export class UnderwritingQuestionsFieldConfig {
    FieldConfigs = UW_QUESTIONS_FIELDS;

    constructor() { }
}

export const UW_QUESTIONS_FIELDS: FieldConfig[] = [
    {
        type: CHECKBOX,
        label: "Do you provide Worker's Compensation for all employees?",
        name: 'provideWorkersCompensationForAllEmployees',
        value: false, //none
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
        label: "Provide Worker's Compensation carrier",
        name: 'compensationCarrier',
        inputType: 'text',
        placeholder: '',        
        parentField: {
            name: 'provideWorkersCompensationForAllEmployees',
            value: true
        }
    },
    {
        type: TEXTAREA,
        label: "Please explain",
        name: 'compensationCarrierExplain',
        placeholder: '',     
        parentField: {
            name: 'provideWorkersCompensationForAllEmployees',
            value: false
        },
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
        label: "Is all equipment operated under the applicant's authority scheduled on the applicant's driver and vehicle schedule?",
        name: 'equipmentOperatedUnderApplicantsAuthorityScheduledOnApplicantsDriverVehicleSchedule',
        value: false, //none
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
        label: "Please explain",
        name: 'equipmentOperatedUnderApplicantsAuthorityScheduledOnApplicantsDriverVehicleScheduleExplain',
        placeholder: '',     
        parentField: {
            name: 'equipmentOperatedUnderApplicantsAuthorityScheduledOnApplicantsDriverVehicleSchedule',
            value: false
        },
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
        label: 'During the past 4 years, has your insurance ever been obtained through an Assigned Risk Plan?',
        name: 'inPast4yearsInsuranceObtainedAssignedRiskPlan',
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
        label: "Please explain",
        name: 'inPast4yearsInsuranceObtainedAssignedRiskPlanExplain',
        placeholder: '',     
        parentField: {
            name: 'inPast4yearsInsuranceObtainedAssignedRiskPlan',
            value: true
        }
    },
    {
        type: CHECKBOX,
        label: 'Has any company provided notice of cancellation/non-renewal or otherwise canceled/refused to renew your insurance, including during the current term?',
        name: 'companyProvidedNoticeOfCancellationNonrenewalOrCanceledRefusedRenew',
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
        label: "Please explain",
        name: 'companyProvidedNoticeOfCancellationNonrenewalOrCanceledRefusedRenewExplain',
        placeholder: '',     
        parentField: {
            name: 'companyProvidedNoticeOfCancellationNonrenewalOrCanceledRefusedRenew',
            value: true
        }
    },
    {
        type: CHECKBOX,
        label: 'Have you ever filed for or contemplated filing for bankruptcy or had bankruptcy proceedings initiated against you by another party?',
        name: 'filedOrContemplatedFilingBankruptcy',
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
        label: "Please explain",
        name: 'filedOrContemplatedFilingBankruptcyExplain',
        placeholder: '',     
        parentField: {
            name: 'filedOrContemplatedFilingBankruptcy',
            value: true
        }
    },
    {
        type: CHECKBOX,
        label: 'Has your operating authority ever been suspended or revoked or have you received notice of intent to suspend?',
        name: 'operatingAuthoritySuspendedRevoked',
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
        label: "Please explain",
        name: 'operatingAuthoritySuspendedRevokedExplain',
        placeholder: '',     
        parentField: {
            name: 'operatingAuthoritySuspendedRevoked',
            value: true
        }
    }
];
