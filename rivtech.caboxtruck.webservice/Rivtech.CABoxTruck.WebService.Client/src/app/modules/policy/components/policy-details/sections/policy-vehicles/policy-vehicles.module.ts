import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyVehiclesRoutingModule } from './policy-vehicles-routing.module';
import { PolicyVehiclesComponent } from './policy-vehicles.component';
import { VehicleInformationModule } from './vehicle-information/vehicle-information.module';
import { RiskSpecificsData } from '../../../../../../modules/submission/data/risk-specifics.data';


@NgModule({
  declarations: [PolicyVehiclesComponent],
  imports: [
    CommonModule,
    PolicyVehiclesRoutingModule,
    VehicleInformationModule
  ],
  providers: [
    RiskSpecificsData
  ]
})
export class PolicyVehiclesModule { }
