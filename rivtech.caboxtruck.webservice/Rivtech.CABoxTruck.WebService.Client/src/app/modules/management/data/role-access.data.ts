import { FormBuilder } from '@angular/forms';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { ITableFormControl, ITableTd, ITableTh, ITableTr } from '../../../shared/models/dynamic/table.interface';
import * as _ from 'lodash';
import { TableFormcontrols } from '../../../shared/constants/user.options.constants';
//import { PagerService } from '../../../core/services/pagerService';
import { RoleTableLabelConstants } from '../../../shared/constants/user.label.constants';
import { UserService } from '../../../core/services/management/user.service';
import { environment } from '../../../../environments/environment';
import { BaseClass } from '../../../shared/base-class';
import { UserViewModel } from '../../../shared/models/management/user-view-model';
import { RoleGroupService } from '../../../core/services/management/role-group.service';
import { TreeviewItem } from 'ngx-treeview';
import JsUtils from '../../../shared/utilities/js.utils';
import { Menu } from '../../../shared/models/management/menu';
import { UserMenuSection } from '../../../shared/models/userMenuSection';
import Utils from '../../../shared/utilities/utils';
import { ToastrService } from 'ngx-toastr';
import { GenericLabel } from '../../../shared/constants/generic.labels.constants';


@Injectable({
  providedIn: 'root'
})
export class RoleAccessData extends BaseClass {
  userRoleList: any[] = [];
  userRolePageItems: any[] = [];
  userRolepPageNumber: number = 1;
  userRoleTableRows: ITableTr[] = [];
  tableFormControls: ITableFormControl[] = TableFormcontrols;

  roleGroupList: any[];
  treeviewItems: TreeviewItem[];

  roleGroupMenuList: any[];
  menuCategoryList: any[];
  menuSectionList: any[];
  displayedMenuSectionList: any[];
  userMenuSectionList: any[];

  public selectedItems: number[];

  userRoleTableHeaders: ITableTh[] = [
    { value: RoleTableLabelConstants.userName },
    { value: RoleTableLabelConstants.name },
    { value: RoleTableLabelConstants.email },
    { value: RoleTableLabelConstants.status },
  ];

  userRolePager: any = {};

  constructor(private fb: FormBuilder,
    private router: Router,
    private roleGroupService: RoleGroupService,
    //private pagerService: PagerService,
    private userService: UserService,
    private toastr: ToastrService
  ) {
    super();
  }

  initializeForms() {
    this.userRoleList = [];
  }

  setUserRolePage(page: number): void {
    /* if (page < 1) {
      return;
    }
    this.userRolePager = this.pagerService.getPager(this.userRoleList.length, page);
    this.userRolePageItems = this.userRoleList.slice(this.userRolePager.startIndex, this.userRolePager.endIndex + 1);
    this.userRolepPageNumber = page; */
  }

