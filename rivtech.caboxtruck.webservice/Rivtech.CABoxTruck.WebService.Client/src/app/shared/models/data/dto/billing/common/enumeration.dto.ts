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


export class EnumerationDTO2 implements IEnumerationDTO2 {
  id?: string | undefined;
  name?: string | undefined;

  constructor(data?: IEnumerationDTO2) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IEnumerationDTO2 {
  id?: string | undefined;
  name?: string | undefined;
}