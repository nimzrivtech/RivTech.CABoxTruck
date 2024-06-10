export class ReversePaymentDTO implements IReversePaymentDTO {
    riskId?: string;
    paymentId?: string;
    reversalTypeId?: string;
    comment: string;
    appId: string;

    constructor(data?: IReversePaymentDTO) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
}

export interface IReversePaymentDTO {
    riskId?: string;
    paymentId?: string;
    reversalTypeId?: string;
    comment: string;
}