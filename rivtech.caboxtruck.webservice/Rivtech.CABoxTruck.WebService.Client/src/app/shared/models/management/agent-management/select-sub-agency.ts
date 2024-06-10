export class SelectSubAgency implements ISelectSubAgency {
    id?: string;
    subAgency?: string;
    subAgencyId?: string;
    selected: boolean;
  
    constructor(data?: ISelectSubAgency) {
      if (data) {
          for (var property in data) {
              if (data.hasOwnProperty(property))
                  (<any>this)[property] = (<any>data)[property];
          }
      }
    }
  }
  
  export interface ISelectSubAgency {
    id?: string;
    subAgency?: string;
    subAgencyId?: string;
    selected: boolean;
  }