import { Component, OnDestroy, OnInit } from '@angular/core';
import { ReferenceCodeConstants } from '../../../shared/constants/login.labels.constants';
import { BsModalRef } from 'ngx-bootstrap';
import { ValidationService } from '../../../core/services/validation.service';
import { UserTwoFactorAuthResponse } from '../../../shared/models/user-validation/user-two-factor-auth.response';
import { Router } from '@angular/router';
import Utils from '../../../shared/utilities/utils';
import { NavigationService } from '../../../core/services/navigation/navigation.service';
import { Subscription, timer } from 'rxjs';
import { TimerPipe } from '../../../shared/custom pipes/timer-pipe';
import { environment } from '../../../../environments/environment';
import { AuthService } from '../../../core/services/auth.service';
import { AuditLogService } from '../../../core/services/management/auditLog.service';
import { LoginLabelsConstants } from '../../../shared/constants/login.labels.constants';

@Component({
  selector: 'app-reference-code-modal',
  templateUrl: './reference-code-modal.component.html',
  styleUrls: ['./reference-code-modal.component.scss'],
  providers: [ TimerPipe ]
})
export class ReferenceCodeModalComponent implements OnInit, OnDestroy {
  title: string;
  description1: string;
  description2: string;

  referenceNumber: string;
  username: string;
  emailAddress: string;

  errorMessage: string;
  successMessage: string;
  isValid: boolean = true;

  inputtedCode: string;

  public ReferenceCodeConstants = ReferenceCodeConstants;

  isResendCodeClicked: boolean = false;

  // Resend Code Timer
  countDown: Subscription;
  counter: number = ReferenceCodeConstants.countdown;
  tick: number = ReferenceCodeConstants.tick;

  constructor(private bsModalRef: BsModalRef
              , private validationService: ValidationService
              , private router: Router
              , private navService: NavigationService
              , private timerPipe: TimerPipe
              , private authService: AuthService
              , private auditLogService: AuditLogService) { }

  ngOnInit() {
    const auth = localStorage.getItem(environment.AuthKey);
    if (JSON.parse(auth).token) { // to do expiration token if available
      const userInfo: string = atob(JSON.stringify(auth).split('.')[1]);
      this.emailAddress = JSON.parse(userInfo).email;
    }
  }

  cancel(): void {
    this.bsModalRef.hide();
  }

   // this called every time when user changed the code
  onCodeChanged(code: string): void {
    this.inputtedCode = code;
  }

  // this called only if user entered full code
  onCodeCompleted(code?: string): void {
    this.inputtedCode = code;
  }

  onContinue(): void {
    this.processTwoAuthFactor();
  }

  onResendCode(): void {
    this.isResendCodeClicked = true;
    this.counter = this.ReferenceCodeConstants.countdown;
    this.tick = this.ReferenceCodeConstants.tick;
    Utils.blockUI();
    this.validationService.getUserTwoFactorAuth(this.username).subscribe(result => {
      this.isResendCodeClicked = true;
      this.referenceNumber = result;
      this.isValid = true;
      this.successMessage = this.ReferenceCodeConstants.CodeSuccessSent;
      this.resendCodeTimer();
      Utils.unblockUI();

    }, err => {
      console.log(err);
      Utils.unblockUI();
    });
  }

  resendCodeTimer(): void {
    if (this.countDown) {
      this.countDown.unsubscribe();
    }
    this.countDown = timer(0, this.tick).subscribe(() => {
      --this.counter;
      this.counter = +this.timerPipe.transform(this.counter);
      if (+this.counter === 0) {
        this.isResendCodeClicked = false;
        this.countDown.unsubscribe();
      }
    });
  }

  processTwoAuthFactor(): void {
    if (this.inputtedCode && this.inputtedCode !== '') {
      const payload = { userName: this.username,
        referenceCode: this.inputtedCode,
        referenceNumber: this.referenceNumber,
        isActive: true
      };

      this.validationService.postUserTwoFactorAuth(payload).subscribe((result: UserTwoFactorAuthResponse) => {
      if (result.isValid) {
        this.isValid = result.isValid;
        this.errorMessage = result.error;

        this.navService.initializeMenus().subscribe(() => {
          Utils.unblockUI();
          this.bsModalRef.hide();
          this.router.navigate(['/dashboard']);
          this.authService.logUserLogin(this.username).subscribe(() => {
            return;
          });
        }, err => {
          Utils.unblockUI();
        });
      } else {
        this.logError(this.ReferenceCodeConstants.invalidCode);
        this.isValid = result.isValid;
        this.errorMessage = this.ReferenceCodeConstants.invalidCode;
      }
        },
          err => {
            this.logError(this.ReferenceCodeConstants.verificationError);
            this.isValid = err.error.isValid;
            this.errorMessage = this.ReferenceCodeConstants.invalidCode;
        });
    } else {
      this.isValid = false;
      this.errorMessage = this.ReferenceCodeConstants.CodeisEmpty;
    }
  }

  ngOnDestroy() {
    if (this.countDown) {
      this.countDown.unsubscribe();
    }
  }

  logError(description: string): void {
    const twoFactorAuthPayload = {
      userId: 0,
      keyId: this.username,
      auditType: LoginLabelsConstants.loginAuditType,
      description: description,
      method: LoginLabelsConstants.method.processAuth
    };
    this.auditLogService.insertToAuditLog(twoFactorAuthPayload).subscribe();
  }
}
