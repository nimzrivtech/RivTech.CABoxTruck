import { Injectable } from '@angular/core';
import { BaseComponent } from '../../../shared/base-component';
import { ApplicantBusinessValidationService } from '../validations/applicant-business-validation.service';
import { ApplicantNameaddressValidationService } from '../validations/applicant-nameaddress-validation.service';
import { ApplicantFilingsValidationService } from '../validations/filings-information-validation.service';
import { BrokerValidationService } from '../validations/broker-validation.service';
import { LimitsValidationService } from '../validations/limits-validation.service';
import { DestinationInfoValidationService } from '../validations/destination-info-validation.service';
import { DriverInfoValidationService } from '../validations/driver-information-validation.service';
import { DotInfoValidationService } from '../validations/dot-information-validation.service';
import { MaintenanceSafetyValidationService } from '../validations/maintenance-safety-validation.service';
import { GeneralLiabilityCargoValidationService } from '../validations/general-liability-cargo-validation.service';
import { UnderwritingQuestionsValidationService } from '../validations/underwriting-questions-validation.service';
import { PathConstants } from '../../../shared/constants/path.constants';
import { HistoricalCoverageValidationService } from '../validations/historical-coverage-validation.service';
import { VehicleValidationService } from '../validations/vehicle-validation.service';
import { DriverValidationService } from '../validations/driver-validation.service';
import { QuoteValidationService } from '../validations/quote-validation.service';
import { SubmissionData } from '../../../modules/submission/data/submission.data';
import { RiskSpecificsData } from '../../../modules/submission/data/risk-specifics.data';
import { ClaimsHistoryValidationService } from '../validations/claims-history-validation.service';
import { BindValidationService } from '../validations/bind-validation.service';
import { ApplicantAdditionalInterestValidationService } from '../validations/applicant-additionalinterest-validation.service';

@Injectable()
export class SubmissionNavValidateService extends BaseComponent {

  // validationList = new ValidationListModel();
  constructor(private applicantNameAddressValidation: ApplicantNameaddressValidationService,
    private applicantBusinessValidation: ApplicantBusinessValidationService,
    private applicantAdditionalInterestValidation: ApplicantAdditionalInterestValidationService,
    private bindingValidation: BindValidationService,
    private applicantFilingsValidation: ApplicantFilingsValidationService,
    private brokerValidation: BrokerValidationService,
    private limitsValidation: LimitsValidationService,
    private quotesValidation: QuoteValidationService,
    private historicalCoverageValidation: HistoricalCoverageValidationService,
    private claimsHistoryValidation: ClaimsHistoryValidationService,
    private vehicleValidation: VehicleValidationService,
    private driverValidation: DriverValidationService,
    private destinationValidation: DestinationInfoValidationService,
    private driverInfoValidation: DriverInfoValidationService,
    private dotInfoValidation: DotInfoValidationService,
    private maintenanceSafetyValidation: MaintenanceSafetyValidationService,
    private generalLiabilityCargoValidation: GeneralLiabilityCargoValidationService,
    private underwritingQuestionsValidation: UnderwritingQuestionsValidationService,
    private submissionData: SubmissionData,
    private riskSpecificsData: RiskSpecificsData
  ) {
    super();
  }

  // reset() {
  //   this.validationList = new ValidationListModel();
  // }

  get applicantNameAndAddressValidStatus() {
    return this.applicantNameAddressValidation.nameAndAddressSection;
  }

  get applicantNameAndAddressBindValidStatus() {
    return this.applicantNameAddressValidation.nameAndAddressSectionBinding;
  }

  get applicantBusinessValidStatus() {
    return this.applicantBusinessValidation.businessDetailsSection;
  }

  get applicantBusinessBindValidStatus() {
    return this.applicantBusinessValidation.businessDetailsSectionBinding;
  }

  get applicantFilingValidStatus() {
    return this.applicantFilingsValidation.filingInformationSection;
  }

  get applicantAdditionalInterestValidStatus() {
    return this.applicantAdditionalInterestValidation.additionalInterestSectionBinding;
  }

