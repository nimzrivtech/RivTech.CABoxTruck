import { EntityDTO } from '../entityDto';

export class SubAgentSubmissionDto implements IsubAgentSubmissionDto {
  id?: string | undefined = null;
  SubAgencyId?: string | undefined = null;
  entityId?: string | undefined = null;
  effectiveDate?: Date | undefined = null;
  expirationDate?: Date | undefined = null;
  addProcessDate?: Date | undefined = null;
  removeProcessDate?: Date | undefined = null;
  entity?: EntityDTO | undefined = null;

  constructor(init?: Partial<IsubAgentSubmissionDto>) {
    Object.assign(this, init);
  }
}

export interface IsubAgentSubmissionDto {
  id?: string | undefined;
  SubAgencyId?: string | undefined;
  entityId?: string | undefined;
  effectiveDate?: Date | undefined;
  expirationDate?: Date | undefined;
  addProcessDate?: Date | undefined;
  removeProcessDate?: Date | undefined;
  entity?: EntityDTO | undefined;
}
