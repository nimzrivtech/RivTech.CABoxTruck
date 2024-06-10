import { RiskCoveragePremiumAvgDTO } from "./riskCoveragePremiumAvgDto";

export class RiskCoveragePremiumDTO implements IRiskCoveragePremiumDTO {
    id?: string =null;
    riskCoverageId?: string = null;
    autoLiabilityPremium?: number = null;
    cargoPremium?: number = null;
    physicalDamagePremium?: number = null;
    vehicleLevelPremium?: number = null;
    generalLiabilityPremium?: number = null;
    proratedGeneralLiabilityPremium?: number = null;
    comprehensivePremium?: number = null;
    collisionPremium?: number = null;
    avgPerVehicle?: number = null;
    riskMgrFeeAL?: number = null;
    riskMgrFeePD?: number = null;
    policyTotals?: number = null;
    premium?: number = null;
    proratedPremium?: number = null;
    fees?: number = null;
    taxesAndStampingFees?: number = null;
    depositAmount?: number = null;
    installmentFee?: number = null;
    perInstallmentTax?: number = null;
    perInstallment?: number = null;
    totalInstallmentFee?: number = null;
    totalAmounDueFull?: number = null;
    totalAmountDueInstallment?: number = null;
    accountDriverFactor?: number = null;
    additionalInsuredPremium?: number = null;
    premiumTaxesFees?: number = null;
    primaryNonContributoryPremium?: number = null;
    trailerInterchangeAL?: number = null;
    proratedTrailerInterchangePremiumAL?: number = null;
    nonOwnedTrailerPremium?: number = null;
    proratedNonOwnedTrailerPremium?: number = null;
    subTotalDue?: number = null;
    totalAnnualPremiumWithPremiumTax?: number = null;
    totalOtherPolicyLevelPremium?: number = null;
    waiverOfSubrogationPremium?: number = null;
    hiredPhysicalDamage?: number = null;
    trailerInterchange?: number = null;
    arsCommissionAmount?: number = null;
    effectiveDate?: Date = null;
    expirationDate?: Date = null;
    addProcessDate?: Date = null;
    addedBy?: number = null;
    removeProcessDate?: Date = null;
    removedBy?: number = null;

    riskCoveragePremiumAvg?: RiskCoveragePremiumAvgDTO | undefined = null;
    constructor (init?: Partial<RiskCoveragePremiumDTO>) {
        Object.assign(this, init);
    }
}

export interface IRiskCoveragePremiumDTO {
    riskCoverageId?: string | undefined;
    autoLiabilityPremium?: number | undefined;
    cargoPremium?: number | undefined;
    physicalDamagePremium?: number | undefined;
    vehicleLevelPremium?: number | undefined;
    avgPerVehicle?: number | undefined;
    generalLiabilityPremium?: number | undefined;
    proratedGeneralLiabilityPremium?: number | undefined;
    riskMgrFeeAL?: number | undefined;
    riskMgrFeePD?: number | undefined;
    policyTotals?: number | undefined;
    premium?: number | undefined;
    proratedPremium?: number | undefined;
    fees?: number | undefined;
    taxesAndStampingFees?: number | undefined;
    installmentFee?: number | undefined;
    perInstallmentTax?: number | undefined;
    perInstallment?: number | undefined;
    totalInstallmentFee?: number | undefined;
    totalAmounDueFull?: number | undefined;
    totalAmountDueInstallment?: number | undefined;
    accountDriverFactor?: number | undefined;
    additionalInsuredPremium?: number | undefined;
    premiumTaxesFees?: number | undefined;
    primaryNonContributoryPremium?: number | undefined;
    subTotalDue?: number | undefined;
    totalAnnualPremiumWithPremiumTax?: number | undefined;
    totalOtherPolicyLevelPremium?: number | undefined;
    waiverOfSubrogationPremium?: number | undefined;
    hiredPhysicalDamage?: number | undefined;
    trailerInterchange?: number | undefined;
    arsCommissionAmount?: number | undefined;
    effectiveDate?: Date | undefined;
    expirationDate?: Date | undefined;
    addProcessDate?: Date | undefined;
    addedBy?: number | undefined;
    removeProcessDate?: Date | undefined;
    removedBy?: number | undefined;

    trailerInterchangeAL?: number | undefined;
    proratedTrailerInterchangePremiumAL?: number | undefined;
    nonOwnedTrailerPremium?: number | undefined;
    proratedNonOwnedTrailerPremium?: number | undefined;

    riskCoveragePremium?: RiskCoveragePremiumDTO | undefined;
}
