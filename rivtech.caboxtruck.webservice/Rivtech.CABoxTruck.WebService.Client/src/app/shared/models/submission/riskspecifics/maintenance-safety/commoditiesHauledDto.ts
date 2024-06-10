export class CommoditiesHauledDto {
    id?: string = null;
    riskDetailId?: string = '';
    commodity?: string = '';
    percentageOfCarry?: number = null;
    totalPercentageOfCarry?: number = 0;
    public constructor(init?: Partial<CommoditiesHauledDto>) {
        Object.assign(this, init);
    }
}
