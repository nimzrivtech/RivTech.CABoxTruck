
export class NoteDTO implements INoteDTO {
    id?: number;
    riskDetailId: string | undefined;
    categoryId?: number;
    description: string | undefined;
    createdBy?: number | undefined;
    createdByName?: string | undefined;
    createdDate?: Date | undefined;
    sourcePage?: string | undefined;

    noteCategoryOption?: NoteCategoryOptionDTO | undefined;

    constructor(init?: Partial<NoteDTO>) {
        Object.assign(this, init);
      }
}

export interface INoteDTO {
    id?: number;
    riskDetailId: string | undefined;
    categoryId?: number;
    description: string | undefined;
    createdBy?: number | undefined;
    createdByName?: string | undefined;
    createdDate?: Date | undefined;
    sourcePage?: string | undefined;

    noteCategoryOption?: NoteCategoryOptionDTO | undefined;
}

export class NoteCategoryOptionDTO implements INoteCategoryOptionDTO {
    id?: number;
    description?: string | undefined;
    isActive?: boolean;

    constructor(init?: Partial<NoteCategoryOptionDTO>) {
        Object.assign(this, init);
      }
}

export interface INoteCategoryOptionDTO {
    id?: number;
    description?: string | undefined;
    isActive?: boolean;
}


