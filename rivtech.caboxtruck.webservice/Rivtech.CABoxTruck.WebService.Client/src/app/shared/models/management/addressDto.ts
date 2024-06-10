export class AddressDTO implements IAddressDTO {
    id?: string;
    streetAddress1?: string | undefined;
    streetAddress2?: string | undefined;
    cityZipCodeID?: number;
    zipCodeExt?: string | undefined;
    longitude?: number;
    latitude?: number;
    isGarageIndoor?: boolean;
    isGarageOutdoor?: boolean;
    isGarageFenced?: boolean;
    isGarageLighted?: boolean;
    isGarageWithSecurityGuard?: boolean;
    zipCode?: string | undefined;
    city?: string;
    stateCode?: string;

    constructor(data?: IAddressDTO) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {(<any>this)[property] = (<any>data)[property];}
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data['id'];
            this.streetAddress1 = _data['streetAddress1'];
            this.streetAddress2 = _data['streetAddress2'];
            this.cityZipCodeID = _data['cityZipCodeID'];
            this.zipCodeExt = _data['zipCodeExt'];
            this.longitude = _data['longitude'];
            this.latitude = _data['latitude'];
            this.isGarageIndoor = _data['isGarageIndoor'];
            this.isGarageOutdoor = _data['isGarageOutdoor'];
            this.isGarageFenced = _data['isGarageFenced'];
            this.isGarageLighted = _data['isGarageLighted'];
            this.isGarageWithSecurityGuard = _data['isGarageWithSecurityGuard'];
            this.zipCode = _data['zipCode'];
            this.city = _data['city'];
            this.stateCode = _data['stateCode'];
        }
    }

    static fromJS(data: any): AddressDTO {
        data = typeof data === 'object' ? data : {};
        const result = new AddressDTO();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data['id'] = this.id;
        data['streetAddress1'] = this.streetAddress1;
        data['streetAddress2'] = this.streetAddress2;
        data['cityZipCodeID'] = this.cityZipCodeID;
        data['zipCodeExt'] = this.zipCodeExt;
        data['longitude'] = this.longitude;
        data['latitude'] = this.latitude;
        data['isGarageIndoor'] = this.isGarageIndoor;
        data['isGarageOutdoor'] = this.isGarageOutdoor;
        data['isGarageFenced'] = this.isGarageFenced;
        data['isGarageLighted'] = this.isGarageLighted;
        data['isGarageWithSecurityGuard'] = this.isGarageWithSecurityGuard;
        data['zipCode'] = this.zipCode ;
        data['city'] = this.city;
        data['stateCode'] = this.stateCode;
        return data;
    }
}

export interface IAddressDTO {
    id?: string;
    streetAddress1?: string | undefined;
    streetAddress2?: string | undefined;
    cityZipCodeID?: number;
    zipCodeExt?: string | undefined;
    longitude?: number;
    latitude?: number;
    isGarageIndoor?: boolean;
    isGarageOutdoor?: boolean;
    isGarageFenced?: boolean;
    isGarageLighted?: boolean;
    isGarageWithSecurityGuard?: boolean;
    zipCode?: string | undefined;
    city?: string;
    stateCode?: string;
}