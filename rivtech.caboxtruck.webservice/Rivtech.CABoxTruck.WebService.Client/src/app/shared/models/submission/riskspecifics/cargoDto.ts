export interface ICargoDto {
    areRequiredToCarryCargoInsurance?: boolean;
    requireRefrigerationBreakdownCoverage?: boolean;
    haulHazardousMaterials?: boolean;
    haveRefrigeratedUnits?: boolean;
    areCommoditiesStoredInTruckOvernight?: boolean;
    areCommoditiesStoredInTruckOvernightExplain?: string;
    hadCargoLoss?: boolean;
    hadCargoLossExplain?: string;
}

export class CargoDto implements ICargoDto {
    areRequiredToCarryCargoInsurance?: boolean;
    requireRefrigerationBreakdownCoverage?: boolean;
    haulHazardousMaterials?: boolean;
    haveRefrigeratedUnits?: boolean;
    areCommoditiesStoredInTruckOvernight?: boolean;
    areCommoditiesStoredInTruckOvernightExplain?: string;
    hadCargoLoss?: boolean;
    hadCargoLossExplain?: string;

    public constructor(init?: Partial<ICargoDto>) {
        Object.assign(this, init);
    }
}