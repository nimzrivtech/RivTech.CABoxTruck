import { BackgroundCheckOptionDto } from './background-check-option.dto';
import { DriverTrainingOptionDto } from './driver-training-option.dto';
import { MvrPullingFrequencyDto } from './mvr-pulling-frequency.dto';

export class DriverInfoPageOptionsDto implements IDriverInfoPageOptionsDto {
  mvrPullingFrequencyOptions?: MvrPullingFrequencyDto[] = null;
  backgroundCheckOptions?: BackgroundCheckOptionDto[] = null;
  driverTrainingOptions?: DriverTrainingOptionDto[] = null;


  constructor(init?: Partial<IDriverInfoPageOptionsDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IDriverInfoPageOptionsDto {
  mvrPullingFrequencyOptions?: MvrPullingFrequencyDto[];
  backgroundCheckOptions?: BackgroundCheckOptionDto[];
  driverTrainingOptions?: DriverTrainingOptionDto[];
}
