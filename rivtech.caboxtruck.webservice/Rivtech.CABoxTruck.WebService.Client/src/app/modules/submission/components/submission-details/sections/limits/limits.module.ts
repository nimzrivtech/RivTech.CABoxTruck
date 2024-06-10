import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LimitsRoutingModule } from './limits-routing.module';
import { LimitsComponent } from './limits.component';
import { LimitsPageModule } from './limits-page/limits-page.module';
import { FormsModule } from '@angular/forms';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';


@NgModule({
  declarations: [LimitsComponent],
  imports: [
    CommonModule,
    FormsModule,
    LimitsRoutingModule,
    LimitsPageModule,
    SubmissionDetailsSharedModule,
  ]
})
export class LimitsModule { }
