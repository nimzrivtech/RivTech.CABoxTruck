import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyCoveragesRoutingModule } from './policy-coverages-routing.module';
import { PolicyCoveragesComponent } from './policy-coverages.component';
import { HistoricalCoverageModule } from './historical-coverage/historical-coverage.module';
import { CoverageData } from '../../../../../../modules/submission/data/coverages/coverages.data';
import { RadiusOfOperationsData } from '../../../../../..//modules/submission/data/riskspecifics/radius-of-operations.data';
import { ApplicantData } from '../../../../../..//modules/submission/data/applicant/applicant.data';
import { VehicleData } from '../../../../../..//modules/submission/data/vehicle/vehicle.data';
import { FormsData } from '../../../../../../modules/submission/data/forms/forms.data';
import { BindingData } from '../../../../../../modules/submission/data/binding/binding.data';


@NgModule({
  declarations: [PolicyCoveragesComponent],
  imports: [
    CommonModule,
    PolicyCoveragesRoutingModule,
    HistoricalCoverageModule
  ],
  providers: [
    CoverageData,
    RadiusOfOperationsData,
    ApplicantData,
    VehicleData,
    FormsData,
    BindingData
  ]
})
export class PolicyCoveragesModule { }
