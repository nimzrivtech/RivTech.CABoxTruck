export class UserMenuSection {
    userMenuSectionId: number;
    programId: number;
    menuId: number;
    menuSectionId: number;
    userName: string;
    isActive: boolean;

    public constructor(init?: Partial<UserMenuSection>) {
        Object.assign(this, init);
    }
}
