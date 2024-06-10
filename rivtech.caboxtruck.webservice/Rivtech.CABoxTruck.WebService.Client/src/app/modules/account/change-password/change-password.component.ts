import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../../../shared/validators/custom.validator';
import FormUtils from '../../../shared/utilities/form.utils';
import { PasswordFormConstants } from '../../../shared/constants/login.labels.constants';
import { ErrorMessageConstant } from '../../../shared/constants/error-message.constants';
import { AccountService } from '../../../core/services/account.service';
import { AuthService } from '../../../core/services/auth.service';
import { environment } from '../../../../environments/environment';
import Utils from '../../../shared/utilities/utils';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { Router } from '@angular/router';
import { catchError, debounceTime } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit, OnDestroy {
  public PasswordFormConstants = PasswordFormConstants;
  public ErrorMessageConstant = ErrorMessageConstant;
  changePasswordForm: FormGroup;
  subscription1: Subscription;
  subscription2: Subscription;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
    this.changePasswordForm = this.fb.group({
      currentPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.minLength(8),
        CustomValidators.passwordValidator({ hasNumber: true }),
        CustomValidators.passwordValidator({ hasCapitalCase: true }),
        CustomValidators.passwordValidator({ hasSmallCase: true }),
        CustomValidators.passwordValidator({ hasSpecialCharacters: true }),
      ]],
      confirmPassword: ['', [Validators.required, FormUtils.matchValues('newPassword')]]
    });

    this.initPasswordValidator();
  }

  ngOnDestroy() {
    console.log('unsubscribe');
    this.subscription1.unsubscribe();
    this.subscription2.unsubscribe();
  }

  initPasswordValidator() {
    this.subscription1 = this.changePasswordForm.get('currentPassword')
        .valueChanges
        .pipe(debounceTime(500))
        .subscribe(dataValue => {
          this.authService.validateAuthFromServer(dataValue).subscribe(res => {
            this.changePasswordForm.get('currentPassword').setErrors(null);
            this.changePasswordForm.get('currentPassword').updateValueAndValidity();
          },
          err => {
            this.changePasswordForm.get('currentPassword').setErrors({'incorrectPassword': true});
          });
    });

    this.subscription2 = this.changePasswordForm.get('newPassword')
        .valueChanges
        .pipe(debounceTime(500))
        .subscribe(dataValue => {
          if (this.changePasswordForm.get('currentPassword').value === dataValue) {
            this.changePasswordForm.get('newPassword').setErrors({'incorrectNewPassword': true});
          } else {
            this.changePasswordForm.get('newPassword').setErrors(null);
            this.changePasswordForm.get('newPassword').updateValueAndValidity();
          }
    });
  }

  get form() {
    return this.changePasswordForm.controls;
  }

  get isChangePasswordDisable() {
    return this.changePasswordForm.invalid;
  }

  onSubmit(): void {
    this.form.currentPassword.markAsDirty();
    this.form.newPassword.markAsDirty();
    this.form.confirmPassword.markAsDirty();

    if (this.changePasswordForm.invalid) {
      return;
    }

    Utils.blockUI();

    const payload = {
      appCode: environment.ApplicationId,
      userName: this.authService.getUserName(),
      password: this.form.currentPassword.value,
      newPassword: this.form.confirmPassword.value
    };

    this.accountService.changePassword(payload).subscribe(res => {
      NotifUtils.showSuccessWithConfirmationFromUser(PasswordFormConstants.changeSuccessMessage, () => {
        this.router.navigate(['/dashboard']);
      });
    }, error => {
      Utils.unblockUI();
      NotifUtils.showError('Password was incorrect');
    });

  }

  onCancel(): void {
    this.router.navigate(['/dashboard']);
  }

}
function observableThrowError(error: any): any {
  throw new Error('Function not implemented.');
}

