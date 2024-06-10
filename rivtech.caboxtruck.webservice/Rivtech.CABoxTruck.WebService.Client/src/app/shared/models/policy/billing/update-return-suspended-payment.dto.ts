export class UpdateReturnDetailsDTO implements IUpdateReturnDetailsDTO {
    suspendedPaymentId: string;
    checkIssueDate?: Date;
    checkNumber?: string;
    checkClearDate?: Date;
    checkEscheatDate?: Date;

    constructor(data?: IUpdateReturnDetailsDTO) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
}

export interface IUpdateReturnDetailsDTO {
    suspendedPaymentId: string;
    checkIssueDate?: Date;
    checkNumber?: string;
    checkClearDate?: Date;
    checkEscheatDate?: Date;
}