import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { BindAndIssueLabelConstants } from '@app/shared/constants/bind-and-issue.labels.constants';
import { ErrorMessageConstant } from '@app/shared/constants/error-message.constants';

import NotifUtils from '@app/shared/utilities/notif-utils';
import Utils from '@app/shared/utilities/utils';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import * as moment from 'moment';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap, take, tap } from 'rxjs/operators';
import { PolicyCancellationService } from '../services/policy-cancellation.service';
import { PolicySummaryData } from './policy-summary.data';
import { Router } from '@angular/router';
import { PathConstants } from '../../../shared/constants/path.constants';
import { BindIssueService } from '../../submission/services/bind-issue.service';
import { PolicyBillingData } from './policy-billing.data';
import { BillingService } from '../../../../app/core/services/billing/billing.service';

@Injectable(
    { providedIn: 'root' }
)
export class PolicyCancellationNoticeData {

    cancellationNoticeForm: FormGroup;
    cancellationNoticeDateOptions: IAngularMyDpOptions;
    cancellationEffectiveDateOptions: IAngularMyDpOptions;
    cancellationNoticeReasons: any[] = [];

    constructor(
        private formBuilder: FormBuilder,
        private submissionData: SubmissionData,
        private policySummaryData: PolicySummaryData,
        private policyCancellationService: PolicyCancellationService,
        private bindIssueService: BindIssueService,
        private router: Router,
        public billingData: PolicyBillingData,
        public billingService: BillingService
    ) { }

    initiateFormFields() {
        // fetch cancellation reasons
        this.loadCancellationNoticeReasonDropdown().subscribe();
            
        // init form group
        this.cancellationNoticeForm = this.formBuilder.group({
            noticeDate: [null, Validators.required],
            effectiveDate: [null, Validators.required],
            cancellationReasonType: ['', Validators.required]
        });

        // init datepicker set policy period as date range
        const policyEffectiveDate = moment(this.submissionData.currentServerDateTime).subtract(1, 'day').toDate();
        this.cancellationNoticeDateOptions = {
            dateRange: false,
            dateFormat: 'mm/dd/yyyy',
            disableUntil: { year: policyEffectiveDate.getFullYear(), month: policyEffectiveDate.getMonth() + 1, day: policyEffectiveDate.getDate() }//,
            //disableSince: { year: billingCurrentDueDate.getFullYear() , month: billingCurrentDueDate.getMonth() + 1, day: billingCurrentDueDate.getDate() },
        };

        this.onChangeNoticeDate(this.submissionData.currentServerDateTime);

        return this.cancellationNoticeForm;
    }

    onChangeNoticeDate(startDate) {
        if(startDate?.jsDate)
        {
            startDate = startDate.jsDate;
        }
        
        // set default date
        let defaultNoticeDate = startDate;
        let newCancellationDate = new Date(defaultNoticeDate);
        let defaultCancellationDate = newCancellationDate.setDate(newCancellationDate.getDate() + 29);

        this.billingService.getSummary(this.submissionData.riskDetail.riskId).subscribe(result => {
            
            let billingCurrentDueDate = new Date(result.currentDueDate);
            
        const policyEffectiveDate = moment(this.submissionData.currentServerDateTime).subtract(1, 'day').toDate();

        this.cancellationNoticeDateOptions = {
            dateRange: false,
            dateFormat: 'mm/dd/yyyy',
            disableUntil: { year: policyEffectiveDate.getFullYear(), month: policyEffectiveDate.getMonth() + 1, day: policyEffectiveDate.getDate() },
            disableSince: { year: billingCurrentDueDate.getFullYear() , month: billingCurrentDueDate.getMonth() + 1, day: billingCurrentDueDate.getDate() + 2 },
        };
        });

        this.cancellationEffectiveDateOptions = {
            dateRange: false,
            dateFormat: 'mm/dd/yyyy',
            disableUntil: { year: defaultNoticeDate.getFullYear(), month: defaultNoticeDate.getMonth() + 1, day: defaultNoticeDate.getDate() }//,
            //disableSince: { year: billingCurrentDueDate.getFullYear() , month: billingCurrentDueDate.getMonth() + 1, day: billingCurrentDueDate.getDate() },
        };
        this.cancellationNoticeForm.get('noticeDate').setValue({ isRange: false, singleDate: { jsDate: new Date(defaultNoticeDate) } });
        this.cancellationNoticeForm.get('effectiveDate').setValue({ isRange: false, singleDate: { jsDate: new Date(defaultCancellationDate)  } });

    }

    loadCancellationNoticeReasonDropdown(): Observable<any> {
        return this.policyCancellationService.getCancellationReasons().pipe(
            tap((data) => {
                this.cancellationNoticeReasons = data.filter(c => c.id === 1 || c.id === 3 || c.id === 6 || c.id === 7);
            }),
            catchError((error) => {
                console.error(error);
                return throwError(error);
            })
        );
    }

    ManualCancellationNotice$(): Observable<any> {
        const cancellationNoticeDetails = {
            riskId: this.submissionData.riskDetail.riskId,
            riskDetailId: this.submissionData.riskDetail.id,
            noticeDate: this.cancellationNoticeForm.get('noticeDate').value?.singleDate?.jsDate?.toLocaleDateString(),
            effectiveDate: this.cancellationNoticeForm.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString(),
            cancellationReasonType: this.cancellationNoticeForm.value.cancellationReasonType
        };

        // Utils.blockUI();
        return this.policyCancellationService.ManualCancellationNotice(cancellationNoticeDetails).pipe(
            take(1),
            switchMap(() => this.submissionData.getRiskByIdAsync(this.submissionData.riskDetail.id).pipe(take(1))),
            tap((submissionData) => {
                console.log('RiskDetail', submissionData);
                this.submissionData.riskDetail = submissionData;
                this.policySummaryData.pendingCancellationNoticeDate = submissionData?.pendingCancellationNoticeDate;
                this.policySummaryData.updatePolicyStatus();
            }),
            catchError((error) => {
                console.error(error);
                NotifUtils.showError(ErrorMessageConstant.cancelPolicyErrorMsg);
                return error;
            })
        );
    }

    RescindPolicy() {
        NotifUtils.showConfirmMessage(BindAndIssueLabelConstants.areYouSureYouWantToRescind, () => {
            Utils.blockUI();
            this.bindIssueService.RescindPolicy(this.submissionData.riskDetail.id).pipe(take(1)).subscribe(data => {
            this.submissionData.riskDetail.subStatus = 'Active';
            this.policySummaryData.pendingCancellationNoticeDate = null;
            this.policySummaryData.updatePolicyStatus();
            NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.rescindedSuccessfully, () => {
                this.router.navigateByUrl(`/policies/${this.submissionData.riskDetail.riskId}/${data.id}/${PathConstants.Policy.Summary}`);
            });
            }, (error) => {
            console.error(error);
            NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
            });
        });
    }
}
