import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PathConstants } from '../../../shared/constants/path.constants';
import { UserRoleComponent } from './user-role/user-role.component';


const routes: Routes = [
  {
    path: '',
    component: UserRoleComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RoleRoutingModule { }
