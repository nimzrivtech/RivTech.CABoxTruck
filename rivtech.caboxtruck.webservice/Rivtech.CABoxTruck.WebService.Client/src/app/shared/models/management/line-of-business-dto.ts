import { ProgramDTO } from './program-dto';
export class LineOfBusinessDTO implements ILineOfBusinessDTO {
    id: number;
    name: string;
    sort: number;
    programs: ProgramDTO[];

    constructor(data?: ILineOfBusinessDTO) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
}

export interface ILineOfBusinessDTO {
    id: number;
    name: string;
    sort: number;
    programs: ProgramDTO[];
}