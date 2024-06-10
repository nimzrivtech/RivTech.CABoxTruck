export class BlacklistedDriverDTO {
  id?: string | undefined = null;
  firstName: string;
  lastName: string;
  driverLicenseNumber: string;
  isActive: boolean;
  comments: string;

  public constructor(init?: Partial<BlacklistedDriverDTO>) {
    Object.assign(this, init);
  }
}
