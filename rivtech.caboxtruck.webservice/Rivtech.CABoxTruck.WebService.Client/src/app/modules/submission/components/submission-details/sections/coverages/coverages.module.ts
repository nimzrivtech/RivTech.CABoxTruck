import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoveragesComponent } from './coverages.component';
import { FormsModule } from '@angular/forms';
import { CoveragesRoutingModule } from './coverages-routing.module';
import { SharedModule } from '../../../../../../shared/shared.module';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { HistoricalCoverageModule } from './historical-coverage/historical-coverage.module';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [
    CoveragesComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    CoveragesRoutingModule,
    HistoricalCoverageModule,
    NgxMaskModule.forRoot(maskConfig),
    SubmissionDetailsSharedModule,
  ]
})
export class CoveragesModule { }
