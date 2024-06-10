export class InvalidFormControl implements IInvalidFormControl {
  page?: string | undefined;
  section?: string | undefined;
  formControlName?: string | undefined;
}

export interface IInvalidFormControl {
  page?: string | undefined;
  section?: string | undefined;
  formControlName?: string | undefined;
}
