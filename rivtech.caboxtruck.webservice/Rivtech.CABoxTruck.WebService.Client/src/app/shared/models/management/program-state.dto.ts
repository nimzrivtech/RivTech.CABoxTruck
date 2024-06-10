import { AgentUserEntity } from '../data/dto/agent/agentuser-response.dto';
import { ProgramDTO } from './program-dto';

export class ProgramStateDTO implements IProgramStateDTO {
  id?: number;
  programId?: number;
  stateCode?: string;
  effectiveDate?: Date | string;
  expirationDate?: Date | string;
  addProcessDate?: Date | string;
  removeProcessDate?: Date | string | null;
  program?: ProgramDTO;
  entity?: AgentUserEntity;

  constructor(data?: ProgramStateDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IProgramStateDTO {
  id?: number;
  programId?: number;
  stateCode?: string;
  effectiveDate?: Date | string;
  expirationDate?: Date | string;
  addProcessDate?: Date | string;
  removeProcessDate?: Date | string | null;
  program?: ProgramDTO;
}

