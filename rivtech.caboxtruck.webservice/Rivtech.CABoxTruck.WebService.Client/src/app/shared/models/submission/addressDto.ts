export class AddressDTO implements IAddressDTO {
    id?: string;
    streetAddress1?: string | undefined;
    streetAddress2?: string | undefined;
    city?: string | undefined;
    zipCode?: number | undefined;
    cityZipCodeID?: number;
    state?: string | undefined;
    zipCodeExt?: string | undefined;
    longitude?: number;
    latitude?: number;
    isMainGarage?: boolean;
    isGarageIndoor?: boolean;
    isGarageOutdoor?: boolean;
    isGarageFenced?: boolean;
    isGarageLighted?: boolean;
    isGarageWithSecurityGuard?: boolean;

    constructor(init?: Partial<AddressDTO>) {
        Object.assign(this, init);
      }
}

export interface IAddressDTO {
    id?: string;
    streetAddress1?: string | undefined;
    streetAddress2?: string | undefined;
    city?: string | undefined;
    zipCode?: number | undefined;
    cityZipCodeID?: number;
    state?: string | undefined;
    zipCodeExt?: string | undefined;
    longitude?: number;
    latitude?: number;
    isMainGarage?: boolean;
    isGarageIndoor?: boolean;
    isGarageOutdoor?: boolean;
    isGarageFenced?: boolean;
    isGarageLighted?: boolean;
    isGarageWithSecurityGuard?: boolean;
}
