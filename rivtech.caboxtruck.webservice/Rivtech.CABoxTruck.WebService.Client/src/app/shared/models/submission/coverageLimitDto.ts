import { DropdownDTO } from './dropdownDto';

export class CoverageLimitsDTO implements ICoverageLimitsDTO {
    aL_BodilyInjuryDTO?: DropdownDTO[] | undefined;
    aL_PropertyDamageDTO?: DropdownDTO[] | undefined;
    liabilityDeductibleDTO?: DropdownDTO[] | undefined;
    uIM_BodilyInjuryDTO?: DropdownDTO[] | undefined;
    uIM_PropertyDamageDTO?: DropdownDTO[] | undefined;
    uDIM_BodilyInjuryDTO?: DropdownDTO[] | undefined;
    uDIM_PropertyDamageDTO?: DropdownDTO[] | undefined;
    medPaymentLimitDTO?: DropdownDTO[] | undefined;
    personalInjuryProtectionDTO?: DropdownDTO[] | undefined;
    aL_SymbolCoveredDTO?: DropdownDTO[] | undefined;
    causeOfLossDTO?: DropdownDTO[] | undefined;
    comprehensiveDTO?: DropdownDTO[] | undefined;
    collisionDTO?: DropdownDTO[] | undefined;

    constructor(init?: Partial<CoverageLimitsDTO>) {
        Object.assign(this, init);
      }
}

export interface ICoverageLimitsDTO {
    aL_BodilyInjuryDTO?: DropdownDTO[] | undefined;
    aL_PropertyDamageDTO?: DropdownDTO[] | undefined;
    liabilityDeductibleDTO?: DropdownDTO[] | undefined;
    uIM_BodilyInjuryDTO?: DropdownDTO[] | undefined;
    uIM_PropertyDamageDTO?: DropdownDTO[] | undefined;
    uDIM_BodilyInjuryDTO?: DropdownDTO[] | undefined;
    uDIM_PropertyDamageDTO?: DropdownDTO[] | undefined;
    medPaymentLimitDTO?: DropdownDTO[] | undefined;
    personalInjuryProtectionDTO?: DropdownDTO[] | undefined;
    aL_SymbolCoveredDTO?: DropdownDTO[] | undefined;
    causeOfLossDTO?: DropdownDTO[] | undefined;
    comprehensiveDTO?: DropdownDTO[] | undefined;
    collisionDTO?: DropdownDTO[] | undefined;
}

export class OtherCoveragesDTO implements IOtherCoveragesDTO {
    gL_BodilyInjuryDTO?: DropdownDTO[] | undefined;
    cargoDTO?: DropdownDTO[] | undefined;
    refrigBreakdownDTO?: DropdownDTO[] | undefined;

    constructor(init?: Partial<OtherCoveragesDTO>) {
        Object.assign(this, init);
      }
}

export interface IOtherCoveragesDTO {
    gL_BodilyInjuryDTO?: DropdownDTO[] | undefined;
    cargoDTO?: DropdownDTO[] | undefined;
    refrigBreakdownDTO?: DropdownDTO[] | undefined;
}

