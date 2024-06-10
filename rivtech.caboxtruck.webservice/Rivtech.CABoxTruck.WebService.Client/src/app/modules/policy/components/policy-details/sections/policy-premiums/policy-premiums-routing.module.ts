import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../../../../../../core/guards/auth.guard';
import { PolicyPremiumsPageComponent } from './premiums-page/policy-premiums-page.component';

const routes: Routes = [

  {
    path: '',
    component: PolicyPremiumsPageComponent,
    canActivate: [AuthGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
  ]
})
export class PolicyPremiumsRoutingModule { }
