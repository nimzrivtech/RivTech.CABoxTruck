import { DriverHiringCriteriaDto } from './driver-hiring-criteria.dto';
import { DriverInfoDto } from './driver-info.dto';

export class DriverInfoPageDto implements IDriverInfoPageDto {
  driverInfo?: DriverInfoDto = null;
  driverHiringCriteria?: DriverHiringCriteriaDto = null;

  constructor(init?: Partial<IDriverInfoPageDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IDriverInfoPageDto {
  driverInfo?: DriverInfoDto;
  driverHiringCriteria?: DriverHiringCriteriaDto;
}
