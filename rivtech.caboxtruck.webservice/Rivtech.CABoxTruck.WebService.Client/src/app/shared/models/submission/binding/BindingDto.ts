export class BindingDto {
  id?: string = null;

  bindOptionId?: string | undefined = null;
  paymentTypesId?: string | undefined = null;
  creditedOfficeId?: string | undefined = null;
  surplusLIneNum?: string | undefined = null;
  slaNum?: string | undefined = null;
  producerSLNumber?: string | undefined = null;
  slState?: string | undefined = null;
  minimumEarned: number | undefined = 0;
  riskId?: string = null;

  createdBy?: number | undefined = null;
  createdDate?: Date | undefined = null;
  updatedDate?: Date | undefined = null;

  constructor(init?: Partial<BindingDto>) {
    // Object.assign(this, init);
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}
