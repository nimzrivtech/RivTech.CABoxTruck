
export class UserTwoFactorAuthResponse implements IUserTwoFactorAuthResponseDTO {
  isValid?: boolean | undefined;
  error?: string | undefined;

  constructor(data?: IUserTwoFactorAuthResponseDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IUserTwoFactorAuthResponseDTO {
  isValid?: boolean | undefined;
  error?: string | undefined;
}