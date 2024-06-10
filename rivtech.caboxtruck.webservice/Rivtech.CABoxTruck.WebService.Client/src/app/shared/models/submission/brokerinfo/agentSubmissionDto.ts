import { EntityDTO } from '../entityDto';

export class AgentSubmissionDto implements IagentSubmissionDto {
  id?: string | undefined = null;
  agencyId?: string | undefined = null;
  entityId?: string | undefined = null;
  createdBy?: string | undefined = null;
  createdDate?: Date | undefined = null;
  entity?: EntityDTO | undefined = null;

  constructor(init?: Partial<IagentSubmissionDto>) {
    Object.assign(this, init);
  }
}

export interface IagentSubmissionDto {
  id?: string | undefined;
  agencyId?: string | undefined;
  entityId?: string | undefined;
  createdBy?: string | undefined;
  createdDate?: Date | undefined;
  entity?: EntityDTO | undefined;
}
