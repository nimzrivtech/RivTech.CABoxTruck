export class MenuSection {
    menuSectionId: number;
    programId: number;
    menuId: number;
    sectionLabel: string;
    displayOrder: number;
    createdDate: Date;
    isActive: boolean;

    public constructor(init?: Partial<MenuSection>) {
        Object.assign(this, init);
    }
}
