import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LimitsPageComponent } from './limits-page.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { RequestedCoverageSectionComponent } from './requested-coverage-section/requested-coverage-section.component';
import { AutoLiabilityChartComponent } from './requested-coverage-section/auto-liability-chart/auto-liability-chart.component';
import { OtherCoveragesChartComponent } from './requested-coverage-section/other-coverages-chart/other-coverages-chart.component';
import { LimitsRoutingModule } from '../limits-routing.module';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';



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
    LimitsRoutingModule,
    SubmissionDetailsSharedModule,
  ]
})
export class LimitsPageModule { }
