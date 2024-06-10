import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { takeUntil } from 'rxjs/operators';
import { BaseComponent } from '../../../../shared/base-component';
import { GenericLabel } from '../../../../shared/constants/generic.labels.constants';
import { TableNameConstants } from '../../../../shared/constants/table.name.constants';
import { UserTypeLabelConstants } from '../../../../shared/constants/user.label.constants';
import { Role } from '../../../../shared/models/management/role';
import NotifUtils from '../../../../shared/utilities/notif-utils';
import { UserData } from '../../data/user.data';
import { UserTypeModalComponent } from './user-type-modal/user-type-modals/add-edit-modal/user-type-modal.component';

@Component({
  selector: 'app-user-type',
  templateUrl: './user-type.component.html',
  styleUrls: ['./user-type.component.scss']
})
export class UserTypeComponent extends BaseComponent implements OnInit {
  TableNameConstants = TableNameConstants;
  isListHidden: boolean = false;
  modalRef: BsModalRef;
  editedRowId: number = null;

  constructor(private modalService: BsModalService,
    public userData: UserData) {
    super();
  }

  ngOnInit() {
    this.userData.initUserTypeSection();
   }

  toggleUsersList(): void {
    this.isListHidden = !this.isListHidden;
  }

  addUserType(): void {
    const initialState = {};
    this.modalRef = this.showUserTypeModal(UserTypeModalComponent, false, initialState);
    this.modalRef.content.event.pipe(takeUntil(this.stop$)).subscribe((res: { isFound: boolean; request: Role }) => {
      if (!res.isFound) {
        NotifUtils.showConfirmMessage(GenericLabel.confirmMessage,
          () => {
            this.modalRef.hide();
            this.userData.addNewUserType(res.request);
          });
      } else {
        NotifUtils.showError(GenericLabel.userTypeNameFound);
      }
    });
  }

  editUserType(rowId: any, tableRow: string): void {
    this.userData[tableRow].forEach((item) => {
      if (item.id === rowId) {
        this.editedRowId = rowId;
        const initialState = {
          userTypeModel: this.userData.userTypeList[rowId - 1]
        };
        this.modalRef = this.showUserTypeModal(UserTypeModalComponent, true, initialState);
        this.modalRef.content.event.pipe(takeUntil(this.stop$)).subscribe((res: { isFound: boolean; request: Role }) => {
          if (res.request) {
            NotifUtils.showConfirmMessage(GenericLabel.confirmMessage,
              () => {
                this.modalRef.hide();
                this.updateUserTypeTable(res.request);
              });
          } else {
            NotifUtils.showError(GenericLabel.userTypeNameFound);
          }
        });
      }
    });
  }

  deleteUserType(rowId: any, tableRow: string): void {
    this.userData[tableRow].forEach((item) => {
      if (item.id === rowId) {
        this.editedRowId = rowId;
        const deleteUserType = this.userData.userTypeList[rowId - 1];
        this.userData.deleteUserType(deleteUserType.roleId);
      }
    });
  }

  private showUserTypeModal(component: any, isEdit: boolean, initialState: any): BsModalRef {
    initialState.modalTitle = isEdit ? UserTypeLabelConstants.editUserTypeModal : UserTypeLabelConstants.addUserTypeModal;
    initialState.isEdit = isEdit;

    return this.modalService.show(component, {
      initialState,
      backdrop: 'static',
      ignoreBackdropClick: true,
      keyboard: false
    });
  }

  private updateUserTypeTable(request): void {
    const payload: Role = {
      roleId: request.roleId,
      roleName: request.roleName,
      programIds: request.programIds,
      roleGroupId: request.roleGroupId,
      isActive: request.isActive,
      isInternal: request.isInternal
    };
    this.userData.editUserType(payload.roleId, payload);
  }
}
