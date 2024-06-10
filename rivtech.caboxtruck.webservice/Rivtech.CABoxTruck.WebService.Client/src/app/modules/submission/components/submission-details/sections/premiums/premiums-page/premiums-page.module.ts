import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';
import { PremiumsPageComponent } from './premiums-page.component';
import { PremiumsRoutingModule } from '../premiums-routing.module';
import { PremiumSectionComponent } from './premium-section/premium-section.component';
import { PremiumVehicleChartComponent } from './premium-section/premium-vehicle-chart/premium-vehicle-chart.component';


@NgModule({
  declarations: [
    PremiumsPageComponent,
    PremiumSectionComponent,
    PremiumVehicleChartComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    PremiumsRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
    SubmissionDetailsSharedModule,
  ]
})
export class PremiumsPageModule { }
