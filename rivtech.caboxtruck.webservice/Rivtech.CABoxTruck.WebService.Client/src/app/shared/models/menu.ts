export class Menu {
    menuId: number;
    parentId: number;
    applicationId: number;
    menuLabel: string;
    controllerName: string;
    createdDate: Date;
    isActive: boolean;

    isAdd: boolean;
    isEdit: boolean;
    isDelete: boolean;

    name: string;
    url: string;


    children: [];

    public constructor(init?: Partial<Menu>) {
        Object.assign(this, init);
    }
}