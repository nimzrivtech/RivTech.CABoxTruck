import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdditionalInterestComponent } from './additional-interest/additional-interest.component';


const routes: Routes = [
  {
    path: '',
    component: AdditionalInterestComponent,
    // canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyInterestRoutingModule { }
