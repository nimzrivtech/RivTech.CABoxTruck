export class TrafficLightRatingDto implements ITrafficLightRatingDto {
  id?: string = null;
  description?: string = null;

  constructor(init?: Partial<ITrafficLightRatingDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface ITrafficLightRatingDto {
  id?: string;
  description?: string;
}
