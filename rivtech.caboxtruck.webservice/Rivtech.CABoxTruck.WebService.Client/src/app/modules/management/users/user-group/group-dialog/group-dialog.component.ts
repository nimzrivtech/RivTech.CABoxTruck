import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import NotifUtils from '../../../../../shared/utilities/notif-utils';
import { BsModalRef } from 'ngx-bootstrap';
import { GroupData } from '../../../../../modules/management/data/group.data';
import { ErrorMessageConstant } from '../../../../../shared/constants/error-message.constants';
import { GroupTableLabelConstants } from '../../../../../shared/constants/user.label.constants';
import { GroupDTO, UserGroupDTO } from '../../../../../shared/models/management/group-dto';
import * as _ from 'lodash';
import Utils from '../../../../../shared/utilities/utils';
import { GroupService } from '../../../../../core/services/management/group.service';
import { take } from 'rxjs/operators';
import { UserService } from '../../../../../core/services/management/user.service';
import { AuthService } from 'app/core/services/auth.service';

@Component({
  selector: 'app-group-dialog',
  templateUrl: './group-dialog.component.html',
  styleUrls: ['./group-dialog.component.scss']
})
export class GroupDialogComponent implements OnInit {
  GroupConstant = GroupTableLabelConstants;
  ErrorMessageConstant = ErrorMessageConstant;
  isEdit: boolean = false;
  isloading: boolean = false;
  currentDate = this.authService.getCustomDate();

  editItem: any;
  modalTitle = '';

  constructor(
    public groupData: GroupData,
    public modalRef: BsModalRef,
    private groupService: GroupService,
    private userService: UserService,
    private authService: AuthService
    ) { }

  ngOnInit() {
    this.groupForm.reset();
    this.userGroupForm.reset();
    if (this.isEdit) {
      console.log('edit', this.editItem);
      this.populateForm();
      this.populateUserGroupList();
    }
  }

  get groupForm(): FormGroup {
    return this.groupData.groupForm;
  }

  get userGroupForm(): FormGroup {
    return this.groupData.userGroupForm;
  }

  changeStatus() {
    const status = this.groupData.groupForm.get('active').value;
    if (this.isEdit && !status) {
      NotifUtils.showConfirmMessage(`Do you want to change the status of ${this.groupData.groupForm.get('name').value} from Active into In-Active?`,
        () => {
          this.groupData.groupForm.get('active').setValue(false);
        }, () => {
          this.groupData.groupForm.get('active').setValue(true);
        });
    }
  }

  changeStatusItem(item) {
    const status = item.isActive;
    if (this.isEdit && !status) {
      NotifUtils.showConfirmMessage(`Do you want to change the status of ${item.userName} from Active into In-Active?`,
        () => {
          item.isActive = false;
        }, () => {
          item.isActive = true;
        });
    }
  }

  changeStatusInternal() {
    const status = this.groupData.groupForm.get('internal').value;
    const nextStatus = status ? 'Internal' : 'External';
    const currentStatus = status ? 'External' : 'Internal';
    if (this.isEdit) {
      NotifUtils.showConfirmMessage(`Do you want to change the status of this group from ${currentStatus} to ${nextStatus}?`,
        () => {
          this.groupData.groupForm.get('internal').setValue(status);
          this.groupData.userGroupPageItems = [];
          this.groupData.userGroupList = [];
        }, () => {
          this.groupData.groupForm.get('internal').setValue(!status);
        });
    }
  }

  closeGroup(): void {
    this.modalRef.hide();
  }

  populateForm() {
    this.groupForm.controls['name'].patchValue(this.editItem.name);
    this.groupForm.controls['description'].patchValue(this.editItem.description);
    this.groupForm.controls['active'].patchValue(this.editItem.removedProcessDate ? false : true);
    this.groupForm.controls['internal'].patchValue(this.editItem.isInternalGroup);
  }

  populateUserGroupList() {
    this.groupData.userGroupList = [];
    _.map(this.editItem.userGroups, (item: any) => {
      if (item.removedProcessDate == null) {
        const data: any = {
          id: item.id,
          groupId: item.groupId,
          userName: item.userName,
          isActive: item.removedProcessDate ? false : true
        };
        this.groupData.userGroupList.push(data);
      }
    });
    this.groupData.setUserGroupPage(1);
  }

  saveGroup() {
    const group: GroupDTO = {
      id: this.isEdit ? this.editItem.id : null,
      name: this.groupForm.get('name').value,
      description: this.groupForm.get('description').value,
      programId: 1, // temp centauri programid
      removedProcessDate: this.groupForm.get('active').value ? null : this.currentDate,
      isInternalGroup: this.groupForm.get('internal').value? true : false,
      userGroups: null
    };

    if (this.isEdit && this.groupData.userGroupList.length > 0) {
      const userGroupList: UserGroupDTO[] = [];
      _.map(this.groupData.userGroupList, (item: any) => {
        const data: UserGroupDTO = {
          id: null,
          groupId: item.groupId,
          userName: item.userName,
          removedProcessDate: item.isActive ? null : this.currentDate
        };
        userGroupList.push(data);
      });
      group.userGroups = userGroupList;
    }
    Utils.blockUI();
    this.groupService.saveGroup(group).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      this.groupData.getGroupList();
      this.closeGroup();
    }, (error) => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(error));
    });
  }

  addUsername() {
    const username = this.userGroupForm.get('userName').value;
    const internal = this.groupForm.get('internal').value;
    if (username === '' || username == null) {
      return;
    }
    Utils.blockUI();
    this.userService.getUserInfo(username).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      if (data == null) {
        NotifUtils.showError(ErrorMessageConstant.usernameExistErrorMessage);
        return;
      }

      if (data.isInternal !== internal) {
        NotifUtils.showError(ErrorMessageConstant.userInternalErrorMessage.replace('{0}', internal ? 'Internal' : 'External'));
        return;
      }

      if (this.groupData.userGroupList.some(x => x.userName === username)) {
        NotifUtils.showError(ErrorMessageConstant.usernameExistListErrorMessage);
        this.userGroupForm.get('userName').patchValue(null);
        return;
      }

      this.groupData.userGroupList.push({
        groupId: this.editItem.id,
        userName: data.userName,
        isActive: true
      });
      this.groupData.setUserGroupPage(1);
      this.userGroupForm.get('userName').patchValue(null);
    }, (error) => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(error));
    });
  }

  setPage(page: number): void {
    this.groupData.setUserGroupPage(page);
  }
}
