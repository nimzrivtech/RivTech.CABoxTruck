import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { LayoutService } from '../../../../core/services/layout/layout.service';
import { createPolicyDetailsMenuItems } from '../../components/policy-details/policy-details-navitems';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { take, takeUntil } from 'rxjs/operators';
import { BaseClass } from '../../../../shared/base-class';
import { SubmissionData } from '../../../../modules/submission/data/submission.data';
import NotifUtils from '../../../../shared/utilities/notif-utils';
import { PolicySummaryData } from '../../data/policy-summary.data';
import { PathConstants } from '../../../../shared/constants/path.constants';
import { RiskDTO } from '@app/shared/models/risk/riskDto';
import { PolicyBillingData } from '../../data/policy-billing.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import * as _ from 'lodash';
import { ApplicantData } from '@app/modules/submission/data/applicant/applicant.data';
import { LimitsData } from '@app/modules/submission/data/limits/limits.data';
import { CoverageData } from '@app/modules/submission/data/coverages/coverages.data';
import { ClaimsHistoryoData } from '@app/modules/submission/data/claims/claims-history.data';
import { PolicyNavValidateService } from '@app/core/services/navigation/policy-nav-validate.service';
import { PolicyNavSavingService } from '@app/core/services/navigation/policy-nav-saving.service';
import { VehicleData } from '@app/modules/submission/data/vehicle/vehicle.data';
import { DriverData } from '@app/modules/submission/data/driver/driver.data';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';

@Component({
  selector: 'app-policy-details',
  templateUrl: './policy-details.component.html',
  styleUrls: ['./policy-details.component.css']
})
export class PolicyDetailsComponent extends BaseClass implements OnInit, OnDestroy {

  riskId: string;
  riskDetailId: string;
  // upon navigation saving
  private newCategoryRoute: string;
  private currentCategoryRoute: string;

  constructor(
    private layoutService: LayoutService,
    private route: ActivatedRoute,
    private router: Router,
    private submissionData: SubmissionData,
    private summaryData: PolicySummaryData,
    private activatedRoute: ActivatedRoute,
    private policyBillingData: PolicyBillingData,
    private riskSpecificsData: RiskSpecificsData,
    private policyNavValidationService: PolicyNavValidateService,
    private policyNavSavingService: PolicyNavSavingService,
    private applicantData: ApplicantData,
    private limitsData: LimitsData,
    private vehicleData: VehicleData,
    private driverData: DriverData,
    private coverageData: CoverageData,
    private claimsHistoryData: ClaimsHistoryoData,
    private raterApiData: RaterApiData
  ) {
    super();
  }

  ngOnInit(): void {
    console.log('policy details init');
    this.submissionData.reset();
    this.summaryData.reset();
    this.submissionData.getCurrentServerDateTime2().subscribe((date) => {
      this.submissionData.currentServerDateTime = null;
      this.submissionData.currentServerDateTime = date;
      console.log(this.submissionData.currentServerDateTime);
    });
    this.initiateForms();
    // this.currentCategoryRoute = this.getCurrentCategoryRoute();
    // this.savingUponNavigate();
    this.subscribeToRouteParams();
    //this.raterApiData.preloadRater(true).subscribe();
  }

  ngOnDestroy(): void {
    this.layoutService.clearMenu();
    this.raterApiData.UnloadPremium(true, true);
  }

  @HostListener('window:beforeunload', ['$event'])
  beforeUnloadHandler(event) {
    // unloading premium when close window
    this.raterApiData.UnloadPremium(true);
  }


  initiateForms() {
    this.applicantData.initiateFormFields();
    this.limitsData.initiateFormFields();
    this.coverageData.initiateFormFields();
    this.claimsHistoryData.initiateFormFields();
    this.riskSpecificsData.initiateFormFields(new RiskDTO());
    this.policyBillingData.initiateFormFields();
    this.vehicleData.initiateFormFields();
  }

  subscribeToRouteParams(): void {
    this.route.params.pipe(takeUntil(this.stop$)).subscribe(params => {
      this.checkParamsUuid(params);

      if (this.riskId != null && this.riskDetailId != null) {
        this.getRiskData();
      }
    });
  }

  checkParamsUuid(params: any): void {
    if (params.uuid !== undefined) {
      this.riskId = params.uuid;
      this.riskDetailId = params.uuid2;
      return;
    }
  }

