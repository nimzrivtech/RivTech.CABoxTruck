export class BackgroundCheckOptionDto implements IBackgroundCheckOptionDto {
  id?: string = null;
  description?: string = null;

  constructor(init?: Partial<IBackgroundCheckOptionDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IBackgroundCheckOptionDto {
  id?: string;
  description?: string;
}
