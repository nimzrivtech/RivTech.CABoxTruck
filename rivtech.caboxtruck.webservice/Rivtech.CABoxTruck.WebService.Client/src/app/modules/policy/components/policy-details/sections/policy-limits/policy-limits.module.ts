import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyLimitsRoutingModule } from './policy-limits-routing.module';
import { PolicyLimitsComponent } from './policy-limits.component';
import { LimitsPageModule } from './limits-page/limits-page.module';
import { FormsModule } from '@angular/forms';
import { RiskSpecificsData } from '../../../../../../modules/submission/data/risk-specifics.data';
import { ApplicantData } from '../../../../../../modules/submission/data/applicant/applicant.data';
import { DriverData } from '../../../../../../modules/submission/data/driver/driver.data';
import { FormsData } from '../../../../../../modules/submission/data/forms/forms.data';
import { BindingData } from '../../../../../../modules/submission/data/binding/binding.data';
import { BrokerGenInfoData } from '../../../../../../modules/submission/data/broker/general-information.data';
import { ManuscriptsData } from '../../../../../../modules/submission/data/manuscripts/manuscripts.data';


@NgModule({
  declarations: [PolicyLimitsComponent],
  imports: [
    CommonModule,
    FormsModule,
    PolicyLimitsRoutingModule,
    LimitsPageModule
  ],
  providers: [
    ApplicantData,
    DriverData,
    FormsData,
    RiskSpecificsData,
    BindingData,
    BrokerGenInfoData,
    ManuscriptsData
  ]
})
export class PolicyLimitsModule { }
