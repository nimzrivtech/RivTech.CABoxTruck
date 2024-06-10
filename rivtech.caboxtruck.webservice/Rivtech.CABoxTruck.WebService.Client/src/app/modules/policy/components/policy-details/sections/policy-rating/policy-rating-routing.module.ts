import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CanDeactivateGuard } from '@app/core/guards/candeactivate.guard';
import { PolicyRatingComponent } from './policy-rating.component';


const routes: Routes = [
  {
    path: '',
    component: PolicyRatingComponent,
    canDeactivate: [CanDeactivateGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyRatingRoutingModule { }
