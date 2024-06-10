import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyManuscriptRoutingModule } from './policy-manuscript-routing.module';
import { PolicyManuscriptComponent } from './policy-manuscript.component';
import { ManuscriptsModule } from './manuscripts/manuscripts.module';
import { ApplicantData } from '../../../../../../modules/submission/data/applicant/applicant.data';
import { VehicleData } from '../../../../../../modules/submission/data/vehicle/vehicle.data';
import { FormsData } from '../../../../../../modules/submission/data/forms/forms.data';


@NgModule({
  declarations: [PolicyManuscriptComponent],
  imports: [
    CommonModule,
    PolicyManuscriptRoutingModule,
    ManuscriptsModule
  ],
  providers: [
    ApplicantData,
    VehicleData,
    FormsData
  ]
})
export class PolicyManuscriptModule { }
