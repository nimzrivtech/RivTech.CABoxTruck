import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CanDeactivateGuard } from '@app/core/guards/candeactivate.guard';
import { PathConstants } from '@app/shared/constants/path.constants';
import { BusinessDetailsComponent } from './business-details/business-details.component';
import { FilingsInformationComponent } from './filings-information/filings-information.component';
import { NameAndAddressComponent } from './name-and-address/name-and-address.component';
import { PolicyApplicantComponent } from './policy-applicant.component';


const routes: Routes = [
  {
    path: '',
    component: PolicyApplicantComponent,
    canDeactivate: [CanDeactivateGuard],
    children: [
      {
        path: PathConstants.Submission.Applicant.NameAndAddress,
        component: NameAndAddressComponent,
        canDeactivate: [CanDeactivateGuard]
      },
      {
        path: PathConstants.Submission.Applicant.BusinessDetails,
        component: BusinessDetailsComponent,
        // canDeactivate: [CanDeactivateBusinessDetailsComponentGuard]
      },
      {
        path: PathConstants.Submission.Applicant.FilingsInformation,
        component: FilingsInformationComponent,
        // canDeactivate: [CanDeactivateFilingsInformationComponentGuard]
      },
      { path: '', redirectTo: PathConstants.Submission.Applicant.NameAndAddress, pathMatch: 'full'},
      { path: '**', redirectTo: '/404', pathMatch: 'full'},
    ]
  },

  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyApplicantRoutingModule { }
