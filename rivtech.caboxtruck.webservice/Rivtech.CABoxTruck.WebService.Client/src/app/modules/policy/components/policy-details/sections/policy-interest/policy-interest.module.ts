import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApplicantData } from '../../../../../../modules/submission/data/applicant/applicant.data';
import { DriverData } from '../../../../../../modules/submission/data/driver/driver.data';
import { VehicleData } from '../../../../../../modules/submission/data/vehicle/vehicle.data';
import { LimitsData } from '../../../../../../modules/submission/data/limits/limits.data';
import { RiskSpecificsData } from '../../../../../../modules/submission/data/risk-specifics.data';
import { FormsData } from '../../../../../../modules/submission/data/forms/forms.data';
import { BindingData } from '../../../../../../modules/submission/data/binding/binding.data';
import { PolicyInterestRoutingModule } from './policy-interest-routing.module';
import { PolicyInterestComponent } from './policy-interest.component';
import { AdditionalInterestModule } from './additional-interest/additional-interest.module';


@NgModule({
  declarations: [PolicyInterestComponent],
  imports: [
    CommonModule,
    PolicyInterestRoutingModule,
    AdditionalInterestModule
  ],
  providers: [
    LimitsData,
    ApplicantData,
    DriverData,
    VehicleData,
    FormsData,
    BindingData,
    RiskSpecificsData
  ]
})
export class PolicyInterestModule { }
