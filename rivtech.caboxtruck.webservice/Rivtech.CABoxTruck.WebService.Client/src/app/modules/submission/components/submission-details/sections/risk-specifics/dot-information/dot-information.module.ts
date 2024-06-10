import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { RiskSpecificsRoutingModule } from '../risk-specifics-routing.module';
import { DotInformationComponent } from './dot-information.component';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';

@NgModule({
  declarations: [
    DotInformationComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RiskSpecificsRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
    SubmissionDetailsSharedModule,
  ],
  providers: []
})
export class DotInformationModule { }
