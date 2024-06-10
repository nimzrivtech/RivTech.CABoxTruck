import { Injectable } from '@angular/core';
import { PolicyContactsData } from '@app/modules/submission/data/broker/policy-contacts.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { BrokerGenInfoData } from '../../../modules/submission/data/broker/general-information.data';
import FormUtils from '../../../shared/utilities/form.utils';

@Injectable()
export class BrokerValidationService {
  brokerSection: boolean;
  brokerRequiredToBind: boolean;
  appliedContactRequiredToBind: boolean;

  get BusinessDetailsForm() {
    return this.brokerData.formInfo;
  }
  constructor(public brokerData: BrokerGenInfoData,
    public policyContactsData: PolicyContactsData,
    public submissionData: SubmissionData) { }

  checkBrokerPage() {
    this.brokerSection = this.checkBrokerSection();
    this.brokerRequiredToBind = this.checkBrokerPageBinding();
    this.appliedContactRequiredToBind = this.checkAppliedContactsSection();
  }

  checkBrokerSection() {
    // this.BusinessDetailsForm.controls['agencyId'].updateValueAndValidity();
    // this.BusinessDetailsForm.controls['agentId'].updateValueAndValidity();
    this.BusinessDetailsForm.markAllAsTouched();
    return FormUtils.validateFormControl(this.BusinessDetailsForm.controls['effectiveDate']);
  }

  checkBrokerPageBinding() {
    this.BusinessDetailsForm.controls['agencyId'].updateValueAndValidity();
    this.BusinessDetailsForm.controls['agentId'].updateValueAndValidity();
    this.BusinessDetailsForm.markAllAsTouched();
    return FormUtils.validateForm(this.BusinessDetailsForm); //&& this.checkPolicyContactsSection();
  }

  checkPolicyContactsSection() {
    let isValid: boolean = true;

    if (this.submissionData.isQuoteIssued || this.submissionData.isIssued) {
      isValid = this.submissionData.riskDetail?.riskPolicyContacts?.filter(x => x.typeId == 1)?.length > 0;

      const createdDate: Date = new Date(this.submissionData?.riskDetail?.binding?.createdDate);  

      if (createdDate >= this.submissionData.INTERNAL_UW_EFFECTIVE_DATE) {
       isValid = (this.submissionData.riskDetail?.brokerInfo?.internalUWUserId !== null);
      }
    }

    return isValid;
  }

  checkAppliedContactsSection() {
    this.BusinessDetailsForm.controls['internalUWUserId'].updateValueAndValidity();
    this.BusinessDetailsForm.controls['internalUWEmail'].updateValueAndValidity();
    this.BusinessDetailsForm.markAllAsTouched();
    return FormUtils.validateFormControl(this.BusinessDetailsForm.controls['internalUWUserId']) 
        && FormUtils.validateFormControl(this.BusinessDetailsForm.controls['internalUWEmail'])  
  }
}
