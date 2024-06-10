import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaintenanceSafetyComponent } from './maintenance-safety.component';
import { MaintenanceQuestionsSectionComponent } from './maintenance-questions-section/maintenance-questions-section.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { MaintenanceQuestionsChartComponent } from './maintenance-questions-section/maintenance-questions-chart/maintenance-questions-chart.component';
import { RiskSpecificsRoutingModule } from '../risk-specifics-routing.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { SafetyDevicesSectionComponent } from './safety-devices-section/safety-devices-section.component';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';

@NgModule({
  declarations: [
    MaintenanceSafetyComponent,
    MaintenanceQuestionsSectionComponent,
    MaintenanceQuestionsChartComponent,
    SafetyDevicesSectionComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RiskSpecificsRoutingModule,
    ReactiveFormsModule,
    NgMultiSelectDropDownModule.forRoot(),
    SubmissionDetailsSharedModule,
  ]
})
export class MaintenanceSafetyModule { }
