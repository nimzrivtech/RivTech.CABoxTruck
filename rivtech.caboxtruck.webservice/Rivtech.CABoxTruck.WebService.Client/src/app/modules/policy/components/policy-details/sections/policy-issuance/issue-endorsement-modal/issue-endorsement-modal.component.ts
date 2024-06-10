import { DatePipe } from '@angular/common';
import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { EndorsementProcessMonitor, EndorsementProcessStatus } from '@app/modules/policy/data/endorsement-process-monitor';
import { EndorsementProcessStatusLog } from '@app/modules/policy/data/endorsement-process-status-log';
import { EndorsementProcessStatusLogRetriever } from '@app/modules/policy/data/endorsement-process-status-log-retriever';
import { PolicyIssuanceData } from '@app/modules/policy/data/policy-issuance.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { EndorsementService } from '@app/modules/submission/services/endorsement.service';
import Utils from '@app/shared/utilities/utils';
import { isThisISOWeek } from 'date-fns';
import { ToastrService } from 'ngx-toastr';
import { concat, Observable, of, scheduled, Subject, Subscription, throwError, timer } from 'rxjs';
import { catchError, finalize, map, switchMap, take, takeLast, takeUntil, tap } from 'rxjs/operators';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { Router } from '@angular/router';
import { PathConstants } from '@app/shared/constants/path.constants';

@Component({
  selector: 'app-issue-endorsement-modal',
  templateUrl: './issue-endorsement-modal.component.html',
  styleUrls: ['./issue-endorsement-modal.component.scss']
})
export class IssueEndorsementModalComponent implements OnInit, OnDestroy {

  @Output() close = new EventEmitter<any>(null);

  subs: Subscription[] = [];

  private readonly destroy$ = new Subject();
  logs: EndorsementProcessStatusLog[];
  isError: boolean = false;
  hasRun: boolean = false;
  isRunning: boolean;
  showMoreStatus: boolean = false;
  completed: boolean = false;

  get riskId() {
    return this.submissionData.riskDetail.riskId;
  }

  get riskDetail() {
    return this.submissionData.riskDetail;
  }

  constructor(
    private endorsementProcessMonitor: EndorsementProcessMonitor,
    private submissionData: SubmissionData,
    private policyIssuanceData: PolicyIssuanceData,
    private datePipe: DatePipe,
    private toastr: ToastrService,
    private endorsementService: EndorsementService,
    private router: Router,
    private policySummaryData: PolicySummaryData,
  ) { }

  ngOnInit() {
    this.clearLogs();
    this.policyIssuanceData.hasError = null;

    this.endorsementProcessMonitor.processStatus$.pipe(
      tap(status => this.onMonitorStatusUpdate(status)),
      takeUntil(this.destroy$),
    ).subscribe();

    return this.endorsementService.clearStatusLogs$(this.riskId).pipe(
      takeUntil(this.destroy$),
      switchMap(() => this.policyIssuanceData.processEndorsement$(this.riskDetail.riskId, "IssueEndorsement")),
      catchError((e) => {
        console.error(e);
        if (e.status != 400) {
          this.isError = true;
          this.toastr.error("An error occurred.");
        }
        return throwError(e);
      }),
    ).subscribe(() => {
      this.endorsementProcessMonitor.start(this.riskId);
    });
  }

  onMonitorStatusUpdate(status) {
    switch (status) {
      case EndorsementProcessStatus.RUNNING:
        this.isError = false;
        this.hasRun = true;
        this.retrieveLogs$().pipe(takeUntil(this.destroy$)).subscribe();
        break;
      case EndorsementProcessStatus.ERROR:
        this.hasRun = true;
        this.isError = true;
        this.onProcessingError();
        break;
      case EndorsementProcessStatus.NOT_RUNNING:
        if (this.hasRun == true && this.isError != true) {
          this.endorsementProcessMonitor.stop();
          this.loadProcessedData$().pipe(takeUntil(this.destroy$)).subscribe();
        }

        break;
    }
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
    this.endorsementProcessMonitor.stop();

    if (this.isError) {
      this.endorsementService.dequeue$(this.riskId).pipe(takeUntil(this.destroy$)).subscribe();
      this.endorsementService.clearStatusLogs$(this.riskId).pipe(takeUntil(this.destroy$)).subscribe();
    }

    this.logs = [];
    this.isError = false;
    this.completed = false;
    this.hasRun = false;
  }

  private retrieveLogs$() {
    return this.endorsementService.retrieveStatusLogs$(this.riskId).pipe(
      takeUntil(this.destroy$),
      tap(data => this.updateLogs(data))
    );
  }

  private onProcessingError() {
    this.endorsementProcessMonitor.stop();
    this.policyIssuanceData.hasError = true;

    timer(1000).pipe( // Wait 3secs 
      switchMap(() => this.retrieveLogs$()), // One-last log to get the error log.
      takeUntil(this.destroy$),
    ).subscribe();
  }

  private loadProcessedData$() {
    return this.endorsementService.clearStatusLogs$(this.riskId).pipe(
      takeUntil(this.destroy$),
      tap(() => {
        this.completed = true;
        this.toastr.success("Endorsement issued successfully!");
        this.close.emit(null);
        this.submissionData.getLatestVersion$(this.riskId).pipe(take(1)).subscribe(data => {
        this.policySummaryData.setEnableEditPolicy(false);
        this.policyIssuanceData.isNewlyIssued = true;
        this.policySummaryData.updateEstimatedPremium(true);
        this.policyIssuanceData.resetFormFields();
        this.router.navigateByUrl(`/policies/${this.submissionData.riskDetail.riskId}/${data}/${PathConstants.Policy.Summary}`);
    }, (error) => {
      console.error(error);
    });
      }),
      catchError((err) => {
        console.error(err);
        this.isError = true;
        var log = {
          Status: "Error occured while processing endorsement.",
          DateTime: new Date()
        };
        this.logs.unshift(log as EndorsementProcessStatusLog);
        return of(err);
      })
    );
  }

  private updateLogs(data: EndorsementProcessStatusLog[]) {
    this.logs = data.slice(0);
    this.logs.reverse();
  }

  onClose() {
    this.close.emit(null);
  }

  private clearLogs() {
    this.logs = [];
  }

  /**
   * Event binding of More/Less butotn.
   */
  private toggleShowMoreStatus() {
    this.showMoreStatus = !this.showMoreStatus;
  }

  isErrorMessage(message): boolean {
    var error = message?.substring(0, "ERROR:".length);
    return error == "ERROR:"
  }

  trimErrorPrefix(message) {
    return message.replace("ERROR: ", "");
  }
}
