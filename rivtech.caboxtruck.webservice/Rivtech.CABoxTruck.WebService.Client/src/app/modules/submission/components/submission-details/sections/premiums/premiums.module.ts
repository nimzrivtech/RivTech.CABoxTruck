import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';
import { PremiumsComponent } from './premiums.component';
import { PremiumsPageModule } from './premiums-page/premiums-page.module';
import { PremiumsRoutingModule } from './premiums-routing.module';


@NgModule({
  declarations: [
    PremiumsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PremiumsRoutingModule,
    PremiumsPageModule,
    SubmissionDetailsSharedModule,
  ]
})
export class PremiumsModule { }
