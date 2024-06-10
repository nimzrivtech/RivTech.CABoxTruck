import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HistoricalCoverageSectionComponent } from './historical-coverage-section/historical-coverage-section.component';
import { HistoricalCoverageChartComponent } from './historical-coverage-section/historical-coverage-chart/historical-coverage-chart.component';
import { ProjectionFiguresChartComponent } from './historical-coverage-section/projection-figures-chart/projection-figures-chart.component';
import { CoveragesRoutingModule } from '../coverages-routing.module';
import {SharedModule} from '../../../../../../../shared/shared.module';
import { HistoricalCoverageComponent } from './historical-coverage.component';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [
    HistoricalCoverageSectionComponent,
    HistoricalCoverageChartComponent,
    ProjectionFiguresChartComponent,
    HistoricalCoverageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    CoveragesRoutingModule,
    NgxMaskModule.forRoot(maskConfig),
    SubmissionDetailsSharedModule,
  ]
})
export class HistoricalCoverageModule { }
