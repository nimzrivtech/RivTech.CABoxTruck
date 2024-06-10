import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PolicyDocumentsComponent } from './policy-documents.component';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../../../../../shared/shared.module';
import { PolicyDocumentsRoutingModule } from './policy-documents-routing.module';
import { DocumentsModule } from '../../../../../../modules/documents/documents.module';
import { DataTablesModule } from 'angular-datatables';
import { BindingData } from '@app/modules/submission/data/binding/binding.data';


@NgModule({
  declarations: [PolicyDocumentsComponent],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    DocumentsModule,
    DataTablesModule,
    PolicyDocumentsRoutingModule
  ],
  providers: [
    BindingData
  ]
})
export class PolicyDocumentsModule { }
