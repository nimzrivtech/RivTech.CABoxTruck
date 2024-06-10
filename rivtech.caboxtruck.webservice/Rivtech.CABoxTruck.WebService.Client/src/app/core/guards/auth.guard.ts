import { Injectable, isDevMode } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { PathConstants } from '../../shared/constants/path.constants';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private _authService: AuthService, private _router: Router) { }

  canActivate() {
    if (!this._authService.isLoggedIn()) {
      // Use local login in devmode
      // disable redirect to client management for now
      this._authService.setUserLoggedIn(false);
      this._router.navigate([PathConstants.Login.Index]); return false;
      return;
      if (isDevMode() || environment.envName === 'devtest') {
        this._router.navigate([PathConstants.Login.Index]); return false;
      }
      window.location.href = environment.ClientManagementUrl; return false;
    }
    return true;
  }
}
