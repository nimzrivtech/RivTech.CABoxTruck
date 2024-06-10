import { RiskDTO } from '../../risk/riskDto';

export class ClaimsHistoryDto {
  id?: string | undefined = null;
  riskDetailId?: string | undefined = null;
  order?: number | undefined;
  lossRunDate?: Date | undefined = null;
  groundUpNet?: string | undefined = null;
  biPiPmpIncLossTotal?: number | undefined = null;
  biPiPmpIncLoss100KCap?: number | undefined = null;
  propertyDamageIncLoss?: number | undefined = null;
  claimCountBiPip?: number | undefined = null;
  claimCountPropertyDamage?: number | undefined = null;
  openClaimsUnder500?: number | undefined = null;
  physicalDamageNetLoss?: number | undefined = null;
  physicalDamageClaimCount?: number | undefined = null;
  cargoNetLoss?: number | undefined = null;
  cargoClaimCount?: number | undefined = null;
  generalLiabilityNetLoss?: number | undefined = null;
  generalLiabilityClaimCount?: number | undefined = null;

  constructor(init?: Partial<ClaimsHistoryDto>) {
    Object.assign(this, init);
  }
}
