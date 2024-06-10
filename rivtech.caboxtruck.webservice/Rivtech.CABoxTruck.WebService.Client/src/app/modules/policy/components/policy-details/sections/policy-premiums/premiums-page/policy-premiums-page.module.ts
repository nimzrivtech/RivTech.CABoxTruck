import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { PolicyPremiumsPageComponent } from './policy-premiums-page.component';
import { PolicyPremiumsRoutingModule } from '../policy-premiums-routing.module';
import { PolicyPremiumSectionComponent } from './premium-section/policy-premium-section.component';
import { PolicyPremiumVehicleChartComponent } from './premium-section/premium-vehicle-chart/policy-premium-vehicle-chart.component';


@NgModule({
  declarations: [
    PolicyPremiumsPageComponent,
    PolicyPremiumSectionComponent,
    PolicyPremiumVehicleChartComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    PolicyPremiumsRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
  ]
})
export class PolicyPremiumsPageModule { }
