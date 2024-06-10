import { NgModule } from '@angular/core';
import { DriverComponent } from './driver.component';
import { DDriverInformationModule } from './d-driver-information/d-driver-information.module';
import { DriverRoutingModule } from './driver-routing.module';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';

@NgModule({
  declarations: [DriverComponent],
  imports: [
    DDriverInformationModule,
    DriverRoutingModule,
    SubmissionDetailsSharedModule,
  ],
  providers: []
})
export class DriverModule { }
