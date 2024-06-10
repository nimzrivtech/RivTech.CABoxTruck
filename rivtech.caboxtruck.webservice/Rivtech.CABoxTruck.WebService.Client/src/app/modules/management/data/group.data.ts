import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { GroupService } from '../../../core/services/management/group.service';
import { take } from 'rxjs/operators';
import Utils from '../../../shared/utilities/utils';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { ITableFormControl, ITableTd, ITableTh, ITableTr } from '../../../shared/models/dynamic/table.interface';
import { GroupDTO } from '../../../shared/models/management/group-dto';
import * as _ from 'lodash';
import { TableFormcontrols } from '../../../shared/constants/user.options.constants';
import { GroupTableLabelConstants } from '../../../shared/constants/user.label.constants';
//import { PagerService } from '../../../core/services/pagerService'; // Error in ng build --prod in PR
import { SearchDTO } from '../../../shared/models/management/search.dto';
import { environment } from '../../../../environments/environment';

export interface ApplicantForms {
  groupForm?: FormGroup;
}

@Injectable({
  providedIn: 'root'
})
export class GroupData {
  groupForm: FormGroup;
  userGroupForm: FormGroup;

  groupList: any[] = [];
  userGroupList: any[] = [];
  groupPageItems: any[] = [];
  userGroupPageItems: any[] = [];
  currentGroupPageNumber: number = 1;
  currentUserGroupPageNumber: number = 1;
  groupTableRows: ITableTr[] = [];
  userGroupTableRows: ITableTr[] = [];
  tableFormControls: ITableFormControl[] = TableFormcontrols;

  groupTableHeaders: ITableTh[] = [
    { value: GroupTableLabelConstants.name },
    { value: GroupTableLabelConstants.description },
    { value: GroupTableLabelConstants.status }
  ];

  groupPager: any = {};
  userGroupPager: any = {};

  constructor(private fb: FormBuilder,
    private router: Router,
    private groupService: GroupService,
    //private pagerService: PagerService
  ) {
  }

  initializeForms() {
    this.groupList = [];
    this.groupForm = this.fb.group({
      name: [null, Validators.required],
      description: [null, Validators.required],
      active: [false],
      internal: [false],
    });

    this.userGroupForm = this.fb.group({
      userName: [null, Validators.required],
    });
  }

  setGroupPage(page: number): void {
    /* if (page < 1) {
      return;
    }
    this.groupPager = this.pagerService.getPager(this.groupList.length, page);
    this.groupPageItems = this.groupList.slice(this.groupPager.startIndex, this.groupPager.endIndex + 1);
    this.currentGroupPageNumber = page; */
  }

  setUserGroupPage(page: number): void {
    /* if (page < 1) {
      return;
    }
    this.userGroupPager = this.pagerService.getPager(this.userGroupList.length, page);
    this.userGroupPageItems = this.userGroupList.slice(this.userGroupPager.startIndex, this.userGroupPager.endIndex + 1);
    this.currentUserGroupPageNumber = page; */
  }

  getGroupList() {
    Utils.blockUI();
    this.groupService.getUserGroups(1).pipe(take(1)).subscribe((data: GroupDTO[]) => { //environment.ApplicationId
      Utils.unblockUI();
      this.groupList = [];
      // this.groupTableRows = [];
      if (data.length > 0) {
        this.populateGroupsData(data);
      }
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  private populateGroupsData(result: GroupDTO[]): void {
    _.map(result, (item: GroupDTO) => {
      const types: GroupDTO = {
        id: item.id,
        name: item.name,
        description: item.description,
        programId: item.programId,
        removedProcessDate: item.removedProcessDate,
        isInternalGroup: item.isInternalGroup,
        userGroups: item.userGroups
      };
      this.groupList.push(types);
      // this.addUserTypeTableItem(types);
    });
    this.setGroupPage(1);
  }

  // public addUserTypeTableItem(newItem: any, isAdd: boolean = false): void {
  //   const tr: ITableTd[] = [];
  //   const fields: string[] = [
  //     'name',
  //     'description',
  //     'status'
  //   ];

  //   fields.forEach((item, index) => {
  //     let display: any;

  //     switch (item) {
  //       case 'status':
  //         display = Boolean(newItem['removedProcessDate']) ? 'In-Active': 'Active';
  //         break;
  //       default:
  //         display = Boolean(newItem[item]) ? String(newItem[item]) : '';
  //         break;
  //     }

  //     tr.push({
  //       id: index + 1,
  //       value: Boolean(newItem[item]) ? newItem[item] : '',
  //       display: display,
  //     });
  //   });

  //   this.groupTableRows.push({
  //     id: this.groupTableRows.length > 0 ? this.groupTableRows.length + 1 : 1,
  //     tr: tr
  //   });
  // }

  resetUserGroupList(): void {
    this.groupList = [];
    this.groupPageItems = [];
  }

  searchUserGroup(input: string): void {
    const payload: SearchDTO = {
      term: input,
      appCode: environment.ApplicationId
    };
    Utils.blockUI();
    this.resetUserGroupList();
    this.groupService.searchUserGroup(payload).subscribe((result: GroupDTO[]) => {
      this.groupList = [];
      if (result.length > 0) {
        this.populateGroupsData(result);
      }
      Utils.unblockUI();
    }, error => {
      Utils.unblockUI();
      NotifUtils.showError(error.message);
    });
  }
}
