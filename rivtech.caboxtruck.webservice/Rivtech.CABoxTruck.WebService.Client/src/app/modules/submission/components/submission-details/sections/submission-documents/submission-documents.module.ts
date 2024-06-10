import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SubmissionDocumentsRoutingModule } from './submission-documents-routing.module';
import { SubmissionDocumentsComponent } from './submission-documents.component';
import { DocumentsModule } from '@app/modules/documents/documents.module';
import { SharedModule } from '../../../../../../shared/shared.module';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';

@NgModule({
  declarations: [SubmissionDocumentsComponent],
  imports: [
    CommonModule,
    FormsModule,
    SubmissionDocumentsRoutingModule,
    DocumentsModule,
    SharedModule,
    SubmissionDetailsSharedModule,
  ]
})
export class SubmissionDocumentsModule { }
