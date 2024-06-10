import { Component, OnInit } from '@angular/core';
import { ITableFormControl, ITableTh, ITableTr } from '../../../../shared/models/dynamic/table.interface';
import { Menu } from '../../../../shared/models/management/menu';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { RoleTableLabelConstants } from '../../../../shared/constants/user.label.constants';
import { RoleAccessData } from '../../data/role-access.data';
import { UserRoleModalComponent } from './user-role-modal/user-role-modal.component';

@Component({
  selector: 'app-user-role',
  templateUrl: './user-role.component.html',
  styleUrls: ['./user-role.component.scss']
})
export class UserRoleComponent implements OnInit {
  TableLabel = RoleTableLabelConstants;
  modalRef: BsModalRef;
  menuAuth: Menu;
  isListHidden: boolean = false;
  isloading: boolean = false;
  tableHeader: ITableTh[] = [];
  tableRows: ITableTr[] = [];
  tableFormControls: ITableFormControl[] = [];
  messageIfNoData: string = 'No Data To Display';

  constructor(
    public userRoleData: RoleAccessData,
    private modalService: BsModalService
  ) { }

  ngOnInit() {
    this.userRoleData.getUserRoleList();
    this.userRoleData.getRoleGroupList();
    this.userRoleData.getMenuCategoryList();
    this.userRoleData.getMenuSectionList();
    this.tableRows = this.userRoleData.userRoleTableRows;
  }

  toggleUsersList(): void {
    this.isListHidden = !this.isListHidden;
  }

  setPage(page: number): void {
    this.userRoleData.setUserRolePage(page);
  }

  sortItems(sortedList: ITableTr[]) {
    this.userRoleData.userRoleTableRows = sortedList;
    this.setPage(this.userRoleData.userRolepPageNumber);
  }

  filterUserList() {
    //TODO: Insert logic for filtering
  }

  editUserRole(item) {
    const initialState = { editItem: item };
    this.modalRef = this.showModal(UserRoleModalComponent, true, initialState);
  }

  private showModal(component: any, isEdit: boolean, initialState: any): BsModalRef {
    initialState.modalTitle = 'View User Access';
    // initialState.isEdit = isEdit;

    return this.modalService.show(component, {
      initialState,
      backdrop: 'static',
      ignoreBackdropClick: true,
      keyboard: false
    });
  }
}
