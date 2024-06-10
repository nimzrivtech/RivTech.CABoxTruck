import { IMvrPullingFrequencyDto } from './mvr-pulling-frequency.dto';

export class DriverInfoDto implements IDriverInfoDto {
  id?: string = null;
  riskDetailId?: string = null;
  totalNumberOfDrivers?: number = null;
  driversHired?: number = null;
  driversTerminated?: number;
  mvrPullingFrequency?: IMvrPullingFrequencyDto = null;
  isBusinessPrincipalADriverInPolicy?: boolean = true;
  areVolunteersUsedInBusiness?: boolean = false;
  percentageOfStaff?: number = null;
  isHiringFromOthers?: boolean = false;
  annualCostOfHire?: number = null;
  isHiringFromOthersWithoutDriver?: boolean = false;
  annualCostOfHireWithoutDriver?: number = null;
  isLeasingToOthers?: boolean = false;
  annualIncomeDerivedFromLease?: number = null;
  isLeasingToOthersWithoutDriver?: boolean = false;
  annualIncomeDerivedFromLeaseWithoutDriver?: number = null;
  isThereAssumedLiabilityByContract?: boolean = false;
  doDriversTakeVehiclesHome?: boolean = false;
  areVehiclesSolelyOwnedByApplicant?: boolean = true;
  willBeAddingDeletingVehiclesDuringTerm?: boolean = false;
  addingDeletingVehiclesDuringTermDescription?: string = null;

  constructor(init?: Partial<IDriverInfoDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IDriverInfoDto {
  id?: string;
  riskDetailId?: string;
  totalNumberOfDrivers?: number;
  driversHired?: number;
  driversTerminated?: number;
  mvrPullingFrequency?: IMvrPullingFrequencyDto;
  isBusinessPrincipalADriverInPolicy?: boolean;
  areVolunteersUsedInBusiness?: boolean;
  percentageOfStaff?: number;
  isHiringFromOthers?: boolean;
  annualCostOfHire?: number;
  isHiringFromOthersWithoutDriver?: boolean;
  annualCostOfHireWithoutDriver?: number;
  isLeasingToOthers?: boolean;
  annualIncomeDerivedFromLease?: number;
  isLeasingToOthersWithoutDriver?: boolean;
  annualIncomeDerivedFromLeaseWithoutDriver?: number;
  isThereAssumedLiabilityByContract?: boolean;
  doDriversTakeVehiclesHome?: boolean;
  areVehiclesSolelyOwnedByApplicant?: boolean;
  willBeAddingDeletingVehiclesDuringTerm?: boolean;
  addingDeletingVehiclesDuringTermDescription?: string;
}
