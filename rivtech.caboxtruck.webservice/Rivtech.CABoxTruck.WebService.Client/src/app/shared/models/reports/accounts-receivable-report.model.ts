export class AccountReceivableReportDTO {
    broker: string;
    policyNumber: string;
    insured: string;
    insuredPhone: string;
    inception: string;
    pendingCancellation: string;
    cancellationDate: string;
    pastDueDate: string;
    pastDueAmount: string;
    currentDueDate: string;
    currentAmountDue: string;
    noPayments: string;
    financed: string;

    public constructor(init?: Partial<AccountReceivableReportDTO>) {
      Object.assign(this, init);
    }
}
