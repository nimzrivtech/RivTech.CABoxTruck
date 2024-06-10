export class LvZipCodeDTO implements ILvZipCode {
    zipCode?: number | undefined;
    zipCodeExt?: string | undefined;
    city?: string | undefined;
    stateCode?: string | undefined;
    county?: string | undefined;
    stateFullName?: string | undefined;
  }
  
  export interface ILvZipCode {
    zipCode?: number | undefined;
    zipCodeExt?: string | undefined;
    city?: string | undefined;
    stateCode?: string | undefined;
    county?: string | undefined;
    stateFullName?: string | undefined;
  }
  