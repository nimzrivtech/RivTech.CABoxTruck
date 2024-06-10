export class AgentDashboardDTO {
  submissionAIRCount?: number = 0;
  policyAIRCount?: number = 0;
  policyPECCount?: number = 0;
  policyPNRCount?: number = 0;
  billingPaymentDueCount?: number = 0;
  billingPaymentPastDueCount?: number = 0;
}

export interface IAgentDashboardDTO{
  submissionAIRCount?: number;
  policyAIRCount?: number;
  policyPECCount?: number;
  policyPNRCount?: number;
  billingPaymentDueCount?: number;
  billingPaymentPastDueCount?: number;
}