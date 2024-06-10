
export class FormOtherInfoDTO implements IFormOtherInfoDTO {
    id?: string;
    riskDetailId?: string | undefined;

    formId?: string | undefined;
    radius?: string | undefined;
    estimatedAdditionalPremium?: number | undefined;
    vehicleDays?: number | undefined;
    ratePerDay?: number | undefined;
    grossCombinedWeight?: number | undefined;
    accidentLimitPerVehicle?: number | undefined;
    deductible?: number | undefined;
    physdamLimit?: number | undefined;
    requirement?: string | undefined;
    states?: string | undefined;
    statesDefault?: boolean | undefined;

    compreLimitOfInsurance?: number | undefined;
    compreDeductible?: number | undefined;
    comprePremium?: number | undefined;
    lossLimitOfInsurance?: number | undefined;
    lossDeductible?: number | undefined;
    lossPremium?: number | undefined;
    collisionLimitOfInsurance?: number | undefined;
    collisionDeductible?: number | undefined;
    collisionPremium?: number | undefined;
    fireLimitOfInsurance?: number | undefined;
    fireDeductible?: number | undefined;
    firePremium?: number | undefined;
    fireTheftLimitOfInsurance?: number | undefined;
    fireTheftDeductible?: number | undefined;
    fireTheftPremium?: number | undefined;

    premium?: number | undefined;
    description?: string | undefined;

    injuryLimit?: number | undefined;
    medicalExpenses?: number | undefined;
    damagesRentedToYou?: number | undefined;
    aggregateLimit?: number | undefined;

    descriptionOfAutos?: string | undefined;

    addedFirstPartyBenefits: boolean;
    medicalExpenseBenefits?: number | undefined;
    workLossUpTo?: number | undefined;
    workLossPerMonth?: number | undefined;
    funeralExpenseBenefits?: number | undefined;
    accidentalDeathBenefits?: number | undefined;
    combinationFirstPartyBenefits: boolean;
    maximumTotalLimitAllBenefits?: number | undefined;
    combFuneralExpenseBenefits?: number | undefined;
    combAccidentalDeathBenefits?: number | undefined;

    createdBy?: string | undefined;
    createdDate?: Date | undefined;

    constructor(init?: Partial<FormOtherInfoDTO>) {
        Object.assign(this, init);
    }
}

export interface IFormOtherInfoDTO {
    id?: string;
    riskDetailId?: string | undefined;

    formId?: string | undefined;
    radius?: string | undefined;
    estimatedAdditionalPremium?: number | undefined;
    vehicleDays?: number | undefined;
    ratePerDay?: number | undefined;
    grossCombinedWeight?: number | undefined;
    accidentLimitPerVehicle?: number | undefined;
    deductible?: number | undefined;
    physdamLimit?: number | undefined;
    requirement?: string | undefined;
    states?: string | undefined;
    statesDefault?: boolean | undefined;

    compreLimitOfInsurance?: number | undefined;
    compreDeductible?: number | undefined;
    comprePremium?: number | undefined;
    lossLimitOfInsurance?: number | undefined;
    lossDeductible?: number | undefined;
    lossPremium?: number | undefined;
    collisionLimitOfInsurance?: number | undefined;
    collisionDeductible?: number | undefined;
    collisionPremium?: number | undefined;
    fireLimitOfInsurance?: number | undefined;
    fireDeductible?: number | undefined;
    firePremium?: number | undefined;
    fireTheftLimitOfInsurance?: number | undefined;
    fireTheftDeductible?: number | undefined;
    fireTheftPremium?: number | undefined;

    premium?: number | undefined;
    description?: string | undefined;

    injuryLimit?: number | undefined;
    medicalExpenses?: number | undefined;
    damagesRentedToYou?: number | undefined;
    aggregateLimit?: number | undefined;

    descriptionOfAutos?: string | undefined;

    addedFirstPartyBenefits: boolean;
    medicalExpenseBenefits?: number | undefined;
    workLossUpTo?: number | undefined;
    workLossPerMonth?: number | undefined;
    funeralExpenseBenefits?: number | undefined;
    accidentalDeathBenefits?: number | undefined;
    combinationFirstPartyBenefits: boolean;
    maximumTotalLimitAllBenefits?: number | undefined;
    combFuneralExpenseBenefits?: number | undefined;
    combAccidentalDeathBenefits?: number | undefined;
    coveredAutoText?: string | undefined;
    textForEndorsement?: string | undefined;

    createdBy?: string | undefined;
    createdDate?: Date | undefined;
}

