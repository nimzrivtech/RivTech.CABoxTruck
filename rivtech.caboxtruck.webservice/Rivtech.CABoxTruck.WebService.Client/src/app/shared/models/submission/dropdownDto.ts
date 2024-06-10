export class DropdownDTO implements IDropdownDTO {
    name?: string | undefined;
    createdBy?: number;
    createdDate?: Date;
    id?: number;
    isActive?: boolean;
    stateCode: string | undefined;

    constructor(init?: Partial<DropdownDTO>) {
        Object.assign(this, init);
      }
}

export interface IDropdownDTO {
    name?: string | undefined;
    createdBy?: number;
    createdDate?: Date;
    id?: number;
    isActive?: boolean;
    stateCode: string | undefined;
}
