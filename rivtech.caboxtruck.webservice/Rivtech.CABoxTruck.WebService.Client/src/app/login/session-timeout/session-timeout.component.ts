/* eslint-disable prefer-arrow/prefer-arrow-functions */
/* eslint-disable @angular-eslint/use-lifecycle-interface */
import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { Observable, Subscription, timer } from 'rxjs';
import { AuthService } from 'app/core/services/auth.service';
import { ErrorMessageConstant } from 'app/shared/constants/error-message.constants';
import NotifUtils from 'app/shared/utilities/notif-utils';
import { NavigationEnd, Router } from '@angular/router';
import { PathConstants } from 'app/shared/constants/path.constants';
import { Subject } from 'rxjs/internal/Subject';
import { BsModalService } from 'ngx-bootstrap/modal';
import { LocationStrategy } from '@angular/common';
import { SessionTimeoutLabelsConstants } from '@app/shared/constants/session-timeout-constants';
const counter = timer(0, 2000);
@Component({
  selector: 'app-session-timeout',
  templateUrl: './session-timeout.component.html',
  styleUrls: ['./session-timeout.component.scss']
})
export class SessionTimeoutComponent implements OnInit {
  public clock;
  public SessionTimeoutLabelsConstants  = SessionTimeoutLabelsConstants;
  timer: number;
  private authService: AuthService;
  public countDown: Observable<void>;
  private onDestroy$ = new Subject<void>();
  display = '';
  public intervalSub: Subscription;
  mySubscription: any;
  constructor(
    private modalRef: BsModalRef,
    private router: Router,
    private modalService: BsModalService,
    private location: LocationStrategy
  ) {}

  ngOnInit(){
    this.startTimer();
  }

  startTimer() {
    this.authService.idle.clearInterrupts();
    history.pushState(null, null, window.location.href);//disabled back and forward button of browser
    this.location.onPopState(() => {
      history.pushState(null, null, window.location.href);
    });
    this.timer = +localStorage.getItem('TriggerInactivityTimerSecs');
    let minutes;
    let seconds;
    let isIdle;

    this.intervalSub = Observable.interval(500)
    .takeUntil(this.onDestroy$)
    .subscribe(x => {
        let endTimeInfo = +localStorage.getItem('endTime');
        this.timer = (endTimeInfo - Date.now()) / 1000;
        minutes = Math.floor(this.timer / 60);
        seconds = Math.floor(this.timer % 60);

        minutes = minutes < 10 ? `0${minutes}` : minutes;
        seconds = seconds < 10 ? `0${seconds}` : seconds;

        this.display = `${ minutes }${ SessionTimeoutLabelsConstants.mins } ${ seconds } ${ SessionTimeoutLabelsConstants.secs}`;

        isIdle = localStorage.getItem('ng2Idle.main.idling');
        console.log('for checker timer:',this.timer, isIdle);
        if(this.timer <= 0 ) {
          console.log('timer stop');
          this.authService.reset();
          this.authService.idle.watch();
          this.authService.setAuth(null);
          this.authService.setUserLoggedIn(false);
          localStorage.setItem('isUserLoggedIn','false');
          localStorage.removeItem('isUserLoggedIn');
          this.display  = `00 ${ SessionTimeoutLabelsConstants.mins } 00 ${ SessionTimeoutLabelsConstants.secs }`;
          this.intervalSub.unsubscribe();
          this.authService.idle.onTimeout.unsubscribe();
        } else if (isIdle == 'false' && this.timer > 0) {
          this.closeDialog();
        }
    });

  }

  showUnauthorizedError(): void {
    const defaults: any = {
        title: 'Oops...',
        icon: 'error',
        allowOutsideClick: false
    };
    NotifUtils.showNotice('Your token is invalid or has expired. Please login again.', defaults, () => {
      this.modalRef.hide();
      this.authService.logout();
        this.router.navigate([`/${PathConstants.Login.Index}`]);
    });
  }

  ngOnDestroy(): void {
    this.intervalSub.unsubscribe();
    this.authService.idle.onTimeout.unsubscribe();
    this.onDestroy$.next();
    this.authService.reset();
  }

  removeMinSec(str){
    const searchMins00 = str.search('00 mins');
    let newString;
    if(searchMins00 === 0 ){
      newString = str.slice(7);
    }else{
      newString = str;
    }
    return newString;
  }

  closeDialog(){
    this.authService.idle.setInterrupts(this.authService.createCustomInterruptSources(null));
    this.authService.idle.watch();
    this.display = `00 ${ SessionTimeoutLabelsConstants.mins }  00 ${ SessionTimeoutLabelsConstants.secs}`;
    this.modalRef.hide();
    this.intervalSub.unsubscribe();
  }

  closeTimoutDialog(){
    // localStorage.setItem('isUserLoggedIn','false');
    // this.authService.idle.watch();
    // this.authService.setUserLoggedIn(false);
    // this.authService.setAuth(null);
    // localStorage.setItem('isUserLoggedIn','false');
    window.location.href = '/dashboard';
    // window.location.reload();
    // this.router.navigate(['/dashboard']);
  }

};