export class DashboardDTO {
  submissionCount: number;
  completeCount: number;
  declinedCount: number;
  inResearchCount: number;
  inCompleteCount: number;
  notAcceptedCount: number;
  quoteInProgressCount: number;
  quotedCount: number;
  dataOnlyCount: number;
  testClassCount: number;
  receivedCount: number;
  withdrawnCount: number;
  activePoliciesCount: number;
  cancelledPoliciesCount: number;
  expiredPoliciesCount: number;
  pendingCancellationCount: number;

  renewalCount: number;
  rcompleteCount: number;
  rdeclinedCount: number;
  rinResearchCount: number;
  rinCompleteCount: number;
  rnotAcceptedCount: number;
  rquoteInProgressCount: number;
  rquotedCount: number;
  rreceivedCount: number;
  rwithdrawnCount: number;

  public constructor(init?: Partial<DashboardDTO>) {
    Object.assign(this, init);
  }
}
