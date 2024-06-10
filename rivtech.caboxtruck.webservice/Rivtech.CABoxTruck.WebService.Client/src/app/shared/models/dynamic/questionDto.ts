import { QuestionConfig } from './questionConfigDTO';
import { QuestionValidation } from './questionValidationDto';

export interface IQuestion {
    id: number;
    programId?: number;
    stateId?: number;
    parentQuestionId?: number;
    categoryId: number;
    sectionId: number;
    order: number;
    questionConfig: QuestionConfig;
    questionValidations?: QuestionValidation[];
}

export class Question implements IQuestion {    
    id: number;
    programId?: number;
    stateId?: number;
    parentQuestionId?: number;
    categoryId: number;
    sectionId: number;
    order: number;
    questionConfig: QuestionConfig;
    questionValidations?: QuestionValidation[];

    public constructor(init?: Partial<IQuestion>) {
        Object.assign(this, init);
    }
}