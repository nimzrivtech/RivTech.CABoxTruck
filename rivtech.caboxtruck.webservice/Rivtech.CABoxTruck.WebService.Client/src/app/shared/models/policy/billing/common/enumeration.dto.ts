
export class EnumerationDTO implements IEnumerationDTO {
  id?: string | undefined;
  description?: string | undefined;

  constructor(data?: IEnumerationDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IEnumerationDTO {
  id?: string | undefined;
  description?: string | undefined;
}