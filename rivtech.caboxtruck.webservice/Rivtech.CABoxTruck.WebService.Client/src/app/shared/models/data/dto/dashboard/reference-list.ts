export class ReferenceListDetailsDTO implements IReferenceListDetailsDTO {
    programCode?: string;
    stateCode?: string;
    description?: string;
    url?: string;
    effectiveDate?: string;
    expirationDate?: string;
    displayOrder?: string;
    constructor(data?: IReferenceListDetailsDTO) {
      if (data) {
        for (const property in data) {
          if (data.hasOwnProperty(property)) {
            (<any>this)[property] = (<any>data)[property];
          }
        }
      }
    }
  }
  export interface IReferenceListDetailsDTO {
    programCode?: string;
    stateCode?: string;
    description?: string;
    url?: string;
    effectiveDate?: string;
    expirationDate?: string;
    displayOrder?: string;
  }