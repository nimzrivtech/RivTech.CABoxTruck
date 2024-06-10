export class FilingsInformationDto implements IFilingsInformationDto {
  riskDetailId?: string = '';
  isARatedCarrierRequired?: boolean = false;
  isFilingRequired?: boolean = false;
  requiredFilingIds?: number[] = [];
  usdotNumber?: string = '';
  mcNumber?: string = '';
  isOthersAllowedToOperateCurrent?: boolean = false;
  isOthersAllowedToOperatePast?: boolean = false;
  isOperatorsWorkOnlyForInsured?: boolean = true;
  isDifferentUSDOTNumberLastFiveYears?: boolean = false;
  dotNumberLastFiveYearsDetail?: string = '';
  isInsuredHasMotorCarrierNo?: boolean = false;

  constructor(init?: Partial<IFilingsInformationDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IFilingsInformationDto {
  riskDetailId?: string;
  isARatedCarrierRequired?: boolean;
  isFilingRequired?: boolean;
  requiredFilingIds?: number[];
  usdotNumber?: string;
  mcNumber?: string;
  isOthersAllowedToOperateCurrent?: boolean;
  isOthersAllowedToOperatePast?: boolean;
  isOperatorsWorkOnlyForInsured?: boolean;
  isDifferentUSDOTNumberLastFiveYears?: boolean;
  dotNumberLastFiveYearsDetail?: string;
  isInsuredHasMotorCarrierNo?: boolean;

}
