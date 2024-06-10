export class FilingTypeDto implements IFilingTypeDto {  
  id: number = 0;
  name: string = '';
  isActive: boolean = false;

  constructor(init?: Partial<IFilingTypeDto>) {
    if (init) {
      for (let property in init) {
        if (init.hasOwnProperty(property))
          (<any>this)[property] = (<any>init)[property];
      }
    }
  }
}

export interface IFilingTypeDto {
  id: number;
  name: string;
  isActive: boolean
}


