import { PaymentAccountRequest } from './payment-account-request.dto';

export class PaymentProfileRequest {
    appId?: string | null;
    riskId?: string | null;
    firstName?: string | null;
    lastName?: string;
    phoneNumber?: string;
    email?: string;
    isRecurringPayment?: boolean | null;
    paymentAccount?: PaymentAccountRequest | null;
    public constructor(init?: Partial<PaymentProfileRequest>) {
        Object.assign(this, init);
    }
}