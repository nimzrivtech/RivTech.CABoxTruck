import { Injectable } from '@angular/core';
import { SubmissionData } from '../../../modules/submission/data/submission.data';
import { PolicyHistoryDTO } from '@app/shared/models/policy/PolicyHistoryDto';
import Utils from '@app/shared/utilities/utils';
import { finalize, take } from 'rxjs/operators';
import { PolicyHistoryService } from '../services/policy-history.service';
import { Observable } from 'rxjs';
import { map } from 'jquery';

@Injectable(
    { providedIn: 'root' }
  )
export class PolicyHistoryData {
    historyRecords: PolicyHistoryDTO[] = [];

    constructor(private policyHistoryService: PolicyHistoryService,
                private submissionData: SubmissionData) {}

    populatePolicyHistory() {
        this.historyRecords = [];
        const riskId = this.submissionData.riskDetail?.riskId;
        this.policyHistoryService.getEndorsementHistory(riskId)
            .pipe(take(1),
                finalize(() => Utils.unblockUI())
            ).subscribe(data => {
                this.historyRecords = data;
            });
    }

    getEndorsementHistory(): Observable<any> {
        const riskId = this.submissionData.riskDetail?.riskId;
        return this.policyHistoryService.getEndorsementHistory(riskId).pipe(take(1));
    }
}
