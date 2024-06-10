import { Component, HostListener, OnInit } from '@angular/core';
import { SubmissionData } from '../../../../../modules/submission/data/submission.data';
import { PolicySummaryData } from '../../../../../modules/policy/data/policy-summary.data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { PolicyCancellationComponent } from './policy-header-modals/policy-cancellation/policy-cancellation.component';
import { PolicyHistoryData } from '@app/modules/policy/data/policy-history.data';
import { ToastrService } from 'ngx-toastr';
import { ErrorMessageConstant } from '../../../../../shared/constants/error-message.constants';
import { PolicyHistoryDTO } from '@app/shared/models/policy/PolicyHistoryDto';
import { PolicySummaryHeaderConstants } from '@app/shared/constants/policy-header.constants';
import { PolicyCancellationNoticeComponent } from './policy-header-modals/policy-cancellation-notice/policy-cancellation-notice.component';
import { PolicyCancellationNoticeData } from '@app/modules/policy/data/policy-cancellation-notice.data';
import { Router } from '@angular/router';

@Component({
  selector: 'app-policy-summary',
  templateUrl: './policy-summary.component.html',
  styleUrls: ['./policy-summary.component.scss']
})
export class PolicySummaryComponent implements OnInit {

  isOpen: boolean = true;
  isShow: boolean = false;
  isCalcPremium: boolean = false;
  modalRef: BsModalRef | null;
  public pageHeaderConstants = PolicySummaryHeaderConstants;

  shouldShowCommission: boolean = false;
  brokerCommisionAL: number;
  brokerCommisionPD: number;

  constructor(
    public summaryData: PolicySummaryData,
    public submissionData: SubmissionData,
    public modalService: BsModalService,
    public policyCancellationNoticeData: PolicyCancellationNoticeData,
    private toastr: ToastrService,
    private router: Router,


  ) {
  }

  get policyStatusText() {
    let status = this.summaryData.policyStatus;

    if ((this.summaryData.policyStatus == this.pageHeaderConstants.policyStatusCancelled)
      && (this.summaryData.policySubStatus == this.pageHeaderConstants.policySubStatusRewrite)) {
      status += ' / Rewrite';
    }

    return status;
  }

  get isPendingCancellationSubstatus() {
    const pendingCancellationNoticeDate = new Date(this.summaryData.pendingCancellationNoticeDate);
    return this.summaryData.policyStatus == this.pageHeaderConstants.policyStatusActive
      && this.summaryData.isPendingCancellationPolicy
      && pendingCancellationNoticeDate <= this.submissionData.currentServerDateTime;
  }

  ngOnInit() {
    this.setBrokerCommissionsValue(null);
    this.summaryData.setVehicleTypeHeader();
    const risk = this.submissionData.riskDetail;
    if (risk?.id) {
      // this.summaryData.form.patchValue({status: risk.status, assignedToId: risk.assignedToId});
      this.summaryData.updateEstimatedPremium(true);
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
    const formValue = this.summaryData.form.value;
    this.summaryData.quoteStatus = formValue.status;
    this.summaryData.updateStatus(formValue.status, formValue.assignedToId);
  }

  showManualCancellationModal(): void {
    const initialState = {
      title: 'Policy Cancellation',
      lastIssuedEffectiveDate: this.summaryData.lastIssuedEffectiveDate
    };

    this.modalRef = this.modalService.show(PolicyCancellationComponent, {
      initialState,
      class: 'modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true,
    });
  }

  showManualCancellationNoticeModal(): void {
    const initialState = {
      title: 'Cancellation Notice'
    };

    this.modalRef = this.modalService.show(PolicyCancellationNoticeComponent, {
      initialState,
      class: 'modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: true,
    });
  }

  reinstatePolicy() {
    this.submissionData.ReinstatePolicy();
    setTimeout(() => {
      this.summaryData.updatePolicyStatus();
    }, 1000);
  }

  rescindPolicy() {
    this.policyCancellationNoticeData.RescindPolicy();
  }

  rewritePolicy() {
    if (this.summaryData.disableRewriteAction) { return; }
    this.submissionData.RewritePolicy();
    // let endorsementCount = 0;
    // this.policyHistoryData.getEndorsementHistory().subscribe((data) => {
    //   endorsementCount = data.length;
    //   if (endorsementCount === 1) {
    //     this.submissionData.RewritePolicy();
    //   } else {
    //     this.toastr.error(ErrorMessageConstant.rewritePolicyNotAllowed, 'Failed!');
    //   }
    // });
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

        this.shouldShowCommission = this.summaryData.shouldShowCommission;
        this.brokerCommisionAL = Number(this.summaryData.brokerCommisionAL);
        this.brokerCommisionPD = Number(this.summaryData.brokerCommisionPD);

    } catch (error) {
        console.error("Error in setBrokerCommissionsValue:", error);
    }
}
}
