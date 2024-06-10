import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { UnderwritingQuestionsComponent } from './underwriting-questions.component';

@NgModule({
  declarations: [
    UnderwritingQuestionsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule
  ],
  exports: [
    UnderwritingQuestionsComponent
  ],
  providers: []
})
export class UnderwritingQuestionsModule { }
