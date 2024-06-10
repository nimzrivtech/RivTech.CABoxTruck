export class DatasetDTO implements IDatasetDTO {
    id?: number | undefined;
    value?: number | undefined;
    description?: string | undefined;
    descriptionInUI?: string | undefined;
    orderInUI?: number | undefined;
    supportedInRaterVersion?: string | undefined;
    datasetType?: string | undefined;
    IsActive?: boolean | undefined;

    constructor(init?: Partial<DatasetDTO>) {
        Object.assign(this, init);
    }
}

export interface IDatasetDTO {
    id?: number | undefined;
    value?: number | undefined;
    description?: string | undefined;
    descriptionInUI?: string | undefined;
    orderInUI?: number | undefined;
    supportedInRaterVersion?: string | undefined;
    datasetType?: string | undefined;
    IsActive?: boolean | undefined;
}
