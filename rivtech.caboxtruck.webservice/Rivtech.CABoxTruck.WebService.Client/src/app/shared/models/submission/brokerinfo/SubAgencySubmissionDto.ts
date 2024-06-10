import { EntityDTO } from '../entityDto';

export class SubAgencySubmissionDto implements ISubAgencySubmissionDto {
  id?: string | undefined = null;
  agencyId?: string | undefined = null;
  entityId?: string | undefined = null;
  effectiveDate?: Date | undefined = null;
  expirationDate?: Date | undefined = null;
  addProcessDate?: Date | undefined = null;
  removeProcessDate?: Date | undefined = null;
  entity?: EntityDTO | undefined = null;

  constructor(init?: Partial<ISubAgencySubmissionDto>) {
    Object.assign(this, init);
  }
}

export interface ISubAgencySubmissionDto {
  id?: string | undefined;
  agencyId?: string | undefined;
  entityId?: string | undefined;
  effectiveDate?: Date | undefined;
  expirationDate?: Date | undefined;
  addProcessDate?: Date | undefined;
  removeProcessDate?: Date | undefined;
  entity?: EntityDTO | undefined;
}
