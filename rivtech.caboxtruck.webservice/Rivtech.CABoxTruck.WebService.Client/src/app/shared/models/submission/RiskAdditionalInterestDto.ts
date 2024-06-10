import { EntityDTO } from './entityDto';

export class RiskAdditionalInterestDTO implements IRiskAdditionalInterestDTO {
    id?: string;
    riskDetailId?: string;
    entityId?: string;
    additionalInterestTypeId?: number;
    effectiveDate?: Date | undefined;
    expirationDate?: Date | undefined;
    isWaiverOfSubrogation: boolean = false;
    isPrimaryNonContributory: boolean = false;
    isAutoLiability: boolean = false;
    isGeneralLiability: boolean = false;
    is30DayNotice: boolean = false;
    endorsementNumber?: number;
    entity?: EntityDTO;
    addProcessDate?: Date | undefined;
    isActive: boolean = false;
    fromPreviousEndorsement: boolean = false;
    removeProcessDate?: Date | undefined;

    constructor(init?: Partial<RiskAdditionalInterestDTO>) {
        Object.assign(this, init);
      }
}

export interface IRiskAdditionalInterestDTO {
    id?: string;
    riskDetailId?: string;
    entityId?: string;
    additionalInterestTypeId?: number;
    effectiveDate?: Date | undefined;
    expirationDate?: Date | undefined;
    isWaiverOfSubrogation: boolean | undefined;
    isPrimaryNonContributory: boolean | undefined;
    isAutoLiability: boolean | undefined;
    isGeneralLiability: boolean | undefined;
    is30DayNotice: boolean | undefined;
    endorsementNumber?: number;
    entity?: EntityDTO;
    addProcessDate?: Date | undefined;
    isActive: boolean | undefined;
    fromPreviousEndorsement: boolean | undefined;
    removeProcessDate?: Date | undefined;
}

// TODO: Refactor, this may not be needed
export class SaveRiskAdditionalInterestDTO implements ISaveRiskAdditionalInterestDTO {
    id?: string = '';
    riskDetailId?: string = '';
    additionalInterestTypeId?: number | undefined = null;
    effectiveDate?: string | undefined = '';
    expirationDate?: string | undefined ='';
    endorsementNumber?: number;
    name?: string | undefined = '';
    address?: string | undefined = '';
    city?: string | undefined = null;
    state?: string | undefined = '';
    zipCode?: number | undefined = null;
    isWaiverOfSubrogation: boolean = false;
    isPrimaryNonContributory: boolean = false;
    isAutoLiability: boolean = false;
    isGeneralLiability: boolean = false;
    is30DayNotice: boolean = false;
    fromPreviousEndorsement: boolean = false;

    constructor(init?: Partial<SaveRiskAdditionalInterestDTO>) {
        Object.assign(this, init);
      }
}

export interface ISaveRiskAdditionalInterestDTO {
    id?: string;
    riskDetailId?: string;
    additionalInterestTypeId?: number;
    effectiveDate?: string | undefined;
    expirationDate?: string | undefined;
    endorsementNumber?: number;
    name?: string | undefined;
    address?: string | undefined;
    city?: string | undefined;
    state?: string | undefined;
    zipCode?: number | undefined;
    isWaiverOfSubrogation: boolean;
    isPrimaryNonContributory: boolean;
    fromPreviousEndorsement: boolean | undefined;
}

export class RiskAdditionalInterestsDTO {
    id?: string;
    riskDetailId?: string;
    additionalInterestTypeId?: number;
    effectiveDate?: string | undefined;
    expirationDate?: string | undefined;
    endorsementNumber?: number;
    name?: string | undefined;
    address?: string | undefined;
    city?: string | undefined;
    state?: string | undefined;
    zipCode?: number | undefined;
    isWaiverOfSubrogation: boolean;
    isPrimaryNonContributory: boolean;
    entityType?: string | undefined;
    isActive: boolean = false;

    isValidated: boolean = false;

    constructor(init?: Partial<RiskAdditionalInterestsDTO>) {
        // Object.assign(this, init);
        if (init) {
            for (const property in init) {
              if (init.hasOwnProperty(property)) {
                (<any>this)[property] = (<any>init)[property];
              }
            }
        }
    }
}

export class RiskAddnlInterestPaginationResultDTO implements IRiskAddnlPaginationInterestResultDTO {
    currentPage?: number | undefined;
    firstPage?: number | undefined;
    lastPage?: number | undefined;
    previousPage?: number | undefined;
    totalCount?:  number | undefined;
    totalPages?: number | undefined;
  }
  
  export interface IRiskAddnlPaginationInterestResultDTO  {
    currentPage?: number | undefined;
    firstPage?: number | undefined;
    lastPage?: number | undefined;
    previousPage?: number | undefined;
    totalCount?:  number | undefined;
    totalPages?: number | undefined;
  }

export class RiskAddnlInterestListResultDTO implements IRiskAddnlInterestListResultDTO {
    paginationDetails: RiskAddnlInterestPaginationResultDTO | undefined;
    additionalInterest: RiskAdditionalInterestsDTO[]| undefined;
  }
 
  export interface IRiskAddnlInterestListResultDTO{
    paginationDetails: RiskAddnlInterestPaginationResultDTO | undefined;
    additionalInterest: RiskAdditionalInterestsDTO[] | undefined;
  }
  export class RiskAddnlInterestListRequestDTO implements IRiskAddnlInterestListRequestDTO {
    pageNumber?: number;
    pageSize?: number;
    riskDetailId?: string | undefined;
    fromEndorsement? : boolean;
    sortField?: string;
    order?: string;
  }
  export interface IRiskAddnlInterestListRequestDTO {
    pageNumber?: number;
    pageSize?: number;
    riskDetailId?: string | undefined;
    fromEndorsement? : boolean;
    sortField?: string;
    order?: string;
  }

 