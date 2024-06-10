export class SaferRatingDto implements ISaferRatingDto {
  id?: string = null;
  description?: string = null;

  constructor(init?: Partial<ISaferRatingDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface ISaferRatingDto {
  id?: string;
  description?: string;
}
