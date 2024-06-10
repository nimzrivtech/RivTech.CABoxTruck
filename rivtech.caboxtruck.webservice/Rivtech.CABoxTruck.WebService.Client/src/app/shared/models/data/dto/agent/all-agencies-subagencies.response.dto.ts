import { EnumerationDTO2 } from "../billing/common/enumeration.dto";

export class AllAgenciesSubAgenciesResponseDTO implements IAllAgenciesSubAgenciesResponseDTO {
    agentId?: string | undefined;
    agencies?: EnumerationDTO2[];
    subAgencies?: EnumerationDTO2[];


    constructor(data?: AllAgenciesSubAgenciesResponseDTO) {
      if (data) {
        for (const property in data) {
          if (data.hasOwnProperty(property)) {
            (<any>this)[property] = (<any>data)[property];
          }
        }
      }
    }
  }

  export interface IAllAgenciesSubAgenciesResponseDTO {
    agentId?: string | undefined;
    agencies?: EnumerationDTO2[];
    subAgencies?: EnumerationDTO2[];
  }