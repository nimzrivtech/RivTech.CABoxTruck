import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { AccordionModule, AccordionConfig } from 'ngx-bootstrap/accordion';
import { SubmissionRoutingModule } from './submission-routing.module';
import { AppSidebarModule, AppBreadcrumbModule } from '@coreui/angular';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts';
import { ApplicantData } from './data/applicant/applicant.data';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SubmissionListComponent } from './pages/submission-list/submission-list.component';
import { SubmissionDetailsComponent } from './pages/submission-details/submission-details.component';
import { DataTableModule } from 'angular2-datatable';
import { SubmissionSummaryComponent } from './components/submission-details/submission-summary/submission-summary/submission-summary.component';
import { SubmissionTableComponent } from './components/submission-list/submission-table/submission-table.component';
import { SubmissionSearchFilterComponent } from './components/submission-list/submission-search-filter/submission-search-filter.component';
import { DocumentsModule } from '../documents/documents.module';
import { SubmissionTasksComponent } from './components/submission-details/sections/submission-tasks/submission-tasks.component';
import { TasksModule } from '../tasks/tasks.module';
import { SharedModule } from '../../shared/shared.module';
import { LimitsData } from './data/limits/limits.data';
import { SubmissionNavSavingService } from '../../core/services/navigation/submission-nav-saving.service';
import { SubmissionNavValidateService } from '../../core/services/navigation/submission-nav-validate.service';
import { SubmissionListData } from './data/submission-list.data';
import { CoverageData } from './data/coverages/coverages.data';
import { RiskSpecificsData } from './data/risk-specifics.data';
import { UnderwritingQuestionsData } from './data/riskspecifics/underwriting-questions.data';
import { DotInfoData } from './data/riskspecifics/dot-info.data';
import { DriverInfoData } from './data/riskspecifics/driver-info.data';
import { DriverHiringCriteriaData } from './data/riskspecifics/driver-hiring-criteria.data';
import { DestinationData } from './data/riskspecifics/destination.data';
import { RadiusOfOperationsData } from './data/riskspecifics/radius-of-operations.data';
import { BrokerGenInfoData } from './data/broker/general-information.data';
import { QuoteLimitsData } from './data/quote/quote-limits.data';
import { DriverData } from './data/driver/driver.data';
import { VehicleData } from './data/vehicle/vehicle.data';
import { RaterApiData } from './data/rater-api.data';
import { BindingData } from './data/binding/binding.data';

import { ApplicantBusinessValidationService } from '../../core/services/validations/applicant-business-validation.service';
import { ApplicantNameaddressValidationService } from '../../core/services/validations/applicant-nameaddress-validation.service';
import { ApplicantFilingsValidationService } from '../../core/services/validations/filings-information-validation.service';
import { BrokerValidationService } from '../../core/services/validations/broker-validation.service';
import { LimitsValidationService } from '../../core/services/validations/limits-validation.service';
import { DestinationInfoValidationService } from '../../core/services/validations/destination-info-validation.service';
import { DriverInfoValidationService } from '../../core/services/validations/driver-information-validation.service';
import { DotInfoValidationService } from '../../core/services/validations/dot-information-validation.service';
import { MaintenanceSafetyValidationService } from '../../core/services/validations/maintenance-safety-validation.service';
import { GeneralLiabilityCargoValidationService } from '../../core/services/validations/general-liability-cargo-validation.service';
import { UnderwritingQuestionsValidationService } from '../../core/services/validations/underwriting-questions-validation.service';
import { HistoricalCoverageValidationService } from '../../core/services/validations/historical-coverage-validation.service';
import { VehicleValidationService } from '../../core/services/validations/vehicle-validation.service';
import { DriverValidationService } from '../../core/services/validations/driver-validation.service';
import { QuoteValidationService } from '../../core/services/validations/quote-validation.service';
import { ClaimsHistoryoData } from './data/claims/claims-history.data';
import { ClaimsHistoryValidationService } from '../../core/services/validations/claims-history-validation.service';
import { NotesData } from './data/note/note.data';
import { FormsData } from './data/forms/forms.data';
import { BsModalService, RatingModule } from 'ngx-bootstrap';
import { AddUpdateBindingComponent } from './components/submission-details/sections/bind/modals/add-binding/add-update-binding.component';
import { UploadDocumentComponent } from './components/submission-details/sections/bind/modals/upload-document/upload-document.component';
import { ReportData } from './data/report/report.data';
import { AddQuoteConditionComponent } from './components/submission-details/sections/bind/modals/add-quote-condition/add-quote-condition.component';
import { QuoteProposalComponent } from './components/submission-details/sections/quote-options/quote-proposal/quote-proposal.component';
import { TableData } from './data/tables.data';
import { DocumentData } from '../documents/document.data';
import { BusinessDetailsData } from './data/applicant/business-details.data';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { ManuscriptsData } from './data/manuscripts/manuscripts.data';
import { SubmissionFormsModule } from './components/submission-details/sections/submission-forms/submission-forms.module';
import { BindValidationService } from '../../core/services/validations/bind-validation.service';
import { ApplicantAdditionalInterestValidationService } from '@app/core/services/validations/applicant-additionalinterest-validation.service';
import { SubmissionDetailsSharedModule } from './components/submission-details/submission-details-shared/submission-details-shared.module';
import { PaginationModule } from 'ngx-bootstrap';

