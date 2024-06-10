import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PolicyBindRequestComponent } from './policy-bind-request.component';
import { CanDeactivateGuard } from '@app/core/guards/candeactivate.guard';

const routes: Routes = [
  {
    path: '',
    component: PolicyBindRequestComponent,
    canDeactivate: [CanDeactivateGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyBindRequestRoutingModule { }
