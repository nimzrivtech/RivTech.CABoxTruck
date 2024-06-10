export class MenuCategory {
    menuCategoryId: number;
    programId: number;
    description: string;
    createdDate: Date;
    isActive: boolean;

    public constructor(init?: Partial<MenuCategory>) {
        Object.assign(this, init);
    }
}
