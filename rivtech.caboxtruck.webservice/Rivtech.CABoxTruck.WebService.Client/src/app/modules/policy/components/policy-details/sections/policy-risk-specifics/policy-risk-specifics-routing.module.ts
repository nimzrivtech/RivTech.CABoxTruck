import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PathConstants } from '@app/shared/constants/path.constants';
import { DestinationInformationComponent } from './destination-information/destination-information.component';
import { DotInformationComponent } from './dot-information/dot-information.component';
import { DriverInformationComponent } from './driver-information/driver-information.component';
import { GeneralLiabilityCargoComponent } from './general-liability-cargo/general-liability-cargo.component';
import { MaintenanceSafetyComponent } from './maintenance-safety/maintenance-safety.component';
import { PolicyRiskSpecificsComponent } from './policy-risk-specifics.component';
import { UnderwritingQuestionsComponent } from './underwriting-questions/underwriting-questions.component';


const routes: Routes = [
  {
    path: '',
    component: PolicyRiskSpecificsComponent,
    children: [
      {
        path: PathConstants.Submission.RiskSpecifics.DestinationInfo,
        component: DestinationInformationComponent,
      },
      {
        path: PathConstants.Submission.RiskSpecifics.DriverInfo,
        component: DriverInformationComponent,
      },
      {
        path: PathConstants.Submission.RiskSpecifics.DotInfo,
        component: DotInformationComponent,
      },
      {
        path: PathConstants.Submission.RiskSpecifics.MaintenanceSafety,
        component: MaintenanceSafetyComponent,
      },
      {
        path: PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo,
        component: GeneralLiabilityCargoComponent,
      },
      {
        path: PathConstants.Submission.RiskSpecifics.UWQuestions,
        component: UnderwritingQuestionsComponent,
      },
      { path: '**', redirectTo: PathConstants.Submission.Applicant.Index, pathMatch: 'full' },
    ]
  },
  { path: '**', redirectTo: '/login', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyRiskSpecificsRoutingModule { }
