import { Component, HostListener, OnInit } from '@angular/core';
import { PathConstants } from '../../../../../../shared/constants/path.constants';
import { SummaryData } from '../../../../../../modules/submission/data/summary.data';
import { SubmissionData } from '../../../../../../modules/submission/data/submission.data';
import { Router } from '@angular/router';
@Component({
  selector: 'app-submission-summary',
  templateUrl: './submission-summary.component.html',
  styleUrls: ['./submission-summary.component.scss']
})
export class SubmissionSummaryComponent implements OnInit {
  isOpen: boolean = true;
  isShow: boolean = false;
  isCalcPremium: boolean = false;

  public pathConstants = PathConstants;
  constructor(
    public summaryData: SummaryData,
    public submissionData: SubmissionData,
    private router: Router,
  ) {
  }

  ngOnInit() {
    this.setBrokerCommissionsValue(null);
    this.summaryData.populateAssignedToList();
    this.summaryData.setVehicleTypeHeader();
    const risk = this.submissionData.riskDetail;
    if (risk?.id) {
      this.summaryData.form.patchValue({ status: risk.subStatus, assignedToId: risk.assignedToId });
      this.summaryData.quoteStatus = risk.subStatus;
      this.summaryData.updateEstimatedPremium();
    }
  }

  collapse() {
    this.isOpen = !this.isOpen;
  }

  @HostListener('window:scroll', ['$event'])
  onWindowScroll($event: any) {
    const scrollPosition = window.scrollY;
    if (scrollPosition >= 94) {
      this.isShow = true;
    } else {
      this.isShow = false;
    }
  }

  onStatusChanged() {
    if (this.submissionData.isIssued) { return; }
    const formValue = this.summaryData.form.value;
    this.summaryData.quoteStatus = formValue.status;
    this.summaryData.updateStatus(formValue.status, formValue.assignedToId);
  }

  onChangePolicyNumber(event): void {
    const selected = event?.target?.value;
    const policyObj = this.submissionData.riskDetail.riskRelated.find(x => x.policyNumber === selected);

    if (policyObj.isPolicy) {
      this.router.navigateByUrl(`/policies/${policyObj.id}/${policyObj.current}/summary`);
    } else {
      this.router.navigateByUrl(`/submissions/${policyObj.current}/applicant/name-and-address`);
    }
  }

  async setBrokerCommissionsValue(from: string | null) {
    try {
      await this.summaryData.checkForCommission(from);
    } catch (error) {
      console.error("Error in setBrokerCommissionsValue:", error);
    }
  }


  
}


