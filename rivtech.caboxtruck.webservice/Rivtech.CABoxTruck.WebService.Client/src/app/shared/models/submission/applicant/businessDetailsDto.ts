import { EntitySubsidiaryDTO } from '../entitySubsidiaryDto';

export class BusinessDetailsDTO implements IBusinessDetailsDTO {
    riskDetailId?: string;
    isNewVenture: boolean;
    firstYearUnderCurrentManagement?: number | undefined;
    newVenturePreviousExperienceId?: number | undefined;
    yearsInBusiness?: number | undefined;
    yearsUnderCurrentManagement?: number | undefined;
    mainUseId?: number | undefined;
    mainGVWId?: number | undefined;
    secondaryBusinessGroupId?: number | undefined;
    accountCategoryId?: number | undefined;
    accountCategoryUWId?: number | undefined;
    businessTypeId?: number | undefined;
    yearEstablished?: number | undefined;
    federalIDNumber: string;
    socialSecurityNumber: string;
    naicsCode: string;
    pUCNumber: string;
    hasSubsidiaries: boolean;
    hasCompanies: boolean;
    descOperations: string;

    subsidiaries: EntitySubsidiaryDTO[];
    companies: EntitySubsidiaryDTO[];

    constructor(init?: Partial<BusinessDetailsDTO>) {
        Object.assign(this, init);
      }
}

export interface IBusinessDetailsDTO {
    riskDetailId?: string;
    isNewVenture: boolean;
    firstYearUnderCurrentManagement?: number | undefined;
    newVenturePreviousExperienceId?: number | undefined;
    yearsInBusiness?: number | undefined;
    yearsUnderCurrentManagement?: number | undefined;
    mainUseId?: number | undefined;
    mainGVWId?: number | undefined;
    secondaryBusinessGroupId?: number | undefined;
    accountCategoryId?: number | undefined;
    accountCategoryUWId?: number | undefined;
    businessTypeId?: number | undefined;
    yearEstablished?: number | undefined;
    federalIDNumber: string;
    socialSecurityNumber: string;
    naicsCode: string;
    pUCNumber: string;
    hasSubsidiaries: boolean;
    hasCompanies: boolean;
    descOperations: string;

    subsidiaries: EntitySubsidiaryDTO[];
    companies: EntitySubsidiaryDTO[];
}

