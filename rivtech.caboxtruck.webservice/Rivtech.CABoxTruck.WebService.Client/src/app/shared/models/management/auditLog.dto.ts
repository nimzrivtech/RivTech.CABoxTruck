export class AuditLogDTO implements IAuditLogDTO {
    userId?: number | undefined;
    keyId?: string | undefined;
    auditType?: string | undefined;
    description?: string | undefined;
    method?:	string | undefined;

    constructor(data?: IAuditLogDTO) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
}

export interface IAuditLogDTO {
    userId?: number | undefined;
    keyId?: string | undefined;
    auditType?: string | undefined;
    description?: string | undefined;
    method?:	string | undefined;
}