import { NgModule } from '@angular/core';
import { VehicleComponent } from './vehicle.component';
import { VehicleInformationModule } from './vehicle-information/vehicle-information.module';
import { VehicleRoutingModule } from './vehicle-routing.module';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';

@NgModule({
  declarations: [VehicleComponent],
  imports: [
    VehicleInformationModule,
    VehicleRoutingModule,
    SubmissionDetailsSharedModule,
  ],
  providers:[
  ]
})
export class VehicleModule { }
