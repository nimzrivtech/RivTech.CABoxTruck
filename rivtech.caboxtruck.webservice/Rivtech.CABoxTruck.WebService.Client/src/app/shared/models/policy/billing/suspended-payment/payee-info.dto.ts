export class PayeeInfoDTO implements IPayeeInfoDTO {
  name: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;

  constructor(data?: IPayeeInfoDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IPayeeInfoDTO {
  name: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;
}