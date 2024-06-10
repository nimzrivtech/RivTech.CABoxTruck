export interface ISafetyDevice {
    safetyDeviceCategoryId: number;
    isInPlace?: boolean;
    unitDescription?: string;
    yearsInPlace?: number;
    percentageOfFleet: number;
}

export class SafetyDevice implements ISafetyDevice {
    safetyDeviceCategoryId: number;
    isInPlace?: boolean;
    unitDescription?: string;
    yearsInPlace?: number;
    percentageOfFleet: number;

    public constructor(init?: Partial<ISafetyDevice>) {
        Object.assign(this, init);
    }
}