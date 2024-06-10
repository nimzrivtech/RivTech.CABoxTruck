import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { RiskDTO } from '../../../shared/models/risk/riskDto';
import { RiskService } from '../../../core/services/risk/risk.service';
import { PolicyContactsService } from '../services/policy-contacts.service';
import { SelectItem } from '../../../shared/models/dynamic/select-item';
import { SubmissionData } from './submission.data';
import { AuthService } from '../../../core/services/auth.service';
import { take } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { TransactionSummaryDTO } from '../../../shared/models/policy/billing/transaction-summary.dto';
import { PolicyContactsDTO } from '@app/shared/models/submission/PolicyContactsDto';

@Injectable(
  { providedIn: 'root' }
)
export class SummaryData {
    name: string = 'Business Name';
    userFullName: string = '';
    address: string = 'Business Address';
    policyStatus: string = 'Quoted';
    formType: string = '-';
    quoteNumber: string = '-';
    effectiveDate: string = '02/03/2021';
    agency: string = 'JE Brown Associates';
    producer: string = '';
    code: string = 'TX12345678';
    agencyStatus: string = 'Active';
    coverageA: number = 200000;
    creditOrderStatus: string = 'Ordered-Score Received';
    estimatedPremium: number = 52;
    taxes: number = 1;
    fees: number = 27;
    quoteStatus: string = '';
    riskDetailId: string;
    entityId: string;
    parentRiskDetailId: string;
    endorsementRiskDetailId: string;
    policyNumber: string;

    pageType: string;

    form: FormGroup;
    statusList: SelectItem[] = [];
    assignedToList: SelectItem[] = [];

    powerUnitsCount: number;
    trailerCount: number;
    otherCount: number;
    
    shouldShowCommission:boolean = false;
    brokerCommisionAL: number;
    brokerCommisionPD: number;
    constructor(private fb: FormBuilder, private submissionData: SubmissionData,
      private riskService: RiskService, private authService: AuthService, private policyContactService: PolicyContactsService) {
    }

    initiateFormFields() {
    }

    reset() {
      this.name = 'Business Name';
      this.address = 'Business Address';
      this.policyStatus = 'Quoted';
      this.formType = '-';
      this.quoteNumber = '';
      this.effectiveDate = '';
      this.agency = '';
      this.producer = '';
      this.code = 'TX12345678';
      this.agencyStatus = 'Active';
      this.coverageA = 200000;
      this.creditOrderStatus = 'Ordered-Score Received';
      this.estimatedPremium = null;
      this.taxes = null;
      this.fees = null;
      this.quoteStatus = '';
      this.policyNumber = '';
      this.powerUnitsCount = 0;
      this.trailerCount = 0;
      this.otherCount = 0;
      this.initialize();
      this.setUserDetails();
    }

    initialize() {
      this.form = this.fb.group({
        status: [null],
        assignedToId: [null],
      });

      // TODO: Temp as data sets will come from generic or user mgmt services
      this.statusList = [
        {value: 1, label: 'Received'},
        {value: 2, label: 'In Research'},
        {value: 3, label: 'Incomplete'},
        {value: 4, label: 'Complete'},
        {value: 5, label: 'Withdrawn'},
        {value: 6, label: 'Quoted'},
        {value: 7, label: 'Declined'},
        {value: 8, label: 'Quote in Progress'},
        {value: 9, label: 'Not Accepted'},
      ];
    }

    updateStatus(status: string, assignedToId: number) {
      const riskDetailId = this.submissionData.riskDetail?.id;
      if (riskDetailId) {
        const risk = new RiskDTO();
        risk.id = riskDetailId;
        risk.riskId = this.submissionData.riskDetail?.riskId;
        risk.status = status;
        risk.assignedToId = assignedToId;

        if(this.submissionData.riskDetail?.isRenewal === true && status === 'Data Only' || this.submissionData.riskDetail?.isRenewal === true && status === 'Test Class')  risk.status = null;

        this.riskService.updateStatus(risk).subscribe(() => {
        });
      }
    }

    updateStatusForm(data) {
      const riskDetailId = this.submissionData.riskDetail?.id;
      if (riskDetailId) {
        this.form.controls['status'].patchValue(data);
        this.quoteStatus = data;
      }
    }

    updateSummaryStatus() {
      const riskDetailId = this.submissionData.riskDetail?.id;
      if (riskDetailId) {
        this.submissionData.getRiskDetailStatusByIdAsync(riskDetailId).pipe(take(1)).subscribe(data => {
          this.form.controls['status'].patchValue(data);
          this.quoteStatus = data;
        });
      }
    }

    updateEstimatedPremium() {
      const risk = this.submissionData.riskDetail;
      const bindOptionId = 1; // Number(risk?.binding?.bindOptionId) ?? 1
      const riskCoveragePremium = risk?.riskCoverages[bindOptionId - 1]?.riskCoveragePremium;
      this.estimatedPremium = riskCoveragePremium?.premium;
      this.taxes = (riskCoveragePremium?.taxesAndStampingFees ?? 0);
      this.fees = riskCoveragePremium?.riskMgrFeeAL + riskCoveragePremium?.riskMgrFeePD;
    }

    populateAssignedToList() {
      this.policyContactService.getAllAssignedToPolicyContacts().subscribe(data => {
        this.assignedToList = this.convertEnumerableListToSelectItemList(data);
      });
    }

    get userId(): number {
      const user = this.assignedToList.find(a => a.label.toLowerCase() === this.userFullName.toLowerCase());
      return user ? user.value : this.assignedToList.find(a => a.label === 'Jeff Russell')?.value;
    }

    private setUserDetails() {
      this.authService.getUserDetails().subscribe(u => {
        this.userFullName = `${u.firstName.trim()} ${u.lastName.trim()}`;
      });
    }

    private convertEnumerableListToSelectItemList(list: PolicyContactsDTO[]): SelectItem[] {
      let convertedList: SelectItem[] = [];

      if(list){
          for (const item of list) {
              convertedList.push({
                  label: item.name,
                  value: item.id
              });
          }
      }

      return convertedList;
  }

  setVehicleTypeHeader() {
    this.submissionData?.getVehicleTypeCount().subscribe((vehCount) => {
      // Access the properties of the returned object
      this.powerUnitsCount = vehCount.CountPowerUnit;
      this.trailerCount = vehCount.CountTrailer;
      this.otherCount = vehCount.CountOthers;
      
    });
  }


  async checkForCommission(from: string | null) {
    try {
        let coverage = await this.submissionData.getCoverageCommision(from);
        this.shouldShowCommission = coverage.shouldShowCommission;
        this.brokerCommisionAL = Number(coverage.brokerCommisionAL);
        this.brokerCommisionPD = Number(coverage.brokerCommisionPD);
    } catch (error) {
        console.error("Error in checkForCommission:", error);
        // Handle the error as appropriate
    }
}

get showHeaderCommission(): boolean {
  return this.shouldShowCommission;
}

setFromOptions(brokerCommisionAL: number,brokerCommisionPD:number){
  this.brokerCommisionAL = Number(brokerCommisionAL);
  this.brokerCommisionPD = Number(brokerCommisionPD);
}

}