  saveUserMenuSections(username) {
    const data = [];
    _.map(this.displayedMenuSectionList, (item) => {
      data.push(new UserMenuSection({
        programId: environment.ApplicationId,
        userName: username,
        menuId: item.menuId,
        menuSectionId: item.menuSectionId,
        isActive: this.selectedItems.indexOf(item.menuSectionId) > -1
      }));
    });

    Utils.blockUI();
    this.roleGroupService.postUserMenuSection(data).pipe(takeUntil(this.stop$)).subscribe(() => {
      Utils.unblockUI();
      this.toastr.success(`User Menu Sections Saved Successfully`, `${GenericLabel.success}`);
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  getRoleGroupList() {
    this.roleGroupList = [];
    Utils.blockUI();
    this.roleGroupService.getRoleGroups().pipe(takeUntil(this.stop$)).subscribe(data => {
      Utils.unblockUI();
      if (data.length > 0) {
        this.roleGroupList = data;
      }
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  getUserRoleList(): void {
    Utils.blockUI();
    this.userService.getUsers(environment.ApplicationId).pipe(takeUntil(this.stop$)).subscribe((data: UserViewModel[]) => {
      Utils.unblockUI();
      if (data.length > 0) {
        this.populateUsersData(data);
      }
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  getMenuCategoryList() {
    Utils.blockUI();
    this.roleGroupService.getMenuCategoryProgramId(environment.ApplicationId).pipe(takeUntil(this.stop$)).subscribe(data => {
      Utils.unblockUI();
      if (data.length > 0) {
        this.menuCategoryList = data;
      }
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  getMenuSectionList() {
    Utils.blockUI();
    this.roleGroupService.getMenuSectionProgramId(environment.ApplicationId).pipe(takeUntil(this.stop$)).subscribe(data => {
      Utils.unblockUI();
      if (data.length > 0) {
        this.menuSectionList = data;
      }
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  getUserMenuSectionList(userName: string) {
    Utils.blockUI();
    this.roleGroupService.getUserMenuSection(userName, environment.ApplicationId).pipe(takeUntil(this.stop$)).subscribe(data => {
      Utils.unblockUI();
      this.userMenuSectionList = [];
      if (data.length > 0) {
        this.userMenuSectionList = data;
      }

      this.getTreeViewList(this.roleGroupList[0]?.roleGroupId);
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  getTreeViewList(id) {
    this.roleGroupMenuList = [];
    this.treeviewItems = [];
    Utils.blockUI();
    this.roleGroupService.getMenusByRoleGroupIdProgramId(id, environment.ApplicationId).pipe(takeUntil(this.stop$)).subscribe(data => {
      Utils.unblockUI();
      if (data.length > 0) {
        this.roleGroupMenuList = data;
        this.setTreeView(this.menuCategoryList[0].menuCategoryId);
      }
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });
  }

  setTreeView(menuCategoryId) {
    this.displayedMenuSectionList = [];
    const roleGroupMenuList = this.roleGroupMenuList.filter(x => Number(x.menuCategoryId) === Number(menuCategoryId) && Number(x.programID) === Number(environment.ApplicationId));

    // Add MenuSection per menu
    _.map(roleGroupMenuList, (item) => {
      const section = this.menuSectionList.filter(x => Number(x.menuId) === Number(item.menuId));
      if (section.length > 0) {
        _.map(section, (sect) => {
          roleGroupMenuList.push(
            {
              menuId: sect.menuSectionId,
              parentId: item.menuId,
              applicationId: environment.ApplicationId,
              menuLabel: sect.sectionLabel,
            });
          this.displayedMenuSectionList.push(sect);
        });
      }
    });

    const allowedPages = this.userMenuSectionList.filter(x => x.isActive === true).map(menu => { return menu.menuSectionId; });
    const treeviewItems: TreeviewItem[] = this.getTreeviewItems(roleGroupMenuList, allowedPages);
    treeviewItems.forEach(treeViewItem => treeViewItem.checked = false);

    this.treeviewItems = treeviewItems;
  }

  private getTreeviewItems(menuList: Menu[], allowedPages: number[]) {

    const flat = menuList.map(menu => {
      return {
        id: menu.menuId,
        text: menu.menuLabel,
        value: menu.menuId,
        parentid: menu.menuId === menu.parentId ? 0 : menu.parentId,
        checked: allowedPages.indexOf(menu.menuId) > -1
      };
    });

    const menus: TreeviewItem[] = JsUtils.unflatten(flat).map(menu => {
      return new TreeviewItem({
        text: menu.text, value:
          menu.value, children:
          menu.children
      });
    });

    menus.forEach(menu => menu.checked = allowedPages.indexOf(menu.value) > -1);

    return menus;
  }

  private populateUsersData(result: UserViewModel[]): void {
    this.userRoleList = [];
    _.map(result, (item) => {
      const user: UserViewModel = {
        userId: item.userId,
        userName: item.userName,
        appCode: item.appCode,
        roleId: item.roleId,
        carrierCode: item.carrierCode,
        agencyId: item.agencyId,
        subAgencyId: item.subAgencyId,
        emailAddress: item.emailAddress,
        createdDate: item.createdDate,
        isActive: item.isActive,
        companyName: item.companyName,
        firstName: item.firstName,
        lastName: item.lastName,
        userAccessRights: item.userAccessRights,
        userCredentials: item.userCredentials,
        isSystemUser: item.isSystemUser,
        fullName: `${item.firstName}${' '}${item.lastName}`,
        isLocked: item.isLocked
      };
      if (user.isSystemUser) {
        this.userRoleList.push(user);
      };
      this.addTableItem(user);
    });
    this.setUserRolePage(1);
  }

  public addTableItem(newItem: UserViewModel): void {
    const tr: ITableTd[] = [];
    const fields: string[] = [
      'userName',
      'fullName',
      'emailAddress',
      'isActive'
    ];

    this.userRoleTableRows = [];
    fields.forEach((item, index) => {
      let display: any;

      switch (item) {
        case 'userName':
          display = newItem[item];
          break;
        case 'fullName':
          display = newItem[item];
          break;
        case 'emailAddress':
          display = newItem[item];
          break;
        case 'isActive':
          display = newItem[item] ? 'Active' : 'In-Active';
          break;
        default:
          display = Boolean(newItem[item]) ? String(newItem[item]) : '';
          break;
      }

      tr.push({
        id: index + 1,
        value: Boolean(newItem[item]) ? newItem[item] : '',
        display: display,
      });
    });

    if (newItem.isSystemUser) {
      this.userRoleTableRows.push({
        id: this.userRoleTableRows.length > 0 ? this.userRoleTableRows.length + 1 : 1,
        tr: tr
      });
    }
  }

  resetUserRolesList(): void {
    this.userRoleList = [];
    this.userRoleTableRows = [];
    this.userRolePageItems = [];
  }

  searchRoleAccess(input: string): void {
    Utils.blockUI();
    this.resetUserRolesList();
    this.userService.searchSystemUser({ term: input, appCode: environment.ApplicationId })
      .subscribe((result: UserViewModel[]) => {
        if (result.length > 0) {
          this.populateUsersData(result);
        }
        Utils.unblockUI();
      }, error => {
        Utils.unblockUI();
        NotifUtils.showError(error.message);
      });
  }
}
