import { IChameleonIssuesDto } from './chameleon-issues.dto';
import { ISaferRatingDto } from './safer-rating.dto';
import { ITrafficLightRatingDto } from './traffic-light-rating.dto';

export class DotInfoPageOptionsDto implements IDotInfoPageOptionsDto {
  chameleonIssuesOptions?: IChameleonIssuesDto[] = null;
  saferRatingOptions?: ISaferRatingDto[] = null;
  trafficLightRatingOptions?: ITrafficLightRatingDto[] = null;

  constructor(init?: Partial<IDotInfoPageOptionsDto>) {
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}

export interface IDotInfoPageOptionsDto {
  chameleonIssuesOptions?: IChameleonIssuesDto[];
  saferRatingOptions?: ISaferRatingDto[];
  trafficLightRatingOptions?: ITrafficLightRatingDto[];
}
