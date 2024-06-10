import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyDriversRoutingModule } from './policy-drivers-routing.module';
import { PolicyDriversComponent } from './policy-drivers.component';
import { DDriverInformationModule } from './d-driver-information/d-driver-information.module';
import { DriverData } from '@app/modules/submission/data/driver/driver.data';

@NgModule({
  declarations: [PolicyDriversComponent],
  imports: [
    CommonModule,
    PolicyDriversRoutingModule,
    DDriverInformationModule
  ],
  providers: [
  ]
})
export class PolicyDriversModule { }
