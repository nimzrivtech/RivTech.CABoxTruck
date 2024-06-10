import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersRoutingModule } from './users-routing.module';
import { UsersComponent } from './users.component';
import { SystemUserComponent } from './system-user/system-user.component';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule, TabsModule, TooltipModule } from 'ngx-bootstrap';
import { UserModalComponent } from './system-user/user-modal/user-modal.component';
import { UserAddressComponent } from './system-user/user-modal/user-address/user-address.component';
import { UserInformationComponent } from './system-user/user-modal/user-information/user-information.component';
import { CustomPipesModule } from '../../../shared/custom pipes/custom-pipe.module';
import { UserTypeComponent } from './user-type/user-type.component';
import { UserTypeModalComponent } from './user-type/user-type-modal/user-type-modals/add-edit-modal/user-type-modal.component';
import { UserGroupComponent } from './user-group/user-group.component';
import { GroupDialogComponent } from './user-group/group-dialog/group-dialog.component';
import { NgxMaskModule } from 'ngx-mask';
import { DataTableModule } from 'angular2-datatable';
import { AccountModule } from '../../account/account.module';


@NgModule({
  declarations: [
    UsersComponent,
    SystemUserComponent,
    UserModalComponent,
    UserAddressComponent,
    UserInformationComponent,
    UserTypeComponent,
    UserTypeModalComponent,
    UserGroupComponent,
    GroupDialogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    DataTablesModule,
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    UsersRoutingModule,
    CustomPipesModule,
    NgxMaskModule.forRoot(),
    TooltipModule,
    DataTableModule,
    AccountModule
  ],
  entryComponents: [
    UserModalComponent,
    UserTypeModalComponent,
    GroupDialogComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class UsersModule { }
