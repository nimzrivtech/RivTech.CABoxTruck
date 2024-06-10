export class CityStateZipDto implements ICityStateZipDto {
  id?: string = null;
  city?: string = null;
  stateCode?: string = null;
  zipCode?: string = null;

  constructor(init?: Partial<ICityStateZipDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface ICityStateZipDto {
  id?: string;
  city?: string;
  stateCode?: string;
  zipCode?: string;
}
