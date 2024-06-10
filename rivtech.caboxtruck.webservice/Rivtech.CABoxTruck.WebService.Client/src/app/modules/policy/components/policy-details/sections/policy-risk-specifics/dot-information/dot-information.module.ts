import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { DotInformationComponent } from './dot-information.component';
import { PolicyRiskSpecificsRoutingModule } from '../policy-risk-specifics-routing.module';

@NgModule({
  declarations: [
    DotInformationComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    PolicyRiskSpecificsRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
  ],
  providers: []
})
export class DotInformationModule { }
