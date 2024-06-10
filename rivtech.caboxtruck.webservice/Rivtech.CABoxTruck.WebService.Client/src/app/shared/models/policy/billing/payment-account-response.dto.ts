import { ApplicationUserDto } from './application-user.dto';
import { EnumerationDTO } from './common/enumeration.dto';

export class PaymentAccountResponse {
    id?: string | null;
    paymentProfileId?: string | null;
    instrumentTypeId?: string | null;
    description?: string | null;
    isDefault: boolean;
    createdDate?: Date | string | null;
    createdById?: string;
    instrumentType?: EnumerationDTO;
    createdBy?: ApplicationUserDto;
    accountTypeDisplay?: string;
    public constructor(init?: Partial<PaymentAccountResponse>) {
        Object.assign(this, init);
    }
}