import { Injectable, NgModule } from '@angular/core';
import { Routes, RouterModule, CanDeactivate, UrlTree, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AppAsideModule } from '@coreui/angular';
import { PathConstants } from '../../shared/constants/path.constants';
import { PolicyDetailsComponent } from './pages/policy-details/policy-details.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PolicyListComponent } from './pages/policy-list/policy-list.component';
import { PolicyResolverService } from './resolver/policy-resolver.service';
import { Observable, of } from 'rxjs';

const policyDetailsChildRoutes =  [
  {
    path: PathConstants.Policy.Summary,
    loadChildren: () => import('./components/policy-details/sections/policy-summary-page/policy-summary-page.module').then(m => m.PolicySummaryPageModule)
  },
  {
    path: PathConstants.Policy.History,
    loadChildren: () => import('./components/policy-details/sections/policy-history/policy-history.module').then(m => m.PolicyHistoryModule)
  },
  {
    path: PathConstants.Policy.Billing,
    loadChildren: () => import('./components/policy-details/sections/policy-billing/policy-billing.module').then(m => m.PolicyBillingModule)
  },
  {
    path: PathConstants.Submission.Applicant.Index,
    loadChildren: () => import('./components/policy-details/sections/policy-applicant/policy-applicant.module').then(m => m.PolicyApplicantModule)
  },
  {
    path: PathConstants.Submission.Broker.Index,
    loadChildren: () => import('./components/policy-details/sections/policy-broker/policy-broker.module').then(m => m.PolicyBrokerModule)
  },
  {
    path: PathConstants.Policy.AdditionalInterest,
    loadChildren: () => import('./components/policy-details/sections/policy-interest/policy-interest.module').then(m => m.PolicyInterestModule)
  },
  {
    path: PathConstants.Policy.Manuscript,
    loadChildren: () => import('./components/policy-details/sections/policy-manuscript/policy-manuscript.module').then(m => m.PolicyManuscriptModule)
  },
  {
    path: PathConstants.Submission.Limits.Index,
    loadChildren: () => import('./components/policy-details/sections/policy-limits/policy-limits.module').then(m => m.PolicyLimitsModule),
  },
  {
    path: PathConstants.Submission.Vehicle.Index,
    loadChildren: () => import('./components/policy-details/sections/policy-vehicles/policy-vehicles.module').then(m => m.PolicyVehiclesModule)
  },
  {
    path: PathConstants.Submission.Driver.Index,
    loadChildren: () => import('./components/policy-details/sections/policy-drivers/policy-drivers.module').then(m => m.PolicyDriversModule)
  },
  {
    path: PathConstants.Submission.Coverages.Index,
    loadChildren: () => import('./components/policy-details/sections/policy-coverages/policy-coverages.module').then(m => m.PolicyCoveragesModule),
  },
  {
    path: PathConstants.Submission.Claims.Index,
    loadChildren: () => import('./components/policy-details/sections/policy-claims-history/policy-claims-history.module').then(m => m.PolicyClaimsHistoryModule)
  },
  {
    path: PathConstants.Submission.RiskSpecifics.Index,
    loadChildren: () => import('./components/policy-details/sections/policy-risk-specifics/policy-risk-specifics.module').then(m => m.PolicyRiskSpecificsModule)
  },
  {
    path: PathConstants.Submission.Bind.Index,
    loadChildren: () => import('./components/policy-details/sections/policy-bind-request/policy-bind-request.module').then(m => m.PolicyBindRequestModule)
  },
  {
    path: PathConstants.Submission.FormsSelection.Index,
    loadChildren: () => import('./components/policy-details/sections/policy-forms/policy-forms.module').then(m => m.PolicyFormsModule)
  },
  {
    path: PathConstants.Policy.Rating,
    loadChildren: () => import('./components/policy-details/sections/policy-rating/policy-rating.module').then(m => m.PolicyRatingModule)
  },
  {
    path: PathConstants.Policy.Premium,
    loadChildren: () => import('./components/policy-details/sections/policy-premiums/policy-premiums.module').then(m => m.PolicyPremiumsModule)
  },
  {
    path: PathConstants.Policy.Issuance,
    loadChildren: () => import('./components/policy-details/sections/policy-issuance/policy-issuance.module').then(m => m.PolicyIssuanceModule)
  },
  {
    path: PathConstants.Policy.Documents,
    loadChildren: () => import('./components/policy-details/sections/policy-documents/policy-documents.module').then(m => m.PolicyDocumentsModule)
  },
  {
    path: PathConstants.Policy.Notes,
    loadChildren: () => import('./components/policy-details/sections/policy-notes/policy-notes.module').then(m => m.PolicyNotesModule)
  },
  {
    path: PathConstants.Policy.Tasks,
    loadChildren: () => import('./components/policy-details/sections/policy-tasks/policy-tasks.module').then(m => m.PolicyTasksModule)
  },

  { path: '', redirectTo: PathConstants.Policy.Summary, pathMatch: 'full'},
];

const routes: Routes = [
{
    path: 'list',
    component: PolicyListComponent
  },
  {
    path: ':uuid/:uuid2' ,
    component: PolicyDetailsComponent,
    children: policyDetailsChildRoutes,
    resolve: { data: PolicyResolverService }
  },

  { path: '', redirectTo: PathConstants.Submission.List, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes), AppAsideModule, ModalModule.forRoot()],
  exports: [RouterModule]
})
export class PolicyRoutingModule { }
