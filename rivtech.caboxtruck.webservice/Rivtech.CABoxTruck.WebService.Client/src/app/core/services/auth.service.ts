import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SessionTimeoutComponent } from '@app/login/session-timeout/session-timeout.component';
import { LoginLabelsConstants } from '@app/shared/constants/login.labels.constants';
import { PathConstants } from '@app/shared/constants/path.constants';
import Utils from '@app/shared/utilities/utils';
import { DocumentInterruptSource, Idle, StorageInterruptSource } from '@ng-idle/core';
import { Keepalive } from '@ng-idle/keepalive';
import { BsModalService } from 'ngx-bootstrap';
import { BehaviorSubject, Observable, of, Subject, throwError as observableThrowError } from 'rxjs';
import { catchError, debounceTime, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { TokenResponse } from '../../shared/models/tokenResponse';
import { UserAccessRight } from '../../shared/models/userAccessRight';
import { ClientDetailService } from './generics/clientDetails.services';

@Injectable()
export class AuthService {

  isLogout: boolean;
  userType = new BehaviorSubject<'external' | 'internal'>('external');
  isPasswordChanged: boolean = false;
  isLocked: boolean = false;

  private userLoggedIn = new Subject<boolean>();
  idleState = 'Not started.';
  timedOut = false;
  lastPing?: Date = null;
  title = 'angular-idle-timeout';
  isUserLoggedIn: boolean = false;

  public currentUserRole: string;

  LoginDateFormGroup = new FormGroup({
    loginDate: new FormControl(undefined)
  });

  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute, private httpbackend: HttpBackend
    , private clientDetailService: ClientDetailService,
    private modalService: BsModalService,
    public idle: Idle,
    private keepalive: Keepalive) {
    this.isLogout = true;
    this.userLoggedIn.next(false);
  }

  login(username: string, password: string): Observable<boolean> {
    const url = `/auth`;
    const clientDetails = this.clientDetailService.getClientDetails();

    let data = {
      username: username,
      password: password,
      appCode: environment.ApplicationId
    };

    return this.getAuthFromServer(url, data);
  }


  logout(): boolean {

    this.setAuth(null);
    this.setUserLoggedIn(false);
    return true;
  }


  setAuth(auth: TokenResponse | null): boolean {
    if (auth) {
      localStorage.setItem(environment.AuthKey, JSON.stringify(auth));

      const userInfo: string = atob(JSON.stringify(auth).split('.')[1]);

      localStorage.setItem(environment.UsernameKey, JSON.parse(userInfo).unique_name);
      localStorage.setItem(environment.UserIdentifierKey, JSON.parse(userInfo).sub);
      localStorage.setItem(environment.CurrentUserRole, JSON.parse(userInfo).role_id);
    } else {
      localStorage.removeItem(environment.AuthKey);
    }

    return true;
  }

  getAuth(): TokenResponse | null {
    const item = localStorage.getItem(environment.AuthKey);

    if (item) {
      return JSON.parse(item);
    }

    return null;
  }

  getUserName(): string {
    const userName = localStorage.getItem(environment.UsernameKey);
    return userName ? userName : null;
  }

  getCurrentUserRole(): string {
    const role = localStorage.getItem(environment.CurrentUserRole);
    return role ? role : null;
  }

  getCurrentUserId(): string {
    const userId = localStorage.getItem(environment.UserIdentifierKey);
    return userId ? userId : null;
  }

  isLoggedIn(): boolean {
    let authKey = localStorage.getItem(environment.AuthKey);

    return authKey != null && authKey != '';
  }


  getAuthFromServer(url: string, data: any): Observable<boolean> {
    return this.http.post<any>(`${environment.IdentityServiceUrl}${url}`, data).pipe(
      map((res) => {

        const token = res && res.token;
        if (token) {
           sessionStorage.setItem('isLoggedIn', 'true'); // Set session Storage
          this.setAuth(res);
          const decodedJWT = JSON.parse(window.atob(token.split('.')[1]));
          this.isPasswordChanged = decodedJWT.password_changed;

          if (this.isPasswordChanged === true) {
              // this.saveUserInformation(token);
              return true;
          } else {
              this.router.navigate([`${PathConstants.Account.Index}/${PathConstants.Account.NewUserChangePassword}`]);
              Utils.unblockUI();
          }
      } else {
          return false;
      }
      }),
      catchError(error => {
        const exist = error.error.error.toLowerCase().includes(LoginLabelsConstants.errorMessage.exist);
        const incorrect = error.error.error.toLowerCase().includes(LoginLabelsConstants.errorMessage.incorrect);
        const inactive = error.error.error.toLowerCase().includes(LoginLabelsConstants.errorMessage.userInactive);
        const locked = error.error.error.toLowerCase().includes(LoginLabelsConstants.errorMessage.locked);
        if (exist || incorrect || inactive) {
            const msg = exist ? LoginLabelsConstants.errorMessage.exist :
                incorrect ? LoginLabelsConstants.errorMessage.incorrect : LoginLabelsConstants.errorMessage.userInactive;
            // this.loginAuditLog(msg, data.username);
            if (incorrect) {
              console.log('log failed login');
              this.logFailedAcctLogin(data.username);
            }
        }

        if (locked) {
          this.isLocked = true;

          const separator = '|';
          const errorMessage = error.error.error;
          const lockedMessage: string[] = errorMessage.includes(separator) ? errorMessage.split(separator) : null;

          if (lockedMessage && lockedMessage[1] === LoginLabelsConstants.errorMessage.locked) {
            error.error.error = lockedMessage[0];
            console.log('log logout');
            this.logAccountLckout(data.username);
          } else {
            console.log('log initial lock');
            this.logAccountLckout(data.username, true);
          }
        }

        return observableThrowError(error);
      }));
  }

  logFailedAcctLogin(username: string) {
    const clientDetails = this.clientDetailService.getClientDetails();
    const payload = {
      username: username,
      os: `${clientDetails.os} ${clientDetails.osVersion}`,
      deviceType: clientDetails.mobile ? 'Mobile' : 'PC',
      browser: clientDetails.browser,
      browserVersion: clientDetails.browserVersion,
      appVersion: clientDetails.appVersion
    };

    this.logFailedLogin(payload).subscribe();
  }

  logAccountLckout(username: string, isInitial: boolean = false) {
    const clientDetails = this.clientDetailService.getClientDetails();
    const payload = {
      username: username,
      os: `${clientDetails.os} ${clientDetails.osVersion}`,
      deviceType: clientDetails.mobile ? 'Mobile' : 'PC',
      browser: clientDetails.browser,
      browserVersion: clientDetails.browserVersion
    };

    if (isInitial) {
      this.logLockAccount(payload).subscribe();
    } else {
      this.logLockedAccount(payload).subscribe();
    }
  }

  validateAuthFromServer(password: any): Observable<boolean> {
    const url = `/auth`;

    const data = {
      username: this.getUserName(),
      password: password,
      appCode: environment.ApplicationId
    };

    this.http = new HttpClient(this.httpbackend);
    return this.http.post<any>(`${environment.IdentityServiceUrl}${url}`, data).pipe(
      map((res) => {
        const token = res && res.token;
        if (token) {
          return true;
        }
        return false;
      }),
      catchError(error => {
        return observableThrowError(error);
      }));
  }

  getUserAccessRights(): Observable<UserAccessRight[]> {

    return this.http.get<any>(`${environment.IdentityServiceUrl}/Auth/access-rights`).pipe(
      map((res) => {
        // console.log('access rights: ' + JSON.stringify(res));
        return res;
      }),
      catchError(error => {
        console.log(error);
        return observableThrowError(error);
      }));
  }

  getUserDetails(): Observable<any> {
    return this.http.get<any>(`${environment.IdentityServiceUrl}/api/Users/${this.getUserName()}/${environment.ApplicationId}/user-details`)
      .catch(error => observableThrowError(error));
  }

  public getCustomDate(isForUTCConversion: boolean = false): Date {
    const localStorageDate = localStorage.getItem('loginDate');
    if (localStorageDate !== 'undefined' && localStorageDate !== null && localStorageDate !== 'null') {
      this.LoginDateFormGroup.get('loginDate').setValue(this.reformatToDateTime(JSON.parse(localStorageDate), true));
      return this.LoginDateFormGroup.get('loginDate').value;
    } else {
      const date = new Date();
      const utcDate = new Date(date.toUTCString());
      utcDate.setMilliseconds(date.getMilliseconds());
      return !isForUTCConversion ? new Date() : utcDate;
    }

  }

  public reformatToDateTime(dateValue?: any, isFromCustomDatePicker?: boolean): any {
    const currentDate = new Date();
    switch (isFromCustomDatePicker) {
      case true:
        const date = dateValue?.singleDate?.date;
        return new Date(`${date.month}/${date.day}/${date.year} ${currentDate.getHours()}:${currentDate.getMinutes()}:${currentDate.getSeconds()}.${currentDate.getMilliseconds()}`);
      case false:
        return new Date(`${dateValue.month}/${dateValue.day}/${dateValue.year} ${currentDate.getHours()}:${currentDate.getMinutes()}:${currentDate.getSeconds()}.${currentDate.getMilliseconds()}`);
    }
  }

  logUserLogin(username: string): any {
    return this.http.post(`${environment.ApiUrl}/User/login`, { username: username, appVersion: environment.appVersion }, { responseType: 'text' }).catch(err => err);
  }

  logUserLogout(username: string): any {
    return this.http.post(`${environment.ApiUrl}/User/logout`, { username }, { responseType: 'text' }).catch(err => err);
  }

  logFailedLogin(clientDetails): any {
    return this.http.post(`${environment.ApiUrl}/User/failed`, clientDetails, { responseType: 'text' }).catch(err => err);
  }

  logLockAccount(clientDetails): any {
    return this.http.post(`${environment.ApiUrl}/User/lock`, clientDetails, { responseType: 'text' }).catch(err => err);
  }

  logLockedAccount(clientDetails): any {
    return this.http.post(`${environment.ApiUrl}/User/locked`, clientDetails, { responseType: 'text' }).catch(err => err);
  }

  unlockAccount(clientDetails): any {
    return this.http.post(`${environment.IdentityServiceUrl}/Auth/reset-login`, clientDetails, { responseType: 'text' }).catch(err => err);
  }

  getUserId(): number {
    const id = Number(localStorage.getItem(environment.UserIdentifierKey));
    return id ? id : 0;
  }

  savePasswordAuditLog(action: string, username: string): void {
    /*  const payload = {
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

  saveUserInfo(body: any | undefined): Observable<any> {
    /* const url_ = `${environment.ApiUrl}/User`;  //NOT IMPLEMENT YET IN BoxTruck API
    return this.http.post(url_, body).pipe(
        map((res) => {
            return res;
        }),
        catchError(error => {
            return observableThrowError(error);
        })); */
    return of({});
  }

  getAppSettings(): Observable<any> {
    return this.http.get<any>(`${environment.ApiUrl}/AppSettings`)
      .catch(error => observableThrowError(error));
  }

  initAppSettings() {
    this.getAppSettings().subscribe((data) => {
      // Set settings to localstorage
      localStorage.setItem('ActivateInactivityTimerSecs', data.activateInactivityTimerSecs);
      localStorage.setItem('TriggerInactivityTimerSecs', data.triggerInactivityTimerSecs);

      this.initTimerSecs();
      this.setUserLoggedIn(true);
    });
  }

  initTimerSecs() {
    const ActivateInactivityTimer = localStorage.getItem('ActivateInactivityTimerSecs') ?? '3420';
    const TriggerInactivityTimer = localStorage.getItem('TriggerInactivityTimerSecs') ?? '180';
    console.log('ActivateInactivityTimer: ', ActivateInactivityTimer, ' - TriggerInactivityTimer: ', TriggerInactivityTimer);
    this.idle.setIdle(+ActivateInactivityTimer);//57mins convert to 3420 seconds //
    this.idle.setTimeout((+TriggerInactivityTimer)+1);//3mins convert to 180seconds
    this.idle.setInterrupts(this.createCustomInterruptSources(null));
  }

  idleTime(){
    this.initTimerSecs();

    this.idle.onIdleEnd.subscribe((d) => {
      this.idleState = 'No longer idle.';
      console.log(this.idleState);
      this.idle.stop();
    });

    this.idle.onTimeout.subscribe(() => {
      this.idleState = 'Timed out!';
      this.timedOut = true;
      console.log(this.idleState);
      this.idle.stop();
    });

    this.idle.onIdleStart.subscribe(() => {
      this.idleState = 'You\'ve gone idle!';
      let timer = +localStorage.getItem('TriggerInactivityTimerSecs');
      const endTime = (timer * 1000) + Date.now();
      localStorage.setItem('endTime', ''+endTime);
      console.log(this.idleState);
      if (!this.isLoggedIn()) {
        console.log('already logged out');
        this.setUserLoggedIn(false);
      } else {
        this.callSessionTimeout();
      }
    });

    this.idle.onTimeoutWarning.subscribe((countdown) => {
      this.idleState = 'You will time out in ' + countdown + ' seconds!';
      console.log(this.idleState);
    });
    // sets the ping interval to 15 seconds
    this.keepalive.interval(15);
    this.keepalive.onPing.subscribe(() => this.lastPing = new Date());
    this.getUserLoggedIn().subscribe(userLoggedIn => {
      if (userLoggedIn) {
        this.idle.watch();
        this.timedOut = false;
      } else {
        this.idle.stop();
      }
    });
  }

  ngOnDestroy() {
    this.idle.onTimeout.unsubscribe();
    this.idle.onIdleStart.unsubscribe();
    this.idle.onIdleEnd.unsubscribe();
  }

  createCustomInterruptSources(options) {
    return [
        new DocumentInterruptSource('focus click mousedown keypress keydown keyup DOMMouseScroll mousewheel touchmove MSPointer resize scroll ', options),
        new StorageInterruptSource(options)
    ];
  }

  reset() {
    this.idle.watch();
    this.idleState = 'Started.';
    this.timedOut = false;
  }

  setUserLoggedIn(userLoggedIn: boolean) {
    localStorage.setItem('isUserLoggedIn','true');

    if (!userLoggedIn) {
      localStorage.removeItem('isUserLoggedIn');
    }

    this.userLoggedIn.next(userLoggedIn);
  }

  getUserLoggedIn(): Observable<boolean> {
    return this.userLoggedIn.asObservable();
  }

  callSessionTimeout(){
    this.modalService.show(SessionTimeoutComponent, {
      initialState: {
        modalTitle: 'Session Timeout',
        authService: this
      },
      backdrop: 'static',
      keyboard: false,
      ignoreBackdropClick: true,
      class: 'my-modal'
    });
  }
}
