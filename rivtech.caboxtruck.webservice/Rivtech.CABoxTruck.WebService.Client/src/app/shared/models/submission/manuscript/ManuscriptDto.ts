
export class ManuscriptDTO implements IManuscriptDTO {
    id?: string;
    riskDetailId: string | undefined;
    title: string | undefined;
    description: string | undefined;
    premium: number | undefined;
    proratedPremium: number | undefined;
    isProrate: boolean | undefined;
    isSelected?: boolean | undefined;
    isActive?: boolean | undefined;
    effectiveDate?: Date | undefined;
    expirationDate?: Date | undefined;
    deletedDate?: Date | undefined;
    createdBy?: number | undefined;
    createdDate?: Date | undefined;
    premiumType: string | undefined;
    fromPreviousEndorsement: boolean | undefined;

    constructor(init?: Partial<ManuscriptDTO>) {
        Object.assign(this, init);
      }
}

export interface IManuscriptDTO {
    id?: string;
    riskDetailId: string | undefined;
    title: string | undefined;
    description: string | undefined;
    premium: number | undefined;
    proratedPremium: number | undefined;
    isProrate: boolean | undefined;
    isSelected?: boolean | undefined;
    isActive?: boolean | undefined;
    effectiveDate?: Date | undefined;
    expirationDate?: Date | undefined;
    deletedDate?: Date | undefined;
    createdBy?: number | undefined;
    createdDate?: Date | undefined;
    premiumType: string | undefined;
    fromPreviousEndorsement: boolean | undefined;
}

export class ManuscriptPaginationResulDTO implements IManuscriptPaginationResultDTO {
    currentPage?: number | undefined;
    firstPage?: number | undefined;
    lastPage?: number | undefined;
    previousPage?: number | undefined;
    totalCount?:  number | undefined;
    totalPages?: number | undefined;
  }
  
  export interface IManuscriptPaginationResultDTO  {
    currentPage?: number | undefined;
    firstPage?: number | undefined;
    lastPage?: number | undefined;
    previousPage?: number | undefined;
    totalCount?:  number | undefined;
    totalPages?: number | undefined;
  }

export class ManuscriptListResultDTO implements IManuscriptListResultDTO {
    paginationDetails: ManuscriptPaginationResulDTO | undefined;
    riskManuscript: ManuscriptDTO[]| undefined;
  }
 
  export interface IManuscriptListResultDTO{
    paginationDetails: ManuscriptPaginationResulDTO | undefined;
    riskManuscript: ManuscriptDTO[] | undefined;
  }
  export class ManuscriptListRequestDTO implements IManuscriptListRequestDTO {
    pageNumber?: number;
    pageSize?: number;
    riskDetailId?: string | undefined;
    isPolicy? : boolean
    sortField?: string;
    order?: string;
  }
  export interface IManuscriptListRequestDTO {
    pageNumber?: number;
    pageSize?: number;
    riskDetailId?: string | undefined;
    isPolicy? : boolean
    sortField?: string;
    order?: string;
  }





