import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApplicantComponent } from './applicant.component';
import { FormsModule } from '@angular/forms';
import { KeyManagementPersonnelModule } from './key-management-personnel/key-management-personnel.module';
import { ApplicantRoutingModule } from './applicant-routing.module';
import { FilingsInformationModule } from './filings-information/filings-information.module';
import { SharedModule } from '../../../../../../shared/shared.module';
import { NameAndAddressModule } from './name-and-address/name-and-address.module';
import { BusinessDetailsModule } from './business-details/business-details.module';
import { AdditionalInterestModule } from './additional-interest/additional-interest.module';
import { ManuscriptsModule } from './manuscripts/manuscripts.module';
import { SubmissionModule } from '@app/modules/submission/submission.module';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';

@NgModule({
  declarations: [ApplicantComponent],
  imports: [
    CommonModule,
    FormsModule,
    ApplicantRoutingModule,
    KeyManagementPersonnelModule,
    AdditionalInterestModule,
    ManuscriptsModule,
    NameAndAddressModule,
    BusinessDetailsModule,
    FilingsInformationModule,
    SharedModule,
    SubmissionDetailsSharedModule,
  ],
  providers: [
  ]
})
export class ApplicantModule { }
