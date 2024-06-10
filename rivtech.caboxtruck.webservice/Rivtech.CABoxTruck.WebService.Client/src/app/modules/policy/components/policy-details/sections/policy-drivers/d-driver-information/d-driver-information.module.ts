import { NgModule } from '@angular/core';
import { DDriverInformationComponent } from './d-driver-information.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { IConfig } from 'ngx-mask';
import { DataTablesModule } from 'angular-datatables';
import { DDriverInformationSectionComponent } from './d-driver-information-section/d-driver-information-section.component';
import { StateIdToCodePipe } from '../../../../../../../shared/components/customPipes/state-id-to-code.pipe';
import { DriverFilterPipe } from '../../../../../../../shared/components/customPipes/driver-filter.pipe';

import { DatePipe } from '@angular/common';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { DriverExcelUploadComponent } from './driver-excel-upload/driver-excel-upload.component';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [DDriverInformationComponent, DDriverInformationSectionComponent, DriverExcelUploadComponent],
  imports: [
    DataTablesModule,
    SharedModule,
    NgMultiSelectDropDownModule.forRoot(),
  ],
  exports: [DDriverInformationComponent, SharedModule],
  providers: [
    DatePipe,
  ]
})
export class DDriverInformationModule { }
