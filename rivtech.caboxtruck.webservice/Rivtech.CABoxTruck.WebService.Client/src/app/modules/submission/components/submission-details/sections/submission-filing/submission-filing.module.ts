import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubmissionFilingComponent } from './submission-filing.component';
import { GeneralInformationComponent } from './general-information/general-information.component';
import { FormsModule } from '@angular/forms';
import { SubmissionFilingRoutingModule } from './submission-filing-routing.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule, TabsModule } from 'ngx-bootstrap';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';


@NgModule({
  declarations: [
    SubmissionFilingComponent,
    GeneralInformationComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SubmissionFilingRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
    DataTablesModule,
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    SubmissionDetailsSharedModule,
  ]
})
export class SubmissionFilingModule { }
