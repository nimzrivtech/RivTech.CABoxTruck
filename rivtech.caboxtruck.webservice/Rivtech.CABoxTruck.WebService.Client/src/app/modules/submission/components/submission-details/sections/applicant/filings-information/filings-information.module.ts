import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilingsInformationComponent } from './filings-information.component';
import { FilingsInformationSectionComponent } from './filings-information-section/filings-information-section.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { ApplicantRoutingModule } from '../applicant-routing.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { SelectModule } from 'ng-select';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';

const maskConfig: Partial<IConfig> = {
  validation: false,
};

@NgModule({
  declarations: [
    FilingsInformationComponent,
    FilingsInformationSectionComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    NgxMaskModule.forRoot(maskConfig),
    NgMultiSelectDropDownModule.forRoot(),
    SelectModule,
    ApplicantRoutingModule,
    SubmissionDetailsSharedModule,
  ],
  exports: [
    FilingsInformationComponent,
    FilingsInformationSectionComponent
  ]
})
export class FilingsInformationModule { }
