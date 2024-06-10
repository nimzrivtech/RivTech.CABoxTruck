import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { DestinationInformationComponent } from './destination-information.component';
import { DestinationSectionComponent } from './destination-section/destination-section.component';
import { RadiusOfOperationsSectionComponent } from './radius-of-operations-section/radius-of-operations-section.component';
import { AutocompleteLibModule } from 'angular-ng-autocomplete';
import { PolicyRiskSpecificsRoutingModule } from '../policy-risk-specifics-routing.module';
import { DestinationData } from '@app/modules/submission/data/riskspecifics/destination.data';

@NgModule({
  declarations: [
    DestinationInformationComponent,
    // DestinationInformationFormComponent,
    DestinationSectionComponent,
    RadiusOfOperationsSectionComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    PolicyRiskSpecificsRoutingModule,
    ReactiveFormsModule,
    AutocompleteLibModule,
    NgMultiSelectDropDownModule.forRoot(),
  ],
  providers: [
  ]
})
export class DestinationInformationModule { }
