import { Component, Input, OnInit } from '@angular/core';
import { PolicyCancellationNoticeData } from '@app/modules/policy/data/policy-cancellation-notice.data';
import { BindAndIssueLabelConstants } from '@app/shared/constants/bind-and-issue.labels.constants';
import NotifUtils from '@app/shared/utilities/notif-utils';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { PolicySummaryData } from 'app/modules/policy/data/policy-summary.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { Router } from '@angular/router';
import { PathConstants } from '@app/shared/constants/path.constants';
import Utils from '@app/shared/utilities/utils';
import { catchError, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-policy-cancellation-notice',
  templateUrl: './policy-cancellation-notice.component.html',
  styleUrls: ['./policy-cancellation-notice.component.scss']
})
export class PolicyCancellationNoticeComponent implements OnInit {

  @Input() title: string;
  @Input() lastIssuedEffectiveDate: Date;

  constructor(
    public summaryData: PolicySummaryData,
    public policyCancellationNoticeData: PolicyCancellationNoticeData,
    public modalRef: BsModalRef,
    private submissionData: SubmissionData,
    private policySummaryData: PolicySummaryData,
    private router: Router
  ) { }

  ngOnInit() {
    this.policyCancellationNoticeData.initiateFormFields();
  }

  get fg() { return this.policyCancellationNoticeData.cancellationNoticeForm; }
  get fc() { return this.fg.controls; }
  get fn() { return 'cancellationNoticeForm'; }

  onSubmit() {
    Utils.blockUI();
    this.policyCancellationNoticeData.ManualCancellationNotice$().pipe(
      catchError((err) => { 
        Utils.unblockUI();
        return err;
      })
    ).subscribe(data => {
      NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.cancelledNoticeSuccessfully, () => {
        this.router.navigateByUrl(`/policies/${this.submissionData.riskDetail.riskId}/${this.submissionData.riskDetail.id}/${PathConstants.Policy.Summary}`);
        this.modalRef.hide();
      });
    });
  }

  onClose() {
    this.modalRef.hide();
  }
}
