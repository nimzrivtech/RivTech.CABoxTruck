export class AgencyAddressEntityAddressDTO {
    id?: string;
    entityId?: string;
    addressId?: string;
    addressTypeId?: number;
    addressTypeDescription?: string;
    effectiveDate?: Date;
    expirationDate?: Date;
    removedProcessDate?: Date;
    address?: AgencyAddressDTO;
    rowId?: number;
  }

  export class AgencyAddressDTO {
    id?: string;
    streetAddress1?: string;
    streetAddress2?: string;
    cityZipCodeID?: number;
    zipCode?: string;
    city?: string;
    stateCode?: string;
    county?: string;
    countryCode?: string;
    removedProcessDate?: Date;
  }