import { IVehicleDto, VehiclePremiumsDTO } from './VehicleDto';

export class VehicleModel implements IVehicleDto {
  id?: string | undefined;
  riskDetailId?: string | undefined;
  isEndorsement?: boolean | undefined;
  options?: string | undefined;
  year?: number | undefined;
  age?: string | undefined;
  make?: string | undefined;
  model?: string | undefined;
  vin?: string | undefined;
  style?: string | undefined;
  vehicleType?: string | undefined;
  grossVehicleWeightId?: number | undefined;
  grossVehicleWeightCategoryId?: number | undefined;
  vehicleDescriptionId?: number | undefined;
  description?: string | undefined;
  vehicleBusinessClassId?: number | undefined;
  businessClass?: string | undefined;
  isAutoLiability?: boolean | undefined;
  isCoverageFireTheft?: boolean | undefined;
  coverageFireTheftDeductibleId?: number | undefined;
  coverageFireTheftDeductible?: string | undefined;
  isCoverageCollision?: boolean | undefined;
  coverageCollisionDeductibleId?: number | undefined;
  coverageCollisionDeductible?: string | undefined;
  statedAmount?: number | undefined;
  hasCoverageCargo?: boolean | undefined;
  hasCoverageRefrigeration?: boolean | undefined;
  useClassId?: number | undefined;
  garagingAddressId?: string | undefined;
  stateCode?: string | undefined;
  primaryOperatingZipCode?: string | undefined;
  state?: string | undefined;
  registeredState?: string | undefined;
  radius?: number = null;
  radiusTerritory?: number = null;
  autoLiabilityPremium?: number = null;
  cargoPremium?: number = null;
  physicalDamagePremium?: number = null;
  totalPremium?: number = null;
  proratedPremium?: number = null;

  createdBy?: number | undefined;
  createdDate?: Date | undefined;

  isActive?: boolean | undefined;
  deletedDate?: Date | undefined;
  hasError?: boolean | undefined;

  effectiveDate?: Date | undefined = null;
  expirationDate?: Date | undefined = null;

  canReinstate?: boolean | undefined = null;
  endorsementNumber?: number | undefined = null;
  hasPreviousVersions?: boolean | undefined = null;

  isManualCoverageFtLimit?: boolean = null;
  isManualCollisionLimit?: boolean = null;

  constructor(vehicle?: Partial<IVehicleDto>) {
    Object.assign(this, vehicle);
    this.age = this.calculateAge().toString();
    this.grossVehicleWeightCategoryId = this.grossVehicleWeightId;
  }
  calculateAge() {
    return this.year && this.year > 0 ? (new Date()).getFullYear() - this.year : '';
  }

  isDeleted() {
    console.log(this.deletedDate);
    return this.deletedDate != null;
  }

}
