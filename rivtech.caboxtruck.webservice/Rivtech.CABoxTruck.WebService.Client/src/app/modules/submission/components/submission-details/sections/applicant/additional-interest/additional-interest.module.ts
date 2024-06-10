import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdditionalInterestComponent } from './additional-interest.component';
import { AdditionalInterestSectionComponent } from './additional-interest-section/additional-interest-section.component';
import { AdditionalInterestFormComponent } from './additional-interest-section/additional-interest-form.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule, TabsModule,PaginationModule  } from 'ngx-bootstrap';
import { ApplicantRoutingModule } from '../applicant-routing.module';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';


@NgModule({
  declarations: [
    AdditionalInterestComponent,
    AdditionalInterestSectionComponent,
    AdditionalInterestFormComponent
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
    AdditionalInterestFormComponent,
  ],
  entryComponents: [
    AdditionalInterestFormComponent
  ]
})
export class AdditionalInterestModule { }
