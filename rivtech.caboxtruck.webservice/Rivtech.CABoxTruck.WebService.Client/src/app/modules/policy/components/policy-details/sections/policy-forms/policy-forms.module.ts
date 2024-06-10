import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../../../../../shared/shared.module';
import { DataTablesModule } from 'angular-datatables';
import { PolicyFormsComponent } from './policy-forms.component';
import { PolicyFormsRoutingModule } from './policy-forms-routing.module';
import { FormsPageModule } from './forms-page/forms-page.module';
import { BindingData } from '../../../../../../modules/submission/data/binding/binding.data';
import { LimitsData } from '../../../../../../modules/submission/data/limits/limits.data';
import { FormsData } from '../../../../../../modules/submission/data/forms/forms.data';
import { ManuscriptsData } from '../../../../../../modules/submission/data/manuscripts/manuscripts.data';
import { RiskSpecificsData } from '../../../../../../modules/submission/data/risk-specifics.data';

@NgModule({
  declarations: [PolicyFormsComponent],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    DataTablesModule,
    PolicyFormsRoutingModule,
    FormsPageModule
  ],
  providers: [
    FormsData,
    BindingData,
    ManuscriptsData,
    LimitsData,
    RiskSpecificsData
  ],
})
export class PolicyFormsModule { }
