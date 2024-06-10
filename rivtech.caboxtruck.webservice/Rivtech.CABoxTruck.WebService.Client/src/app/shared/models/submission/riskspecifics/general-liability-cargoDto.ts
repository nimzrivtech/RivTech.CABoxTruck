import { CargoDto } from './cargoDto';
import { GeneralLiabilityDto } from './general-liabilityDto';

export class GeneralLiabilityCargoDto {
    riskDetailId?: string;
    generalLiability?: GeneralLiabilityDto;
    cargo?: CargoDto;
    commoditiesHauled?: any[] = [];

    public constructor(init?: Partial<GeneralLiabilityCargoDto>) {
        this.generalLiability = new GeneralLiabilityDto();
        this.cargo = new CargoDto();

        Object.assign(this, init);
    }
}
