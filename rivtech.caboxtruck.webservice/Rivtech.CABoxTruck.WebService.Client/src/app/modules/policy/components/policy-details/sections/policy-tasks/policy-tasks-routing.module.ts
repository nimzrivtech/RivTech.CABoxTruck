import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PolicyTasksComponent } from './policy-tasks.component';


const routes: Routes = [
  {
    path: '',
    component: PolicyTasksComponent,
    // canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyTasksRoutingModule { }
