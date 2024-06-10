import { Component, OnInit, Inject, OnDestroy, HostListener } from '@angular/core';
import { LayoutService } from '../../../../core/services/layout/layout.service';
import { createSubmissionDetailsMenuItems } from '../../components/submission-details/submission-details-navitems';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { SubmissionData } from '../../data/submission.data';
import { DOCUMENT } from '@angular/common';

import NotifUtils from '../../../../shared/utilities/notif-utils';
import { takeUntil } from 'rxjs/operators';
import * as _ from 'lodash';
import { BaseClass } from '../../../../shared/base-class';
import { CoverageData } from '../../data/coverages/coverages.data';
import { LimitsData } from '../../data/limits/limits.data';
import { QuoteLimitsData } from '../../data/quote/quote-limits.data';
import { ApplicantData } from '../../data/applicant/applicant.data';
import { PathConstants } from '../../../../shared/constants/path.constants';
import { SummaryData } from '../../data/summary.data';
import { SubmissionNavSavingService } from '../../../../core/services/navigation/submission-nav-saving.service';
import { SubmissionNavValidateService } from '../../../../core/services/navigation/submission-nav-validate.service';
import { BrokerGenInfoData } from '../../data/broker/general-information.data';
import { BrokerInfoDto } from '../../../../shared/models/submission/brokerinfo/brokerInfoDto';
import { RiskSpecificsData } from '../../data/risk-specifics.data';
import { RiskDTO } from '../../../../shared/models/risk/riskDto';
import { RadiusOfOperationsData } from '../../data/riskspecifics/radius-of-operations.data';
import { RadiusOfOperationsDto } from '../../../../shared/models/submission/riskspecifics/radius-of-operations.dto';
import { DriverInfoData } from '../../data/riskspecifics/driver-info.data';
import { DriverHiringCriteriaData } from '../../data/riskspecifics/driver-hiring-criteria.data';
import { DriverInfoDto } from '../../../../shared/models/submission/riskspecifics/driver-info.dto';
import { DriverHiringCriteriaDto } from '../../../../shared/models/submission/riskspecifics/driver-hiring-criteria.dto';
import { DotInfoData } from '../../data/riskspecifics/dot-info.data';
import { DotInfoDto } from '../../../../shared/models/submission/riskspecifics/dot-info.dto';
import { UnderwritingQuestionsData } from '../../data/riskspecifics/underwriting-questions.data';
import { UnderwritingQuestionsDto } from '../../../../shared/models/submission/riskspecifics/underwriting-questions.dto';
import { VehicleData } from '../../data/vehicle/vehicle.data';
import { DriverData } from '../../data/driver/driver.data';
import { ValidationListModel } from '../../../../shared/models/validation-list.model';
import { ClaimsHistoryoData } from '../../data/claims/claims-history.data';
import { DriverHeaderDto } from '../../../../shared/models/submission/Driver/DriverDto';
import { RaterApiData } from '../../data/rater-api.data';
import { TableData } from '../../data/tables.data';
import { ManuscriptsData } from '../../data/manuscripts/manuscripts.data';
import { BindingData } from '../../data/binding/binding.data';
import { RiskAdditionalInterestsDTO } from '@app/shared/models/submission/RiskAdditionalInterestDto';

@Component({
  selector: 'app-submission-details',
  templateUrl: './submission-details.component.html',
  styleUrls: ['./submission-details.component.css']
})
export class SubmissionDetailsComponent extends BaseClass implements OnInit, OnDestroy {
  public sidebarMinimized = true;
  private changes: MutationObserver;
  public element: HTMLElement;

  // resizer
  // private ssResizer: BaseResizer = SubmissionSummaryResizer.Initialize();
  // private sdResizer: BaseResizer = SubmissionDetailResizer.Initialize();

  currentSubmissionId: string;

  // upon navigation saving
  private newCategoryRoute: string;
  private currentCategoryRoute: string;

  @HostListener('window:beforeunload', ['$event'])
  beforeUnloadHandler(event) {
    // unloading premium when close window
    this.raterApiData.UnloadPremium(true);
  }

  get xpRaterUpdated() {
    return this.submissionData.riskDetail.updateXPRaterVersion;
  }

