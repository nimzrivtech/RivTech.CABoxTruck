import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PolicyIssuanceComponent } from './policy-issuance.component';


const routes: Routes = [
  {
    path: '',
    component: PolicyIssuanceComponent,
    // canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyIssuanceRoutingModule { }
