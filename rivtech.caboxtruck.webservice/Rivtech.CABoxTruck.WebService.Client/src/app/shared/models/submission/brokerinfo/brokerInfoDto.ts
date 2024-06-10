import { AgencySubmissionDto } from './agencySubmissionDto';
import { AgentSubmissionDto } from './agentSubmissionDto';
import { SubAgencySubmissionDto } from './SubAgencySubmissionDto';
import { SubAgentSubmissionDto } from './subAgentSubmissionDto';

export class BrokerInfoDto implements IBrokerInfoDto {
  id?: string | undefined = null;
  riskDetailId?: string | undefined = null;
  riskId?: string | undefined = null;
  brokerId?: number | undefined = null;

  agencyId?: string | undefined = null;
  agencyName?: string | undefined = null;
  agencyPhone?: string | undefined = null;

  agentId?: string | undefined = null;
  agentName?: string | undefined = null;
  agentEmail?: string | undefined = null;
  agentPhone?: string | undefined = null;

  subAgencyId?: string | undefined = null;
  subAgencyName?: string | undefined = null;
  subAgencyPhone?: string | undefined = null;

  subAgentId?: string | undefined = null;
  subAgentName?: string | undefined = null;
  subAgentEmail?: string | undefined = null;
  subAgentPhone?: string | undefined = null;

  // How long has your agency written this applicant?
  yearsWithAgency?: string | undefined = null;

  // Reason for Move
  reasonMoveId?: string | undefined = null;

  effectiveDate?: Date | undefined = null;
  expirationDate?: Date | undefined = null;

  isIncumbentAgency?: boolean | undefined = null;
  isBrokeredAccount?: boolean | undefined = null;
  isMidtermMove?: boolean | undefined = null;

  //Risk?: RiskDTO | undefined = null;

  agent?: AgentSubmissionDto | undefined = null;
  subAgent?: SubAgentSubmissionDto | undefined = null;

  agency?: AgencySubmissionDto | undefined = null;
  subAgency?: SubAgencySubmissionDto | undefined = null;

  assistantUnderwriterId?: string | undefined = null;
  psrUserId?: string | undefined = null;
  teamLeadUserId?: string | undefined = null;
  internalUWUserId?: string | undefined = null;
  productUWUserId?: string | undefined = null;

  policyNumber?: string | undefined = null;

  constructor(init?: Partial<IBrokerInfoDto>) {
    Object.assign(this, init);
  }
}

export class IBrokerInfoDto {
  id?: string | undefined;
  riskDetailId?: string | undefined;
  riskId?: string | undefined;
  brokerId?: number | undefined;

  agencyId?: string | undefined;
  agencyName?: string | undefined;
  agencyPhone?: string | undefined;

  agentId?: string | undefined;
  agentName?: string | undefined;
  agentEmail?: string | undefined;
  agentPhone?: string | undefined;

  subAgencyId?: string | undefined;
  subAgencyName?: string | undefined;
  subAgencyPhone?: string | undefined;

  subAgentId?: string | undefined;
  subAgentName?: string | undefined;
  subAgentEmail?: string | undefined;
  subAgentPhone?: string | undefined;

  // How long has your agency written this applicant?
  yearsWithAgency?: string | undefined;

  // Reason for Move
  reasonMoveId?: string | undefined;

  effectiveDate?: Date | undefined;
  expirationDate?: Date | undefined;

  isIncumbentAgency?: boolean | undefined;
  isBrokeredAccount?: boolean | undefined;
  isMidtermMove?: boolean | undefined;

  //Risk?: RiskDTO | undefined;

  agent?: AgentSubmissionDto | undefined;
  subAgent?: SubAgentSubmissionDto | undefined;

  agency?: AgencySubmissionDto | undefined;
  subAgency?: SubAgencySubmissionDto | undefined;

  assistantUnderwriterId?: string | undefined = null;
  psrUserId?: string | undefined = null;
  teamLeadUserId?: string | undefined = null;
  internalUWUserId?: string | undefined = null;
  productUWUserId?: string | undefined = null;

  policyNumber?: string | undefined = null;
}

