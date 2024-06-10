import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { RiskSpecificsComponent } from './risk-specifics.component';
import { GeneralInformationComponent } from './general-information/general-information.component';
import { DriverInformationComponent } from './driver-information/driver-information.component';
import { MaintenanceSafetyComponent } from './maintenance-safety/maintenance-safety.component';
import { PathConstants } from '../../../../../../shared/constants/path.constants';
import { UnderwritingQuestionsComponent } from './underwriting-questions/underwriting-questions.component';
import { DestinationInformationComponent } from './destination-information/destination-information.component';
import { GeneralLiabilityCargoComponent } from './general-liability-cargo/general-liability-cargo.component';
import { DotInformationComponent } from './dot-information/dot-information.component';
import { CanDeactivateDestinationInformationComponentGuard } from './destination-information/destination-information-navigation-guard.service';
import { CanDeactivateDriverInformationComponentGuard } from './driver-information/driver-information-navigation-guard.service';
import { CanDeactivateDotInformationComponentGuard } from './dot-information/dot-information-navigation-guard.service';
import { CanDeactivateMaintenanceSafetyComponentGuard } from './maintenance-safety/maintenance-safety-navigation-guard.service';
import { CanDeactivateGeneralLiabilityCargoComponentGuard } from './general-liability-cargo/general-liability-cargo-navigation-guard.service';
import { CanDeactivateUnderwritingQuestionsComponentGuard } from './underwriting-questions/underwriting-questions-navigation-guard.service';

const routes: Routes = [
  {
    path: '',
    component: RiskSpecificsComponent,
    children: [
      {
        path: PathConstants.Submission.RiskSpecifics.DestinationInfo,
        component: DestinationInformationComponent,
        canDeactivate: [CanDeactivateDestinationInformationComponentGuard]
      },
      {
        path: PathConstants.Submission.RiskSpecifics.DriverInfo,
        component: DriverInformationComponent,
        canDeactivate: [CanDeactivateDriverInformationComponentGuard]
      },
      {
        path: PathConstants.Submission.RiskSpecifics.DotInfo,
        component: DotInformationComponent,
        canDeactivate: [CanDeactivateDotInformationComponentGuard]
      },
      {
        path: PathConstants.Submission.RiskSpecifics.MaintenanceSafety,
        component: MaintenanceSafetyComponent,
        canDeactivate: [CanDeactivateMaintenanceSafetyComponentGuard]
      },
      {
        path: PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo,
        component: GeneralLiabilityCargoComponent,
        canDeactivate: [CanDeactivateGeneralLiabilityCargoComponentGuard]
      },
      {
        path: PathConstants.Submission.RiskSpecifics.UWQuestions,
        component: UnderwritingQuestionsComponent,
        canDeactivate: [CanDeactivateUnderwritingQuestionsComponentGuard]
      },
      { path: '**', redirectTo: PathConstants.Submission.Applicant.Index, pathMatch: 'full' },
    ]
  },
  { path: '**', redirectTo: '/login', pathMatch: 'full' },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    CanDeactivateDestinationInformationComponentGuard,
    CanDeactivateDriverInformationComponentGuard,
    CanDeactivateDotInformationComponentGuard,
    CanDeactivateMaintenanceSafetyComponentGuard,
    CanDeactivateGeneralLiabilityCargoComponentGuard,
    CanDeactivateUnderwritingQuestionsComponentGuard
  ]
})
export class RiskSpecificsRoutingModule { }
