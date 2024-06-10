export interface IErrorLogDTO {
    ErrorCode?: string;
    ErrorMessage?: string;
    JsonMessage?: string;
    Action?: string;
    Method?: string;
}

export class ErrorLogDTO implements IErrorLogDTO {
    ErrorCode?: string;
    ErrorMessage?: string;
    JsonMessage?: string;
    Action?: string;
    Method?: string;

    public constructor(init?: Partial<IErrorLogDTO>) {
        Object.assign(this, init);
    }
}