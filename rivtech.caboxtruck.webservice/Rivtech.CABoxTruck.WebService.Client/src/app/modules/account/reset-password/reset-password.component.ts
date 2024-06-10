import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ValidationErrors, AbstractControl } from '@angular/forms';
import { LayoutService } from '../../../core/services/layout/layout.service';
import { Router, ActivatedRoute } from '@angular/router';
import FormUtils from '../../../shared/utilities/form.utils';
import Utils from '../../../shared/utilities/utils';
import { AccountService } from '../../../core/services/account.service';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { ErrorMessageConstant } from '@app/shared/constants/error-message.constants';
import { PasswordFormConstants } from '@app/shared/constants/login.labels.constants';
import { CustomValidators } from '@app/shared/validators/custom.validator';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {

  public PasswordFormConstants = PasswordFormConstants;
  public ErrorMessageConstant = ErrorMessageConstant;
  resetPassForm: FormGroup;
  submitted = false;

  referenceNum: string;

  constructor(private fb: FormBuilder, private layout: LayoutService, private router: Router, private accountService: AccountService, private route: ActivatedRoute) {

    this.route.queryParams.subscribe(params => {
      this.referenceNum = params['ref'];
    });

  }

  ngOnInit() {
    this.resetPassForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8),
        CustomValidators.passwordValidator({ hasNumber: true }),
        CustomValidators.passwordValidator({ hasCapitalCase: true }),
        CustomValidators.passwordValidator({ hasSmallCase: true }),
        CustomValidators.passwordValidator({hasSpecialCharacters: true }),
      ]],
      rePassword: ['',
        [
          Validators.required,
          FormUtils.matchValues('password'),
        ],
      ],
    });
  }

  get isResetPasswordDisable() {
    return this.resetPassForm.invalid;
  }

  get form() {
    return this.resetPassForm.controls;
  }

  onSubmit() {

    this.submitted = true;

    this.form.password.markAsDirty();
    this.form.rePassword.markAsDirty();

    if (this.resetPassForm.invalid) {
      return;
    }


    Utils.blockUI();

    this.accountService.resetPassword(this.referenceNum, this.form.password.value).subscribe(res => {
      NotifUtils.showSuccess('Password has been successfully reset');
      this.router.navigate(['/login']);
    }, error => {
      NotifUtils.showError('Your request has expired.');
    });

  }

  getErrors() {
    return JSON.stringify(this.form.password.errors);
  }


}
