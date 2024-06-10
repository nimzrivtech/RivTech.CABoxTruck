import { RiskCoveragePremiumDTO } from './riskCoveragePremiumDto';

export class RiskCoverageDTO implements IRiskCoverageDTO {
    id?: string;
    riskDetailId?: string;
    autoBILimitId?: number | undefined;
    autoPDLimitId?: number | undefined;
    liabilityDeductibleId?: number | undefined;
    medPayLimitId?: number | undefined;
    pipLimitId?: number | undefined;
    umbiLimitId?: number | undefined;
    umpdLimitId?: number | undefined;
    uimbiLimitId?: number | undefined;
    uimpdLimitId?: number | undefined;
    coveredSymbolsId?: number | undefined;
    specifiedPerils?: number | undefined;
    compDeductibleId?: number | undefined;
    fireDeductibleId?: number | undefined;
    collDeductibleId?: number | undefined;
    glbiLimitId?: number | undefined;
    cargoLimitId?: number | undefined;
    refCargoLimitId?: number | undefined;
    trailerInterchangeLimitId?: number | undefined;
    nonOwnedTrailerLimitId?: number | undefined;
    trailerTypeLimitId?: number | undefined;
    pollutionCovLimitId?: number | undefined;

    alSymbolId?: number | undefined;
    pdSymbolId?: number | undefined;
    liabilityDeductibleSymbolId?: number | undefined;
    medPayLimitSymbolId?: number | undefined;
    pipLimitSymbolId?: number | undefined;
    umbiLimitSymbolId?: number | undefined;
    umpdLimitSymbolId?: number | undefined;
    uimbiLimitSymbolId?: number | undefined;
    uimpdLimitSymbolId?: number | undefined;
    compDeductibleSymbolId?: number | undefined;
    fireDeductibleSymbolId?: number | undefined;
    collDeductibleSymbolId?: number | undefined;
    glbiLimitSymbolId?: number | undefined;
    cargoLimitSymbolId?: number | undefined;
    refCargoLimitSymbolId?: number | undefined;
    trailerInterchangeLimitSymbolId?: number | undefined;
    nonOwnedTrailerLimitSymbolId?: number | undefined;

    brokerCommisionAL?: number | undefined;
    brokerCommisionPD?: number | undefined;
    arsCommission?: number | undefined;
    glScheduleRF?: number | undefined;
    glExperienceRF?: number | undefined;
    cargoScheduleRF?: number | undefined;
    cargoExperienceRF?: number | undefined;
    apdScheduleRF?: number | undefined;
    apdExperienceRF?: number | undefined;
    depositPct?: number | undefined;
    depositPremium?: number | undefined;
    numberOfInstallments?: number | undefined;

    liabilityScheduleRatingFactor?: number | undefined;
    liabilityExperienceRatingFactor?: number | undefined;
    subjectivitiesId?: string | undefined;

    endorsementNumber?: number | undefined;
    optionNumber?: number | undefined;
    effectiveDate?: Date;
    expirationDate?: Date;
    addProcessDate?: Date;
    addedBy?: number;

    riskCoveragePremium?: RiskCoveragePremiumDTO | undefined = null;

    constructor(init?: Partial<RiskCoverageDTO>) {
        Object.assign(this, init);
      }


}

export interface IRiskCoverageDTO {
    id?: string;
    riskDetailId?: string;
    autoBILimitId?: number | undefined;
    autoPDLimitId?: number | undefined;
    liabilityDeductibleId?: number | undefined;
    medPayLimitId?: number | undefined;
    pipLimitId?: number | undefined;
    umbiLimitId?: number | undefined;
    umpdLimitId?: number | undefined;
    uimbiLimitId?: number | undefined;
    uimpdLimitId?: number | undefined;
    coveredSymbolsId?: number | undefined;
    specifiedPerils?: number | undefined;
    compDeductibleId?: number | undefined;
    fireDeductibleId?: number | undefined;
    collDeductibleId?: number | undefined;
    glbiLimitId?: number | undefined;
    cargoLimitId?: number | undefined;
    refCargoLimitId?: number | undefined;
    trailerInterchangeLimitId?: number | undefined;
    nonOwnedTrailerLimitId?: number | undefined;
    trailerTypeLimitId?: number | undefined;
    pollutionCovLimitId?: number | undefined;

