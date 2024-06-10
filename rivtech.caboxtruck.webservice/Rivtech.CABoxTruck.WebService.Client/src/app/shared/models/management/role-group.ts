import { Role } from './role';

export class RoleGroup {
    public roleGroupId: number;
    public description: string;
    public isActive: boolean;
    public roles: Role[];
}
