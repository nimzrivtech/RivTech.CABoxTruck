import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HistoricalCoverageSectionComponent } from './historical-coverage-section/historical-coverage-section.component';
import {SharedModule} from '../../../../../../../shared/shared.module';
import { HistoricalCoverageComponent } from './historical-coverage.component';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { PolicyCoveragesRoutingModule } from '../policy-coverages-routing.module';
import { HistoricalCoverageChartComponent } from './historical-coverage-section/coverage-chart/historical-coverage-chart.component';
import { ProjectionFiguresChartComponent } from './historical-coverage-section/projection-chart/projection-figures-chart.component';

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
    PolicyCoveragesRoutingModule,
    NgxMaskModule.forRoot(maskConfig),
  ]
})
export class HistoricalCoverageModule { }