    alSymbolId?: number | undefined;
    pdSymbolId?: number | undefined;
    liabilityDeductibleSymbolId?: number | undefined;
    medPayLimitSymbolId?: number | undefined;
    pipLimitSymbolId?: number | undefined;
    umbiLimitSymbolId?: number | undefined;
    umpdLimitSymbolId?: number | undefined;
    uimbiLimitSymbolId?: number | undefined;
    uimpdLimitSymbolId?: number | undefined;
    compDeductibleSymbolId?: number | undefined;
    fireDeductibleSymbolId?: number | undefined;
    collDeductibleSymbolId?: number | undefined;
    glbiLimitSymbolId?: number | undefined;
    cargoLimitSymbolId?: number | undefined;
    refCargoLimitSymbolId?: number | undefined;
    trailerInterchangeLimitSymbolId?: number | undefined;
    nonOwnedTrailerLimitSymbolId?: number | undefined;

    brokerCommisionAL?: number | undefined;
    brokerCommisionPD?: number | undefined;
    arsCommission?: number | undefined;
    glScheduleRF?: number | undefined;
    glExperienceRF?: number | undefined;
    cargoScheduleRF?: number | undefined;
    cargoExperienceRF?: number | undefined;
    apdScheduleRF?: number | undefined;
    apdExperienceRF?: number | undefined;
    depositPct?: number | undefined;
    depositAmount?: number | undefined;
    numberOfInstallments?: number | undefined;

    liabilityScheduleRatingFactor?: number | undefined;
    liabilityExperienceRatingFactor?: number | undefined;

    endorsementNumber?: number | undefined;
    optionNumber?: number | undefined;
    effectiveDate?: Date;
    expirationDate?: Date;
    addProcessDate?: Date;
    addedBy?: number;

    riskCoveragePremium?: RiskCoveragePremiumDTO | undefined;
}

export class SaveRiskCoverageDTO implements ISaveRiskCoverageDTO {
    riskDetailId?: string;
    autoBILimitId?: number | undefined;
    autoPDLimitId?: number | undefined;
    liabilityDeductibleId?: number | undefined;
    medPayLimitId?: number | undefined;
    pipLimitId?: number | undefined;
    umbiLimitId?: number | undefined;
    umpdLimitId?: number | undefined;
    uimbiLimitId?: number | undefined;
    uimpdLimitId?: number | undefined;
    coveredSymbolsId?: number | undefined;
    specifiedPerils?: number | undefined;
    compDeductibleId?: number | undefined;
    fireDeductibleId?: number | undefined;
    collDeductibleId?: number | undefined;
    glbiLimitId?: number | undefined;
    cargoLimitId?: number | undefined;
    refCargoLimitId?: number | undefined;
    trailerInterchangeLimitId?: number | undefined;
    nonOwnedTrailerLimitId?: number | undefined;
    trailerTypeLimitId?: number | undefined;
    pollutionCovLimitId?: number | undefined;
    endorsementNumber?: number | undefined;
    optionNumber?: number | undefined;

    alSymbolId?: number | undefined;
    pdSymbolId?: number | undefined;
    liabilityDeductibleSymbolId?: number | undefined;
    medPayLimitSymbolId?: number | undefined;
    pipLimitSymbolId?: number | undefined;
    umbiLimitSymbolId?: number | undefined;
    umpdLimitSymbolId?: number | undefined;
    uimbiLimitSymbolId?: number | undefined;
    uimpdLimitSymbolId?: number | undefined;
    compDeductibleSymbolId?: number | undefined;
    fireDeductibleSymbolId?: number | undefined;
    collDeductibleSymbolId?: number | undefined;
    glbiLimitSymbolId?: number | undefined;
    cargoLimitSymbolId?: number | undefined;
    refCargoLimitSymbolId?: number | undefined;
    trailerInterchangeLimitSymbolId?: number | undefined;
    nonOwnedTrailerLimitSymbolId?: number | undefined;

