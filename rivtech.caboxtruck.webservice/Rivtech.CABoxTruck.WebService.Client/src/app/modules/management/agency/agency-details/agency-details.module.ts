import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../../shared/shared.module';
import { AgentDetailsLicensesComponent } from './agency-agent/agency-agent-modal/agent-details/agent-details-licenses/agent-details-licenses.component';

@NgModule({
  declarations: [AgentDetailsLicensesComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ]
})
export class AgencyDetailsModule { }
