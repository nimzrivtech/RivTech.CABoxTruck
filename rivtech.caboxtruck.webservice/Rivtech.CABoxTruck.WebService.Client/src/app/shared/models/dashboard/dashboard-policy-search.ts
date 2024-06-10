export class DashboardPolicySearchDTO {
  riskId: string;
  latestRiskDetailId: string;

  public constructor(init?: Partial<DashboardPolicySearchDTO>) {
    Object.assign(this, init);
  }
}