  get brokerValidStatus() {
    return this.brokerValidation.brokerSection;
  }

  get brokerBindValidStatus() {
    return this.brokerValidation.brokerRequiredToBind;
  }

  get appliedContactBindingValidStatus() {
    return this.brokerValidation.appliedContactRequiredToBind;
  }

  get limitsValidStatus() {
    return this.limitsValidation.limitsSection;
  }

  get quotesValidStatus() {
    return this.quotesValidation.quoteSection;
  }

  get historicalCoverageValidStatus() {
    if (!this.riskSpecificsData?.hiddenNavItems?.includes('Historical Coverage')) {
      return this.historicalCoverageValidation.historicalCoverageSection;
    } else {
      return true;
    }
  }

  get claimsHistoryValidStatus() {
    if (!this.riskSpecificsData?.hiddenNavItems?.includes('Claims History')) {
      return this.claimsHistoryValidation.claimsSection;
    } else {
      return true;
    }
  }

  get vehicleValidStatus() {
    return this.vehicleValidation.vehicleSection;
  }

  get driverValidStatus() {
    return this.driverValidation.driverSection;
  }

  get destinationValidStatus() {
    return this.destinationValidation.radiusSection;
  }

  get driverInfoValidStatus() {
    return this.driverInfoValidation.driverInfoSection && this.driverInfoValidation.driverHiringSection;
  }

  get dotInfoValidStatus() {
    return this.dotInfoValidation.dotInfoSection;
  }

  get maintenanceSafetyValidStatus() {
    return this.maintenanceSafetyValidation.maintenanceSection && this.maintenanceSafetyValidation.safetySection;
  }

  get generalLiabilityCargoValidStatus() {
    // return this.generalLiabilityCargoValidation.generalLiabilitySection && this.generalLiabilityCargoValidation.cargoSection;
    if (!this.riskSpecificsData?.hiddenNavItems?.includes('General Liability & Cargo') && !this.riskSpecificsData.isEitherGLAndMTCDisabled) {
      return this.generalLiabilityCargoValidation.generalLiabilitySection && this.generalLiabilityCargoValidation.cargoSection;
    } else {
      return true;
    }
  }

  get underwritingQuestionsValidStatus() {
    return this.underwritingQuestionsValidation.underwritingQuestionsSection;
  }

  get riskSpecificsValidationStatus() {
    return this.destinationValidStatus && this.driverInfoValidStatus
            && this.dotInfoValidStatus && this.maintenanceSafetyValidStatus
            && this.generalLiabilityCargoValidStatus && this.underwritingQuestionsValidStatus;
  }

  get uwAnswersConfirmed() {
    return {
      badge: this.underwritingQuestionsValidation.areAnswersConfirmed,
      enable: this.underwritingQuestionsValidation.areAnswersConfirmed
    };
  }

  get bindValidStatus() {
    return this.bindingValidation.bindSection;
  }

