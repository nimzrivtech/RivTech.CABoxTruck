export class RiskSummaryDto implements IRiskSummaryDto {
  id?: string = null;
  currentRiskId?: string = null;
  submissionNumber?: string = null;
  policyNumber?: string = null;
  insuredName?: string = null;
  broker?: string = null;
  inceptionDate?: Date = null;
  expirationDate?: Date = null;
  status?: string = null;
  numberOfUnits?: string = null;
  cntPowerUnit?: string = null;
  cntTrailer?: string = null;
  cntOther?: string = null;
  state?: string = null;
  submissionType?: string = null;
  policyLimit?: string = null;
  newVenture?: string = null;
  neededBy?: string = null;
  brokerContact?: string = null;
  qAC?: string = null;
  midTerm?: string = null;
  lastNoteAdded?: string = null;
  auSpecialist?: string = null;
  auRep?: string = null;
  aupsr?: string = null;
  brokerZip?: string = null;
  brokerState?: string = null;
  brokerCity?: string = null;
  dateSubmitted?: Date = null;
  underwriter?: string = null;
  expiringPolicy?: string = null;
  useClass?: string = null;
  lossRatio?: string = null;
  assignedToId?: string;
  assignedToName?: string;
  insuredDBA?: string;

  constructor(init?: Partial<IRiskSummaryDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IRiskSummaryDto {
  id?: string;
  currentRiskId?: string;
  submissionNumber?: string;
  policyNumber?: string;
  insuredName?: string;
  broker?: string;
  inceptionDate?: Date;
  expirationDate?: Date;
  status?: string;
  numberOfUnits?: string;
  cntPowerUnit?: string;
  cntTrailer?: string;
  cntOther?: string;
  state?: string;
  submissionType?: string;
  policyLimit?: string;
  newVenture?: string;
  neededBy?: string;
  brokerContact?: string;
  qAC?: string;
  midTerm?: string;
  lastNoteAdded?: string;
  auSpecialist?: string;
  auRep?: string;
  aupsr?: string;
  brokerZip?: string;
  brokerState?: string;
  brokerCity?: string;
  dateSubmitted?: Date;
  underwriter?: string;
  expiringPolicy?: string;
  useClass?: string;
  lossRatio?: string;
  assignedToId?: string;
  assignedToName?: string;
  insuredDBA?: string;
}

export class PaginationResultDTO implements IPaginationResultDTO {
  currentPage?: number | undefined;
  firstPage?: number | undefined;
  lastPage?: number | undefined;
  previousPage?: number | undefined;
  totalCount?:  number | undefined;
  totalPages?: number | undefined;
}

export interface IPaginationResultDTO  {
  currentPage?: number | undefined;
  firstPage?: number | undefined;
  lastPage?: number | undefined;
  previousPage?: number | undefined;
  totalCount?:  number | undefined;
  totalPages?: number | undefined;
}
export class ListResultDTO implements IListResultDTO {
  paginationDetails: PaginationResultDTO | undefined;
  riskSummaryDtos: RiskSummaryDto[] | undefined;
}

export interface IListResultDTO {
  paginationDetails: PaginationResultDTO | undefined;
  riskSummaryDtos: RiskSummaryDto[] | undefined;
}
