export class AgenciesResponseDTO implements IAgenciesResponseDTO {
    id?:          string | undefined;
    entityId?:    string | undefined;
    createdBy?:   number | undefined;
    createdDate?: Date | undefined;
    entity?:      AgenciesEntity | undefined;
    hasBindingAuthority?:  boolean | undefined;
    isActive?: boolean | undefined;
    agencyCode?: string | undefined;
    constructor(data?: AgenciesResponseDTO) {
      if (data) {
        for (const property in data) {
          if (data.hasOwnProperty(property)) {
            (<any>this)[property] = (<any>data)[property];
          }
        }
      }
    }
  }

  export interface IAgenciesResponseDTO {
    id?:          string | undefined;
    entityId?:    string | undefined;
    createdBy?:   number | undefined;
    createdDate?: Date | undefined;
    entity?:      AgenciesEntity | undefined;
    hasBindingAuthority?:  boolean | undefined;
    isActive?: boolean | undefined;
    agencyCode?: string | undefined;
  }

  export interface AgenciesEntity {
    id?:                      string | undefined;
    isIndividual?:            boolean | undefined;
    firstName?:               string | undefined;
    middleName?:              string | undefined;
    lastName?:                string | undefined;
    companyName?:             string | undefined;
    fullName?:                string | undefined;
    homePhone?:               string | undefined;
    mobilePhone?:             string | undefined;
    workPhone?:               string | undefined;
    workPhoneExtension?:      string | undefined;
    workFax?:                 string | undefined;
    personalEmailAddress?:    string | undefined;
    workEmailAddress?:        string | undefined;
    birthDate?:               Date | undefined;
    age?:                     number | undefined;
    driverLicenseNumber?:     string | undefined;
    driverLicenseState?:      string | undefined;
    driverLicenseExpiration?: Date | undefined;
    genderID?:                number | undefined;
    maritalStatusID?:         number | undefined;
    businessTypeID?:          number | undefined;
    programId?:               number | undefined;
    dba?:                     string | undefined;
    yearEstablished?:         number | undefined;
    federalIDNumber?:         string | undefined;
    naicsCodeID?:             number | undefined;
    additionalNAICSCodeID?:   number | undefined;
    iccmcDocketNumber?:       string | undefined;
    usdotNumber?:             string | undefined;
    pucNumber?:               string | undefined;
    agencyCode?:              string | undefined;
    hasSubsidiaries?:         boolean | undefined;
    isActive?:                boolean | undefined;
    createdBy?:               number | undefined;
    entityAddresses?:         AgenciesEntityAddress[] | undefined;
    deleteAddresses?:         string[] | undefined;
}

export interface AgenciesEntityAddress {
    id?:                 string | undefined;
    entityId?:           string | undefined;
    addressId?:          string | undefined;
    addressTypeId?:      number | undefined;
    effectiveDate?:      Date | undefined;
    expirationDate?:     Date | undefined;
    addProcessDate?:     Date | undefined;
    addedBy?:            number | undefined;
    removedProcessDate?: Date | undefined;
    removedBy?:          null | undefined;
    address?:            AgenciesAddress | undefined;
}

export interface AgenciesAddress {
    id?:                        string | undefined;
    streetAddress1?:            string | undefined;
    streetAddress2?:            string | undefined;
    cityZipCodeID?:             number | undefined;
    zipCodeExt?:                string | undefined;
    longitude?:                 number | undefined;
    latitude?:                  number | undefined;
    isGarageIndoor?:            boolean | undefined;
    isGarageOutdoor?:           boolean | undefined;
    isGarageFenced?:            boolean | undefined;
    isGarageLighted?:           boolean | undefined;
    isGarageWithSecurityGuard?: boolean | undefined;
}