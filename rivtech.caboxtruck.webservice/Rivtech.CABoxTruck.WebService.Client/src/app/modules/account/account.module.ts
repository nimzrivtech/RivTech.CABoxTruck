import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { AccountService } from '../../core/services/account.service';
import { SharedModule } from '../../shared/shared.module';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ChangePasswordNewUserComponent } from './change-password-new-user/change-password-new-user.component';

@NgModule({
  declarations: [ForgotPasswordComponent, ResetPasswordComponent, ChangePasswordComponent, ChangePasswordNewUserComponent],
  imports: [
    CommonModule,
    AccountRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    AccountService
  ]
})
export class AccountModule { }
