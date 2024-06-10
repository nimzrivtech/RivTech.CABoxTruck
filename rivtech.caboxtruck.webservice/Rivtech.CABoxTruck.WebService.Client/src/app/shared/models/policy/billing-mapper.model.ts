export interface IBillingMapper {
  code?: string;
  billingCode?: string;
}
export class BillingMapper implements IBillingMapper {
  code?: string;
  billingCode?: string;
  public constructor(init?: Partial<BillingMapper>) {
    Object.assign(this, init);
  }
}