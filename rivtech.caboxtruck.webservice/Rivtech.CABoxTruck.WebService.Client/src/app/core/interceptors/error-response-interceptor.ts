import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { Injector, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { tap, catchError, take } from 'rxjs/operators';
import { PathConstants } from '../../shared/constants/path.constants';
import NotifUtils from '../../shared/utilities/notif-utils';
import { ErrorMessageConstant } from '../../shared/constants/error-message.constants';
import { ErrorlogService } from '../services/errorlog.service';
import { ErrorLogDTO } from '../../shared/models/submission/errorlogDto';

@Injectable()
export class ErrorResponseInterceptor implements HttpInterceptor {
    currentRequest: HttpRequest<any>;
    auth: AuthService;
    constructor(
        private injector: Injector,
        private router: Router,
        private errorLogService: ErrorlogService
    ) {
     }

    // intercept any
    intercept(request: HttpRequest<any>,
        next: HttpHandler): Observable<HttpEvent<any>> {
        this.auth = this.injector.get(AuthService);

        let token = null;
        if (this.auth.isLoggedIn()) {
            let auth = this.auth.getAuth();
            token = auth ? auth.token : null;
        }

        // if (!token) {
        //     return next.handle(request);
        // }

        // save current request
        this.currentRequest = request;
        return next.handle(request).pipe(
            tap((event: HttpEvent<any>) => {
                if (event instanceof HttpResponse) {
                    // do nothing
                }
            }),
            catchError(error => {
                return this.handleError(error)
            }));
    }

    handleError(error: any) {
        if (error.status === 401 || error.status === 405) {
            if (window.location.pathname != `/${PathConstants.Login.Index}`) {
                NotifUtils.showError('Your token is invalid or has expired. Please login again.');
            }

            this.auth.logout();
            this.router.navigate([`/${PathConstants.Login.Index}`]);
            return;
        } else {
            if (error.status == 429) {
                NotifUtils.showError(ErrorMessageConstant.tooManyRequestErrorMEssage);
            } else {
                NotifUtils.showError(ErrorMessageConstant.contactAdminErrorMessage);
            }
            if (error.status === 500) {
                this.errorLogService.saveErrorLog(new ErrorLogDTO({
                    ErrorCode: error.status,
                    ErrorMessage: error.message,
                    Action: error.url,
                    JsonMessage: JSON.stringify(error)
                  })).pipe(take(1)).subscribe();
            }
        }
        return throwError(error);
    }
}
