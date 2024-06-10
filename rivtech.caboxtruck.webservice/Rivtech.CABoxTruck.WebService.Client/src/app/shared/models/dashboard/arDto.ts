export class AccountsReceivableInputDTO {
  sortNameBy: string | undefined;
  receivableType: string | undefined;
  broker: string | undefined;
  pastDueOnly: boolean;
  cancellationStatus: string | undefined;
  noPaymentsMade: boolean;
  valuationDate?: Date | undefined;

  public constructor(init?: Partial<AccountsReceivableInputDTO>) {
    Object.assign(this, init);
  }
}

export class AccountsReceivableDTO {
  policyDetailId: string | undefined;
  policyNumber: string | undefined;
  insured: string | undefined;
  insuredPhone: string | undefined;
  inception: string | undefined;
  broker: string | undefined;
  financed: string | undefined;
  pastDueAmount: string | undefined;
  pastDueDate: string | undefined;
  currentDueDate: string | undefined;
  currentAmountDue: string | undefined;
  noPayments: string | undefined;
  pendingCancellation: string | undefined;
  cancellationDate: string | undefined;

  public constructor(init?: Partial<AccountsReceivableDTO>) {
    Object.assign(this, init);
  }
}
