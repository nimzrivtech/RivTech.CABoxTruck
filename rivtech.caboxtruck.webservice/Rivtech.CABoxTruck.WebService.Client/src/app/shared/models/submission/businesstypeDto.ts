export class BusinessTypeDTO implements IBusinessTypeDTO {
    name?: string | undefined;
    createdBy?: number;
    createdDate?: Date;
    id?: number;
    isActive?: boolean;

    constructor(init?: Partial<BusinessTypeDTO>) {
        Object.assign(this, init);
      }
}

export interface IBusinessTypeDTO {
    name?: string | undefined;
    createdBy?: number;
    createdDate?: Date;
    id?: number;
    isActive?: boolean;
}