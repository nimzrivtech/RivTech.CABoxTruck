import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HistoricalCoverageComponent } from './historical-coverage/historical-coverage.component';
import { AuthGuard } from '../../../../../../core/guards/auth.guard';
import { CanDeactivateHistoricalCoverageComponentGuard } from './historical-coverage/historical-coverage-navigation-guard.service';

const routes: Routes = [

  {
    path: '',
    component: HistoricalCoverageComponent,
    canActivate: [AuthGuard],
    canDeactivate: [CanDeactivateHistoricalCoverageComponentGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    CanDeactivateHistoricalCoverageComponentGuard
  ]
})
export class CoveragesRoutingModule { }
