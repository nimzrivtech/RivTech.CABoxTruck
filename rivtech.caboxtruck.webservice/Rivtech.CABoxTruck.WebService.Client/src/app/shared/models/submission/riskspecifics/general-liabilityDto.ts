export interface IGeneralLiabilityDto {
    contractuallyRequiredToCarryGeneralLiabilityInsurance?: boolean;
    haveOperationsOtherThanTrucking?: boolean;
    haveOperationsOtherThanTruckingExplain?: string;
    operationsAtStorageLotOrImpoundYard?: boolean;
    anyStorageOfGoods?: boolean;
    anyWarehousing?: boolean;
    anyStorageOfVehiclesForOthers?: boolean;
    anyLeasingSpaceToOthers?: boolean;
    anyFreightForwarding?: boolean;
    anyStorageOfFuelsAndOrChemicals?: boolean;
    hasApplicantHadGeneralLiabilityLoss?: boolean;
    hasApplicantHadGeneralLiabilityLossExplain?: string;
    glAnnualPayroll?: number;
}

export class GeneralLiabilityDto implements IGeneralLiabilityDto {
    contractuallyRequiredToCarryGeneralLiabilityInsurance?: boolean;
    haveOperationsOtherThanTrucking?: boolean;
    haveOperationsOtherThanTruckingExplain?: string;
    operationsAtStorageLotOrImpoundYard?: boolean;
    anyStorageOfGoods?: boolean;
    anyWarehousing?: boolean;
    anyStorageOfVehiclesForOthers?: boolean;
    anyLeasingSpaceToOthers?: boolean;
    anyFreightForwarding?: boolean;
    anyStorageOfFuelsAndOrChemicals?: boolean;
    hasApplicantHadGeneralLiabilityLoss?: boolean;
    hasApplicantHadGeneralLiabilityLossExplain?: string;
    glAnnualPayroll?: number;

    public constructor(init?: Partial<IGeneralLiabilityDto>) {
        Object.assign(this, init);
    }
}