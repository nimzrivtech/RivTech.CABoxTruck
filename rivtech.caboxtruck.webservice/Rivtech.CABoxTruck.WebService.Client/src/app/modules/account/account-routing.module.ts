import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PathConstants } from '../../shared/constants/path.constants';
import { ChangePasswordNewUserComponent } from './change-password-new-user/change-password-new-user.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';


const routes: Routes = [
  {
    path: PathConstants.Account.ForgotPassword,
    component: ForgotPasswordComponent,
  },
  {
    path: PathConstants.Account.ResetPassword,
    component: ResetPasswordComponent,
  },
  {
    path: PathConstants.Account.ChangePassword,
    component: ChangePasswordComponent,
  },
  {
    path: PathConstants.Account.NewUserChangePassword,
    component: ChangePasswordNewUserComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
