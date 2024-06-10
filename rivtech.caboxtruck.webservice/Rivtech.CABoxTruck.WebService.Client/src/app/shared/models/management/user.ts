export class UserDTO {
    userId: number;
    userName: string;
    firstName: string;
    lastName: string;
    programId: number;
    carrierCode: string;
    agencyId: number;
    subAgencyId: number;
    emailAddress: string;
    createdDate: string;
    isActive: boolean;
    isInternal: boolean;
    companyName: string;
    torrentUserName: string;
    userAccessRights: UserAccessRight[];
    userCredentials: UserCredentials;
}

export class UserCredentials {
    userCredentialId: number;
    userName: string;
    password: string;
    salt: string;
    createdDate: Date;
    expirationDate: Date;
    isActive: boolean;
}

export class UserAccessRight {
    userAccessRightId: number;
    userName: string;
    roleId: number;
    menuId: number;
    isView: boolean;
    isAdd: boolean;
    isEdit: boolean;
    isDelete: boolean;
    isActive: boolean;
    public constructor(init?: Partial<UserAccessRight>) {
        Object.assign(this, init);
    }
}
