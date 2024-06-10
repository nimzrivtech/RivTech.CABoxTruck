import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LayoutService } from '../../../core/services/layout/layout.service';
import { Router } from '@angular/router';
import { AccountService } from '../../../core/services/account.service';
import NotifUtils from '../../../shared/utilities/notif-utils';
import Utils from '../../../shared/utilities/utils';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {

  forgotPassForm: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder, private layout: LayoutService, private router: Router, private accountService: AccountService) { }

  ngOnInit() {
    // this.layout.submissionSidebarVisible = false;

    this.forgotPassForm = this.fb.group({
      email: ['', [Validators.required, Validators.pattern(
        '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,5}$'
      )]]
    });
  }

  get form() { return this.forgotPassForm.controls; }

  get isForgotPasswordDisable() {
    return this.forgotPassForm.invalid;
  }

  onSubmit() {

    this.submitted = true;

    this.form.email.markAsDirty();

    if (this.forgotPassForm.invalid) {
      return;
    }

    Utils.blockUI();

    this.accountService.sendForgotPasswordEmail(this.form.email.value).subscribe(res => {
      this.showConfirmationMessage();
    }, error => {
      if (error.status === 404) {
        this.showConfirmationMessage();
      }
    });

  }

  showConfirmationMessage() {
    NotifUtils.showSuccess('Password Reset link has been sent to your Email Account, please check your email');
    this.router.navigate(['/login']);
  }

}
