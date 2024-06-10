import { Component, EventEmitter, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import * as _ from 'lodash';
import { UserTypeLabelConstants } from '../../../../../../../shared/constants/user.label.constants';
import { ErrorMessageConstant } from '../../../../../../../shared/constants/error-message.constants';
import { GenericLabel } from '../../../../../../../shared/constants/generic.labels.constants';
import { BaseClass } from '../../../../../../../shared/base-class';
import { UserData } from '../../../../../data/user.data';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { Role } from '../../../../../../../shared/models/management/role';
import { UserType } from '../../../../../../../shared/models/management/user-type';
import { take } from 'rxjs/operators';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-user-type-modal',
  templateUrl: './user-type-modal.component.html',
  styleUrls: ['./user-type-modal.component.scss']
})
export class UserTypeModalComponent extends BaseClass implements OnInit {
  UsersTypeConstants = UserTypeLabelConstants;
  ErrorMessageConstant = ErrorMessageConstant;
  Generic = GenericLabel;
  isEdit: boolean = false;
  modalTitle: string;
  hasLoaded: boolean = false;
  selectedCategory: number = null;
  selectedProgram: any[] = [];
  checked: any[] = [];
  event: EventEmitter<any> = new EventEmitter<any>();
  userTypeModel: UserType;

  constructor(public modalRef: BsModalRef,
    public userData: UserData) {
    super();
  }

  ngOnInit() {
    this.userData.initializeForms();
    if (this.isEdit) {
      this.initializeEditForm();
    }
  }

  private initializeEditForm(): void {
    this.userType.get('accessCategory').setValue(this.userTypeModel.roleGroupId);
    this.userType.get('typeName').setValue(this.userTypeModel.roleName);
    this.userType.get('isInternal').setValue(this.userTypeModel.isInternal);
    this.userType.get('isActive').setValue(this.userTypeModel.isActive);
  }

  get userType(): FormGroup {
    return this.userData.userTypeFormGroup;
  }

  closeUserType(): void {
    this.modalRef.hide();
  }

  saveUserType(): void {
    const rolesList = this.userData.userCategories.find(value => value.roleGroupId === Number(this.userType.get('accessCategory').value)).roles;
    const role = rolesList.find(value => value.roleName === this.userType.get('typeName').value);
    const request: Role = {
      roleId: this.isEdit ? this.userTypeModel.roleId : role?.roleId ?? 0,
      roleName: this.userType.get('typeName').value,
      programIds: [1],
      roleGroupId: Number(this.userType.get('accessCategory').value),
      isActive: this.userType.get('isActive').value,
      isInternal: this.userType.get('isInternal').value
    };
    this.checkTypeNameUniqueness(request);
    this.userData.checkTypeNameUniqueness(request.roleName, 1);
  }

  private checkTypeNameUniqueness(req: Role): void {
    this.userData.isTypeNameNotFound.pipe(take(1)).subscribe(isNotFound => {
      if (isNotFound) {
        this.event.emit({ isFound: false, request: req });
      } else {
        if (!this.isEdit || (this.isEdit && req.roleName !== this.userTypeModel.roleName)){
          this.event.emit({ isFound: true, request: null });
        } else {
          this.event.emit({ isFound: false, request: req });
        }
      }
    });
  }

  changeStatus(): void {
    const status = this.userType.get('isActive').value;
    if (this.isEdit && !status) {
      NotifUtils.showConfirmMessage(`Do you want to change the status of ${this.userType.get('typeName').value} from Active into In-Active?`,
        () => {
          this.userType.get('isActive').setValue(false);
        }, () => {
          this.userType.get('isActive').setValue(true);
        });
    }
  }
}
