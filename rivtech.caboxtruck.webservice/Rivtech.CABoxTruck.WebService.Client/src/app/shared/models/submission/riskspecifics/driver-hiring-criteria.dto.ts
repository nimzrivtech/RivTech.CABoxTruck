import { BackgroundCheckOptionDto } from './background-check-option.dto';
import { DriverTrainingOptionDto } from './driver-training-option.dto';

export class DriverHiringCriteriaDto implements IDriverHiringCriteriaDto {
  id?: string = null;
  riskDetailId?: string = null;
  areDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years?: boolean = false;
  doDriversHave5YearsDrivingExperience?: boolean = false;
  isAgreedToReportAllDriversToRivington?: boolean = true;
  areFamilyMembersUnder21PrimaryDriversOfCompanyAuto?: boolean = false;
  areDriversProperlyLicensedDotCompliant?: boolean = true;
  isDisciplinaryPlanDocumented?: boolean = false;
  isThereDriverIncentiveProgram?: boolean = false;
  backGroundCheckIncludes?: BackgroundCheckOptionDto[] = null;
  driverTrainingIncludes?: DriverTrainingOptionDto[] = null;

  constructor(init?: Partial<IDriverHiringCriteriaDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IDriverHiringCriteriaDto {
  id?: string;
  riskDetailId?: string;
  areDriversDrivingSimilarVehicleCommerciallyForMoreThan2Years?: boolean;
  doDriversHave5YearsDrivingExperience?: boolean;
  isAgreedToReportAllDriversToRivington?: boolean;
  areFamilyMembersUnder21PrimaryDriversOfCompanyAuto?: boolean;
  areDriversProperlyLicensedDotCompliant?: boolean;
  isDisciplinaryPlanDocumented?: boolean;
  isThereDriverIncentiveProgram?: boolean;
  backGroundCheckIncludes?: BackgroundCheckOptionDto[];
  driverTrainingIncludes?: DriverTrainingOptionDto[];
}