  public validateCurrentCategory(currentCategoryRoute: string): void {
    try {
      switch (currentCategoryRoute.toLowerCase()) {
        case PathConstants.Submission.Applicant.NameAndAddress:
          this.applicantNameAddressValidation.checkNameAndAddressPage();
          this.applicantNameAddressValidation.checkNameAndAddressPageBinding();
          this.submissionData.validationList.applicantNameAndAddress = this.applicantNameAndAddressValidStatus;
          this.submissionData.validationList.applicantNameAndAddressBinding = this.applicantNameAndAddressBindValidStatus;
          break;
        case PathConstants.Submission.Applicant.BusinessDetails:
          this.applicantBusinessValidation.checkBusinessDetailsPage();
          this.applicantBusinessValidation.checkBusinessDetailsPageBinding();
          this.submissionData.validationList.businessDetails = this.applicantBusinessValidStatus;
          this.submissionData.validationList.businessDetailsBinding = this.applicantBusinessBindValidStatus;
          break;
        case PathConstants.Submission.Applicant.FilingsInformation:
          this.applicantFilingsValidation.checkFilingsInformationPage();
          this.submissionData.validationList.filingsInfo = this.applicantFilingValidStatus;
          break;
        case PathConstants.Submission.Applicant.AdditionalInterest:
          this.applicantAdditionalInterestValidation.checkAdditionalInterestPageBinding();
          this.submissionData.validationList.additionalInterestBinding = this.applicantAdditionalInterestValidStatus;
          break;
        case PathConstants.Submission.Broker.Index:
          this.brokerValidation.checkBrokerPage();
          this.brokerValidation.checkBrokerPageBinding();
          this.submissionData.validationList.broker = this.brokerValidStatus;
          this.submissionData.validationList.brokerBinding = this.brokerBindValidStatus;
          this.submissionData.validationList.appliedContactBinding = this.appliedContactBindingValidStatus;
          break;
        case PathConstants.Submission.Limits.LimitsPage:
          this.limitsValidation.checkLimitsPage();
          this.submissionData.validationList.limits = this.limitsValidStatus;
          this.vehicleValidation.checkVehicleHeader();
          this.submissionData.validationList.vehicle = this.vehicleValidStatus;
          break;
        case PathConstants.Submission.Quote.Index:
          this.quotesValidation.checkQuotePage();
          this.submissionData.validationList.quoteOptions = this.quotesValidStatus;
          break;
        case PathConstants.Submission.Coverages.HistoricalCoverage:
          this.historicalCoverageValidation.checkHistoricalCoveragePage();
          this.submissionData.validationList.historicalCoverages = this.historicalCoverageValidStatus;
          break;
        case PathConstants.Submission.Claims.Index:
          this.claimsHistoryValidation.checkClaimsPage();
          this.submissionData.validationList.claimsHistory = this.claimsHistoryValidStatus;
          break;
        case PathConstants.Submission.Vehicle.Index:
          this.vehicleValidation.checkVehiclePage();
          this.submissionData.validationList.vehicle = this.vehicleValidStatus;
          break;
        case PathConstants.Submission.Driver.Index:
          this.driverValidation.checkDriverPage();
          this.submissionData.validationList.driver = this.driverValidStatus;
          break;
        case PathConstants.Submission.RiskSpecifics.Index:
          this.submissionData.validationList.riskSpecifics = this.riskSpecificsValidationStatus;
          break;
        case PathConstants.Submission.RiskSpecifics.DestinationInfo:
          this.destinationValidation.checkRadiusOfOperationPage();
          this.submissionData.validationList.destinationInfo = this.destinationValidStatus;
          break;
        case PathConstants.Submission.RiskSpecifics.DriverInfo:
          this.driverInfoValidation.checkDriverInfoPage();
          this.submissionData.validationList.driverInfo = this.driverInfoValidStatus;
          break;
        case PathConstants.Submission.RiskSpecifics.DotInfo:
          this.dotInfoValidation.checkDotInfoPage();
          this.submissionData.validationList.dotInfo = this.dotInfoValidStatus;
          break;
        case PathConstants.Submission.RiskSpecifics.MaintenanceSafety:
          this.maintenanceSafetyValidation.checkMaintenanceSafetyPage();
          this.submissionData.validationList.maintenanceSafety = this.maintenanceSafetyValidStatus;
          break;
        case PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo:
          this.generalLiabilityCargoValidation.checkGeneralLiabilityCargoPage();
          this.submissionData.validationList.generalLiabilityCargo = this.generalLiabilityCargoValidStatus;
          break;
        case PathConstants.Submission.RiskSpecifics.UWQuestions:
          this.underwritingQuestionsValidation.checkUnderwritingQuestionsPage();
          this.submissionData.validationList.underwritingQuestions = this.underwritingQuestionsValidStatus;
          break;
        case PathConstants.Submission.Bind.Index:
          this.bindingValidation.checkBindPage();
          this.submissionData.validationList.bind = this.bindValidStatus;
          break;
        default:
          return;
      }
      this.submissionData.validationList.uwAnswersConfirmed = this.uwAnswersConfirmed;
    } catch (e) { }
  }

