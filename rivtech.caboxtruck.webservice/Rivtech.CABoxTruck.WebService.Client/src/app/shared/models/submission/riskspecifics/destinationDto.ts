export class DestinationDto implements IDestinationDto {
  id?: string = null;
  destination?: string;
  city?: string = null;
  state?: string = null;
  zipCode?: string = null;
  percentageOfTravelGiven?: number = null;
  percentageOfTravelActual?: number = null;
  totalPercentageOfTravelGiven?: number = 0;
  totalPercentageOfTravelActual?: number = 0;
  currentDestinations?: string[] = [];
  riskDetailId?: string = null;

  constructor(init?: Partial<IDestinationDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IDestinationDto {
  id?: string;
  destination?: string;
  city?: string;
  state?: string;
  zipCode?: string;
  percentageOfTravelGiven?: number;
  percentageOfTravelActual?: number;
  totalPercentageOfTravelGiven?: number;
  totalPercentageOfTravelActual?: number;
  currentDestinations?: string[];
  riskDetailId?: string;
}
