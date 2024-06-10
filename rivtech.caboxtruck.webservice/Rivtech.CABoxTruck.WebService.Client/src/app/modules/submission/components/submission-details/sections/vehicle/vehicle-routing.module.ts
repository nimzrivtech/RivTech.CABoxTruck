import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CanDeactivateVehicleComponentGuard } from './vehicle-navigation-guard.service';
import { VehicleComponent } from './vehicle.component';

const routes: Routes = [
  {
    path: '',
    component: VehicleComponent,
    canDeactivate: [CanDeactivateVehicleComponentGuard]
  },
  { path: '**', redirectTo: '/login', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    CanDeactivateVehicleComponentGuard
  ]
})
export class VehicleRoutingModule { }
