
export class Role {
    public roleId?: number;
    public roleName?: string;
    // public applicationId?: number;
    public programIds?: number | Array<number>;
    public roleGroup?: any;
    public roleGroupId?: number;
    public createdDate?: Date;
    public isActive?: boolean;
    public isInternal?: boolean;
}
