export class EntityAddressDTO {
  id: string;
  //entityId
  //addressId
  addressTypeId: string;
  //createdBy
  //createdDate
  effectiveDate?: string;
  expirationDate?: string;
  //removedProcessDate?
  //removedBy?
  address?: AddressDTO[];
}

export class AddressDTO {
  id: string;
  streetAddress1?: string;
  streetAddress2?: string;
  cityZipCodeID: number;
  zipCode?: string;
  city?: string;
  stateCode?: string;
  county?: string;
  countryCode?: string;
  // zipCodeExt?: string;
  // Longitude: number;
  // Latitude: number;
  isGarageIndoor: boolean;
  isGarageOutdoor: boolean;
  isGarageFenced: boolean;
  isGarageLighted: boolean;
  isGarageWithSecurityGuard: boolean;
  //createdBy
  //createdDate
  //effectiveDate?: string;
  //expirationDate?: string;
  //removedProcessDate?
  //removedBy?
}