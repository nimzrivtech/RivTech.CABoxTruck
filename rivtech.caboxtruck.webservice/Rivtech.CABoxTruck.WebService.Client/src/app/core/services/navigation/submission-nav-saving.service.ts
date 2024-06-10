import { Injectable, OnInit } from '@angular/core';
import { PathConstants } from '../../../shared/constants/path.constants';
import { BaseComponent } from '../../../shared/base-component';
import { ApplicantData } from '../../../modules/submission/data/applicant/applicant.data';
import { LimitsData } from '../../../modules/submission/data/limits/limits.data';
import { CoverageData } from '../../../modules/submission/data/coverages/coverages.data';
import { RiskSpecificsData } from '../../../modules/submission/data/risk-specifics.data';
import { UnderwritingQuestionsData } from '../../../modules/submission/data/riskspecifics/underwriting-questions.data';
import { DotInfoData } from '../../../modules/submission/data/riskspecifics/dot-info.data';
import { DriverInfoData } from '../../../modules/submission/data/riskspecifics/driver-info.data';
import { RadiusOfOperationsData } from '../../../modules/submission/data/riskspecifics/radius-of-operations.data';
import { BrokerGenInfoData } from '../../../modules/submission/data/broker/general-information.data';
import { SubmissionData } from '../../../modules/submission/data/submission.data';
import { QuoteLimitsData } from '../../../modules/submission/data/quote/quote-limits.data';
import { ClaimsHistoryoData } from '../../../modules/submission/data/claims/claims-history.data';
import { LayoutService } from '../layout/layout.service';
import { createSubmissionDetailsMenuItems } from '../../../modules/submission/components/submission-details/submission-details-navitems';
import { SubmissionNavValidateService } from './submission-nav-validate.service';
import { DriverData } from '../../../modules/submission/data/driver/driver.data';
import { FormsData } from '../../../modules/submission/data/forms/forms.data';
import { Subject } from 'rxjs';
import { BindingData } from '../../../modules/submission/data/binding/binding.data';
import { VehicleData } from '../../../modules/submission/data/vehicle/vehicle.data';
import Utils from '@app/shared/utilities/utils';
import { concat, concatMap, finalize, startWith, switchMap, tap } from 'rxjs/operators';
import { SummaryData } from '../../../modules/submission/data/summary.data';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import * as _ from 'lodash';
@Injectable()
export class SubmissionNavSavingService extends BaseComponent implements OnInit {

  isSaving: boolean = false;
  saveCategoryEndSource = new Subject<void>();
  public saveCategoryEnded = this.saveCategoryEndSource.asObservable();

  constructor(
    private submissionData: SubmissionData,
    private summaryData: SummaryData,
    private applicantData: ApplicantData,
    private limitsData: LimitsData,
    private coverageData: CoverageData,
    private riskSpecificData: RiskSpecificsData,
    private uwQuestionsData: UnderwritingQuestionsData,
    private dotInfoData: DotInfoData,
    private driverInfoData: DriverInfoData,
    private radiusOfOperationsData: RadiusOfOperationsData,
    private brokerGenInfoData: BrokerGenInfoData,
    private quoteLimitsData: QuoteLimitsData,
    private claimsHistoryData: ClaimsHistoryoData,
    private layoutService: LayoutService,
    private submissionNavValidationService: SubmissionNavValidateService,
    private vehicleData: VehicleData,
    private driverData: DriverData,
    private formsData: FormsData,
    private bindingData: BindingData,
    private router: Router,
    ///
  ) {
    super();
  }

  ngOnInit() {

  }

