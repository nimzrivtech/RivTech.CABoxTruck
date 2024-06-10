import { MaintenanceQuestionDto } from './maintenance-questionDto';
import { SafetyDevicesDto } from './safety-deviceDto';

export class MaintenanceSafetyDto {
    riskDetailId?: string;
    maintenanceQuestion?: MaintenanceQuestionDto;
    safetyDevices?: SafetyDevicesDto;

    public constructor(init?: Partial<MaintenanceSafetyDto>) {
        this.maintenanceQuestion = new MaintenanceQuestionDto();
        this.safetyDevices = new SafetyDevicesDto();

        Object.assign(this, init);
    }
}
