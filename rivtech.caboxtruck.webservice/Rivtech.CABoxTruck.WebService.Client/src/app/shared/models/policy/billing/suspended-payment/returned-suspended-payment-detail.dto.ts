export class ReverseReturnedViewDTO implements IReverseReturnedViewDTO {
  returnDate?: Date;
  payee?: string;
  amount?: number;
  payer?: string;
  suspenseReason?: string;
  status?: string;
  comments?: string;

  constructor(data?: ReverseReturnedViewDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IReverseReturnedViewDTO {
  returnDate?: Date;
  payee?: string;
  amount?: number;
  payer?: string;
  suspenseReason?: string;
  status?: string;
  comments?: string;
}