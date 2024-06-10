import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PolicyBrokerComponent } from './policy-broker.component';


const routes: Routes = [
  {
    path: '',
    component: PolicyBrokerComponent,
    // canDeactivate: [CanDeactivateBrokerComponentGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyBrokerRoutingModule { }
