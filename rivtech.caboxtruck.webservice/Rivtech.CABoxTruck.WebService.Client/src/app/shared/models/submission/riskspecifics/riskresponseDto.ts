export interface IRiskResponseDto {
    riskDetailId?: number;
    questionId?: boolean;
    responseValue?: string;
}

export class RiskResponseDto implements IRiskResponseDto {
    riskDetailId?: number;
    questionId?: boolean;
    responseValue?: string;

    public constructor(init?: Partial<IRiskResponseDto>) {
        Object.assign(this, init);
    }
}
