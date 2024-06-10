import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, throwError as observableThrowError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { CookieService } from 'ngx-cookie-service';


@Injectable()
export class AccountService {

    isLogout: boolean;

    public currentUserRole: string;

    constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute, private cookieService: CookieService) {
        this.isLogout = true;
    }


    sendForgotPasswordEmail(emailAddress: string) {

        const data = {
            email: emailAddress,
            appCode: environment.ApplicationId
        };

        return this.http.post(`${environment.GenericServiceUrl}/api/user/forgot-password`, data, { responseType: 'text' }).pipe(
            map((res) => {
                return res;
            }),
            catchError(error => {
                console.log(error);
                return observableThrowError(error);
            }));
    }


    resetPassword(referenceNum: string, password: string) {

        const data = {
            ref: referenceNum,
            password: password,
            appCode: environment.ApplicationId
        };

        return this.http.put(`${environment.GenericServiceUrl}/api/user/forgot-password`, data, { responseType: 'text' }).pipe(
            map((res) => {
                return res;
            }),
            catchError(error => {
                console.log(error);
                return observableThrowError(error);
            }));
    }

    changePassword(data: any) {
        return this.http.put(`${environment.GenericServiceUrl}/api/user/change-password`, data, { responseType: 'text' }).pipe(
            map((res) => {
                return res;
            }),
            catchError(error => {
                return observableThrowError(error);
            }));
    }

    userResetPassword(data: any) {
        return this.http.put(`${environment.GenericServiceUrl}/api/user/system-change-password`, data, { responseType: 'text' }).pipe(
            map((res) => {
                return res;
            }),
            catchError(error => {
                return observableThrowError(error);
            }));
    }

    savePasswordAuditLog(action: string, username: string): void {
        /*   const payload = {
            userId: this.getUserId(),
            keyId: username,
            auditType: '',
            description: '',
            method: ''
          };

          switch (action) {
              case PasswordFormConstants.auditLog.action.requestForgot:
                  payload.auditType = PasswordFormConstants.auditLog.forgotPwRequestLog.auditType;
                  payload.description = PasswordFormConstants.auditLog.forgotPwRequestLog.description.replace('{0}', username);
                  payload.method = PasswordFormConstants.auditLog.methodName.requestForgot;
                  break;
              case PasswordFormConstants.auditLog.action.successForgot:
                  payload.auditType = PasswordFormConstants.auditLog.forgotPwSuccessLog.auditType;
                  payload.description = PasswordFormConstants.auditLog.forgotPwSuccessLog.description.replace('{0}', username);
                  payload.method = PasswordFormConstants.auditLog.methodName.successForgot;
                  break;
              case PasswordFormConstants.auditLog.action.successChange:
                  payload.auditType = PasswordFormConstants.auditLog.changePwSuccessLog.auditType;
                  payload.description = PasswordFormConstants.auditLog.changePwSuccessLog.description.replace('{0}', username);
                  payload.method = PasswordFormConstants.auditLog.methodName.change;
                  break;
              case PasswordFormConstants.auditLog.action.successReset:
                  payload.auditType = PasswordFormConstants.auditLog.resetPwSuccessLog.auditType;
                  payload.description = PasswordFormConstants.auditLog.resetPwSuccessLog.description.replace('{0}', username);
                  payload.method = PasswordFormConstants.auditLog.methodName.reset;
                  break;
          }

          this.auditLogService.insertToAuditLog(payload).subscribe(); */
    }


}
