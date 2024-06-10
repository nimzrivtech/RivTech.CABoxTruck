import { Injectable } from '@angular/core';
import { createPolicyDetailsMenuItems } from '@app/modules/policy/components/policy-details/policy-details-navitems';
import { ApplicantData } from '@app/modules/submission/data/applicant/applicant.data';
import { RiskSpecificsData } from '../../../modules/submission/data/risk-specifics.data';
import { SubmissionData } from '../../../modules/submission/data/submission.data';
import { BaseComponent } from '../../../shared/base-component';
import { PathConstants } from '../../../shared/constants/path.constants';
import { LayoutService } from '../layout/layout.service';
import { ApplicantNameaddressValidationService } from '../validations/applicant-nameaddress-validation.service';
import { BrokerValidationService } from '../validations/broker-validation.service';
import { ClaimsHistoryValidationService } from '../validations/claims-history-validation.service';
import { DriverValidationService } from '../validations/driver-validation.service';
import { GeneralLiabilityCargoValidationService } from '../validations/general-liability-cargo-validation.service';
import { HistoricalCoverageValidationService } from '../validations/historical-coverage-validation.service';
import { LimitsValidationService } from '../validations/limits-validation.service';
import { VehicleValidationService } from '../validations/vehicle-validation.service';
import { ValidationListModel } from '@app/shared/models/validation-list.model';

@Injectable()
export class PolicyNavValidateService extends BaseComponent {

    constructor(private submissionData: SubmissionData,
                private applicantNameAddressValidation: ApplicantNameaddressValidationService,
                private brokerValidation: BrokerValidationService,
                private limitsValidation: LimitsValidationService,
                private vehicleValidation: VehicleValidationService,
                private driverValidation: DriverValidationService,
                private historicalCoverageValidation: HistoricalCoverageValidationService,
                private claimsHistoryValidation: ClaimsHistoryValidationService,
                private generalLiabilityCargoValidation: GeneralLiabilityCargoValidationService,
                private riskSpecificsData: RiskSpecificsData,
                private applicantData: ApplicantData,
                private layoutService: LayoutService
                ) {
        super();
    }

    get applicantNameAndAddressValidStatus() {
        return this.applicantNameAddressValidation.nameAndAddressSection;
    }

    get applicantNameAndAddressBindValidStatus() {
      return this.applicantNameAddressValidation.nameAndAddressSectionBinding;
    }

    get brokerPolicyContactsValidStatus() {
      return this.brokerValidation.checkPolicyContactsSection();
  }

    get limitsValidStatus() {
        return this.limitsValidation.limitsSection;
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

    get generalLiabilityCargoValidStatus() {
        // return this.generalLiabilityCargoValidation.generalLiabilitySection && this.generalLiabilityCargoValidation.cargoSection;
        if (!this.riskSpecificsData?.hiddenNavItems?.includes('General Liability & Cargo') && !this.riskSpecificsData.isEitherGLAndMTCDisabled) {
          return this.generalLiabilityCargoValidation.generalLiabilitySection && this.generalLiabilityCargoValidation.cargoSection;
        } else {
          return true;
        }
    }

    get riskSpecificsValidationStatus() {
        return this.generalLiabilityCargoValidStatus;
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
            case PathConstants.Submission.Broker.Index:
              this.brokerValidation.checkPolicyContactsSection();
              this.submissionData.validationList.broker = this.brokerPolicyContactsValidStatus;
              break;
            case PathConstants.Submission.Limits.LimitsPage:
              this.limitsValidation.checkLimitsPage();
              this.submissionData.validationList.limits = this.limitsValidStatus;
              this.vehicleValidation.checkVehiclePage();
              this.submissionData.validationList.vehicle = this.vehicleValidStatus;
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
              this.driverValidation.checkPolicyDriverSection();
              this.submissionData.validationList.driver = this.driverValidStatus;
              break;
            case PathConstants.Submission.RiskSpecifics.Index:
              this.submissionData.validationList.riskSpecifics = this.riskSpecificsValidationStatus;
              break;
            case PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo:
              this.generalLiabilityCargoValidation.checkGeneralLiabilityCargoPolicyPage();
              this.submissionData.validationList.generalLiabilityCargo = this.generalLiabilityCargoValidStatus;
              break;
            default:
              return;
          }
        } catch (e) { }
    }

    validateCategories(): void {
        //Reset validation list to discard values from submission when switching policies
        this.submissionData.validationList = new ValidationListModel();

        this.validateCurrentCategory(PathConstants.Submission.Applicant.NameAndAddress);
        this.validateCurrentCategory(PathConstants.Submission.Broker.Index);
        this.validateCurrentCategory(PathConstants.Submission.Limits.LimitsPage);
        this.validateCurrentCategory(PathConstants.Submission.Coverages.HistoricalCoverage);
        this.validateCurrentCategory(PathConstants.Submission.Claims.Index);
        this.validateCurrentCategory(PathConstants.Submission.Vehicle.Index);
        this.validateCurrentCategory(PathConstants.Submission.Driver.Index);
        this.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
        this.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo);
        this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList,
            this.riskSpecificsData.hiddenNavItems));
    }
}
