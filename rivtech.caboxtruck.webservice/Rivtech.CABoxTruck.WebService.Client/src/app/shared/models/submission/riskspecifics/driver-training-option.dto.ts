export class DriverTrainingOptionDto implements IDriverTrainingOptionDto {
  id?: string = null;
  description?: string = null;

  constructor(init?: Partial<IDriverTrainingOptionDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IDriverTrainingOptionDto {
  id?: string;
  description?: string;
}
