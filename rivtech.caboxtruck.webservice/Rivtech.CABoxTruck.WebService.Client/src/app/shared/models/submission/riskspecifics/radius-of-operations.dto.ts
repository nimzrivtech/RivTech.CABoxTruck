export class RadiusOfOperationsDto implements IRadiusOfOperationsDto {
  id?: string = null;
  riskDetailId?: string = null;
  zeroToFiftyMilesPercentage?: number = null;
  fiftyOneToTwoHundredMilesPercentage?: number = null;
  twoHundredPlusMilesPercentage?: number = null;
  ownerOperatorPercentage?: number = null;
  ratingTypeId?: number = null;
  anyDestinationToMexicoPlanned?: boolean = null;
  anyNyc5BoroughsExposure?: boolean = null;
  exposureDescription?: string = null;
  averageMilesPerVehicle?: string = null;

  constructor(init?: Partial<IRadiusOfOperationsDto>) {
    if (init) {
      init.zeroToFiftyMilesPercentage = init.zeroToFiftyMilesPercentage ?? 0;
      init.fiftyOneToTwoHundredMilesPercentage = init.fiftyOneToTwoHundredMilesPercentage ?? 0;
      init.twoHundredPlusMilesPercentage = init.twoHundredPlusMilesPercentage ?? 0;
      init.ownerOperatorPercentage = init.ownerOperatorPercentage ?? 0;
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IRadiusOfOperationsDto {
  id?: string;
  riskDetailId?: string;
  zeroToFiftyMilesPercentage?: number;
  fiftyOneToTwoHundredMilesPercentage?: number;
  twoHundredPlusMilesPercentage?: number;
  ownerOperatorPercentage?: number;
  ratingTypeId?: number;
  anyDestinationToMexicoPlanned?: boolean;
  anyNyc5BoroughsExposure?: boolean;
  exposureDescription?: string;
  averageMilesPerVehicle?: string;
}
