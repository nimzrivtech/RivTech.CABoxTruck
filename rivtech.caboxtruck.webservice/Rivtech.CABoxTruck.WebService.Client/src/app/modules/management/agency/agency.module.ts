import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgencyRoutingModule } from './agency-routing.module';
import { AgencyComponent } from './agency.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule, TabsModule, TooltipModule } from 'ngx-bootstrap';
import { SharedModule } from '../../../shared/shared.module';
import { AgencyDetailsComponent } from './agency-details/agency-details.component';
import { AgencyInfoComponent } from './agency-details/agency-info/agency-info.component';
import { AgencySubagencyComponent } from './agency-details/agency-subagency/agency-subagency.component';
import { AgencyAgentComponent } from './agency-details/agency-agent/agency-agent.component';
import { AgencyProgramStateComponent } from './agency-details/agency-program-state/agency-program-state.component';
import { AgencyAddressComponent } from './agency-details/agency-address/agency-address.component';
import { AgencyFlatCommissionsComponent } from './agency-details/agency-flat-commissions/agency-flat-commissions.component';
import { AgencyFlatCommissionModalComponent } from './agency-details/agency-flat-commissions/agency-flat-commission-modal/agency-flat-commission-modal.component';
import { AgencyProgramStateModalComponent } from './agency-details/agency-program-state/agency-program-state-modal/agency-program-state-modal.component';
import { AgencyAgentModalComponent } from './agency-details/agency-agent/agency-agent-modal/agency-agent-modal.component';
import { GenericAddressComponent } from '../../../shared/components/generic-address/generic-address.component';
import { AgencyAddressModalComponent } from './agency-details/agency-address/agency-address-modal/agency-address-modal.component';
import { DataTableModule } from 'angular2-datatable';
import { SubAgencyModalComponent } from './agency-details/agency-subagency/sub-agency-modal/sub-agency-modal.component';
import { AgentDetailsComponent } from './agency-details/agency-agent/agency-agent-modal/agent-details/agent-details.component';
import { AgentDetailsLicensesComponent } from './agency-details/agency-agent/agency-agent-modal/agent-details/agent-details-licenses/agent-details-licenses.component';
import { AgentDetailsSubagencyComponent } from './agency-details/agency-agent/agency-agent-modal/agent-details/agent-details-subagency/agent-details-subagency.component';
import { AgencyScheduleComponent } from './agency-details/agency-schedule/agency-schedule.component';
import { AgentDetailsLicensesModalComponent } from './agency-details/agency-agent/agency-agent-modal/agent-details/agent-details-licenses/agent-details-licenses-modal/agent-details-licenses-modal.component';
import { AgentDetailsProgramStateComponent } from './agency-details/agency-agent/agency-agent-modal/agent-details/agent-details-program-state/agent-details-program-state.component';
import { NgxMaskModule } from 'ngx-mask';
import { CustomPipesModule } from '../../../shared/custom pipes/custom-pipe.module';
import { AccountModule } from '../../account/account.module';

@NgModule({
  declarations: [
    AgencyComponent,
    AgencyAddressComponent,
    SubAgencyModalComponent,
    AgencyDetailsComponent,
    AgencyInfoComponent,
    AgencySubagencyComponent,
    AgencyAgentComponent,
    AgencyProgramStateComponent,
    AgencyFlatCommissionsComponent,
    AgencyFlatCommissionModalComponent,
    AgencyProgramStateModalComponent,
    AgencyAgentModalComponent,
    AgencyAddressModalComponent,
    AgentDetailsComponent,
    AgentDetailsLicensesComponent,
    AgentDetailsSubagencyComponent,
    AgencyScheduleComponent,
    AgentDetailsLicensesModalComponent,
    AgentDetailsProgramStateComponent

  ],
  imports: [
    CommonModule,
    SharedModule,
    AgencyRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    DataTablesModule,
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    TooltipModule,
    DataTableModule,
    CustomPipesModule,
    NgxMaskModule.forRoot(),
    AccountModule
  ],
  entryComponents:[
    SubAgencyModalComponent,
    AgencyFlatCommissionModalComponent,
    AgencyProgramStateModalComponent,
    AgencyAgentModalComponent,
    GenericAddressComponent,
    AgencyAddressModalComponent,
    AgencyAgentModalComponent,
    AgentDetailsLicensesComponent,
    AgentDetailsSubagencyComponent,
    AgentDetailsLicensesModalComponent
  ]
})
export class AgencyModule { }
