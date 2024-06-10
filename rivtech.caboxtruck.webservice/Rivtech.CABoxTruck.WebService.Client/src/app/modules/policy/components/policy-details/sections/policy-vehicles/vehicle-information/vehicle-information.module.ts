import { NgModule } from '@angular/core';
import { VehicleInformationComponent } from './vehicle-information.component';
import { VehicleInformationSectionComponent } from './vehicle-information-section/vehicle-information-section.component';
import { OptionsCheckboxComponent } from './vehicle-information-section/options-checkbox.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { IConfig } from 'ngx-mask';
import { TextMaskModule } from 'angular2-text-mask';
import { DataTablesModule } from 'angular-datatables';
import { VehiclesExcelUploadComponent } from './vehicles-excel-upload/vehicles-excel-upload.component';
import { PolicyVehiclesRoutingModule } from '../policy-vehicles-routing.module';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [
    VehicleInformationComponent,
    VehicleInformationSectionComponent,
    VehiclesExcelUploadComponent,
    OptionsCheckboxComponent
  ],
  imports: [
    PolicyVehiclesRoutingModule,
    TextMaskModule,
    DataTablesModule,
    SharedModule
  ],
  exports: [
    VehicleInformationComponent,
    SharedModule
  ],
  providers: [
  ]
})
export class VehicleInformationModule { }
