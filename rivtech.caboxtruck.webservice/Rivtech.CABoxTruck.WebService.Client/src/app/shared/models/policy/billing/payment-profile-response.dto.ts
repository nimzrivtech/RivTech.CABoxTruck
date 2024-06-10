import { ApplicationUserDto } from './application-user.dto';
import { PaymentAccountResponse } from './payment-account-response.dto';

export class PaymentProfileResponse {
    id?: string | null;
    riskId?: string | null;
    firstName?: string | null;
    lastName?: string;
    phoneNumber?: string;
    email?: string;
    isRecurringPayment?: boolean | null;
    createdDate?: Date | string | null;
    createdById?: string;
    paymentAccounts?: PaymentAccountResponse[] | null;
    createdBy?: ApplicationUserDto;
    public constructor(init?: Partial<PaymentProfileResponse>) {
        Object.assign(this, init);
    }
}