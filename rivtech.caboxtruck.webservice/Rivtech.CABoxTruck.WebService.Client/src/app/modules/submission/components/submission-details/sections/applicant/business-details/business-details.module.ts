import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BusinessDetailsComponent } from './business-details.component';
import { BusinessDetailsSectionComponent } from './business-details-section/business-details-section.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { ApplicantRoutingModule } from '../applicant-routing.module';
import { SubsidiaryDialogComponent } from './business-details-section/subsidiary-dialog/subsidiary-dialog.component';
import { CompanyDialogComponent } from './business-details-section/company-dialog/company-dialog.component';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';

@NgModule({
  declarations: [
    BusinessDetailsComponent,
    BusinessDetailsSectionComponent,
    SubsidiaryDialogComponent,
    CompanyDialogComponent
  ],
  imports: [
    CommonModule,
    ApplicantRoutingModule,
    SharedModule,
    SubmissionDetailsSharedModule,
  ],
  entryComponents: [
    SubsidiaryDialogComponent,
    CompanyDialogComponent
  ],
})
export class BusinessDetailsModule { }
