import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ManuscriptsComponent } from './manuscripts/manuscripts.component';


const routes: Routes = [
  {
    path: '',
    component: ManuscriptsComponent,
    // canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyManuscriptRoutingModule { }
