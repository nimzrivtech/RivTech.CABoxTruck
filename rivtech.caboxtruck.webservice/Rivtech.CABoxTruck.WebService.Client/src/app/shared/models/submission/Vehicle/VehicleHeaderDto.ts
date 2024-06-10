export class VehicleHeaderDto implements IVehicleHeaderDto {
    riskDetailId?: string | undefined;
    trailerTypeLimitId?: number | undefined;
    mainUseId?: number | undefined;
    mainGVWId?: number | undefined;
    secondaryBusinessGroupId?: number | undefined;
    accountCategoryId?: number | undefined;
    accountCategoryUWId?: number | undefined;
    optionNumber?: number | undefined;

    constructor(init?: Partial<VehicleHeaderDto>) {
        Object.assign(this, init);
      }
}

export interface IVehicleHeaderDto {
    riskDetailId?: string | undefined;
    trailerTypeLimitId?: number | undefined;
    mainUseId?: number | undefined;
    mainGVWId?: number | undefined;
    secondaryBusinessGroupId?: number | undefined;
    accountCategoryId?: number | undefined;
    accountCategoryUWId?: number | undefined;
    optionNumber?: number | undefined;
}

