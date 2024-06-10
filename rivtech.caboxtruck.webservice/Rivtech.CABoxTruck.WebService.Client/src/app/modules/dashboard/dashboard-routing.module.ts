import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { RoleGuard } from '../../core/guards/role.guard';
import { AuthGuard } from '../../core/guards/auth.guard';


const routes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    data: {
      controllerName: '/dashboard'
    },
    canActivate: [AuthGuard, RoleGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
