import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManuscriptsComponent } from './manuscripts.component';
import { ManuscriptsSectionComponent } from './manuscripts-section/manuscripts-section.component';
import { ManuscriptDialogComponent } from './manuscripts-section/manuscript-dialog.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule, TabsModule, PaginationModule } from 'ngx-bootstrap';
import { ApplicantRoutingModule } from '../applicant-routing.module';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';


@NgModule({
  declarations: [
    ManuscriptsComponent,
    ManuscriptsSectionComponent,
    ManuscriptDialogComponent
  ],
  imports: [
    CommonModule,
    ApplicantRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
    DataTablesModule,
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    SharedModule,
    SubmissionDetailsSharedModule,
    PaginationModule.forRoot()
  ],
  exports: [
    ManuscriptDialogComponent,
  ],
  entryComponents: [
    ManuscriptDialogComponent
  ]
})
export class ManuscriptsModule { }
