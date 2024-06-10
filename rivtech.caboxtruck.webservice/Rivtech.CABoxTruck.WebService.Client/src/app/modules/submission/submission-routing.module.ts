import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppAsideModule } from '@coreui/angular';
import { PathConstants } from '../../shared/constants/path.constants';
import { ModalModule } from 'ngx-bootstrap/modal';
import { SubmissionListComponent } from './pages/submission-list/submission-list.component';
import { SubmissionDetailsComponent } from './pages/submission-details/submission-details.component';

import { BrokerData } from './data/broker.data';
import { BrokerFieldConfig } from './fields/broker.field-config';
import { RiskSpecificsFieldConfig } from './fields/risk-specifics/risk-specifics.field-config';
import { RiskSpecificsGeneralInfoFieldConfig } from './fields/risk-specifics/general-information.field-config';
import { DriverInfoFieldConfig } from './fields/risk-specifics/driver-information.field-config';
import { DriverHiringCriteriaFieldConfig } from './fields/risk-specifics/driver-hiring-criteria.field-config';
import { MaintenanceQuestionFieldConfig } from './fields/risk-specifics/maintenance-question.field-config';
import { CoverageQuestionFieldConfig } from './fields/risk-specifics/coverage-question.field-config';
import { UnderwritingQuestionsFieldConfig } from './fields/risk-specifics/underwriting-questions.field-config';
import { CoverageQuestionsFieldConfig } from './fields/coverage/coverage-questions.field-config';
import { GeneralLiabilityFieldConfig } from './fields/coverage/general-liability.field-config';
import { CargoQuestionsFieldConfig } from './fields/coverage/cargo-questions.field-config';
import { AuthGuard } from '../../core/guards/auth.guard';
import { SubmissionResolverService } from './resolver/submission-resolver.service';

const submissionDetailsChildRoutes = [
  {
    path: PathConstants.Submission.Applicant.Index,
    loadChildren: () => import('./components/submission-details/sections/applicant/applicant.module').then(m => m.ApplicantModule),
  },
  {
    path: PathConstants.Submission.Broker.Index,
    loadChildren: () => import('./components/submission-details/sections/broker/broker.module').then(m => m.BrokerModule),
  },
  {
    path: PathConstants.Submission.Limits.Index,
    loadChildren: () => import('./components/submission-details/sections/limits/limits.module').then(m => m.LimitsModule),
    canActivate: [AuthGuard],
  },
  {
    path: PathConstants.Submission.Coverages.Index,
    loadChildren: () => import('./components/submission-details/sections/coverages/coverages.module').then(m => m.CoveragesModule),
    canActivate: [AuthGuard],
  },
  {
    path: PathConstants.Submission.Claims.Index,
    loadChildren: () => import('./components/submission-details/sections/claims-history/claims-history.module').then(m => m.ClaimsHistoryModule)
  },
  {
    path: PathConstants.Submission.RiskSpecifics.Index,
    loadChildren: () => import('./components/submission-details/sections/risk-specifics/risk-specifics.module').then(m => m.RiskSpecificsModule)
  },
  {
    path: PathConstants.Submission.Vehicle.Index,
    loadChildren: () => import('./components/submission-details/sections/vehicle/vehicle.module').then(m => m.VehicleModule)
  },
  {
    path: PathConstants.Submission.Driver.Index,
    loadChildren: () => import('./components/submission-details/sections/driver/driver.module').then(m => m.DriverModule)
  },
  {
    path: PathConstants.Submission.Quote.Index,
    loadChildren: () => import('./components/submission-details/sections/quote-options/quote-options.module').then(m => m.QuoteOptionsModule)
  },
  {
    path: PathConstants.Submission.Premiums.Index,
    loadChildren: () => import('./components/submission-details/sections/premiums/premiums.module').then(m => m.PremiumsModule)
  },
  {
    path: PathConstants.Submission.Bind.Index,
    loadChildren: () => import('./components/submission-details/sections/bind/bind.module').then(m => m.BindModule)
  },
  {
    path: PathConstants.Submission.FormsSelection.Index,
    loadChildren: () => import('./components/submission-details/sections/submission-forms/submission-forms.module').then(m => m.SubmissionFormsModule)
  },
  {
    path: PathConstants.Submission.Notes.Index,
    loadChildren: () => import('./components/submission-details/sections/submission-notes/submission-notes.module').then(m => m.SubmissionNotesModule)
  },
  {
    path: PathConstants.Submission.Documents,
    loadChildren: () => import('./components/submission-details/sections/submission-documents/submission-documents.module').then(m => m.SubmissionDocumentsModule)
  },
  { path: '', redirectTo: PathConstants.Submission.Applicant.NameAndAddress, pathMatch: 'full' },
  // { path: '**', redirectTo: '/404', pathMatch: 'full'},
];

const routes: Routes = [
  {
    path: PathConstants.Submission.List,
    component: SubmissionListComponent,
  },
  {
    path: ':id',
    component: SubmissionDetailsComponent,
    children: submissionDetailsChildRoutes,
    resolve: { data: SubmissionResolverService }
  },

  { path: '', redirectTo: PathConstants.Submission.List, pathMatch: 'full' },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes), AppAsideModule, ModalModule.forRoot()],
  exports: [RouterModule],
  providers: [
    BrokerData,
    BrokerFieldConfig,
    RiskSpecificsFieldConfig,
    RiskSpecificsGeneralInfoFieldConfig,
    DriverInfoFieldConfig,
    DriverHiringCriteriaFieldConfig,
    MaintenanceQuestionFieldConfig,
    CoverageQuestionFieldConfig,
    UnderwritingQuestionsFieldConfig,
    CoverageQuestionsFieldConfig,
    GeneralLiabilityFieldConfig,
    CargoQuestionsFieldConfig
  ]
})
export class SubmissionRoutingModule { }
