import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../../../../../../core/guards/auth.guard';
import { CanDeactivateLimitsPageComponentGuard } from './limits-page/limits-navigation-guard.service';
import { LimitsPageComponent } from './limits-page/limits-page.component';

const routes: Routes = [

  {
    path: '',
    component: LimitsPageComponent,
    canActivate: [AuthGuard],
    canDeactivate: [CanDeactivateLimitsPageComponentGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    CanDeactivateLimitsPageComponentGuard
  ]
})
export class LimitsRoutingModule { }
