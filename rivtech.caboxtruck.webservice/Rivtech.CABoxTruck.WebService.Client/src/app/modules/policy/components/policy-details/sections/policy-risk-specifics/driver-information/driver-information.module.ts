import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DriverInformationSectionComponent } from './driver-information-section/driver-information-section.component';
import { DriverInformationComponent } from './driver-information.component';
import { FormsModule } from '@angular/forms';
import { DrivingHiringCriteriaSectionComponent } from './driving-hiring-criteria-section/driving-hiring-criteria-section.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { PolicyRiskSpecificsRoutingModule } from '../policy-risk-specifics-routing.module';

@NgModule({
  declarations: [
    DriverInformationComponent,
    DriverInformationSectionComponent,
    DrivingHiringCriteriaSectionComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    PolicyRiskSpecificsRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
  ],
  providers: []
})
export class DriverInformationModule { }
