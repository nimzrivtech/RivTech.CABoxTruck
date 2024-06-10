import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PolicyNotesComponent } from './policy-notes.component';


const routes: Routes = [
  {
    path: '',
    component: PolicyNotesComponent,
    // canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyNotesRoutingModule { }
