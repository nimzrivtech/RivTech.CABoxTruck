import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SubmissionFormsRoutingModule } from './submission-forms-routing.module';
import { FormsModule } from '@angular/forms';
import { FormsPageModule } from './forms-page/forms-page.module';
import { SubmissionFormsComponent } from './submission-forms.component';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';


@NgModule({
  declarations: [SubmissionFormsComponent],
  imports: [
    CommonModule,
    FormsModule,
    SubmissionFormsRoutingModule,
    FormsPageModule,
    SubmissionDetailsSharedModule,
  ]
})
export class SubmissionFormsModule { }
