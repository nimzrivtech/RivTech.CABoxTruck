import { EntityDTO } from '../entityDto';

export class AgencySubmissionDto implements IagencySubmissionDto {
  id?: string | undefined = null;
  entityId?: string | undefined = null;
  createdBy?: string | undefined = null;
  createdDate?: Date | undefined = null;
  entity?: EntityDTO | undefined = null;

  constructor(init?: Partial<IagencySubmissionDto>) {
    Object.assign(this, init);
  }
}

export interface IagencySubmissionDto {
  id?: string | undefined;
  entityId?: string | undefined;
  createdBy?: string | undefined;
  createdDate?: Date | undefined;
  entity?: EntityDTO | undefined;
}
