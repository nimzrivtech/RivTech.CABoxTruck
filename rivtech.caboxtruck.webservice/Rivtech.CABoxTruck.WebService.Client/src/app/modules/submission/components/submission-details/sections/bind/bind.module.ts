import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BindComponent } from './bind.component';
import { BindRequirementsComponent } from './bind-requirements/bind-requirements.component';
import { BindingComponent } from './binding/binding.component';
import { BindRoutingModule } from './bind-routing.module';

import { NgxMaskModule, IConfig } from 'ngx-mask';
import { SharedModule } from '../../../../../../shared/shared.module';
import { DataTablesModule } from 'angular-datatables';
import { SelectItemIdToLabelPipe } from '../../../../../../shared/components/customPipes/select-item-id-to-label.pipe';
import { BsModalService } from 'ngx-bootstrap';
import { QuoteConditionComponent } from './quote-condition/quote-condition.component';
import { BindDocumentsComponent } from './bind-documents/bind-documents.component';
import { DocumentsModule } from '@app/modules/documents/documents.module';
import { NotePageModule } from '../submission-notes/note-page/note-page.module';
import { BindNotesComponent } from './bind-notes/bind-notes.component';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [BindComponent,
    BindingComponent,
    BindRequirementsComponent,
    BindDocumentsComponent,
    QuoteConditionComponent,
    BindNotesComponent
  ],
  imports: [
    DocumentsModule,
    CommonModule,
    SharedModule,
    BindRoutingModule,
    DataTablesModule,
    NotePageModule,
    NgxMaskModule.forRoot(maskConfig),
    SubmissionDetailsSharedModule,
  ],
  exports: [SharedModule],
  providers: [BsModalService]
})
export class BindModule { }
