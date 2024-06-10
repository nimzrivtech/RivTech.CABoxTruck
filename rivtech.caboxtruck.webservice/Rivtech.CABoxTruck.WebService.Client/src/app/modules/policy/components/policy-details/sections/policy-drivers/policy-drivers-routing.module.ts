import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CanDeactivateGuard } from '@app/core/guards/candeactivate.guard';
import { PolicyDriversComponent } from './policy-drivers.component';


const routes: Routes = [
  {
    path: '',
    component: PolicyDriversComponent,
    canDeactivate: [CanDeactivateGuard]
    // canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyDriversRoutingModule { }
