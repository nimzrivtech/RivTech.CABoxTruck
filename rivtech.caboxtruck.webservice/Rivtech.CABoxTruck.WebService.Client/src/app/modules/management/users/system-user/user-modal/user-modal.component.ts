import { Component, EventEmitter, OnInit } from '@angular/core';
import { UserData } from '../../../../../modules/management/data/user.data';
import { BsModalRef } from 'ngx-bootstrap';
import { UserViewModel } from '../../../../../shared/models/management/user-view-model';
import { FormGroup } from '@angular/forms';
import { GenericUserLabels, UsersLabelConstants } from '../../../../../shared/constants/user.label.constants';
import NotifUtils from '../../../../../shared/utilities/notif-utils';
import { PasswordFormConstants } from '../../../../../shared/constants/login.labels.constants';
import Utils from '../../../../../shared/utilities/utils';
import { environment } from '../../../../../../environments/environment';
import { AuthService } from '../../../../../core/services/auth.service';
import { AccountService } from '../../../../../core/services/account.service';
import { UserNameValidator } from 'app/modules/management/data/validators/usernameValidator';

@Component({
  selector: 'app-user-modal',
  templateUrl: './user-modal.component.html',
  styleUrls: ['./user-modal.component.scss']
})
export class UserModalComponent implements OnInit {
  isEdit: boolean = false;
  userToEdit: UserViewModel;
  genericUserLabels = GenericUserLabels;
  usersLabelConstants = UsersLabelConstants;
  modalTitle = '';
  modalButton = 'Add User';

  constructor(public modalRef: BsModalRef,
    private userData: UserData,
    public authService: AuthService,
    public accountService: AccountService,
    private userNameValidator: UserNameValidator
  ) { }
  public event: EventEmitter<any> = new EventEmitter<any>();

  ngOnInit() {
    this.userData.isSystemUserSuccess = false;
    this.userData.UserInfoFormGroup.reset();
    this.userData.deleteAddresses = [];
    this.userData.UserInfoFormGroup.get('isLocked').disable();
    if (this.isEdit) {
      this.userData.UserInfoFormGroup.get('userName').clearAsyncValidators();
      this.userData.UserInfoFormGroup.get('userName').updateValueAndValidity();
      this.userData.getUserEntityInfo(this.userToEdit.userName);
      this.userData.populateUserInformation(this.userToEdit);
      this.userData.setUserId(this.userToEdit.userName);
      this.userData.UserInfoFormGroup.get('userName').setAsyncValidators(this.userNameValidator.usernameValidator());
    } else {
      this.userData.isAccountLocked = false;
      this.userData.UserInfoFormGroup.get('userName').clearAsyncValidators();
      this.userData.UserInfoFormGroup.get('userName').updateValueAndValidity();
      this.userData.UserInfoFormGroup.get('userName').setAsyncValidators(this.userNameValidator.usernameValidator());
      this.userData.UserInfoFormGroup.get('userName').enable();
      this.userData.UserInfoFormGroup.get('isActive').setValue(false);
    }
  }

  get userInformation(): FormGroup {
    return this.userData.UserInfoFormGroup;
  }

  get isSavingSuccess(): boolean {
    return this.userData.isSystemUserSuccess;
  }

  closeModal(): void {
    this.modalRef.hide();
  }

  saveUser(): void {
    this.userData.UserInfoFormGroup.markAllAsTouched();
    Utils.blockUI();
    if (this.isEdit) {
      this.userData.putUserInformation();
    } else {
      this.userData.postUserInformation();
    }

    this.userData.isExisting.subscribe( value => {
      if (!value) {
        this.closeModal();
      }
    });
  }

  resetUserPassword(): void {
    NotifUtils.showConfirmMessage(PasswordFormConstants.resetPasswordConfirmation,
      () => {
        Utils.blockUI();
        const emailAddress = this.userData.UserInfoFormGroup.get('workEmailAddress').value;
        const payload = {
          appCode: environment.ApplicationId,
          userName: this.userData.UserInfoFormGroup.get('userName').value
        };

        this.accountService.userResetPassword(payload).subscribe(() => {
          Utils.unblockUI();
          this.authService.savePasswordAuditLog(PasswordFormConstants.auditLog.action.successReset, payload.userName);
          NotifUtils.showSuccessWithConfirmationFromUser(PasswordFormConstants.resetPasswordRequestSuccess.replace('{0}', emailAddress));
        }, error => {
          Utils.unblockUI();
          NotifUtils.showError(PasswordFormConstants.resetPasswordError.replace('{0}', emailAddress));
        });
      },
      () => { }
    );
  }

}
