import { DriverIncidentDto } from "./DriverIncidentDto";

export class DriverDto {
  id?: string = null;
  riskDetailId?: string = null;
  entityId?: string = null;
  birthDate?: Date | undefined = null;
  age?: string | undefined = null;
  stateId?: string | undefined = null;
  submissionNumber?: string | undefined = null;
  policyNumber?: string | undefined = null;

  state?: string | undefined = null;

  stateFull?: string | undefined = null;

  outStateDriver?: boolean | undefined = null;
  yrsDrivingExp?: number | undefined = null;
  yrsCommDrivingExp?: number | undefined = null;

  include?: boolean | undefined = null;
  lastName?: string = null;
  firstName?: string = null;
  middleName?: string = null;
  isMvr?: boolean | undefined = null;
  yearPoint?: string = null;
  ageFactor?: string = null;
  totalFactor?: string = null;
  isOutOfState?: boolean | undefined = null;
  mvrDate?: Date | undefined = null;
  hireDate?: Date | undefined = null;

  isExcludeKO?: boolean | undefined = null;
  koReason?: string = null;

  licenseNumber?: string = null;

  options?: string = null;
  createdBy?: number | undefined = null;
  createdDate?: Date | undefined = null;

  isActive?: boolean | undefined;
  deletedDate?: Date | undefined;
  hasError?: boolean | undefined;

  isValidated?: boolean | undefined;
  isValidatedKO?: boolean | undefined;

  isExpanded?: boolean = false;
  isMainRow?: boolean = true;
  previousDrivers?: DriverDto[] | undefined = null;
  canReinstate?: boolean = false;

  public driverIncidents?: DriverIncidentDto[] | undefined = null;

  constructor(init?: Partial<DriverDto>) {
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

export class DriverHeaderDto {
  id?: string = null;
  riskDetailId?: string = null;
  driverRatingTab?: boolean | undefined = null;
  accidentInfo?: boolean | undefined = null;
  mvrHeaderDate?: Date | undefined = null;
  applyFilter?: string | undefined = null;
  accDriverFactor?: string | undefined = null;
  driverVehicleRatio?: string | undefined = null;

  constructor(init?: Partial<DriverDto>) {
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

