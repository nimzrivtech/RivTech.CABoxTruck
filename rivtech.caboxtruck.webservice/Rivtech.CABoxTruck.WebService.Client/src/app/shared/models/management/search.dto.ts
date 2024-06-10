export class SearchDTO implements ISearchDTO {
    term: string;
    appCode?: number;
    agencyId?: string;

    constructor(data?: ISearchDTO) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
}

export interface ISearchDTO {
    term: string;
    appCode?: number;
    agencyId?: string;
}