import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuotePageComponent } from './quote-page.component';
import { QuotesRoutingModule } from '../quote-options-routing.module';
import { QuoteSectionComponent } from './quote-section/quote-section.component';
import { QuoteAutoLiabilityChartComponent } from './quote-section/auto-liability-chart/auto-liability-chart.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';


@NgModule({
  declarations: [
    QuotePageComponent,
    QuoteSectionComponent,
    QuoteAutoLiabilityChartComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    QuotesRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
    SubmissionDetailsSharedModule,
  ]
})
export class QuotePageModule { }
