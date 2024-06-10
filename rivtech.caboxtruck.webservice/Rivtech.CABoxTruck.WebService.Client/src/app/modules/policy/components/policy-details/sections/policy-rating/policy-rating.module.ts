import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PolicyRatingRoutingModule } from './policy-rating-routing.module';
import { PolicyRatingComponent } from './policy-rating.component';
import { SharedModule } from '@app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PolicyRatingFactorsComponent } from './policy-rating-factors/policy-rating-factors.component';
import { QuoteLimitsData } from '@app/modules/submission/data/quote/quote-limits.data';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';



@NgModule({
  declarations: [
    PolicyRatingComponent,
    PolicyRatingFactorsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    PolicyRatingRoutingModule,
  ],
  providers: [
    QuoteLimitsData,
  ]
})
export class PolicyRatingModule { }
