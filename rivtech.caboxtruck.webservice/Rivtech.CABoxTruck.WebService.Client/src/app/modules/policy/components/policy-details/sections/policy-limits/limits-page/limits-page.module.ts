import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LimitsPageComponent } from './limits-page.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { RequestedCoverageSectionComponent } from './requested-coverage-section/requested-coverage-section.component';
import { AutoLiabilityChartComponent } from './requested-coverage-section/auto-liability-chart/auto-liability-chart.component';
import { OtherCoveragesChartComponent } from './requested-coverage-section/other-coverages-chart/other-coverages-chart.component';
import { PolicyLimitsRoutingModule } from '../policy-limits-routing.module';

@NgModule({
  declarations: [
    LimitsPageComponent,
    RequestedCoverageSectionComponent,
    AutoLiabilityChartComponent,
    OtherCoveragesChartComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PolicyLimitsRoutingModule
  ],
  providers: [
  ]
})
export class LimitsPageModule { }
