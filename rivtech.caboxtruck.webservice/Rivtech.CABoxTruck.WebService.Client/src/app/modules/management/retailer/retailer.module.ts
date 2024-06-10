import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule, TabsModule, TooltipModule } from 'ngx-bootstrap';
import { SharedModule } from '../../../shared/shared.module';
import { GenericAddressComponent } from '../../../shared/components/generic-address/generic-address.component';
import { DataTableModule } from 'angular2-datatable';
import { NgxMaskModule } from 'ngx-mask';
import { CustomPipesModule } from '../../../shared/custom pipes/custom-pipe.module';
import { AccountModule } from '../../account/account.module';
import { RetailerComponent } from './retailer.component';
import { RetailerRoutingModule } from './retailer-routing.module';
import { RetailerDetailsComponent } from './retailer-details/retailer-details.component';
import { RetailerInfoComponent } from './retailer-details/retailer-info/retailer-info.component';
import { RetailerAgentComponent } from './retailer-details/retailer-agent/retailer-agent.component';
import { RetailerAgentModalComponent } from './retailer-details/retailer-agent/retailer-agent-modal/retailer-agent-modal.component';
import { RetailerAgentDetailsComponent } from './retailer-details/retailer-agent/retailer-agent-modal/retailer-agent-details/retailer-agent-details.component';
import { RetailerInfoAgencyComponent } from './retailer-details/retailer-info/retailer-info-agency/retailer-info-agency.component';

@NgModule({
  declarations: [
    RetailerComponent,
    RetailerDetailsComponent,
    RetailerInfoComponent,
    RetailerAgentComponent,
    RetailerAgentModalComponent,
    RetailerDetailsComponent,
    RetailerAgentDetailsComponent,
    RetailerInfoAgencyComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    RetailerRoutingModule,
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
    RetailerAgentModalComponent,
    GenericAddressComponent,
    RetailerAgentModalComponent,
  ]
})
export class RetailerModule { }
