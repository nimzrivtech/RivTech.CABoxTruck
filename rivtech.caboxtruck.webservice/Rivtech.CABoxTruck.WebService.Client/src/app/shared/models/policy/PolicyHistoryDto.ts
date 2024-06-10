export class PolicyHistoryDTO {
    riskDetailId?: string | undefined;
    effectiveDate?: Date | undefined;
    endorsementNumber?: string | undefined;
    currentPremium?: number | undefined;
    newPremium?: number | undefined;
    proratedPremium?: number | undefined;
    premiumChange?: number | undefined;
    details?: string | undefined;
    isExpanded: boolean;
    policyStatus?: string | undefined;
    dateCreated?: Date | undefined;
    previousRiskDetailId?: string | undefined;

    constructor() {
        this.currentPremium = 0;
        this.newPremium = 0;
        this.proratedPremium = 0;
        this.premiumChange = 0;
        this.isExpanded = false;
    }
}