  constructor(
    private router: Router,
    private summaryData: SummaryData,
    private submissionData: SubmissionData,
    private applicantData: ApplicantData,
    private coverageData: CoverageData,
    private limitsData: LimitsData,
    private vehicleData: VehicleData,
    private driverData: DriverData,
    private quoteLimitsData: QuoteLimitsData,
    private brokerData: BrokerGenInfoData,
    private riskSpecificsData: RiskSpecificsData,
    private radiusOperationData: RadiusOfOperationsData,
    private driverInfoData: DriverInfoData,
    private driverHiringData: DriverHiringCriteriaData,
    private dotInfoData: DotInfoData,
    private underwritingQuestionsData: UnderwritingQuestionsData,
    private claimsHistoryData: ClaimsHistoryoData,
    private manuscriptsData: ManuscriptsData,
    private raterApiData: RaterApiData,
    private bindingData: BindingData,
    private layoutService: LayoutService,
    private activatedRoute: ActivatedRoute,
    private submissionNavSavingService: SubmissionNavSavingService,
    private submissionNavValidationService: SubmissionNavValidateService,
    private tableData: TableData,
    @Inject(DOCUMENT) _document?: any) {
    super();
    this.changes = new MutationObserver((mutations) => {
      this.sidebarMinimized = _document.body.classList.contains('sidebar-minimized');
    });
    this.element = _document.body;
    this.changes.observe(<Element>this.element, {
      attributes: true,
      attributeFilter: ['class']
    });
  }

