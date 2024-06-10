import { AfterContentChecked, AfterViewInit, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SubmissionNavSavingService } from '../../../../../../../core/services/navigation/submission-nav-saving.service';
import { FormsData } from '../../../../../../../modules/submission/data/forms/forms.data';
import { QuoteLimitsData } from '../../../../../../../modules/submission/data/quote/quote-limits.data';
import { RaterApiData } from '../../../../../../../modules/submission/data/rater-api.data';
import { ReportData } from '../../../../../../../modules/submission/data/report/report.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { ErrorMessageConstant } from '../../../../../../../shared/constants/error-message.constants';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import Utils from '../../../../../../../shared/utilities/utils';
import { BsModalService } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { PremiumCalculateButtonComponent } from '../../../../../../rating/components/premium-calculate-button/premium-calculate-button.component';
import { PreloadingStatus } from '@app/modules/rating/models/preloading-status';
import { SummaryData } from '@app/modules/submission/data/summary.data';
import { BindingData } from '@app/modules/submission/data/binding/binding.data';

@Component({
  selector: 'app-submission-action-bar',
  templateUrl: './submission-action-bar.component.html',
  styleUrls: ['./submission-action-bar.component.scss']
})
export class SubmissionActionBarComponent implements OnInit, AfterViewInit {

  @ViewChild('calculateButton') btnCalculate: PremiumCalculateButtonComponent;
  @ViewChild('viewQuoteButton') btnViewQuote: ElementRef;

  @Output() clickType = new EventEmitter();
  @Input() showViewQuote: boolean = false;
  @Input() disableBack: boolean = false;
  @Input() disableNext: boolean = false;
  @Input() disableCalculate: boolean = false;
  @Input() disableViewQuote: boolean = false;
  @Input() disableIssue: boolean = false;
  @Input() showIssue: boolean = false;
  @Input() hideQuote: boolean = false;
  @Input() showQuoteProposalOnly: boolean = false;
  @Input() showGeneratePolicyNumber: boolean = false;

  private preloadStatus: PreloadingStatus = PreloadingStatus.InProgress;
  private raterModulesValid: boolean;

  constructor(
    private reportData: ReportData,
    private submissionService: SubmissionNavSavingService,
    private router: Router,
    private raterApiData: RaterApiData,
    private toastrService: ToastrService,
    public submissionData: SubmissionData,
    public quoteLimitsData: QuoteLimitsData,
    private modalService: BsModalService,
    private formsData: FormsData,
    private riskSpecificsData: RiskSpecificsData,
    private summaryData: SummaryData,
    private bindingData: BindingData
  ) {

  }



  get ratingOptions() {
    return this.submissionData.riskDetail.riskCoverages?.map(opt => opt.optionNumber);
  }

  get ratingTooltip() {
    if (!this.raterModulesValid)
      return this.raterApiData.showRequiredtoRateToolTip;
    else
      return this.raterApiData.showPremiumToolTipPreload;
  }


  ngOnInit() {
    setTimeout(() => {

      this.raterApiData.areModulesValidToRatePremium.subscribe((isValid) => {
        if (!this.btnCalculate) return;

        this.raterModulesValid = isValid;

        if (this.submissionData.riskDetail.submissionNumber != null) {
          this.btnCalculate.withValidModules(isValid);
          if (this.preloadStatus == PreloadingStatus.Complete) {
            this.btnViewQuote.nativeElement.disabled = this.btnCalculate.buttonsDisabled;
          }
        }
      });

      this.raterApiData.preloadingStatus.subscribe((status) => {
        if (!this.btnCalculate) return;
        this.preloadStatus = status;
        console.log(`${PreloadingStatus[this.preloadStatus]}`);
        if (this.preloadStatus === PreloadingStatus.InProgress) {
          this.btnCalculate.startPreloading();
        } else if (this.preloadStatus === PreloadingStatus.Complete) {
          this.btnCalculate.endPreloading();
        } else if (this.preloadStatus == PreloadingStatus.Failed) {
          this.btnCalculate.endPreloadingWithError(this.ratingTooltip);
        }
        this.btnViewQuote.nativeElement.disabled = this.btnCalculate.buttonsDisabled;
      });

      this.raterApiData.validateRequiredToRatePremiumModules();
    }, 100);

  }

  ngAfterViewInit(): void {
    /*
    setTimeout(() => {
      this.btnCalculate?.disableRating();
    }, 200);
    */

  }

  onClick(event?) {
    this.clickType.emit(event);
  }

  issuePolicy() {
    Utils.blockUI();
    const bindOptionId = Number(this.submissionData?.riskDetail?.binding?.bindOptionId) ?? 1;
    this.riskSpecificsData.resetGeneralLiabilityAndCargoIfDisabledInBindOption(bindOptionId);
    // recalculate premium before issuing policy
    this.raterApiData.calculateForQuote(bindOptionId, true)
      .then(() => {
        this.formsData.refreshQuoteOptionsAndSummaryHeader();
        const estimatedPremium = this.submissionData?.riskDetail?.riskCoverages[bindOptionId - 1]?.riskCoveragePremium?.premium;
        // should prevent issuance if premium is 0;
        //if (estimatedPremium < 1) { throw new Error('Estimated Premium is Invalid.'); }
        // issue policy
        this.submissionData.IssuePolicy();
      }).catch((error) => {
        console.error(error);
        NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
      }).finally(() => {
        setTimeout(() => {
          this.raterApiData.disableCalculatePremium = false;
        }, 6000);
      });
  }

  viewQuote() {
    Utils.blockUI();
    this.bindingData.retrieveDropdownValues(() => {
      this.bindingData.initializeForms();
      setTimeout(() => {
        this.bindingData.loadQuoteConditionDefaults();
        this.reportData.viewQuoteProposal();
      }, 1500);
    });
  }

  raterOptionClicked(option: number) {
    Utils.blockUI();
    this.submissionService.saveCurrentPage(this.router.url)
      .then(() => {
        var optionVehicles = this.submissionData.riskDetail.vehicles.filter(v => v.options?.split(",").includes(`${option}`) ?? false);
        if(optionVehicles.length > 0) {
          return Promise.resolve();
        }
        var error =`No vehicles checked for Option ${option}.`;
        this.toastrService.error(error);
        return Promise.reject(error);
      })
      .then((resultData) => {
        return this.raterApiData.preloadRater().toPromise()
          .then(() => Promise.resolve(resultData))
          .catch(() => Promise.reject("Failed to preload rater."));
      })
      .then(resultData => {
        console.log(resultData);
        Utils.blockUIWithMessage(`Rating Option ${option}...`);
        return this.raterApiData.calculateForQuote(option);
      })
      .then(resultData => {
        Utils.unblockUI();
        console.log(resultData);

        this.btnCalculate.completeRating();
        return Promise.resolve({ data: resultData });
      })
      .catch(error => {
        console.log(error);
        this.btnCalculate.failRating(error);
        Utils.unblockUI();
        return Promise.resolve({ error: error });
      })
      .finally(() => {
        console.log("Preloading rater after rating...");
        this.raterApiData.preloadRater().subscribe();
      });
  }

  generatePolicyNumber() {
    if (this.submissionData.riskDetail.policyNumber != null) { return; }
    this.submissionData.GeneratePolicyNumber(() => {
      this.summaryData.policyNumber = this.submissionData.riskDetail.policyNumber;
    });
  }

  get disablePolicyNumber() {
    if (this.submissionData.riskDetail.policyNumber) { return true; }
    return false;
  }
}
