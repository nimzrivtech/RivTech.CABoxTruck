import { Component, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { UrlTree } from '@angular/router';
import { CanComponentDeactivate } from '@app/core/guards/candeactivate.guard';
import { PolicyService } from '@app/core/services/policy/policy.service';
import { PolicyPremiumDetailsDTO } from '@app/modules/policy/components/policy-details/sections/policy-rating/policy-premium-details-dto';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { QuoteLimitsData } from '@app/modules/submission/data/quote/quote-limits.data';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { RiskCoveragePremiumDTO } from '@app/shared/models/submission/limits/riskCoveragePremiumDto';
import NotifUtils from '@app/shared/utilities/notif-utils';
import Utils from '@app/shared/utilities/utils';
import { Observable, of, Subject } from 'rxjs';
import { catchError, map, finalize, tap, switchMap, takeUntil } from 'rxjs/operators';
import { PolicyRatingFactorsDTO } from '../policy-rating-factors-dto';

@Component({
  selector: 'app-policy-rating-factors',
  templateUrl: './policy-rating-factors.component.html',
  styleUrls: ['./policy-rating-factors.component.scss']
})
export class PolicyRatingFactorsComponent implements OnInit, OnDestroy {

  @ViewChild('btnShowLoadingUploader') btnShowLoadingUploader;
  @Output() public onUploadFinished = new EventEmitter();
  private readonly destroy$ = new Subject();

  hideMe: boolean = false;
  hasExperienceRater: boolean = false;
  currencySettings: any = {
    align: 'left',
    allowNegative: false,
    precision: 2,
    nullable: true,
  };

  public premiumDetails: PolicyPremiumDetailsDTO;
  public fgFactors: FormGroup;

  get riskCoveragePremiums(): RiskCoveragePremiumDTO {
    return this.submissionData.riskDetail.riskCoverage.riskCoveragePremium;
  }

  constructor(public quoteLimitsData: QuoteLimitsData,
    private submissionData: SubmissionData,
    private formBuilder: FormBuilder,
    public raterApiData: RaterApiData,
    private policyService: PolicyService,
    public policySummaryData: PolicySummaryData) {
  }

  get riskDetail() { return this.submissionData.riskDetail; }
  get riskId() { return this.riskDetail.riskId; }

  ngOnInit() {
    console.log(`Loading rating factor data...`);
    try {
      Utils.blockUI();
      this.autoUpdateRatingFactors();
      this.loadData$().subscribe(() => Utils.unblockUI());
    } catch (e) {
      console.error(`Error initializing rating factors component.\n${e.message}`);
      Utils.unblockUI();
    }

    this.raterApiData.hasExperienceRater().subscribe(hasExpRater => {
      this.hasExperienceRater = hasExpRater;
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadData$() {
    return this.policyService.getPolicyPremiumDetails$(this.riskId).pipe(
      tap((data) => this.premiumDetails = data),
      switchMap(() => this.raterApiData.loadRatingFactors$(this.riskDetail.id)),
    );
  }

  autoUpdateRatingFactors() {
    this.raterApiData.ratingFactors.pipe(
      takeUntil(this.destroy$)
    ).subscribe(data => {
      this.fgFactors = this.formBuilder.group({
        alScheduleRF: [data?.alScheduleRF],
        pdScheduleRF: [data?.pdScheduleRF],
        cargoScheduleRF: [data?.cargoScheduleRF],
        glScheduleRF: [data?.glScheduleRF]
      });
    }, err => console.error(err));
  }

  saveFactors(): Observable<any> {
    let values = this.fgFactors.getRawValue();
    return this.raterApiData.updateRatingFactors(values);
  }

  clickDownload(obj) {
    obj.preventDefault();
    obj.stopPropagation();

    obj.target.disabled = true;

    this.raterApiData.btnShowLoadingUploader = this.btnShowLoadingUploader;
    this.raterApiData.onUploadFinished = this.onUploadFinished;
    this.raterApiData.DownloadPremiumRater(obj);
  }

  clickDownloadExp(obj) {
    obj.preventDefault();
    obj.stopPropagation();

    obj.target.disabled = true;

    this.raterApiData.btnShowLoadingUploader = this.btnShowLoadingUploader;
    this.raterApiData.onUploadFinished = this.onUploadFinished;
    this.raterApiData.DownloadExperienceRater(obj);
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

}
