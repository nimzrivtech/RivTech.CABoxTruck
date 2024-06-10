export class RiskHistoryDTO implements IRiskHistoryDTO {
    id?: string | undefined;
    riskDetailId?: string;
    historyYear?: number | undefined;
    policyTermStartDate?: Date | undefined;
    policyTermEndDate?: Date | undefined;
    insuranceCarrierOrBroker?: string | undefined;
    liabilityLimits?: string | undefined;
    liabilityDeductible?: string | undefined;
    collisionDeductible?: string | undefined;
    comprehensiveDeductible?: string | undefined;
    cargoLimits?: string | undefined;
    refrigeratedCargoLimits?: string | undefined;
    autoLiabilityPremiumPerVehicle?: number | undefined;
    physicalDamagePremiumPerVehicle?: number | undefined;
    totalPremiumPerVehicle?: number | undefined;
    numberOfVehicles?: number | undefined;
    numberOfPowerUnits?: number | undefined;
    numberOfPowerUnitsAL?: number | undefined;
    numberOfPowerUnitsAPD?: number | undefined;
    numberOfSpareVehicles?: number | undefined;
    numberOfDrivers?: number | undefined;
    lossRunDateForEachTerm?: Date | undefined;
    grossRevenues?: number | undefined;
    totalFleetMileage?: number | undefined;
    numberOfOneWayTrips?: number | undefined;

    constructor(init?: Partial<RiskHistoryDTO>) {
        Object.assign(this, init);
      }
}

export interface IRiskHistoryDTO {
    id?: string | undefined;
    riskDetailId?: string;
    historyYear?: number | undefined;
    policyTermStartDate?: Date | undefined;
    policyTermEndDate?: Date | undefined;
    insuranceCarrierOrBroker?: string | undefined;
    liabilityLimits?: string | undefined;
    liabilityDeductible?: string | undefined;
    collisionDeductible?: string | undefined;
    comprehensiveDeductible?: string | undefined;
    cargoLimits?: string | undefined;
    refrigeratedCargoLimits?: string | undefined;
    autoLiabilityPremiumPerVehicle?: number | undefined;
    physicalDamagePremiumPerVehicle?: number | undefined;
    totalPremiumPerVehicle?: number | undefined;
    numberOfVehicles?: number | undefined;
    numberOfPowerUnits?: number | undefined;
    numberOfPowerUnitsAL?: number | undefined;
    numberOfPowerUnitsAPD?: number | undefined;
    numberOfSpareVehicles?: number | undefined;
    numberOfDrivers?: number | undefined;
    lossRunDateForEachTerm?: Date | undefined;
    grossRevenues?: number | undefined;
    totalFleetMileage?: number | undefined;
    numberOfOneWayTrips?: number | undefined;
}

export class RiskHistoryListDTO implements IRiskHistoryListDTO {
    riskDetailId?: string;
    riskHistories?: RiskHistoryDTO[] | undefined;

    constructor(init?: Partial<RiskHistoryListDTO>) {
        Object.assign(this, init);
      }
}

export interface IRiskHistoryListDTO {
    riskDetailId?: string;
    riskHistories?: RiskHistoryDTO[] | undefined;
}
