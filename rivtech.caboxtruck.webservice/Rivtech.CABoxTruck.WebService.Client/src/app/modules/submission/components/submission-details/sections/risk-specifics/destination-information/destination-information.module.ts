import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { RiskSpecificsRoutingModule } from '../risk-specifics-routing.module';
import { DestinationInformationComponent } from './destination-information.component';
import { DestinationSectionComponent } from './destination-section/destination-section.component';
import { DestinationInformationFormComponent } from './destination-section/destination-information-form/destination-information-form.component';
import { RadiusOfOperationsSectionComponent } from './radius-of-operations-section/radius-of-operations-section.component';
import { AutocompleteLibModule } from 'angular-ng-autocomplete';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';

@NgModule({
  declarations: [
    DestinationInformationComponent,
    DestinationInformationFormComponent,
    DestinationSectionComponent,
    RadiusOfOperationsSectionComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RiskSpecificsRoutingModule,
    ReactiveFormsModule,
    AutocompleteLibModule,
    NgMultiSelectDropDownModule.forRoot(),
    SubmissionDetailsSharedModule,
  ],
  providers: []
})
export class DestinationInformationModule { }
