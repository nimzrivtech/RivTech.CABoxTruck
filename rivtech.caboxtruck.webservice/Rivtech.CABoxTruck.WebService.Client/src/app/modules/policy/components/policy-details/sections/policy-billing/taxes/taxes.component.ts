import { Component, OnInit } from '@angular/core';
import { PolicyBillingLabelsConstants } from '../../../../../../../shared/constants/policy-billing.labels.constants';
import { BaseClass } from '../../../../../../../shared/base-class';
import { PolicyBillingData } from '../../../../../data/policy-billing.data';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { DatePipe } from '@angular/common';
import { SubmissionData } from '../../../../../../submission/data/submission.data';

@Component({
  selector: 'app-taxes',
  templateUrl: './taxes.component.html',
  styleUrls: ['./taxes.component.scss']
})
export class TaxesComponent extends BaseClass implements OnInit {
  isOpen: boolean = true;

  public taxesConstants = PolicyBillingLabelsConstants.taxes;

  feeModalRef: BsModalRef | null;
  confirmationModalRef: BsModalRef | null;

  datePipe: DatePipe;

  constructor(public billingData: PolicyBillingData,
    public submissionData: SubmissionData,) {
    super();
  }

  ngOnInit() {
    this.datePipe = new DatePipe('en-US');
    
    if (this.submissionData.riskDetail.riskId) {
      this.billingData.listTransactionTaxesByRiskId(this.submissionData.riskDetail.riskId);
    }
  }

  collapse(): void {
    this.isOpen = !this.isOpen;
  }
}
