export class Validation {    
    validationTypeId: number;
    value: string;
}

export interface IQuestionValidation {
    validation: Validation;
}

export class QuestionValidation implements IQuestionValidation {    
    validation: Validation;

    public constructor(init?: Partial<IQuestionValidation>) {
        Object.assign(this, init);
    }
}