import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../shared/base-component';
import { GroupTableLabelConstants } from '../../../../shared/constants/user.label.constants';
import NotifUtils from '../../../../shared/utilities/notif-utils';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { takeUntil } from 'rxjs/operators';
import { TableNameConstants } from '../../../../shared/constants/table.name.constants';
import { GroupData } from '../../data/group.data';
import { GroupDialogComponent } from './group-dialog/group-dialog.component';

@Component({
  selector: 'app-user-group',
  templateUrl: './user-group.component.html',
  styleUrls: ['./user-group.component.scss']
})
export class UserGroupComponent extends BaseComponent implements OnInit {
  TableNameConstants = TableNameConstants;
  isListHidden: boolean = false;
  modalRef: BsModalRef;
  editedRowId: number = null;
  messageIfNoData: string = 'No Data To Display';

  constructor(
    public groupData: GroupData,
    private modalService: BsModalService) {
      super();
     }

  ngOnInit() {
    this.groupData.initializeForms();
    this.groupData.getGroupList();
  }

  toggleList(): void {
    this.isListHidden = !this.isListHidden;
  }

  addGroup(): void {
    const initialState = {};
    this.modalRef = this.showGroupModal(GroupDialogComponent, false, initialState);
  }

  editGroup(item) {
    const initialState = { editItem: item };
    this.modalRef = this.showGroupModal(GroupDialogComponent, true, initialState);
  }

  // editUserType(rowId: any, tableRow: string): void {
  //   this.groupData[tableRow].forEach((item) => {
  //     if (item.id === rowId) {
  //       this.editedRowId = rowId;
  //       const initialState = {
  //         editItem: this.groupData.groupList[rowId - 1]
  //       };
  //       this.modalRef = this.showGroupModal(GroupDialogComponent, true, initialState);
  //     }
  //   });
  // }

  // deleteUserType(rowId: any, tableRow: string): void {
  //   this.userData[tableRow].forEach((item) => {
  //     if (item.id === rowId) {
  //       this.editedRowId = rowId;
  //       const deleteUserType = this.userData.userTypeList[rowId - 1];
  //       this.userData.deleteUserType(deleteUserType.roleId);
  //     }
  //   });
  // }

  setPage(page: number): void {
    this.groupData.setGroupPage(page);
  }

  private showGroupModal(component: any, isEdit: boolean, initialState: any): BsModalRef {
    initialState.modalTitle = isEdit ? GroupTableLabelConstants.editGroupModal : GroupTableLabelConstants.addGroupModal;
    initialState.isEdit = isEdit;

    return this.modalService.show(component, {
      initialState,
      backdrop: 'static',
      ignoreBackdropClick: true,
      keyboard: false
    });
  }
}
