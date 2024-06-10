import { IRadiusOfOperationsDto } from './riskspecifics/radius-of-operations.dto';

export class RiskHeaderDto {
    public id?: string | undefined = null;
    public riskDetailId?: string | undefined = null;
    public submissionNumber?: string | undefined = null;

    public effectiveDate?: Date | undefined = null;
    public expirationDate?: Date | undefined = null;
    public insuredName?: string | undefined = null;
    public broker?: string | undefined = null;
    public radiusOfOperations?: IRadiusOfOperationsDto | undefined = null;

    constructor(init?: Partial<RiskHeaderDto>) {
        Object.assign(this, init);
    }
}
