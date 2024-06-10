import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuoteOptionsComponent } from './quote-options.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuotesRoutingModule } from './quote-options-routing.module';
import { QuotePageComponent } from './quote-page/quote-page.component';
import { QuotePageModule } from './quote-page/quote-page.module';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';


@NgModule({
  declarations: [
    QuoteOptionsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    QuotesRoutingModule,
    QuotePageModule,
    SubmissionDetailsSharedModule,
  ]
})
export class QuoteOptionsModule { }
