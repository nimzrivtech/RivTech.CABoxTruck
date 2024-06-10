import { Component, OnInit } from '@angular/core';
import { RoleAccessData } from '../../../../../modules/management/data/role-access.data';
import { BsModalRef } from 'ngx-bootstrap';
import * as _ from 'lodash';

@Component({
  selector: 'app-user-role-modal',
  templateUrl: './user-role-modal.component.html',
  styleUrls: ['./user-role-modal.component.scss']
})
export class UserRoleModalComponent implements OnInit {
  editItem: any;
  modalTitle = '';
  isEdit = false;
  constructor(
    public modalRef: BsModalRef,
    public userRoleData: RoleAccessData
    ) { }

  ngOnInit() {
    this.userRoleData.getUserMenuSectionList(this.editItem.userName);
  }

  closeDialog() {
    this.modalRef.hide();
  }

  tabChange(id) {
    this.userRoleData.getTreeViewList(id);
  }

  saveUserRoleSection() {
    this.userRoleData.saveUserMenuSections(this.editItem.userName);
  }
}