  hasInvalid(): boolean {
    this.validateCategories();

    return !(
      (this.submissionData.validationList.applicantNameAndAddress || this.submissionData.validationList.applicantNameAndAddress === undefined) &&
      (this.submissionData.validationList.applicantNameAndAddressBinding || this.submissionData.validationList.applicantNameAndAddressBinding === undefined) &&
      (this.submissionData.validationList.additionalInterestBinding || this.submissionData.validationList.additionalInterestBinding === undefined) &&
      (this.submissionData.validationList.businessDetails  || this.submissionData.validationList.businessDetails === undefined) &&
      (this.submissionData.validationList.businessDetailsBinding  || this.submissionData.validationList.businessDetailsBinding === undefined) &&
      (this.submissionData.validationList.filingsInfo  || this.submissionData.validationList.filingsInfo === undefined) &&
      (this.submissionData.validationList.broker || this.submissionData.validationList.broker === undefined) &&
      (this.submissionData.validationList.brokerBinding || this.submissionData.validationList.brokerBinding === undefined) &&
      (this.submissionData.validationList.limits || this.submissionData.validationList.limits === undefined) &&
      (this.submissionData.validationList.quoteOptions || this.submissionData.validationList.quoteOptions === undefined) &&
      (this.submissionData.validationList.historicalCoverages || this.submissionData.validationList.historicalCoverages === undefined) &&
      (this.submissionData.validationList.claimsHistory || this.submissionData.validationList.claimsHistory === undefined) &&
      (this.submissionData.validationList.vehicle || this.submissionData.validationList.vehicle === undefined) &&
      (this.submissionData.validationList.driver || this.submissionData.validationList.driver === undefined) &&
      (this.submissionData.validationList.riskSpecifics || this.submissionData.validationList.riskSpecifics === undefined) &&
      (this.submissionData.validationList.destinationInfo || this.submissionData.validationList.destinationInfo === undefined) &&
      (this.submissionData.validationList.driverInfo || this.submissionData.validationList.driverInfo === undefined) &&
      (this.submissionData.validationList.dotInfo || this.submissionData.validationList.dotInfo === undefined) &&
      (this.submissionData.validationList.maintenanceSafety || this.submissionData.validationList.maintenanceSafety === undefined) &&
      (this.submissionData.validationList.generalLiabilityCargo || this.submissionData.validationList.generalLiabilityCargo === undefined) &&
      (this.submissionData.validationList.underwritingQuestions || this.submissionData.validationList.underwritingQuestions === undefined) &&
      (this.submissionData.validationList.bind || this.submissionData.validationList.bind === undefined)
    );
  }

  validateCategories(): void {
    this.validateCurrentCategory(PathConstants.Submission.Applicant.NameAndAddress);
    this.validateCurrentCategory(PathConstants.Submission.Applicant.BusinessDetails);
    this.validateCurrentCategory(PathConstants.Submission.Applicant.FilingsInformation);
    this.validateCurrentCategory(PathConstants.Submission.Applicant.AdditionalInterest);
    this.validateCurrentCategory(PathConstants.Submission.Broker.Index);
    this.validateCurrentCategory(PathConstants.Submission.Limits.LimitsPage);
    this.validateCurrentCategory(PathConstants.Submission.Quote.Index);
    this.validateCurrentCategory(PathConstants.Submission.Coverages.HistoricalCoverage);
    this.validateCurrentCategory(PathConstants.Submission.Claims.Index);
    this.validateCurrentCategory(PathConstants.Submission.Vehicle.Index);
    this.validateCurrentCategory(PathConstants.Submission.Driver.Index);
    this.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
    this.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DestinationInfo);
    this.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DriverInfo);
    this.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DotInfo);
    this.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.MaintenanceSafety);
    this.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo);
    this.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.UWQuestions);
    this.validateCurrentCategory(PathConstants.Submission.Bind.Index);
  }
}
