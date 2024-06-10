import { Component, OnInit } from '@angular/core';
import { RoleAccessData } from '../../../../../../modules/management/data/role-access.data';
import { TreeviewItem, TreeviewConfig } from 'ngx-treeview';

export class TreeViewItem1 {
  text: string;
  value: number;
  children: any[];
}

@Component({
  selector: 'app-role-pages-section',
  templateUrl: './role-pages-section.component.html',
  styleUrls: ['./role-pages-section.component.scss']
})
export class RolePagesSectionComponent implements OnInit {
  menuCategory: any;
  config = TreeviewConfig.create({
    hasAllCheckBox: true,
    hasFilter: true,
    hasCollapseExpand: true,
    decoupleChildFromParent: false,
    maxHeight: 400
  });

  constructor(public userRoleData: RoleAccessData) { }

  ngOnInit() {
    this.menuCategory = this.userRoleData.menuCategoryList[0].menuCategoryId;
    // this.userRoleData.getTreeViewList(this.userRoleData.roleGroupList[0]?.roleGroupId);
  }

  updateTree() {
    this.userRoleData.setTreeView(this.menuCategory);
  }

  onSelectedChange(event) {
    this.userRoleData.selectedItems = event;
  }

  // public get selectedItemsIncludingParent(): number[] {
  //   let allSelectedItems: number[] = this.selectedItems;

  //   let menuList: Menu[] = this.navService.allMenuList;

  //   this.selectedItems.forEach(item => {
  //     let menu = menuList.filter(m => m.menuId == item)[0];

  //     if(menu){
  //       if (allSelectedItems.indexOf(menu.parentId) < 0) {
  //         allSelectedItems.push(menu.parentId);
  //       }
  //     }
  //   });

  //   return allSelectedItems;
  // }

}