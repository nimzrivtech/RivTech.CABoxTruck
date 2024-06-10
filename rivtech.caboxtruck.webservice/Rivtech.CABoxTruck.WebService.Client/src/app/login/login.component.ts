import { Component, isDevMode, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LayoutService } from '../core/services/layout/layout.service';
import { AuthService } from '../core/services/auth.service';
import NotifUtils from '../shared/utilities/notif-utils';
import { NavigationService } from '../core/services/navigation/navigation.service';
import Utils from '../shared/utilities/utils';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder, private layout: LayoutService, private router: Router, private authService: AuthService, private navService: NavigationService) { }

  ngOnInit() {
    //This login cannot be used in production
    // if (!isDevMode() && environment.envName !== 'devtest') {
    //   window.location.href = environment.ClientManagementUrl;
    // }

    if(localStorage.getItem('isUserLoggedIn') === 'true'){
      this.router.navigate(['/dashboard']);
    }

    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  get form() { return this.loginForm.controls; }

  onSubmit() {
    this.submitted = true;

    this.form.username.markAsDirty();
    this.form.password.markAsDirty();

    if (this.loginForm.invalid) {
      return;
    }

    Utils.blockUI();

    this.authService.login(this.form.username.value, this.form.password.value)
        .subscribe(res => {
                this.authService.logUserLogin(this.form.username.value).subscribe(() => {
                    return;
                });
        if (this.authService.isPasswordChanged === true) {
          this.navService.initializeMenus().subscribe(() => {
            Utils.unblockUI();
            this.authService.initAppSettings();
            this.router.navigate(['/dashboard']);
          }, err => {
            Utils.unblockUI();
          });
        }
      },
        err => {
          Utils.unblockUI();
          NotifUtils.showError(err.error.error);
        }
      );
  }
}
