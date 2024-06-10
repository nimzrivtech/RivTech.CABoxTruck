export class VehicleDto implements IVehicleDto {

  id?: string | undefined = null;
  riskDetailId?: string | undefined = null;
  isEndorsement?: boolean | undefined = null
  options?: string | undefined = null;
  year?: number | undefined = null;
  make?: string | undefined = null;
  model?: string | undefined = null;
  vin?: string | undefined = null;
  style?: string | undefined = null;
  vehicleType?: string | undefined = 'Power Unit';
  grossVehicleWeightId?: number | undefined = null;
  grossVehicleWeight?: string | undefined = null;
  vehicleDescriptionId?: number | undefined = null;
  description?: string | undefined = null;
  vehicleBusinessClassId?: number | undefined = null;
  businessClass?: string | undefined = null;
  isAutoLiability?: boolean | undefined = true;
  isCoverageFireTheft?: boolean | undefined = false;
  coverageFireTheftDeductibleId?: number | undefined = null;
  isCoverageCollision?: boolean | undefined = false;
  coverageCollisionDeductibleId?: number | undefined = null;
  coverageCollisionDeductible?: string | undefined = null;
  statedAmount?: number | undefined = null;
  hasCoverageCargo?: boolean | undefined = false;
  hasCoverageRefrigeration?: boolean | undefined = false;
  useClassId?: number | undefined = null;
  garagingAddressId?: string | undefined = null;
  primaryOperatingZipCode?: string | undefined = null;
  state?: string | undefined = null;

  stateFull?: string | undefined = null;

  registeredState?: string | undefined = null;
  radius?: number | undefined = null;
  radiusTerritory?: number | undefined = null;
  autoLiabilityPremium?: number | undefined = null;
  cargoPremium?: number | undefined = null;
  physicalDamagePremium?: number | undefined = null;
  totalPremium?: number | undefined = null;
  proratedPremium?: number | undefined = null;
  refrigerationPremium?: number | undefined = null;
  comprehensivePremium?: number | undefined = null;
  collisionPremium?: number | undefined = null;

  createdBy?: number | undefined = null;
  createdDate?: Date | undefined = null;

  isActive?: boolean | undefined = null;
  deletedDate?: Date | undefined = null;
  hasError?: boolean | undefined = null;

  effectiveDate?: Date | undefined = null;
  expirationDate?: Date | undefined = null;

  isValidated?: boolean | undefined;
  isExpanded?: boolean = false;
  isMainRow?: boolean = true;
  previousVehicles?: VehicleDto[] | undefined = null;
  canReinstate?: boolean = null;
  hasPreviousVersions?: boolean = null;
  isManualCoverageFtLimit?: boolean = null;
  isManualCollisionLimit?: boolean = null;

  endorsementNumber?: number | undefined = null;

  constructor(init?: Partial<IVehicleDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }

  coverageFireTheft?: string | undefined = null;
  coverageCollision?: string | undefined = null;
  coverageCargo?: string | undefined = null;
  coverageRefrigeration?: string | undefined = null;
  coverageFireTheftDesc?: string | undefined = null;
  // RegisteredState?: string | undefined = null;
}

export interface IVehicleDto {

  id?: string | undefined;
  riskDetailId?: string | undefined;
  options?: string | undefined;
  year?: number | undefined;
  make?: string | undefined;
  model?: string | undefined;
  vin?: string | undefined;
  style?: string | undefined;
  vehicleType?: string | undefined;
  grossVehicleWeightId?: number | undefined;
  grossVehicleWeight?: string | undefined;
  vehicleDescriptionId?: number | undefined;
  description?: string | undefined;
  vehicleBusinessClassId?: number | undefined;
  businessClass?: string | undefined;
  isAutoLiability?: boolean | undefined;
  isCoverageFireTheft?: boolean | undefined;
  coverageFireTheftDeductibleId?: number | undefined;
  isCoverageCollision?: boolean | undefined;
  coverageCollisionDeductibleId?: number | undefined;
  coverageCollisionDeductible?: string | undefined;
  statedAmount?: number | undefined;
  hasCoverageCargo?: boolean | undefined;
  hasCoverageRefrigeration?: boolean | undefined;
  useClassId?: number | undefined;
  garagingAddressId?: string | undefined;
  primaryOperatingZipCode?: string | undefined;
  state?: string | undefined;

  stateFull?: string | undefined;

  registeredState?: string | undefined;
  radius?: number | undefined;
  radiusTerritory?: number | undefined;
  autoLiabilityPremium?: number | undefined;
  cargoPremium?: number | undefined;
  physicalDamagePremium?: number | undefined;
  totalPremium?: number | undefined;
  refrigerationPremium?: number | undefined;
  comprehensivePremium?: number | undefined;
  collisionPremium?: number | undefined;

  createdBy?: number | undefined;
  createdDate?: Date | undefined;

  isActive?: boolean | undefined;
  deletedDate?: Date | undefined;
  hasError?: boolean | undefined;

  isValidated?: boolean | undefined;

  coverageFireTheft?: string | undefined ;
  coverageCollision?: string | undefined;
  coverageCargo?: string | undefined ;
  coverageRefrigeration?: string | undefined ;
  coverageFireTheftDesc?: string | undefined ;
}

export class VehiclePremiumsDTO {
  al: VehiclePremiumItemDTO | undefined = null;
  pd: VehiclePremiumItemDTO | undefined = null;
  cargo: VehiclePremiumItemDTO | undefined = null;
  collision: VehiclePremiumItemDTO | undefined = null;
  compFT: VehiclePremiumItemDTO | undefined = null;
}

export class VehiclePremiumItemDTO {
  annual: number | undefined = null;
  grossProrated: number | undefined = null;
  netProrated: number | undefined = null;
}