    brokerCommisionAL?: number | undefined;
    brokerCommisionPD?: number | undefined;
    glScheduleRF?: number | undefined;
    glExperienceRF?: number | undefined;
    cargoScheduleRF?: number | undefined;
    cargoExperienceRF?: number | undefined;
    apdScheduleRF?: number | undefined;
    apdExperienceRF?: number | undefined;
    depositPct?: number | undefined;
    depositAmount?: number | undefined;
    numberOfInstallments?: number | undefined;

    constructor(init?: Partial<SaveRiskCoverageDTO>) {
        Object.assign(this, init);
      }
}

export interface ISaveRiskCoverageDTO {
    riskDetailId?: string;
    autoBILimitId?: number | undefined;
    autoPDLimitId?: number | undefined;
    liabilityDeductibleId?: number | undefined;
    medPayLimitId?: number | undefined;
    pipLimitId?: number | undefined;
    umbiLimitId?: number | undefined;
    umpdLimitId?: number | undefined;
    uimbiLimitId?: number | undefined;
    uimpdLimitId?: number | undefined;
    coveredSymbolsId?: number | undefined;
    specifiedPerils?: number | undefined;
    compDeductibleId?: number | undefined;
    fireDeductibleId?: number | undefined;
    collDeductibleId?: number | undefined;
    glbiLimitId?: number | undefined;
    cargoLimitId?: number | undefined;
    refCargoLimitId?: number | undefined;
    trailerInterchangeLimitId?: number | undefined;
    nonOwnedTrailerLimitId?: number | undefined;
    trailerTypeLimitId?: number | undefined;
    pollutionCovLimitId?: number | undefined;
    endorsementNumber?: number | undefined;
    optionNumber?: number | undefined;

    alSymbolId?: number | undefined;
    pdSymbolId?: number | undefined;
    liabilityDeductibleSymbolId?: number | undefined;
    medPayLimitSymbolId?: number | undefined;
    pipLimitSymbolId?: number | undefined;
    umbiLimitSymbolId?: number | undefined;
    umpdLimitSymbolId?: number | undefined;
    uimbiLimitSymbolId?: number | undefined;
    uimpdLimitSymbolId?: number | undefined;
    compDeductibleSymbolId?: number | undefined;
    fireDeductibleSymbolId?: number | undefined;
    collDeductibleSymbolId?: number | undefined;
    glbiLimitSymbolId?: number | undefined;
    cargoLimitSymbolId?: number | undefined;
    refCargoLimitSymbolId?: number | undefined;
    trailerInterchangeLimitSymbolId?: number | undefined;
    nonOwnedTrailerLimitSymbolId?: number | undefined;

    brokerCommisionAL?: number | undefined;
    brokerCommisionPD?: number | undefined;
    glScheduleRF?: number | undefined;
    glExperienceRF?: number | undefined;
    cargoScheduleRF?: number | undefined;
    cargoExperienceRF?: number | undefined;
    apdScheduleRF?: number | undefined;
    apdExperienceRF?: number | undefined;
    depositPct?: number | undefined;
    depositPremium?: number | undefined;
    numberOfInstallments?: number | undefined;
}

export class RiskCoverageListDTO implements IRiskCoverageListDTO {
    riskDetailId?: string;
    riskCoverages?: RiskCoverageDTO[] | undefined;

    constructor(init?: Partial<RiskCoverageListDTO>) {
        Object.assign(this, init);
      }
}

export interface IRiskCoverageListDTO {
    riskDetailId?: string;
    riskCoverages?: RiskCoverageDTO[] | undefined;
}
