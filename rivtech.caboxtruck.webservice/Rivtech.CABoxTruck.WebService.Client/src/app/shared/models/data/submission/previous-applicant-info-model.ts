export class PreviousApplicantInfoModel implements IPreviousApplicantInfoModel {
  firstName?: string | undefined;
  middleName?: string | undefined;
  nameSuffix?: string | undefined;
  lastName?: string | undefined;
  birthDate?: any | undefined;

  public constructor(init?: Partial<PreviousApplicantInfoModel>) {
    Object.assign(this, init);
  }
}

export interface IPreviousApplicantInfoModel {
    firstName?: string | undefined;
    middleName?: string | undefined;
    nameSuffix?: string | undefined;
    lastName?: string | undefined;
    birthDate?: any | undefined;
}
