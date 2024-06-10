export class ApplicationUserDTO implements IApplicationUserDTO {
  id?: string | undefined;

  constructor(data?: IApplicationUserDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IApplicationUserDTO {
  id?: string | undefined;
}