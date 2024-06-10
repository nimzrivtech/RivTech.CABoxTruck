import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HistoricalCoverageComponent } from './historical-coverage/historical-coverage.component';


const routes: Routes = [
  {
    path: '',
    component: HistoricalCoverageComponent,
    // canActivate: [AuthGuard],
    // canDeactivate: [CanDeactivateHistoricalCoverageComponentGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyCoveragesRoutingModule { }
