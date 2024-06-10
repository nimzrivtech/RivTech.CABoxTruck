import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyApplicantRoutingModule } from './policy-applicant-routing.module';
import { PolicyApplicantComponent } from './policy-applicant.component';
import { FormsModule } from '@angular/forms';
import { NameAndAddressModule } from './name-and-address/name-and-address.module';
import { BusinessDetailsModule } from './business-details/business-details.module';
import { FilingsInformationModule } from './filings-information/filings-information.module';
import { ApplicantData } from '../../../../../../modules/submission/data/applicant/applicant.data';
import { VehicleData } from '../../../../../../modules/submission/data/vehicle/vehicle.data';
import { FormsData } from '../../../../../../modules/submission/data/forms/forms.data';
import { BindingData } from '../../../../../../modules/submission/data/binding/binding.data';
import { ApplicantFilingsValidationService } from '../../../../../../core/services/validations/filings-information-validation.service';
import { ApplicantBusinessValidationService } from '../../../../../../core/services/validations/applicant-business-validation.service';
import { RaterApiData } from '../../../../../../modules/submission/data/rater-api.data';
import { CoverageData } from '../../../../../../modules/submission/data/coverages/coverages.data';
import { RadiusOfOperationsData } from '../../../../../../modules/submission/data/riskspecifics/radius-of-operations.data';
import { BusinessDetailsData } from '../../../../../../modules/submission/data/applicant/business-details.data';
import { BsModalService } from 'ngx-bootstrap';


@NgModule({
  declarations: [PolicyApplicantComponent],
  imports: [
    CommonModule,
    FormsModule,
    PolicyApplicantRoutingModule,
    NameAndAddressModule,
    BusinessDetailsModule,
    FilingsInformationModule
  ],
  providers: [
    VehicleData,
    FormsData,
    BindingData,
    ApplicantFilingsValidationService,
    ApplicantBusinessValidationService,
    RaterApiData,
    CoverageData,
    RadiusOfOperationsData,
    BusinessDetailsData,
    BsModalService
  ]
})
export class PolicyApplicantModule { }
