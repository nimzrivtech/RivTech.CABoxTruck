import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BusinessDetailsComponent } from './business-details.component';
import { BusinessDetailsSectionComponent } from './business-details-section/business-details-section.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { SubsidiaryDialogComponent } from './business-details-section/subsidiary-dialog/subsidiary-dialog.component';
import { CompanyDialogComponent } from './business-details-section/company-dialog/company-dialog.component';
import { PolicyApplicantRoutingModule } from '../policy-applicant-routing.module';

@NgModule({
  declarations: [
    BusinessDetailsComponent,
    BusinessDetailsSectionComponent,
    SubsidiaryDialogComponent,
    CompanyDialogComponent
  ],
  imports: [
    CommonModule,
    PolicyApplicantRoutingModule,
    SharedModule,
  ],
  entryComponents: [
    SubsidiaryDialogComponent,
    CompanyDialogComponent
  ],
})
export class BusinessDetailsModule { }
