import { UserAccessRight, UserCredentials } from './user';

export class UserViewModel {
    public userId?: number;
    public userName?: string;
    public firstName?: string;
    public lastName?: string;
    public appCode?: number;
    public roleId?: number;
    public carrierCode?: string;
    public agencyId?: string | undefined;
    public subAgencyId?: string | undefined;
    public emailAddress?: string;
    public isAgent?: boolean;
    public isSystemUser?: boolean;
    public isAgencyAdmin?: boolean;
    public isSubAgencyAdmin?: boolean;
    public createdDate?: Date; //UPDATE
    public isInternal?: boolean;
    public isActive: boolean;
    public companyName?: string; //UPDATE
    public userAccessRights?: UserAccessRight[];
    public userCredentials?: UserCredentials;
    public fullName?: string;
    public oldUserName?: string;
    public torrentUserName?: string;

    public isLocked?: boolean;
}
