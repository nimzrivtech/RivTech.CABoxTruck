import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClaimsHistoryComponent } from './claims-history.component';
import { RouterModule, Routes } from '@angular/router';
import { CanDeactivateClaimsHistoryComponentGuard } from './claims-history-navigation-guard.service';

const routes: Routes = [
  {
    path: '',
    component: ClaimsHistoryComponent,
    canDeactivate: [CanDeactivateClaimsHistoryComponentGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    CanDeactivateClaimsHistoryComponentGuard
  ]
})
export class ClaimsHistoryRoutingModule { }
