import { SelectItem } from '../../../../../shared/models/dynamic/select-item';

export interface ISafetyDevicesDto {
    safetyDeviceCategory?: SelectItem[];
    safetyDevice?: SafetyDeviceDto[];
}

export class SafetyDevicesDto implements ISafetyDevicesDto {
    safetyDeviceCategory?: SelectItem[];
    safetyDevice?: SafetyDeviceDto[];

    public constructor(init?: Partial<ISafetyDevicesDto>) {
        Object.assign(this, init);
    }
}

export interface ISafetyDeviceDto {
    safetyDeviceCategoryId?: number;
    isInPlace?: boolean;
    unitDescription?: string;
    yearsInPlace?: number;
    percentageOfFleet?: number;
}

export class SafetyDeviceDto implements ISafetyDeviceDto {
    safetyDeviceCategoryId?: number;
    isInPlace?: boolean;
    unitDescription?: string;
    yearsInPlace?: number;
    percentageOfFleet?: number;

    public constructor(init?: Partial<ISafetyDeviceDto>) {
        Object.assign(this, init);
    }
}