  public saveCurrentCategory(currentCategoryRoute: string) {
    if (this.submissionData?.isIssued) { return; }
    try {
      switch (currentCategoryRoute.toLowerCase()) {
        case PathConstants.Submission.Applicant.NameAndAddress:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          break;
        case PathConstants.Submission.Applicant.BusinessDetails:
          this.isSaving = true;
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.applicantData.saveBusinessDetails(() => {
            if (this.submissionData.riskDetail?.businessDetails?.yearsInBusiness > 0 && this.submissionData.riskDetail?.claimsHistory?.length > 0) {
              this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Claims.Index);
            }
            this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificData.hiddenNavItems));
            this.finishSaving();
            // this.summaryData.updateSummaryStatus();
          });
          break;
        case PathConstants.Submission.Applicant.FilingsInformation:
          this.applicantData.saveFilingsInformation();
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          break;
        case PathConstants.Submission.Applicant.AdditionalInterest:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          break;
        case PathConstants.Submission.Applicant.Manuscripts:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          break;
        case PathConstants.Submission.Limits.LimitsPage:
          this.isSaving = true;
          this.vehicleData.isVehicleUpdated = true;
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          if (this.submissionData?.riskDetail?.vehicles === null || this.submissionData?.riskDetail?.vehicles?.length < 1) {
            this.vehicleData.isVehicleUpdated = false;
          }
          let previousRiskCoverage = this.submissionData.riskDetail.riskCoverage;
          this.limitsData.saveRiskCoverageLimit()
            .pipe(
              concatMap(() => this.vehicleData.updateOtherCoverages(this.limitsData.oldLimitsData)),
              tap(() => {
                this.limitsData.hasRiskCoverage = true;
                this.riskSpecificData.resetGeneralLiabilityCargo();
              }),
              finalize(() => {
                // need to put callback in case error on saving. this is for rating validaiton and badge update.
                if (this.riskSpecificData.hiddenNavItems.length > 0) {
                  this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo);
                  this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
                  this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificData.hiddenNavItems));
                } else {
                  if (this.limitsData.hasRiskCoverage) {
                    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo);
                    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
                    this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificData.hiddenNavItems));
                  }
                }

                this.bindingData.retrieveDropdownValues(() => {
                  setTimeout(() => {
                    this.bindingData.loadBindReqDefaults(false);
                    if ((this.submissionData.riskDetail.quoteConditions ?? []).length < 1) {
                      this.bindingData.loadQuoteConditionDefaults();
                    } else {
                      this.bindingData.loadQuoteConditionDefaults(previousRiskCoverage);
                    }
                  }, 500);
                });
                this.finishSaving();
              })
            ).subscribe();
          break;
        case PathConstants.Submission.Driver.Index:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.driverData.saveDriverheader(() => {
            this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DriverInfo);
            this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
            this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificData.hiddenNavItems));
          });
          break;
        case PathConstants.Submission.Vehicle.Index:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.vehicleData.onExitVehicleScreen(() => {
            this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.MaintenanceSafety);
            this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
            this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificData.hiddenNavItems));
          });
          break;
        case PathConstants.Submission.Coverages.HistoricalCoverage:
          this.isSaving = true;
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.coverageData.saveRiskHistory(() => this.finishSaving());
          break;
        case PathConstants.Submission.RiskSpecifics.MaintenanceSafety:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.riskSpecificData.saveMaintenanceSafety();
          this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
          break;
        case PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.riskSpecificData.saveGeneralLiabilityCargo();
          this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
          break;
        case PathConstants.Submission.RiskSpecifics.UWQuestions:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.uwQuestionsData.saveUnderwritingQuestions();
          this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
          break;
        case PathConstants.Submission.RiskSpecifics.DotInfo:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.dotInfoData.saveDotInfo();
          this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
          break;
        case PathConstants.Submission.RiskSpecifics.DriverInfo:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.driverInfoData.saveDrivingInfo();
          this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
          break;
        case PathConstants.Submission.RiskSpecifics.DestinationInfo:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.radiusOfOperationsData.saveDestinationRadiusOperation();
          this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
          break;
        case PathConstants.Submission.Broker.Index:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          break;
        case PathConstants.Submission.Quote.Index:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.isSaving = true;
          this.quoteLimitsData.saveOptions()
            .pipe(
              finalize(() => {
                this.finishSaving();
              })
            ).subscribe();

          break;
        case PathConstants.Submission.Claims.Index:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.claimsHistoryData.save();
          break;
        case PathConstants.Submission.FormsSelection.Index:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.formsData.save();
          break;
        case PathConstants.Submission.FormsSelection.Index:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.formsData.save();
          break;
        case PathConstants.Submission.Bind.Index:
          this.onStatusChanged(currentCategoryRoute.toLowerCase());
          this.bindingData.saveBinding();
          this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Bind.Index);
          this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList));
          break;
        default:
          return;
      }

      // this.finishSaving();
    } catch (e) { }
  }

  public finishSaving() {
    this.isSaving = false;
    this.saveCategoryEndSource.next();
  }


  saveCurrentPage(pageUrl: string) {
    const promise = new Promise((resolve, reject) => {
      try {
        const pageId = pageUrl.split('/').pop();
        this.saveCurrentCategory(pageId);
        setTimeout(() => {
          resolve('Page is saved!');
        }, 500);
      } catch (error) {
        reject(error);
      }
    });
    return promise;
  }

  onStatusChanged(currentRoute?: string ) {
    if (this.submissionData.isIssued) { return; }
    const newCategoryRoute = this.getCurrentCategoryRoute();
    switch (currentRoute.toLowerCase()){
      case PathConstants.Submission.Applicant.NameAndAddress:
        if(newCategoryRoute == PathConstants.Submission.Applicant.BusinessDetails) {return;}
        else {
          this.updateStatus();
        }
        break;
        
      default:
        this.updateStatus();
    }
   
  }

  private updateStatus() {
    const formValue = this.summaryData.form.value;
    
    // if (formValue.status === 'Received') formValue.status = 'In Research';
    this.summaryData.quoteStatus = formValue.status;
    this.summaryData.updateStatus(formValue.status, formValue.assignedToId);
    this.summaryData.updateStatusForm(formValue.status);
  }

  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }
}

