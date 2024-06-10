import { BillingDetailDTO } from './billing-detail.dto';

export class PaymentAccountRequest {
    id?: string | null;
    appId?: string | null;
    riskId?: string | null;
    instrumentTypeId?: string | null;
    isDefault?: boolean;
    billingDetail?: BillingDetailDTO;
    public constructor(init?: Partial<PaymentAccountRequest>) {
        Object.assign(this, init);
    }
}