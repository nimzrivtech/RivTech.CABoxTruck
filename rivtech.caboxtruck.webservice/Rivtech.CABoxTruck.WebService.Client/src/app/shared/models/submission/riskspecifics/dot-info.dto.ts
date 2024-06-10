import { IChameleonIssuesDto } from './chameleon-issues.dto';
import { ISaferRatingDto } from './safer-rating.dto';
import { ITrafficLightRatingDto } from './traffic-light-rating.dto';

export class DotInfoDto implements IDotInfoDto {
  id?: string;
  riskDetailId?: string;
  doesApplicantRequireDotNumber?: boolean = false;
  doesApplicantPlanToStayInterstate?: boolean = true;
  doesApplicantHaveInsterstateAuthority?: boolean = true;
  dotRegistrationDate?: Date = null;
  isDotCurrentlyActive?: boolean = true;
  chameleonIssues?: IChameleonIssuesDto = null;
  currentSaferRating?: ISaferRatingDto = null;
  issCabRating?: ITrafficLightRatingDto = null;
  isThereAnyPolicyLevelAccidents?: boolean;
  numberOfPolicyLevelAccidents?: number = 0;
  isUnsafeDrivingChecked?: boolean = false;
  isHoursOfServiceChecked?: boolean = false;
  isDriverFitnessChecked?: boolean = false;
  isControlledSubstanceChecked?: boolean = false;
  isVehicleMaintenanceChecked?: boolean = false;
  isCrashChecked?: boolean = false;

  constructor(init?: Partial<IDotInfoDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IDotInfoDto {
  id?: string;
  riskDetailId?: string;
  doesApplicantRequireDotNumber?: boolean;
  doesApplicantPlanToStayInterstate?: boolean;
  doesApplicantHaveInsterstateAuthority?: boolean;
  dotRegistrationDate?: Date;
  isDotCurrentlyActive?: boolean;
  chameleonIssues?: IChameleonIssuesDto;
  currentSaferRating?: ISaferRatingDto;
  issCabRating?: ITrafficLightRatingDto;
  isThereAnyPolicyLevelAccidents?: boolean;
  numberOfPolicyLevelAccidents?: number;
  isUnsafeDrivingChecked?: boolean;
  isHoursOfServiceChecked?: boolean;
  isDriverFitnessChecked?: boolean;
  isControlledSubstanceChecked?: boolean;
  isVehicleMaintenanceChecked?: boolean;
  isCrashChecked?: boolean;
}
