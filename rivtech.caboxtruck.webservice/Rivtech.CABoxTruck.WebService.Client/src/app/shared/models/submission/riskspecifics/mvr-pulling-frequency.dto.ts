export class MvrPullingFrequencyDto implements IMvrPullingFrequencyDto {
  id?: string = null;
  description?: string = null;

  constructor(init?: Partial<IMvrPullingFrequencyDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IMvrPullingFrequencyDto {
  id?: string;
  description?: string;
}
