import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {CustomValidators} from '../../../../../shared/validators/custom.validator';
import Utils from '@app/shared/utilities/utils';
import {DashboardService} from '../../../../../modules/dashboard/services/dashboard.service';
import {takeUntil} from 'rxjs/operators';
import {BaseClass} from '@app/shared/base-class';
import { Router } from '@angular/router';
import NotifUtils from '@app/shared/utilities/notif-utils';

@Component({
  selector: 'app-process-endorsement-modal',
  templateUrl: './process-endorsement-modal.component.html',
  styleUrls: ['./process-endorsement-modal.component.scss']
})
export class ProcessEndorsementModalComponent extends BaseClass implements OnInit {
  title: any;
  private riskId: string = '';
  public processEndorsementFrm: FormGroup;

  constructor(public bsModalRef: BsModalRef, private fb: FormBuilder,
              private dashboardService: DashboardService,
              private router: Router,) { super(); }

  ngOnInit() {
    this.processEndorsementFrm = this.fb.group({
      policyNumber: ['', [Validators.required, CustomValidators.spaceValidator]],
    });
  }

  okMyModal(): void {
    const policyNumber = this.processEndorsementFrm.get('policyNumber').value;

    Utils.blockUI();
    this.dashboardService.getRiskByPolicyNumber(policyNumber).pipe(takeUntil(this.stop$)).subscribe({
      next: (response: any) => {
        this.router.navigate([`/policies/${response.riskId}/${response.latestRiskDetailId}/summary`]);
        Utils.unblockUI();
        },
      error: err => {
        Utils.unblockUI();
        NotifUtils.showError('Policy number not found');
      }
    });
    this.hideMyModal();
  }


  hideMyModal(): void {
    this.bsModalRef.hide();
  }
}
