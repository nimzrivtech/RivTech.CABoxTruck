import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../../../../../shared/shared.module';
import { DocumentsModule } from '../../../../../../modules/documents/documents.module';
import { DataTablesModule } from 'angular-datatables';
import { PolicyTasksComponent } from './policy-tasks.component';
import { PolicyTasksRoutingModule } from './policy-tasks-routing.module';


@NgModule({
  declarations: [PolicyTasksComponent],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    DocumentsModule,
    DataTablesModule,
    PolicyTasksRoutingModule
  ]
})
export class PolicyTasksModule { }
