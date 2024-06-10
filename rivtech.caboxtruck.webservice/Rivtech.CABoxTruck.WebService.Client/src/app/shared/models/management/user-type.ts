export class UserType {
    roleId: number;
    programId: number;
    roleGroupId: number;
    roleName: string;
    createdDate: Date;
    isInternal: boolean;
    isActive: boolean;
    roleGroup: RoleGroup;
}

export class RoleGroup {
    roleGroupId: number;
    description: string;
    isActive: boolean;
}