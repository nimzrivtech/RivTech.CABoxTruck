import { RiskCoverageDTO } from '../limits/riskCoverageDto';

export class QuoteConditionRequestDTO {
  public id: string | undefined = null;
  public riskCoverage?: RiskCoverageDTO | undefined = null;
  public previousRiskCoverage?: RiskCoverageDTO | undefined = null;
  public isPolicy?: boolean | undefined = false;
  constructor(init?: Partial<QuoteConditionRequestDTO>) {
    Object.assign(this, init);
  }
}
