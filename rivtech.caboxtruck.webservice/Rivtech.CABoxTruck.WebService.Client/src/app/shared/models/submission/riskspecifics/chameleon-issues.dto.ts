export class ChameleonIssuesDto implements IChameleonIssuesDto {
  id?: string = null;
  description?: string = null;

  constructor(init?: Partial<IChameleonIssuesDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IChameleonIssuesDto {
  id?: string;
  description?: string;
}
