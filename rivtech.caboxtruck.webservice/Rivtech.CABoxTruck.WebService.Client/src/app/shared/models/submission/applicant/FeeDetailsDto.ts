export class FeeDetailsDto {
    stateCode: string | undefined;
    feeKindId: string | undefined;
    description: string | undefined;
    amount: number | undefined;
    isActive: boolean | undefined;

    constructor(init?: Partial<FeeDetailsDto>) {
        Object.assign(this, init);
      }
}
