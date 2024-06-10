import { Injectable, OnInit } from '@angular/core';
import { createPolicyDetailsMenuItems } from '@app/modules/policy/components/policy-details/policy-details-navitems';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { ApplicantData } from '@app/modules/submission/data/applicant/applicant.data';
import { ClaimsHistoryoData } from '@app/modules/submission/data/claims/claims-history.data';
import { CoverageData } from '@app/modules/submission/data/coverages/coverages.data';
import { LimitsData } from '@app/modules/submission/data/limits/limits.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { VehicleData } from '@app/modules/submission/data/vehicle/vehicle.data';
import { BaseComponent } from '@app/shared/base-component';
import { PathConstants } from '@app/shared/constants/path.constants';
import { concat, Subject } from 'rxjs';
import { concatMap, finalize, tap } from 'rxjs/operators';
import { LayoutService } from '../layout/layout.service';
import { PolicyNavValidateService } from './policy-nav-validate.service';
import { DocumentData } from '@app/modules/documents/document.data';
@Injectable()
export class PolicyNavSavingService extends BaseComponent implements OnInit {

    isSaving: boolean = false;
    saveCategoryEndSource = new Subject<void>();
    public saveCategoryEnded = this.saveCategoryEndSource.asObservable();

    constructor(
        private submissionData: SubmissionData,
        private policySummaryData: PolicySummaryData,
        private applicantData: ApplicantData,
        private limitsData: LimitsData,
        private coverageData: CoverageData,
        private vehicleData: VehicleData,
        private riskSpecificData: RiskSpecificsData,
        private claimsHistoryData: ClaimsHistoryoData,
        private layoutService: LayoutService,
        private policyNavValidationService: PolicyNavValidateService,
        public documentData: DocumentData,
    ) {
        super();
    }

    ngOnInit(): void {
        this.documentData.loadDocuments();
    }

    public saveCurrentCategory(currentCategoryRoute: string): void {
        if (!this.policySummaryData.canEditPolicy) { return; }

        console.log(`Current route: ${currentCategoryRoute}`);
        try {
            switch (currentCategoryRoute.toLocaleLowerCase()) {
                case PathConstants.Submission.Applicant.NameAndAddress:
                    this.applicantData.updateNameAndAddressMidterm();
                    break;

                /*
                case PathConstants.Submission.Limits.LimitsPage:
                    this.isSaving = true;
                    this.limitsData.saveRiskCoverageLimit().pipe(
                        concatMap(() => this.vehicleData.updateOtherCoverages(this.limitsData.oldLimitsData, true)),
                        finalize(() => {
                            // always trigger validation on general liab & cargo
                            this.policyNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo);
                            this.policyNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
                            this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificData.hiddenNavItems));

                            this.riskSpecificData.resetGeneralLiabilityCargo();
                            setTimeout(() => {
                                this.finishSaving();
                            }, 1500);
                        })
                    ).subscribe();
                    
                    break;
                */
                case PathConstants.Submission.Coverages.HistoricalCoverage:
                    this.isSaving = true;
                    this.coverageData.saveRiskHistory(() => this.finishSaving());
                    break;
                case PathConstants.Submission.Claims.Index:
                    this.claimsHistoryData.save();
                    break;
                case PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo:
                    this.riskSpecificData.saveGeneralLiabilityCargo();
                    this.policyNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.Index);
                    break;
                default:
                    return;
            }
        } catch (e) { }
    }

    public finishSaving() {
        this.isSaving = false;
        this.saveCategoryEndSource.next();
    }
}
