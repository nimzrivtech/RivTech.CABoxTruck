import { Component, Input, OnInit } from '@angular/core';
import { PolicyCancellationData } from '@app/modules/policy/data/policy-cancellation.data';
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
  selector: 'app-policy-cancellation',
  templateUrl: './policy-cancellation.component.html',
  styleUrls: ['./policy-cancellation.component.scss']
})
export class PolicyCancellationComponent implements OnInit {

  @Input() title: string;
  @Input() lastIssuedEffectiveDate: Date;

  constructor(
    public summaryData: PolicySummaryData,
    public policyCancellationData: PolicyCancellationData,
    public modalRef: BsModalRef,
    private submissionData: SubmissionData,
    private policySummaryData: PolicySummaryData,
    private router: Router
  ) { }

  ngOnInit() {
    this.policyCancellationData.lastIssuedEffectiveDate = this.lastIssuedEffectiveDate;
    this.policyCancellationData.initiateFormFields();
  }

  get fg() { return this.policyCancellationData.cancellationForm; }
  get fc() { return this.fg.controls; }
  get fn() { return 'cancellationForm'; }

  onSubmit() {
    Utils.blockUI();
    this.policyCancellationData.CancelPolicy$().pipe(
      catchError((err) => { 
        Utils.unblockUI();
        return err;
      })
    ).subscribe(data => {
      NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.cancelledSuccessfully, () => {
        this.router.navigateByUrl(`/policies/${this.submissionData.riskDetail.riskId}/${this.submissionData.riskDetail.id}/${PathConstants.Policy.Summary}`);
        this.modalRef.hide();
      });
    });
  }

  onClose() {
    this.modalRef.hide();
  }
}
