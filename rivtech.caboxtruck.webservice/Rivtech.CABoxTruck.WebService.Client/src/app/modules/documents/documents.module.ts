import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocumentsListComponent } from './components/documents-list/documents-list.component';
import { DocumentsSearchFilterComponent } from './components/documents-search-filter/documents-search-filter.component';
import { DocumentsAddDialogComponent } from './components/documents-add-dialog/documents-add-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EmailDocumentsButtonComponent } from './components/email-documents-button/email-documents-button.component';
import { DownloadDocumentsButtonComponent } from './components/download-documents-button/download-documents-button.component';
import { UploadDocumentButtonComponent } from './components/upload-document-button/upload-document-button.component';
import { DocumentApiService } from './document-api.service';

import { SharedModule } from '../../shared/shared.module';
import { PaginationModule } from 'ngx-bootstrap';

@NgModule({
  declarations: [
    DocumentsListComponent,
    DocumentsSearchFilterComponent,
    DocumentsAddDialogComponent,
    EmailDocumentsButtonComponent,
    DownloadDocumentsButtonComponent,
    UploadDocumentButtonComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    PaginationModule.forRoot()
  ],
  entryComponents: [
    DocumentsAddDialogComponent,
  ],
  exports: [
    DocumentsListComponent,
    DocumentsSearchFilterComponent,
    EmailDocumentsButtonComponent,
    DownloadDocumentsButtonComponent,
    UploadDocumentButtonComponent,
  ],
  providers: [
    DocumentApiService
  ]
})
export class DocumentsModule { }
