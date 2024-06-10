export class BlanketCoverageDto implements IBlanketCoverageDto {
  riskDetailId?: string = '';
  isAdditionalInsured?: boolean = false;
  isWaiverOfSubrogation?: boolean = false;
  isPrimaryNonContributory?: boolean = false;

  constructor(init?: Partial<IBlanketCoverageDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IBlanketCoverageDto {
  riskDetailId?: string;
  isAdditionalInsured?: boolean;
  isWaiverOfSubrogation?: boolean;
  isPrimaryNonContributory?: boolean;
}
