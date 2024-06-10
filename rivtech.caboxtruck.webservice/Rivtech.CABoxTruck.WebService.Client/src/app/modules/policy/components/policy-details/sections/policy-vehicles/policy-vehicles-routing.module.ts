import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PolicyVehiclesComponent } from './policy-vehicles.component';
import { CanDeactivateGuard } from '@app/core/guards/candeactivate.guard';


const routes: Routes = [
  {
    path: '',
    component: PolicyVehiclesComponent,
    // canActivate: [AuthGuard],
    canDeactivate: [CanDeactivateGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyVehiclesRoutingModule { }
