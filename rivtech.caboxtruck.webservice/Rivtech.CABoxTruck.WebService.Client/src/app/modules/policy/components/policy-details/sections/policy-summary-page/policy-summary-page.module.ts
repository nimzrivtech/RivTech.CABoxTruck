import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../../../../../shared/shared.module';
import { DataTablesModule } from 'angular-datatables';
import { PolicySummaryPageRoutingModule } from './policy-summary-page-routing.module';
import { PolicySummaryPageComponent } from './policy-summary-page.component';
import { InsuredSectionComponent } from './insured-section/insured-section.component';
import { AutoLiabilitySectionComponent } from './auto-liability-section/auto-liability-section.component';
import { PhysicalDamageSectionComponent } from './physical-damage-section/physical-damage-section.component';
import { GlmtcSectionComponent } from './glmtc-section/glmtc-section.component';
import { AdditionalInterestModule } from '../policy-interest/additional-interest/additional-interest.module';
import { ApplicantData } from '../../../../../../modules/submission/data/applicant/applicant.data';
import { VehicleData } from '../../../../../../modules/submission/data/vehicle/vehicle.data';
import { FormsData } from '../../../../../../modules/submission/data/forms/forms.data';
import { BindingData } from '../../../../../../modules/submission/data/binding/binding.data';


@NgModule({
  declarations: [PolicySummaryPageComponent, InsuredSectionComponent, AutoLiabilitySectionComponent, PhysicalDamageSectionComponent, GlmtcSectionComponent],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    DataTablesModule,
    PolicySummaryPageRoutingModule,
    AdditionalInterestModule
  ],
  providers: [
    ApplicantData,
    VehicleData,
    FormsData,
    BindingData
  ]
})
export class PolicySummaryPageModule { }
