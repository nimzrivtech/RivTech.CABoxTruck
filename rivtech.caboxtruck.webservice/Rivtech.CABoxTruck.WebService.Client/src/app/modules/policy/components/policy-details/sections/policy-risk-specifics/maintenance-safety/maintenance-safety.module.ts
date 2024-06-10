import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaintenanceSafetyComponent } from './maintenance-safety.component';
import { MaintenanceQuestionsSectionComponent } from './maintenance-questions-section/maintenance-questions-section.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { SafetyDevicesSectionComponent } from './safety-devices-section/safety-devices-section.component';
import { PolicyRiskSpecificsRoutingModule } from '../policy-risk-specifics-routing.module';
import { MaintenanceQuestionsChartComponent } from './maintenance-questions-section/devices-chart/maintenance-questions-chart.component';
import { RiskSpecificsData } from '../../../../../../../modules/submission/data/risk-specifics.data';


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
    PolicyRiskSpecificsRoutingModule,
    ReactiveFormsModule,
    NgMultiSelectDropDownModule.forRoot(),
  ]
})
export class MaintenanceSafetyModule { }
