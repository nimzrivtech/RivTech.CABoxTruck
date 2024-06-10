export class RiskCoveragePremiumAvgDTO implements IRiskCoveragePremiumAvgDTO {
    avgAlVehicleTotalPremium: number = null;
    avgAlVehiclePowerPremium: number = null;
    avgAlVehicleTrailerOtherPremium: number = null;
    avgApdVehicleTotalPremium: number = null;
    avgApdVehiclePowerPremium: number = null;
    avgApdVehicleTrailerOtherPremium: number = null;
    avgMtcRefVehicleTotalPremium: number = null;
    avgMtcRefVehiclePowerPremium: number = null;
    avgMtcRefVehicleTrailerOtherPremium: number = null;
    avgTotalVehiclePremium: number = null;
    avgTotalVehiclePowerPremium: number = null;
    avgTotalVehicleTrailerOtherPremium: number = null;

    constructor (init?: Partial<RiskCoveragePremiumAvgDTO>) {
        Object.assign(this, init);
    }
}

export interface IRiskCoveragePremiumAvgDTO {
    avgAlVehicleTotalPremium?: number | undefined;
    avgAlVehiclePowerPremium?: number | undefined;
    avgAlVehicleTrailerOtherPremium?: number | undefined;
    avgApdVehicleTotalPremium?: number | undefined;
    avgApdVehiclePowerPremium?: number | undefined;
    avgApdVehicleTrailerOtherPremium?: number | undefined;
    avgMtcRefVehicleTotalPremium?: number | undefined;
    avgMtcRefVehiclePowerPremium?: number | undefined;
    avgMtcRefVehicleTrailerOtherPremium?: number | undefined;
    avgTotalVehiclePremium?: number | undefined;
    avgTotalVehiclePowerPremium?: number | undefined;
    avgTotalVehicleTrailerOtherPremium?: number | undefined;
}
