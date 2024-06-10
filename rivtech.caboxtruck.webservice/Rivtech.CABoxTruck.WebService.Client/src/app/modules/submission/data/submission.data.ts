import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { RiskDTO } from '../../../shared/models/risk/riskDto';
import { Observable, throwError } from 'rxjs';
import { SubmissionService } from '../services/submission.service';
import { ValidationListModel } from '../../../shared/models/validation-list.model';
import { BindIssueService } from '../services/bind-issue.service';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { catchError, take, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { PathConstants } from '../../../shared/constants/path.constants';
import { BindAndIssueLabelConstants } from '../../../shared/constants/bind-and-issue.labels.constants';
import { ErrorMessageConstant } from '../../../shared/constants/error-message.constants';
import { LvRiskStatus } from '@app/shared/constants/risk-status';
import Utils from '@app/shared/utilities/utils';
import { EndorsementService } from '../services/endorsement.service';
import { threadId } from 'worker_threads';
import { formatDate } from '@angular/common';

@Injectable(
  { providedIn: 'root' }
)
export class SubmissionData {

  readonly RATER_VERSION_SUPPORT = 7;
  readonly INTERNAL_UW_EFFECTIVE_DATE = new Date('2024-02-28'); //Add InternalUW Validation Effective Date Feb 28, 2024
  formInfo: FormGroup;
  riskDetail: RiskDTO;
  isNew: boolean = false;
  isQuoteIssued: boolean = false;
  isNewlyIssued: boolean = false;
  public validationList = new ValidationListModel();
  currentServerDateTime: Date;

  constructor(private formBuilder: FormBuilder,
    private submissionService: SubmissionService,
    private bindIssueService: BindIssueService,
    private endorsementService: EndorsementService,
    private router: Router
  ) { }

  initiateFormGroup(obj: RiskDTO): FormGroup {
    this.formInfo = this.formBuilder.group({
      id: [''],
      riskDetailId: [''],
      submissionNumber: [''],
      effectiveDate: [''],
      expirationDate: [''],
      insuredName: [''],
      broker: [''],
    });
    this.isQuoteIssued = false;
    return this.formInfo;
  }

  reset() {
    this.initiateFormGroup(new RiskDTO());
  }

  initiateFormValues(obj: RiskDTO) {
    this.formInfo.patchValue(obj);
  }

  getRiskByIdAsync(id: string) {
    return this.submissionService.getRiskByIdAsync(id);
  }

  getRiskDetailStatusByIdAsync(id: string) {
    return this.submissionService.getRiskDetailStatusByIdAsync(id);
  }

  getCurrentServerDateTime() {
    let currentServerDateTime = new Date();
    this.submissionService.getServerDateTime()
      .pipe(
        tap((data) => currentServerDateTime = data),
        catchError((error) => {
          console.error(error);
          return throwError(error);
        })
      );

    return currentServerDateTime;
  }

  getCurrentServerDateTime2() {
    return this.submissionService.getServerDateTime().pipe(take(1));
  }

  public update(obj: RiskDTO): Observable<RiskDTO> {
    return this.submissionService.put(obj);
  }

  public post(obj: RiskDTO): Observable<RiskDTO> {
    return this.submissionService.post(obj);
  }

  getCoverage(isVehicle: boolean) {
    let coverage = this.riskDetail?.riskCoverage;
    const riskCoverages = this.riskDetail?.riskCoverages;
    if (!coverage) {
      if (riskCoverages) {
        coverage = riskCoverages[0];
      }
    }

    if (isVehicle) return coverage;

    const optionId = this.riskDetail?.binding?.bindOptionId ?? '1';
    if (optionId != '1' && riskCoverages.length > 1) {
      coverage = riskCoverages[Number(optionId) - 1];
    }
    return coverage;
  }

  getCoverageCommision(from: string | null): Promise<{ brokerCommisionAL: number | null, brokerCommisionPD: number | null, shouldShowCommission: boolean }> {
    return new Promise((resolve, reject) => {
        try {
            let coverage = this.riskDetail?.riskCoverage;
            let ispolicy = this.riskDetail?.isPolicy;
            let shouldShowCommission = false;

            if (ispolicy == true) {
                shouldShowCommission = true;
            } else {
              if(this.riskDetail.isRaterVersionLocked)
                shouldShowCommission = true;
            }

            const result = {
                brokerCommisionAL: coverage?.brokerCommisionAL,
                brokerCommisionPD: coverage?.brokerCommisionPD,
                shouldShowCommission: shouldShowCommission
            };

            resolve(result);
        } catch (error) {
            reject(error);
        }
    });
}

setFromOptions(brokerCommisionAL: number,brokerCommisionPD:number){
  let coverage = this.riskDetail?.riskCoverage;
  coverage.brokerCommisionAL = Number(brokerCommisionAL);
  coverage.brokerCommisionPD = Number(brokerCommisionPD);
}

  IssuePolicy() {
    NotifUtils.showConfirmMessage(BindAndIssueLabelConstants.areYouSureYouWantToIssue, () => {
      Utils.blockUI();
      this.bindIssueService.Issue(this.riskDetail.id).pipe(take(1)).subscribe(data => {
        NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.issuedSuccessfully, () => {
          this.isNewlyIssued = true;
          this.router.navigateByUrl(`/policies/${this.riskDetail.riskId}/${data.id}/${PathConstants.Policy.Summary}`);
        });
      }, (error) => {
        console.error(error);
        NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
      });
    });
  }

  ReinstatePolicy() {
    NotifUtils.showConfirmMessage(BindAndIssueLabelConstants.areYouSureYouWantToReinstate, () => {
      Utils.blockUI();
      this.bindIssueService.ReinstatePolicy(this.riskDetail.id).pipe(take(1)).subscribe(data => {
        NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.reinstatedSuccessfully, () => {
          this.router.navigateByUrl(`/policies/${this.riskDetail.riskId}/${data.id}/${PathConstants.Policy.Summary}`);
        });
      }, (error) => {
        console.error(error);
        NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
      });
    });
  }

  RewritePolicy() {
    NotifUtils.showConfirmMessage(BindAndIssueLabelConstants.areYouSureYouWantToRewrite, () => {
      Utils.blockUI();
      this.bindIssueService.RewritePolicy(this.riskDetail.id).pipe(take(1)).subscribe(data => {
        NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.rewriteSuccessfully, () => {
          this.getRiskByIdAsync(this.riskDetail.id).pipe(take(1)).subscribe((submissionData) => {
            console.log('RiskDetail', submissionData);
            this.riskDetail = submissionData;
            this.router.navigateByUrl(`/submissions/${data.id}/${PathConstants.Submission.Applicant.Index}/${PathConstants.Submission.Applicant.NameAndAddress}`);
          });
        });
      }, (error) => {
        console.error(error);
        NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
      });
    });
  }

  CopySubmission() {
    NotifUtils.showConfirmMessage(BindAndIssueLabelConstants.areYouSureYouWantToCopySubmission, () => {
      Utils.blockUI();
      this.bindIssueService.CopySubmission(this.riskDetail.id).pipe(take(1)).subscribe(data => {
        NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.copySubmissionSuccessfully, () => {
          this.getRiskByIdAsync(data.id).pipe(take(1)).subscribe((submissionData) => {
            this.riskDetail = submissionData;
            window.location.href = `/submissions/${data.id}/${PathConstants.Submission.Applicant.Index}/${PathConstants.Submission.Applicant.NameAndAddress}`;
          });
        });
      }, (error) => {
        console.error(error);
        NotifUtils.showError(ErrorMessageConstant.copySubmissionErrorMsg);
      });
    });
  }

  GeneratePolicyNumber(callbackFn?: Function) {
    Utils.blockUI();
    this.endorsementService.GeneratePolicyNumber(this.riskDetail.id).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      this.riskDetail.policyNumber = data;
      if (callbackFn) { callbackFn(); }
    }, (error) => {
      Utils.unblockUI();
      console.error(error);
      NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
    });

    return this.riskDetail.policyNumber
  }

  get isIssued(): boolean {
    return this.riskDetail?.isPolicy;
  }

  get isCancelledPolicy() {
    return LvRiskStatus.find(x => x.description === this.riskDetail.status)?.description === 'Canceled';
  }

  get isPendingCancellationPolicy() {
    return LvRiskStatus.find(x => x.description === this.riskDetail.subStatus)?.description === 'Pending Cancellation';
  }
  

  getLatestVersion$(riskId: string) : Observable<string> {
    return this.submissionService.getLatestVersion$(riskId);
  }
  
  updatePolicyNumber(oldPolicyNumber) {
    if (oldPolicyNumber && this.riskDetail.riskRelated?.length > 1) {
      const fieldIndex = this.riskDetail.riskRelated?.findIndex(x => x.policyNumber === oldPolicyNumber);
      if (fieldIndex > -1) {
        this.riskDetail.riskRelated[fieldIndex].policyNumber = this.riskDetail.policyNumber;
      }
    }
  }

  raterVersionSupport(id: number = this.RATER_VERSION_SUPPORT): boolean {
    const raterVersionId = this.riskDetail?.raterVersionId ?? this.RATER_VERSION_SUPPORT;
    return raterVersionId == null || raterVersionId >= id || raterVersionId == 0;
  }

  getVehicleTypeCount() {
    if (this.riskDetail?.id !== null && this.riskDetail?.id !== undefined) {
      return this.getRiskByIdAsync(this.riskDetail?.id).pipe(take(1)).map((submissionData) => {
        this.riskDetail.CountPowerUnit = submissionData.countPowerUnit;
        this.riskDetail.CountTrailer = submissionData.countTrailer;
        this.riskDetail.CountOthers = submissionData.countOthers;
        return {
          CountPowerUnit: submissionData.countPowerUnit,
          CountTrailer: submissionData.countTrailer,
          CountOthers: submissionData.countOthers,
        };
      });

    }
    
  }

  isNewFeatureApplicableByQuoteDate(fieldName: string): boolean{
    const quoteDate = this.riskDetail?.requestedQuoteDate;
    if(quoteDate == null && !this.riskDetail.isRaterVersionLocked)
      return true;

    let fieldVersions = this.riskDetail?.fieldVersions?.find(v => v.fieldName == fieldName 
      && formatDate(quoteDate, 'yyyy-MM-dd','en_US') >= formatDate(v.effectiveDate, 'yyyy-MM-dd','en_US'));
    
    return fieldVersions != null;
  }
}
