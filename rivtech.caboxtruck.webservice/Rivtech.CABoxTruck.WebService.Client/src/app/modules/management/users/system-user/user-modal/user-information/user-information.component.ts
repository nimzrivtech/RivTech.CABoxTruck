import { Component, OnInit } from '@angular/core';
import { ErrorMessageConstant } from '../../../../../../shared/constants/error-message.constants';
import { UsersLabelConstants } from '../../../../../../shared/constants/user.label.constants';
import { ChangeDetectorRef } from '@angular/core';
import { UserData } from '../../../../../../modules/management/data/user.data';
import { AuthService } from 'app/core/services/auth.service';
import { UserRole } from '@app/shared/constants/user-roles.constants';
import { environment } from '../../../../../../../environments/environment';
import NotifUtils from '@app/shared/utilities/notif-utils';

@Component({
  selector: 'app-user-information',
  templateUrl: './user-information.component.html',
  styleUrls: ['./user-information.component.scss']
})
export class UserInformationComponent implements OnInit {
  UsersLabelConstants = UsersLabelConstants;
  ErrorMessageConstant = ErrorMessageConstant;
  isOpen: boolean = false;
  constructor(public userData: UserData, private detect: ChangeDetectorRef, private authService: AuthService) { }

  ngOnInit() {
  }

  collapse() {
    this.isOpen = !this.isOpen;
  }

  calculateAge(date) {
    const dateOfBirth = date?.jsDate;

    if (!!!dateOfBirth) {
      this.userData.UserInfoFormGroup.get('age').setValue('0');
      return 0;
    }

    const today = this.authService.getCustomDate();
    const birthDate = new Date(dateOfBirth);
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();

    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    this.userData.UserInfoFormGroup.get('age').setValue(age);
    this.detect.detectChanges();
  }

  unlockSwitch() {
    const field = this.userData.UserInfoFormGroup.get('isLocked');
    const username = this.userData.UserInfoFormGroup.get('userName').value;
    if (field.value === false) {
      NotifUtils.showConfirmMessage(`Are you sure you want to unlock this account?`,
        () => {
          this.userData.unlockAccount(username, () => {
            this.userData.UserInfoFormGroup.get('isLocked').disable();
          });
        },
        () => {
          this.userData.UserInfoFormGroup.get('isLocked').setValue(true);
         }
      );
    }
  }
}
