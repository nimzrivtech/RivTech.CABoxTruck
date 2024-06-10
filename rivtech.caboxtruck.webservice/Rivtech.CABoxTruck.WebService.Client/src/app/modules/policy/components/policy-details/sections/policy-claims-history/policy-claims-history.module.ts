import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyClaimsHistoryRoutingModule } from './policy-claims-history-routing.module';
import { PolicyClaimsHistoryComponent } from './policy-claims-history.component';
import { ClaimsHistorySectionComponent } from './liability-claims-info/claims-history-section.component';
import { SharedModule } from '../../../../../../shared/shared.module';
import { ClaimsHistoryoData } from '../../../../../../modules/submission/data/claims/claims-history.data';
import { CoverageData } from '../../../../../../modules/submission/data/coverages/coverages.data';
import { RadiusOfOperationsData } from '../../../../../../modules/submission/data/riskspecifics/radius-of-operations.data';
import { ApplicantData } from '../../../../../../modules/submission/data/applicant/applicant.data';
import { VehicleData } from '../../../../../../modules/submission/data/vehicle/vehicle.data';
import { FormsData } from '../../../../../../modules/submission/data/forms/forms.data';
import { BindingData } from '../../../../../../modules/submission/data/binding/binding.data';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { ClaimsHistoryValidationService } from '@app/core/services/validations/claims-history-validation.service';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [PolicyClaimsHistoryComponent, ClaimsHistorySectionComponent],
  imports: [
    CommonModule,
    SharedModule,
    PolicyClaimsHistoryRoutingModule,
    NgxMaskModule.forRoot(maskConfig)
  ],
  providers: [
    ClaimsHistoryoData,
    CoverageData,
    RadiusOfOperationsData,
    ApplicantData,
    VehicleData,
    FormsData,
    BindingData,
    ClaimsHistoryValidationService
  ]
})
export class PolicyClaimsHistoryModule { }