  ngOnInit() {
    this.initiateForms();
    this.savingUponNavigate();
    this.summaryData.reset();
    this.submissionData.getCurrentServerDateTime2().subscribe((date) => {
      this.submissionData.currentServerDateTime = null;
      this.submissionData.currentServerDateTime = date;
      console.log(this.submissionData.currentServerDateTime);
    });
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
      this.validatedAdditionalInterest();
      this.layoutService.updateMenu(createSubmissionDetailsMenuItems(riskDetailId, null, ['General Liability & Cargo', 'Manuscripts']));
      this.currentCategoryRoute = this.getCurrentCategoryRoute();
      this.setSummaryHeader();
      this.updateStatusList();
      this.raterApiData.disableCalculatePremium = true;
      if (!this.submissionData.isNew) {
        this.checkBindRequiredStatus();
        this.populateForms();
        this.validateCategories();
        this.raterApiData.preloadRater().subscribe();
      }
    }
  }

  validatedAdditionalInterest() {
    this.submissionData.riskDetail?.additionalInterest?.forEach(ai => {
      ai.isValidated = this.getValidated(ai);
    });
  }

  getValidated(item: RiskAdditionalInterestsDTO) {
    return !this.isEmpty(item.address) && !this.isEmpty(item.city) && !this.isEmpty(item.state) && !this.isEmpty(item.zipCode);
  }

  isEmpty(str) {
      return (!str || str.length === 0 );
  }

  ngOnDestroy() {
    this.layoutService.clearMenu();

    // Unload premium when change submission
    this.raterApiData.UnloadPremium(true);
  }

  checkBindRequiredStatus() {
    const preBindQuote = this.submissionData.riskDetail?.riskDocuments?.some(f => f.fileName === 'Pre-Bind Quote' && f.isConfidential);
    if (preBindQuote === true) {
      this.submissionData.isQuoteIssued = preBindQuote;
      this.enableBindRequired();
    }
  }

  enableBindRequired() {
    this.applicantData.enableBindRequiredFields(true);
    this.bindingData.enableBindRequiredFields(true);
  }

  setSummaryHeader() {
    if (!this.submissionData.isNew) {
      this.summaryData.name = this.submissionData.riskDetail.nameAndAddress.businessName;
      this.summaryData.address = this.submissionData.riskDetail.nameAndAddress.businessAddress;
      const nameAddress = this.submissionData.riskDetail.nameAndAddress;
      this.summaryData.address = `${nameAddress.businessAddress}, ${nameAddress.city}, ${nameAddress.state} ${nameAddress.zipCode}`;
      this.summaryData.quoteNumber = this.submissionData.riskDetail.submissionNumber;
      this.summaryData.quoteStatus = this.submissionData.riskDetail.status;
      this.summaryData.policyNumber = this.submissionData.riskDetail.policyNumber;
      this.summaryData.effectiveDate = this.submissionData.riskDetail.brokerInfo?.effectiveDate?.toLocaleString();
      this.summaryData.agency = this.submissionData.riskDetail.brokerInfo?.agency?.entity?.companyName;
      this.summaryData.producer = this.submissionData.riskDetail.brokerInfo?.agent?.entity?.fullName;
      this.summaryData.estimatedPremium = null; // in progress
    }

    this.summaryData.pageType = PathConstants.Submission.Index;
  }

  savingUponNavigate() {
    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .pipe(takeUntil(this.stop$)).subscribe((event: NavigationEnd) => {
        this.newCategoryRoute = this.getCurrentCategoryRoute();
        if (this.currentCategoryRoute
          && (this.newCategoryRoute !== this.currentCategoryRoute)
          && this.submissionData.riskDetail.id && this.submissionData.isNew === false) {
          this.submissionNavSavingService.saveCurrentCategory(this.currentCategoryRoute);
          this.submissionNavValidationService.validateCurrentCategory(this.currentCategoryRoute);
          this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList,
            this.riskSpecificsData.hiddenNavItems));
        }
        this.currentCategoryRoute = this.newCategoryRoute;
        this.updateStatusList();
      });
  }

  initiateForms() {
    this.submissionData.riskDetail = new RiskDTO();
    this.submissionData.validationList = new ValidationListModel();

    this.riskSpecificsData.isInitialGeneralLiabAndCargo = true;
    this.riskSpecificsData.isInitialMaintenanceAndSatefy = true;
    this.driverHiringData.isInitial = true;
    // this.submissionNavValidationService.reset();

    this.limitsData.initiateFormFields();
    this.quoteLimitsData.initiateFormFields();
    this.applicantData.initiateFormFields();
    this.manuscriptsData.initiateFormFields();
    this.brokerData.initiateFormGroup(new BrokerInfoDto());
    this.driverData.initiateHeaderFormGroup(new DriverHeaderDto());
    this.riskSpecificsData.initiateFormFields(new RiskDTO());
    this.radiusOperationData.initiateFormGroup(new RadiusOfOperationsDto());
    this.driverInfoData.initiateFormGroup(new DriverInfoDto());
    this.driverHiringData.initiateFormGroup(new DriverHiringCriteriaDto());
    this.dotInfoData.initiateFormGroup(new DotInfoDto());
    this.underwritingQuestionsData.initiateFormGroup(new UnderwritingQuestionsDto());
    this.coverageData.initiateFormFields();
    this.claimsHistoryData.initiateFormFields();
    this.bindingData.initializeForms();
    this.vehicleData.initiateFormFields();
  }

  populateForms() {
    this.applicantData.populateNameAndAddressFields();
    this.applicantData.populateBusinessDetailsFields();
    this.applicantData.populateFilingsInformationFields();
    this.applicantData.populateBlanketCoverageFields();
    this.applicantData.getAdditionalInterests();
    this.applicantData.setFeeDetails(this.submissionData?.riskDetail?.nameAndAddress?.state);
    this.limitsData.populateLimitsFields();
    this.limitsData.retrieveDropDownValues();

    this.quoteLimitsData.loadQuoteOptions().subscribe();
    // this.quoteLimitsData.populateQuoteFields();
    // this.quoteLimitsData.retrieveDropDownValues();

    this.vehicleData.populateVehicleField();
    this.brokerData.populateBrokerFields();

    this.driverData.populateDriverHeaderField();

    this.radiusOperationData.populateRadiusOperationField();
    this.driverInfoData.populateDrivingInfoField();
    this.driverHiringData.populateDrivingHiringCriteriaField();
    this.dotInfoData.populateDotInfoFields();
    this.underwritingQuestionsData.populateUnderwritingQuestionsField();
    this.coverageData.populateHistoryFields();
    this.riskSpecificsData.populateMaintenanceSafetyForm();
    this.riskSpecificsData.populateGeneralLiabilityForm();
    this.riskSpecificsData.populateCommoditiesHauled();
    this.claimsHistoryData.populateHistoryFields();
    this.manuscriptsData.populateFields(false);

    // Bind Initialize and populate
    this.bindingData.initializeForms();
  }

  validateCategories(): void {
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.NameAndAddress);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.BusinessDetails);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.FilingsInformation);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.AdditionalInterest);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Broker.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Limits.LimitsPage);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Vehicle.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Driver.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Coverages.HistoricalCoverage);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Claims.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DestinationInfo);
    this.driverHiringData.isInitial = false; // disable isInitial to get through the validation
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DriverInfo);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DotInfo);
    this.riskSpecificsData.isInitialMaintenanceAndSatefy = false; // disable isInitial to get through the validation
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.MaintenanceSafety);
    this.riskSpecificsData.isInitialGeneralLiabAndCargo = false; // disable isInitial to get through the validation
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.UWQuestions);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Quote.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Bind.Index);
    this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList,
      this.riskSpecificsData.hiddenNavItems));
  }

  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }

  private updateStatusList() {
    console.log(this.submissionData.riskDetail);
    if (!this.submissionData.riskDetail.isRenewal) {
      const checkDataOnly = this.summaryData.statusList.findIndex(item => item.value === 10);
      const checkTestClass = this.summaryData.statusList.findIndex(item => item.value === 11);

      if (checkDataOnly === -1) this.summaryData.statusList.push({ value: 10, label: 'Data Only' });
      if (checkTestClass === -1) this.summaryData.statusList.push({ value: 11, label: 'Test Class' });

    } else {
      const removeDataOnly = this.summaryData.statusList.findIndex(item => item.value === 10);
      const removeTestClass = this.summaryData.statusList.findIndex(item => item.value === 11);

      if (removeDataOnly !== -1 && removeTestClass !== -1) this.summaryData.statusList.splice(removeDataOnly, 2);
    }
  }
}
