import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DriverInformationSectionComponent } from './driver-information-section/driver-information-section.component';
import { DriverInformationComponent } from './driver-information.component';
import { FormsModule } from '@angular/forms';
import { DrivingHiringCriteriaSectionComponent } from './driving-hiring-criteria-section/driving-hiring-criteria-section.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { DriverInformationChartComponent } from './driver-information-section/driver-information-chart/driver-information-chart.component';
import { RiskSpecificsRoutingModule } from '../risk-specifics-routing.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { DriverInfoData } from '../../../../../data/riskspecifics/driver-info.data';
import { DriverHiringCriteriaData } from '../../../../../data/riskspecifics/driver-hiring-criteria.data';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';

@NgModule({
  declarations: [
    DriverInformationComponent,
    DriverInformationSectionComponent,
    DrivingHiringCriteriaSectionComponent,
    DriverInformationChartComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RiskSpecificsRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
    SubmissionDetailsSharedModule,
  ],
  providers: []
})
export class DriverInformationModule { }
