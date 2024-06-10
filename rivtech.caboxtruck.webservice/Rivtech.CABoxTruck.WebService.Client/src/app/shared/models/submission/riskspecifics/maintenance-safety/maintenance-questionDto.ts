import { SelectItem } from '../../../dynamic/select-item';

export interface IMaintenanceQuestionDto {
    safetyMeetings?: number | undefined;
    personInCharge?: string;
    garagingType?: SelectItem[];
    vehicleMaintenance?: SelectItem[];
    isMaintenanceProgramManagedByCompany?: boolean;
    provideCompleteMaintenanceOnVehicles?: boolean;
    areDriverFilesAvailableForReview?: boolean;
    areAccidentFilesAvailableForReview?: boolean;
    companyPractice?: SelectItem[];
}

export class MaintenanceQuestionDto implements IMaintenanceQuestionDto {
    safetyMeetings?: number | undefined;
    personInCharge?: string;
    garagingType?: SelectItem[];
    vehicleMaintenance?: SelectItem[];
    isMaintenanceProgramManagedByCompany?: boolean;
    provideCompleteMaintenanceOnVehicles?: boolean;
    areDriverFilesAvailableForReview?: boolean;
    areAccidentFilesAvailableForReview?: boolean;
    companyPractice?: SelectItem[];

    public constructor(init?: Partial<IMaintenanceQuestionDto>) {
        Object.assign(this, init);
    }
}