  getRiskData() {
    console.log('get riskDetail');
    const routeResolver = this.activatedRoute.snapshot.data['data'];
    const riskDetailId = routeResolver.risk?.id;
    if (!routeResolver.isValid && !routeResolver.isNew) {
      NotifUtils.showError('There was an error trying to access the data. Please try again.', () => { });
      this.router.navigate(['/submissions/']);
    } else {
      this.submissionData.isNew = false;
      if (routeResolver.isNew) {
        this.submissionData.isNew = true;
      }
      this.submissionData.riskDetail = routeResolver.risk;
      console.log('riskdetail', this.submissionData.riskDetail);
      this.setSummaryHeader();
      this.populateForms(this.submissionData.riskDetail);
      this.enableRequiredFields();
      this.policyNavValidationService.validateCategories();
      this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.riskId, this.riskDetailId, this.submissionData?.validationList, this.riskSpecificsData.hiddenNavItems));
      this.raterApiData.preloadRater(true).subscribe();
    }
  }

  enableRequiredFields() {
    this.applicantData.enableBindRequiredFields(true);
  }

  setSummaryHeader() {
    if (!this.submissionData.isNew) {
      this.summaryData.name = this.submissionData.riskDetail.nameAndAddress.businessName;
      this.summaryData.address = this.submissionData.riskDetail.nameAndAddress.businessAddress;
      const nameAddress = this.submissionData.riskDetail.nameAndAddress;
      this.summaryData.address = `${nameAddress.businessAddress}, ${nameAddress.city}, ${nameAddress.state} ${nameAddress.zipCode}`;
      this.summaryData.quoteNumber = this.submissionData.riskDetail.submissionNumber;
      this.summaryData.policyNumber = this.submissionData.riskDetail.policyNumber;
      this.summaryData.quoteStatus = this.submissionData.riskDetail.status;
      this.summaryData.effectiveDate = this.submissionData.riskDetail.brokerInfo?.effectiveDate?.toLocaleString();
      this.summaryData.pendingCancellationDate = this.submissionData.riskDetail.pendingCancellationDate;
      this.summaryData.pendingCancellationNoticeDate = this.submissionData.riskDetail.pendingCancellationNoticeDate;
      this.summaryData.agency = this.submissionData.riskDetail.brokerInfo?.agency?.entity?.companyName;
      this.summaryData.producer = this.submissionData.riskDetail.brokerInfo?.agent?.entity?.fullName;
      this.summaryData.policyStatus = this.submissionData.riskDetail.status;
      this.summaryData.policySubStatus = this.submissionData.riskDetail.subStatus;
      // this.summaryData.estimatedPremium = null; // in progress
      this.summaryData.updateEstimatedPremium(true);
    }

    this.summaryData.pageType = PathConstants.Submission.Index;
  }

  private populateForms(data: RiskDTO): void {
    this.policyBillingData.populatePolicyBillingPage(data);
    this.applicantData.retrieveDropdowValues();
    this.applicantData.populateNameAndAddressFields();
    this.applicantData.populateBusinessDetailsFields();
    this.applicantData.populateFilingsInformationFields();
    this.limitsData.retrieveDropDownValues(true);
    this.limitsData.populatePolicyLimitsFields();
    this.coverageData.populateHistoryFields();
    this.claimsHistoryData.populateHistoryFields();
    this.riskSpecificsData.populateGeneralLiabilityForm();
    this.riskSpecificsData.populateCommoditiesHauled();
    //this.vehicleData.populateVehicleField();
    this.vehicleData.populateVehicleHeaderDetails();
    // currently all vehicles & drivers are being copied
    // if (this.submissionData.isNewlyIssued) {
    //   this.vehicleData.deleteUnboundVehicles();
    //   this.driverData.deleteUnboundDrivers();
    // }
  }

  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }

  savingUponNavigate() {
    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .pipe(takeUntil(this.stop$)).subscribe((event: NavigationEnd) => {
        this.newCategoryRoute = this.getCurrentCategoryRoute();
        console.log(this.newCategoryRoute);
        if (this.submissionData.isIssued) {
          // TODO: Make this work and able to centralize saving and forms validation
          // Current Implementation: Saving and validation were put into individual components
          // this.policyNavSavingService.saveCurrentCategory(this.currentCategoryRoute);
          // this.policyNavValidationService.validateCurrentCategory(this.currentCategoryRoute);
          // this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));
        }
        this.currentCategoryRoute = this.newCategoryRoute;
      });
  }

  validateRequiredToRateModules(): boolean {
    const excludeBindreq = ['businessDetailsBinding', 'bind', 'brokerBinding'];
    const riskValidationList = Object.keys(this.submissionData.validationList).filter(x => !excludeBindreq.find(r => r === x));
    const invalidModule = riskValidationList
    .find(e => e !== 'uwAnswersConfirmed' && this.submissionData.validationList[e] == false);
    const lstToValidate: Array<any> = [undefined, null];
    return (lstToValidate.indexOf(invalidModule) !== -1);
  }
}
