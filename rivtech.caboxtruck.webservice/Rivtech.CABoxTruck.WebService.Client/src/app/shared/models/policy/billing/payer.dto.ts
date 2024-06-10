export class PayerDTO implements IPayerDTO {
    name?: string;
    address?: string;
    city?: number;
    state?: number;
    zip?: number;

    constructor(data?: IPayerDTO) {
      if (data) {
        for (const property in data) {
          if (data.hasOwnProperty(property)) {
            (<any>this)[property] = (<any>data)[property];
          }
        }
      }
    }
  }

  export interface IPayerDTO {
    name?: string;
    address?: string;
    city?: number;
    state?: number;
    zip?: number;
  }