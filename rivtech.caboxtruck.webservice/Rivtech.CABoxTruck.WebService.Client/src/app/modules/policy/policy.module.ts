import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { PolicyListComponent } from './pages/policy-list/policy-list.component';
import { PolicyDetailsComponent } from './pages/policy-details/policy-details.component';
import { PolicyRoutingModule } from './policy-routing.module';
import { PolicyTableComponent } from './components/policy-list/policy-table/policy-table.component';
import { PolicySearchFilterComponent } from './components/policy-list/policy-search-filter/policy-search-filter.component';
import { PolicySummaryComponent } from './components/policy-details/policy-summary/policy-summary.component';
import { FormsModule } from '@angular/forms';
import { DocumentsModule } from '../documents/documents.module';
import { SharedModule } from '../../shared/shared.module';
import { TableData } from '../submission/data/tables.data';
import { DocumentData } from '../documents/document.data';
import { PolicyBillingData } from './data/policy-billing.data';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PolicyHistoryData } from './data/policy-history.data';
import { PolicyHistoryService } from './services/policy-history.service';
import { PolicyNavValidateService } from '@app/core/services/navigation/policy-nav-validate.service';
import { ApplicantNameaddressValidationService } from '@app/core/services/validations/applicant-nameaddress-validation.service';
import { ApplicantData } from '../submission/data/applicant/applicant.data';
import { VehicleData } from '../submission/data/vehicle/vehicle.data';
import { FormsData } from '../submission/data/forms/forms.data';
import { BindingData } from '../submission/data/binding/binding.data';
import { LimitsValidationService } from '@app/core/services/validations/limits-validation.service';
import { VehicleValidationService } from '@app/core/services/validations/vehicle-validation.service';
import { DriverValidationService } from '@app/core/services/validations/driver-validation.service';
import { HistoricalCoverageValidationService } from '@app/core/services/validations/historical-coverage-validation.service';
import { ClaimsHistoryValidationService } from '@app/core/services/validations/claims-history-validation.service';
import { GeneralLiabilityCargoValidationService } from '@app/core/services/validations/general-liability-cargo-validation.service';
import { LimitsData } from '../submission/data/limits/limits.data';
import { CoverageData } from '../submission/data/coverages/coverages.data';
import { ClaimsHistoryoData } from '../submission/data/claims/claims-history.data';
import { RadiusOfOperationsData } from '../submission/data/riskspecifics/radius-of-operations.data';
import { PolicyNavSavingService } from '@app/core/services/navigation/policy-nav-saving.service';
import { PolicyIssuanceData } from './data/policy-issuance.data';
import { RaterApiData } from '../submission/data/rater-api.data';
import { RatingModule } from '../rating/rating.module';
import { BrokerValidationService } from '@app/core/services/validations/broker-validation.service';
import { PolicyRatingComponent } from './components/policy-details/sections/policy-rating/policy-rating.component';
import { DestinationData } from '../submission/data/riskspecifics/destination.data';


@NgModule({
  declarations: [
    PolicyListComponent,
    PolicyDetailsComponent,
    PolicyTableComponent,
    PolicySearchFilterComponent,
    PolicySummaryComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    PolicyRoutingModule,
    DocumentsModule,
    BsDropdownModule.forRoot(),
    RatingModule,
  ],
  providers: [
    TableData,
    DocumentData,
    PolicyBillingData,
    PolicyHistoryData,
    PolicyHistoryService,
    PolicyIssuanceData,
    RaterApiData,
    CurrencyPipe,
    ApplicantData,
    VehicleData,
    FormsData,
    BindingData,
    LimitsData,
    CoverageData,
    ClaimsHistoryoData,
    RadiusOfOperationsData,
    RaterApiData,
    DestinationData,

    PolicyNavValidateService,
    PolicyNavSavingService,
    ApplicantNameaddressValidationService,
    BrokerValidationService,
    LimitsValidationService,
    VehicleValidationService,
    DriverValidationService,
    HistoricalCoverageValidationService,
    ClaimsHistoryValidationService,
    GeneralLiabilityCargoValidationService
  ]
})
export class PolicyModule { }
