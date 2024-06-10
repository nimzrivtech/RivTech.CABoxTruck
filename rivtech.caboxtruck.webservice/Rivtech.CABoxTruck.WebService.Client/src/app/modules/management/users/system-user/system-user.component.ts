import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { BaseComponent } from '../../../../shared/base-component';
import { UserViewModel } from '../../../../shared/models/management/user-view-model';
import { UserData } from '../../data/user.data';
import { UserModalComponent } from './user-modal/user-modal.component';
import { GenericUserLabels, UserTableLabelConstants } from '../../../../shared/constants/user.label.constants';
import { GenericAddressData } from '../../../../shared/components/generic-address/data/generic-address.data';
import { TableConstants } from '../../../../shared/constants/table.constants';
import { GenericLabel } from '../../../../shared/constants/generic.labels.constants';
import NotifUtils from '@app/shared/utilities/notif-utils';
@Component({
  selector: 'app-system-user',
  templateUrl: './system-user.component.html',
  styleUrls: ['./system-user.component.scss']
})

export class SystemUserComponent extends BaseComponent implements OnInit {
  TableLabel = UserTableLabelConstants;
  modalRef: BsModalRef;
  isListHidden: boolean = false;
  TableConstants = TableConstants;

  constructor(
    private modalService: BsModalService,
    public userData: UserData,
    private genericAddressData: GenericAddressData) {
    super();
  }

  ngOnInit() {
    this.userData.initializeForms();
    this.userData.resetList();
    this.userData.getUserList();
    this.userData.getAllRoles();
    this.genericAddressData.getAddressType();
  }

  openAddUserModal(): void {
    this.clearAddressList();
    const initialState = {
      isEdit: false,
      modalTitle: GenericUserLabels.add
    };
    this.modalRef = this.showUserModal(initialState);
  }

  openEditUserModal(data: UserViewModel): void {
    this.clearAddressList();
    const userInfo: UserViewModel = {
      userId: data.userId,
      userName: data.userName,
      firstName: data.firstName,
      lastName: data.lastName,
      appCode: data.appCode,
      roleId: data.roleId,
      carrierCode: data.carrierCode,
      agencyId: data.agencyId,
      subAgencyId: data.subAgencyId,
      emailAddress: data.emailAddress,
      createdDate: new Date(data.createdDate),
      isActive: data.isActive,
      companyName: data.companyName,
      userAccessRights: data.userAccessRights,
      userCredentials: data.userCredentials,
      fullName: `${data.firstName}${' '}${data.lastName}`,
      isLocked: data.isLocked
    };

    const initialState = {
      isEdit: true,
      modalTitle: GenericUserLabels.edit,
      modalButton: GenericUserLabels.save,
      userToEdit: userInfo
    };

    this.modalRef = this.showUserModal(initialState);
  }

  showUserModal(initialState): BsModalRef {
    return this.modalService.show(UserModalComponent, {
      initialState,
      class: 'modal-lg',
      backdrop: 'static',
      keyboard: false
    });
  }

  toggleUsersList(): void {
    this.isListHidden = !this.isListHidden;
  }

  clearAddressList(): void {
    this.genericAddressData.addressTableRows = [];
    this.genericAddressData.addressList = [];
  }

  deleteUser(userItem?: any): void {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete ${userItem.fullName}?`,
      () => {
        this.userData.deleteUser(userItem.userId);
      },
      () => { }
    );
  }
}
