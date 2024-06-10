import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ManagementRoutingModule } from './management-routing.module';
import { ManagementComponent } from './management.component';
import { FormsModule } from '../../views/forms/forms.module';
import { AppSidebarModule } from '@coreui/angular';
import { ReactiveFormsModule } from '@angular/forms';
import { AgencyData } from './data/agency.data';
import { SubAgencyData } from './data/sub-agency.data';
import { GenericAddressData } from '../../shared/components/generic-address/data/generic-address.data';
import { RoleData } from './data/role.data';
import { AddRoleData } from './data/add-role.data';
import { ZipCodeData } from './data/zipcode.data';
import { UserData } from './data/user.data';
import { AgentData } from './data/agent.data';
import { ReferenceCodeModalComponent } from '../login/reference-code-modal/reference-code-modal.component';
import { CodeInputModule } from 'angular-code-input';
import { ProgramStateData } from './data/program-state.data';
import { RetailerData } from './data/retailer.data';
import { RetailerAgentData } from './data/retailer-agent.data';

@NgModule({
  declarations: [ManagementComponent, ReferenceCodeModalComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AppSidebarModule,
    ManagementRoutingModule,
    CodeInputModule.forRoot({
      codeLength: 6,
      isCharsCode: true,
      code: ''
    }),
  ],
  exports: [ReferenceCodeModalComponent],
  providers: [AgencyData, SubAgencyData, RetailerData, RetailerAgentData, AgentData, GenericAddressData, UserData, RoleData, AddRoleData, ZipCodeData, ProgramStateData],
  entryComponents: [ReferenceCodeModalComponent]
})
export class ManagementModule { }
