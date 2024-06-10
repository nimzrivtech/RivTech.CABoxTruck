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

@Injectable(
    { providedIn: 'root' }
)
export class PolicyCancellationData {

    cancellationForm: FormGroup;
    cancellationEffectiveDateOptions: IAngularMyDpOptions;
    cancellationReasons: any[] = [];
    lastIssuedEffectiveDate: Date;

    constructor(
        private formBuilder: FormBuilder,
        private submissionData: SubmissionData,
        private policySummaryData: PolicySummaryData,
        private policyCancellationService: PolicyCancellationService,
        private router: Router
    ) { }

    initiateFormFields() {
        // fetch cancellation reasons
        this.loadCancellationReasonDropdown().subscribe();

        // init datepicker set policy period as date range
        const lastIssuedEffectiveDate = this.lastIssuedEffectiveDate ?? this.submissionData.riskDetail?.brokerInfo?.effectiveDate;
        const policyEffectiveDate = moment(this.submissionData.riskDetail?.brokerInfo?.effectiveDate).subtract(1, 'day').toDate();
        const policyExpirationDate = moment(this.submissionData.riskDetail?.brokerInfo?.expirationDate).add(1, 'day').toDate();
        this.cancellationEffectiveDateOptions = {
            dateRange: false,
            dateFormat: 'mm/dd/yyyy',
            disableUntil: { year: policyEffectiveDate.getFullYear(), month: policyEffectiveDate.getMonth() + 1, day: policyEffectiveDate.getDate() }//,
            //disableSince: { year: policyExpirationDate.getFullYear() , month: policyExpirationDate.getMonth() + 1, day: policyExpirationDate.getDate() },
        };

        // init form group
        this.cancellationForm = this.formBuilder.group({
            effectiveDate: [null, Validators.required],
            cancellationReasonType: ['', Validators.required],
            isShortRate: [false] // always false
        });

        // set default date
        this.cancellationForm.get('effectiveDate').setValue({ isRange: false, singleDate: { jsDate: new Date(lastIssuedEffectiveDate) } });

        return this.cancellationForm;
    }

    loadCancellationReasonDropdown(): Observable<any> {
        return this.policyCancellationService.getCancellationReasons().pipe(
            tap((data) => {
                this.cancellationReasons = data;
            }),
            catchError((error) => {
                console.error(error);
                return throwError(error);
            })
        );
    }

    CancelPolicy$(): Observable<any> {
        const cancellationDetails = {
            riskDetailId: this.submissionData.riskDetail.id,
            effectiveDate: this.cancellationForm.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString(),
            cancellationReasonType: this.cancellationForm.value.cancellationReasonType,
            isShortRate: this.cancellationForm.value.isShortRate
        };

        // Utils.blockUI();
        return this.policyCancellationService.CancelPolicy(cancellationDetails).pipe(
            take(1),
            switchMap(() => this.submissionData.getRiskByIdAsync(this.submissionData.riskDetail.id).pipe(take(1))),
            tap((submissionData) => {
                console.log('RiskDetail', submissionData);
                this.submissionData.riskDetail = submissionData;
                this.policySummaryData.name = submissionData?.nameAndAddress?.businessName;
                this.policySummaryData.setEnableEditPolicy(false);
                this.policySummaryData.updateEstimatedPremium(true);
                this.policySummaryData.updatePolicyStatus();
            }),
            switchMap(() => this.policySummaryData.coverageSummary$()), // includes update of status)
            catchError((error) => {
                console.error(error);
                NotifUtils.showError(ErrorMessageConstant.cancelPolicyErrorMsg);
                return error;
            })
        );
    }
}