@NgModule({
  declarations: [
    SubmissionSummaryComponent,
    SubmissionTableComponent,
    SubmissionListComponent,
    SubmissionDetailsComponent,
    SubmissionSearchFilterComponent,
    SubmissionTasksComponent,
    QuoteProposalComponent,
    AddUpdateBindingComponent,
    UploadDocumentComponent,
    AddQuoteConditionComponent,
  ],
  imports: [
    CommonModule,
    AccordionModule,
    // BrowserAnimationsModule,
    // NoopAnimationsModule,
    DataTableModule,

    FormsModule,
    ReactiveFormsModule,
    NgxDropzoneModule,


    SubmissionRoutingModule,
    AppSidebarModule,


    AppBreadcrumbModule.forRoot(),

    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    NgMultiSelectDropDownModule.forRoot(),
    ChartsModule,

    DocumentsModule,
    TasksModule,
    SharedModule,
    SubmissionFormsModule,

    RatingModule,
    SubmissionDetailsSharedModule,
    PaginationModule.forRoot(),
  ],
  entryComponents: [AddUpdateBindingComponent, UploadDocumentComponent, AddQuoteConditionComponent,
    QuoteProposalComponent
  ],
  exports: [
  ],
  providers: [
    ApplicantData,
    LimitsData,
    CoverageData,
    RiskSpecificsData,
    UnderwritingQuestionsData,
    DotInfoData,
    DriverData,
    DriverInfoData,
    DriverHiringCriteriaData,
    DestinationData,
    RadiusOfOperationsData,
    BrokerGenInfoData,
    QuoteLimitsData,
    VehicleData,
    RaterApiData,
    ClaimsHistoryoData,
    NotesData,
    FormsData,
    BindingData,
    ReportData,
    DocumentData,
    BusinessDetailsData,
    ManuscriptsData,

    SubmissionNavSavingService,
    SubmissionNavValidateService,
    ApplicantBusinessValidationService,
    ApplicantNameaddressValidationService,
    ApplicantFilingsValidationService,
    ApplicantAdditionalInterestValidationService,
    BrokerValidationService,
    LimitsValidationService,
    HistoricalCoverageValidationService,
    VehicleValidationService,
    DriverValidationService,
    DestinationInfoValidationService,
    DriverInfoValidationService,
    DotInfoValidationService,
    MaintenanceSafetyValidationService,
    GeneralLiabilityCargoValidationService,
    UnderwritingQuestionsValidationService,
    QuoteValidationService,
    ClaimsHistoryValidationService,
    BindValidationService,
    BsModalService,
    { provide: AccordionConfig, useValue: { closeOthers: true } },
    SubmissionListData,
    TableData,
    SubmissionSummaryComponent
  ]
})
export class SubmissionModule { }

