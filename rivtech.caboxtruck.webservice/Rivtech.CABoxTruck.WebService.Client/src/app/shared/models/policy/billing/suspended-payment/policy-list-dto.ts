export class PolicySearchResultDTO implements IPolicySearchResultDTO {
  riskId?: string;
  policyNumber?: string;
  insuredName?: string;
  propertyAddress?: string;
  parentRiskId?: string;
  isExactSearch?: boolean;

  constructor(data?: IPolicySearchResultDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPolicySearchResultDTO {
  riskId?: string;
  policyNumber?: string;
  insuredName?: string;
  propertyAddress?: string;
  parentRiskId?: string;
  isExactSearch?: boolean;
}