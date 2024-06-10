import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthService } from './services/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { AuthGuard } from './guards/auth.guard';
import { AuthInterceptor } from './interceptors/auth-interceptor';
import { RoleGuard } from './guards/role.guard';
import { ErrorResponseInterceptor } from './interceptors/error-response-interceptor';
import { QuestionService } from './services/submission/question.service';
import { CommonService } from './services/common.service';
import { CanDeactivateGuard } from './guards/candeactivate.guard';
import { ClientDetailService } from './services/generics/clientDetails.services';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorResponseInterceptor,
      multi: true
    },

    AuthService,
    ClientDetailService,
    CommonService,
    CookieService,
    QuestionService,
    DatePipe,

    AuthGuard,
    RoleGuard,
    CanDeactivateGuard
  ]
})
export class CoreModule { }
