import { UserAccessRight } from './user';

export class RolePagesUserAccess extends UserAccessRight {
    public parentId: number;
    public menuLabel: string;
    public constructor(init?: Partial<RolePagesUserAccess>) {
        super();
        Object.assign(this, init);
    }
}
