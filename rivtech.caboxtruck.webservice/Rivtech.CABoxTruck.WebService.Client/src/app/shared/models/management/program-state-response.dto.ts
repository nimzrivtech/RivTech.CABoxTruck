import { ProgramStateDTO } from './program-state.dto';

export class ProgramStateResponseDTO implements IProgramStateResponseDTO {
    stateCodes?: StateCodeValuePair[];
    programStates?: ProgramStateDTO[];
  constructor(data?: ProgramStateResponseDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IProgramStateResponseDTO {
  stateCodes?: StateCodeValuePair[];
  programStates?: ProgramStateDTO[];
}

export class StateCodeValuePair {
  stateCode?: string;
  stateName?: string;
}