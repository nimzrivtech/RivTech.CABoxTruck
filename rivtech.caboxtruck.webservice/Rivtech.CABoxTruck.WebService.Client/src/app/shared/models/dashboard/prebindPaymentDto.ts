export class PreBindPaymentInputDTO {
  fromDate?: string | undefined;
  toDate?: string | undefined;

  public constructor(init?: Partial<PreBindPaymentInputDTO>) {
    Object.assign(this, init);
  }
}

export class PreBindPaymentDTO {
  id: string | undefined;
  insuredName: string | undefined;
  accessCode: string | undefined;
  payeeName: string | undefined;
  payeeEmail: string | undefined;
  paymentDateTime: string | undefined;
  paymentMethod: string | undefined;
  paymentAmount: string | undefined;
  confirmationCode: string | undefined;

  public constructor(init?: Partial<PreBindPaymentDTO>) {
    Object.assign(this, init);
  }
}
