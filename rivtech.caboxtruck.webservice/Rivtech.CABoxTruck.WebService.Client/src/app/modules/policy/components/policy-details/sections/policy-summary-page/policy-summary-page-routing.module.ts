import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PolicySummaryPageComponent } from './policy-summary-page.component';


const routes: Routes = [
  {
    path: '',
    component: PolicySummaryPageComponent,
    // canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicySummaryPageRoutingModule { }
