import { Injectable, Injector } from "@angular/core";
import { HttpHandler, HttpEvent, HttpInterceptor, HttpRequest } from "@angular/common/http";

import { Observable } from "rxjs";
import { AuthService } from '../services/auth.service';


@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private injector: Injector) { }
    intercept(
        request: HttpRequest<any>,
        next: HttpHandler): Observable<HttpEvent<any>> {
        const authService = this.injector.get(AuthService);

        let token = null;
        if (authService.isLoggedIn()) {
            const auth = authService.getAuth();
            token = auth ? auth.token : null;
        }

        if (token) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`,
                    // 'Content-Source': 'web/angular',
                    // 'Content-Type': 'application/json'
                }
            });
        }
        return next.handle(request);
    }
}
