export class PaymentPostingInputDTO {
  fromDate?: string | undefined;
  toDate?: string | undefined;

  public constructor(init?: Partial<PaymentPostingInputDTO>) {
    Object.assign(this, init);
  }
}

export class PaymentPostingDTO {
  id: string | undefined;
  policyNumber: string | undefined;
  companyName: string | undefined;
  checkNumber: string | undefined;
  paymentAmount: string | undefined;
  voidDate: string | undefined;
  paymentDate: string | undefined;
  premium: string | undefined;
  tax: string | undefined;
  nonPremium: string | undefined;
  overPayment: string | undefined;
  brokerCommisionAL: string | undefined;
  brokerCommisionPD: string | undefined;

  public constructor(init?: Partial<PaymentPostingDTO>) {
    Object.assign(this, init);
  }
}
