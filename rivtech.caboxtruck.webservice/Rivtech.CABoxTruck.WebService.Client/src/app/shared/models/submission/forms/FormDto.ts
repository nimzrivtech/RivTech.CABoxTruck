import { FormVersionDTO } from "./FormVersionDTO";
export class FormDTO implements IFormDTO {
    id?: string;
    riskDetailId?: string | undefined;
    isSelected?: boolean | undefined;
    include?: boolean | undefined;
    other?: string | undefined;
    otherEditable?: boolean | undefined;
    form?: FormInfoDTO | undefined;
    createdBy?: string | undefined;
    createdDate?: Date | undefined;
    formVersions?: FormVersionDTO[] | undefined;
    latestFormVersion?: FormVersionDTO | undefined;

    constructor(init?: Partial<FormDTO>) {
        Object.assign(this, init);
    }
}

export interface IFormDTO {
    id?: string;
    riskDetailId?: string | undefined;
    isSelected?: boolean | undefined;
    include?: boolean | undefined;
    other?: string | undefined;
    otherEditable?: boolean | undefined;
    form?: FormInfoDTO | undefined;
    createdBy?: string | undefined;
    createdDate?: Date | undefined;
    formVersions?: FormVersionDTO[] | undefined;
    latestFormVersion?: FormVersionDTO | undefined;
}

export class FormInfoDTO implements IFormInfoDTO {
    id?: string;
    name?: string;
    sortOrder?: number | undefined;
    formNumber?: string | undefined;
    isMandatory?: boolean | undefined;
    fileName?: string | undefined;
    isSupplementalDocument?: boolean | undefined;

    constructor(init?: Partial<FormDTO>) {
        Object.assign(this, init);
    }
}

export interface IFormInfoDTO {
    id?: string;
    name?: string;
    sortOrder?: number | undefined;
    formNumber?: string | undefined;
    isMandatory?: boolean | undefined;
    fileName?: string | undefined;
    isSupplementalDocument?: boolean | undefined;
}
