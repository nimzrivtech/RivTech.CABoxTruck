export class EntitySubsidiaryDTO implements IEntitySubsidiaryDTO {
    id?: string;
    entityId?: string;
    isSubsidiaryCompany?: boolean;
    name?: string | undefined;
    typeOfBusinessId?: number | undefined;
    numVechSizeComp?: number;
    relationship?: string | undefined;
    isIncludedInInsurance?: boolean;
    isCurrentlyOwned?: boolean;
    nameYearOperations?: number | undefined;
    isOthersAllowedOperate?: boolean;
    isOthersAllowedPrevOperate?: boolean;
    effectiveDate?: Date;
    expirationDate?: Date | undefined;

    constructor(init?: Partial<EntitySubsidiaryDTO>) {
        Object.assign(this, init);
      }
}

export interface IEntitySubsidiaryDTO {
    id?: string;
    entityId?: string;
    isSubsidiaryCompany?: boolean;
    name?: string | undefined;
    typeOfBusinessId?: number | undefined;
    numVechSizeComp?: number;
    relationship?: string | undefined;
    isIncludedInInsurance?: boolean;
    isCurrentlyOwned?: boolean;
    nameYearOperations?: number | undefined;
    isOthersAllowedOperate?: boolean;
    isOthersAllowedPrevOperate?: boolean;
    effectiveDate?: Date;
    expirationDate?: Date | undefined;
}

export class SaveEntitySubsidiaryDTO implements ISaveEntitySubsidiaryDTO {
    id?: string;
    riskDetailId?: string;
    entityId?: string;
    isSubsidiaryCompany?: boolean;
    name?: string | undefined;
    typeOfBusinessId?: number | undefined;
    numVechSizeComp?: number;
    relationship?: string | undefined;
    isIncludedInInsurance?: boolean;
    isCurrentlyOwned?: boolean;
    nameYearOperations?: number | undefined;
    isOthersAllowedOperate?: boolean;
    isOthersAllowedPrevOperate?: boolean;

    constructor(init?: Partial<SaveEntitySubsidiaryDTO>) {
        Object.assign(this, init);
      }
}

export interface ISaveEntitySubsidiaryDTO {
    id?: string;
    entityId?: string;
    isSubsidiaryCompany?: boolean;
    name?: string | undefined;
    typeOfBusinessId?: number | undefined;
    numVechSizeComp?: number;
    relationship?: string | undefined;
    isIncludedInInsurance?: boolean;
    isCurrentlyOwned?: boolean;
    nameYearOperations?: number | undefined;
    isOthersAllowedOperate?: boolean;
    isOthersAllowedPrevOperate?: boolean;
}
