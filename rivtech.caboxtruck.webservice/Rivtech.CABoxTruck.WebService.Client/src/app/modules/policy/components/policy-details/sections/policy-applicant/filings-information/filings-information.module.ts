import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilingsInformationComponent } from './filings-information.component';
import { FilingsInformationSectionComponent } from './filings-information-section/filings-information-section.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { SelectModule } from 'ng-select';
import { PolicyApplicantRoutingModule } from '../policy-applicant-routing.module';

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
    PolicyApplicantRoutingModule
  ],
  exports: [
    FilingsInformationComponent,
    FilingsInformationSectionComponent
  ]
})
export class FilingsInformationModule { }
