import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { debounceTime, take } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { AccountService } from '../../../core/services/account.service';
import { AuthService } from '../../../core/services/auth.service';
import { ErrorMessageConstant } from '../../../shared/constants/error-message.constants';
import { PasswordFormConstants } from '../../../shared/constants/login.labels.constants';
import FormUtils from '../../../shared/utilities/form.utils';
import NotifUtils from '../../../shared/utilities/notif-utils';
import Utils from '../../../shared/utilities/utils';
import { CustomValidators } from '../../../shared/validators/custom.validator';

@Component({
  selector: 'app-change-password-new-user',
  templateUrl: './change-password-new-user.component.html',
  styleUrls: ['./change-password-new-user.component.scss']
})
export class ChangePasswordNewUserComponent implements OnInit, OnDestroy {

  public PasswordFormConstants = PasswordFormConstants;
  public ErrorMessageConstant = ErrorMessageConstant;
  changePasswordForm: FormGroup;
  subscription1: Subscription;
  subscription2: Subscription;

  constructor( private fb: FormBuilder,
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
    const auth = this.authService.getAuth();
    const token = auth ? auth.token : null;

    this.accountService.changePassword(payload).subscribe(res => {
      // this.authService.savePasswordAuditLog(PasswordFormConstants.auditLog.action.successChange, payload.userName);
      NotifUtils.showSuccessWithConfirmationFromUser(PasswordFormConstants.newUserChangePassword, () => {
        // this.authService.saveUserInformation(token);
        this.authService.logout();
        this.router.navigate(['/login']);
      });
    }, error => {
      Utils.unblockUI();
      NotifUtils.showError('Password was incorrect');
    });

  }

  onCancel(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}
