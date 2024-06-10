export class ValidationListDTO  {
    public Applicant = new ValidateApplicant();
    public Limits = new ValidateLimits();
    public Coverages = new ValidateCoverages();

    constructor(init?: Partial<ValidationListDTO>) {
        Object.assign(this, init);
    }
}

export class ValidateApplicant  {
    public NameAndAddress: boolean = false;
    public BusinessDetails: boolean = false;
    public FilingsInformation: boolean = false;
    public AdditionalInterest: boolean = false;

    constructor(init?: Partial<ValidateApplicant>) {
        Object.assign(this, init);
    }
}

export class ValidateLimits  {
    public Limits: boolean = false;

    constructor(init?: Partial<ValidateLimits>) {
        Object.assign(this, init);
    }
}

export class ValidateCoverages  {
    public HistoricalCoverage: boolean = false;

    constructor(init?: Partial<ValidateCoverages>) {
        Object.assign(this, init);
    }
}
