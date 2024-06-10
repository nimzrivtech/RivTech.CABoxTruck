export class SuspendedPayeeRequestDTO implements ISuspendedPayeeRequestDTO {
  name: string;
  address: string;
  city: string;
  state: string;
  zip: string;

  constructor(data?: ISuspendedPayeeRequestDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface ISuspendedPayeeRequestDTO {
  name: string;
  address: string;
  city: string;
  state: string;
  zip: string;
}