import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RoleRoutingModule } from './role-routing.module';
import { RoleComponent } from './role.component';
import { UserRoleComponent } from './user-role/user-role.component';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule, TabsModule } from 'ngx-bootstrap';
import { CustomPipesModule } from '../../../shared/custom pipes/custom-pipe.module';
import { UserRoleModalComponent } from './user-role/user-role-modal/user-role-modal.component';
import { RolePagesSectionComponent } from './user-role/user-role-modal/role-pages-section/role-pages-section.component';
import { TreeviewModule } from 'ngx-treeview';


@NgModule({
  declarations: [
    RoleComponent,
    UserRoleComponent,
    UserRoleModalComponent,
    RolePagesSectionComponent
  ],
  imports: [
    CommonModule,
    RoleRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    DataTablesModule,
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    TreeviewModule.forRoot(),
    CustomPipesModule
  ],
  entryComponents: [
    UserRoleModalComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class RoleModule { }
