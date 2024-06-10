import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { UnderwritingQuestionsComponent } from './underwriting-questions.component';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';

@NgModule({
  declarations: [
    UnderwritingQuestionsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule,
    SubmissionDetailsSharedModule,
  ],
  exports: [
    UnderwritingQuestionsComponent
  ],
  providers: []
})
export class UnderwritingQuestionsModule { }
