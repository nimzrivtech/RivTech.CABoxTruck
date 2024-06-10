import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PolicyClaimsHistoryComponent } from './policy-claims-history.component';


const routes: Routes = [
  {
    path: '',
    component: PolicyClaimsHistoryComponent,
    // canDeactivate: [CanDeactivateClaimsHistoryComponentGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyClaimsHistoryRoutingModule { }
