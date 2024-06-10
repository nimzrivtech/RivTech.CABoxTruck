export class FieldVersionDTO {
    public id: string | undefined = null;
    public fieldName: string | undefined = null;
    public description: string | undefined = null;
    public effectiveDate: Date | undefined = null;
  
    constructor(init?: Partial<FieldVersionDTO>) {
      Object.assign(this, init);
    }
  }
  