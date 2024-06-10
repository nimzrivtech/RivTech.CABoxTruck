export interface IQuestionConfig {
    questionTypeId: number;
    label: string;
    defaultValue: string;
    responseTypeId: number;
}

export class QuestionConfig implements IQuestionConfig {
    questionTypeId: number;
    label: string;
    defaultValue: string;
    responseTypeId: number;

    public constructor(init?: Partial<IQuestionConfig>) {
        Object.assign(this, init);
    }
}