export class BrokerCommissionInputDTO {
  fromDate?: string | undefined;
  toDate?: string | undefined;

  public constructor(init?: Partial<BrokerCommissionInputDTO>) {
    Object.assign(this, init);
  }
}

export class BrokerCommissionDTO {
  id: string | undefined;
  policyNumber: string | undefined;
  broker: string | undefined;
  companyName: string | undefined;
  checkNumber: string | undefined;
  paymentAmount: string | undefined;
  paymentDate: string | undefined;
  brokerCommisionAL: string | undefined;
  brokerCommisionPD: string | undefined;
  commisionALAmount: string | undefined;
  commisionPDAmount: string | undefined;
  commissionTotal: string | undefined;

  public constructor(init?: Partial<BrokerCommissionDTO>) {
    Object.assign(this, init);
  }
}
