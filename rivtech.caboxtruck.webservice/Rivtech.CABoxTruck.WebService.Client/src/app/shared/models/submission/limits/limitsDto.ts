
export class LimitsDTO implements ILimitsDTO {
    autoBILimits?: LimitDropdownDTO[] | undefined;
    autoPDLimits?: LimitDropdownDTO[] | undefined;
    umbiLimits?: LimitDropdownDTO[] | undefined;
    umpdLimits?: LimitDropdownDTO[] | undefined;
    uimbiLimits?: LimitDropdownDTO[] | undefined;
    uimpdLimits?: LimitDropdownDTO[] | undefined;
    medPayLimits?: LimitDropdownDTO[] | undefined;
    pipLimits?: LimitDropdownDTO[] | undefined;
    liabilityDeductibleLimits?: LimitDropdownDTO[] | undefined;
    comprehensiveLimits?: LimitDropdownDTO[] | undefined;
    collisionLimits?: LimitDropdownDTO[] | undefined;
    glLimits?: LimitDropdownDTO[] | undefined;
    cargoLimits?: LimitDropdownDTO[] | undefined;
    refLimits?: LimitDropdownDTO[] | undefined;
    trailerInterchangeLimits?: LimitDropdownDTO[] | undefined;
    nonOwnedTrailerLimits?: LimitDropdownDTO[] | undefined;
    trailerTypeLimits?: LimitDropdownDTO[] | undefined;
    pollutionCovLimits?: LimitDropdownDTO[] | undefined;

    constructor(init?: Partial<LimitsDTO>) {
        Object.assign(this, init);
      }
}

export interface ILimitsDTO {
    autoBILimits?: LimitDropdownDTO[] | undefined;
    autoPDLimits?: LimitDropdownDTO[] | undefined;
    umbiLimits?: LimitDropdownDTO[] | undefined;
    umpdLimits?: LimitDropdownDTO[] | undefined;
    uimbiLimits?: LimitDropdownDTO[] | undefined;
    uimpdLimits?: LimitDropdownDTO[] | undefined;
    medPayLimits?: LimitDropdownDTO[] | undefined;
    pipLimits?: LimitDropdownDTO[] | undefined;
    liabilityDeductibleLimits?: LimitDropdownDTO[] | undefined;
    comprehensiveLimits?: LimitDropdownDTO[] | undefined;
    collisionLimits?: LimitDropdownDTO[] | undefined;
    glLimits?: LimitDropdownDTO[] | undefined;
    cargoLimits?: LimitDropdownDTO[] | undefined;
    refLimits?: LimitDropdownDTO[] | undefined;
    trailerInterchangeLimits?: LimitDropdownDTO[] | undefined;
    nonOwnedTrailerLimits?: LimitDropdownDTO[] | undefined;
    trailerTypeLimits?: LimitDropdownDTO[] | undefined;
    pollutionCovLimits?: LimitDropdownDTO[] | undefined;
}

export class LimitDropdownDTO implements ILimitDropdownDTO {
    id?: number;
    limitDisplay?: string | undefined;
    limitValue?: string | undefined;
    singleLimit?: string | undefined;
    perPerson?: string | undefined;
    perAccident?: string | undefined;
    stateCode: string | undefined;
    isActive?: boolean;

    constructor(init?: Partial<LimitDropdownDTO>) {
        Object.assign(this, init);
      }
}

export interface ILimitDropdownDTO {
    id?: number;
    limitDisplay?: string | undefined;
    limitValue?: string | undefined;
    singleLimit?: string | undefined;
    perPerson?: string | undefined;
    perAccident?: string | undefined;
    stateCode: string | undefined;
    isActive?: boolean;
}


