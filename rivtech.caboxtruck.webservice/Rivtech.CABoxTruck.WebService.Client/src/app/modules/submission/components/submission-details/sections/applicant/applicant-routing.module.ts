import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ApplicantComponent } from './applicant.component';
import { PathConstants } from '../../../../../../shared/constants/path.constants';
import { AdditionalInterestComponent } from './additional-interest/additional-interest.component';
import { FilingsInformationComponent } from './filings-information/filings-information.component';
import { GeneralInformationData } from '../../../../data/applicant/general-information.data';
import { NameAndAddressComponent } from './name-and-address/name-and-address.component';
import { BusinessDetailsComponent } from './business-details/business-details.component';
import { CanDeactivateBusinessDetailsComponentGuard } from './business-details/business-details-navigation-guard.service';
import { CanDeactivateNameAndAddressComponentGuard } from './name-and-address/name-and-address-navigation-guard.service';
import { CanDeactivateFilingsInformationComponentGuard } from './filings-information/filings-information-navigation-guard.service';
import { ManuscriptsComponent } from './manuscripts/manuscripts.component';
import { CanDeactivateGuard } from '@app/core/guards/candeactivate.guard';

const routes: Routes = [
  {
    path: '',
    component: ApplicantComponent,
    children: [
      {
        path: PathConstants.Submission.Applicant.NameAndAddress,
        component: NameAndAddressComponent,
        // canDeactivate: [CanDeactivateNameAndAddressComponentGuard]
        canDeactivate: [CanDeactivateGuard]
      },
      {
        path: PathConstants.Submission.Applicant.BusinessDetails,
        component: BusinessDetailsComponent,
        canDeactivate: [CanDeactivateBusinessDetailsComponentGuard]
      },
      {
        path: PathConstants.Submission.Applicant.FilingsInformation,
        component: FilingsInformationComponent,
        canDeactivate: [CanDeactivateFilingsInformationComponentGuard]
      },
      {
        path: PathConstants.Submission.Applicant.AdditionalInterest,
        component: AdditionalInterestComponent
      },
      {
        path: PathConstants.Submission.Applicant.Manuscripts,
        component: ManuscriptsComponent
      },
      { path: '', redirectTo: PathConstants.Submission.Applicant.NameAndAddress, pathMatch: 'full'},
      { path: '**', redirectTo: '/404', pathMatch: 'full'},
    ]
  },

  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    GeneralInformationData,
    CanDeactivateNameAndAddressComponentGuard,
    CanDeactivateBusinessDetailsComponentGuard,
    CanDeactivateFilingsInformationComponentGuard
  ]
})
export class ApplicantRoutingModule { }
