import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClaimsHistoryComponent } from './claims-history.component';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { SharedModule } from '../../../../../../shared/shared.module';
import { ClaimsHistoryRoutingModule } from '../claims-history/claims-history-routing.module';
import { ClaimsHistorySectionComponent } from './liability-claims-info/claims-history-section.component';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [ClaimsHistoryComponent,
    ClaimsHistorySectionComponent],
  imports: [
    CommonModule,
    SharedModule,
    ClaimsHistoryRoutingModule,
    NgxMaskModule.forRoot(maskConfig),
    SubmissionDetailsSharedModule,
  ],
  exports: [SharedModule]
})
export class ClaimsHistoryModule { }
