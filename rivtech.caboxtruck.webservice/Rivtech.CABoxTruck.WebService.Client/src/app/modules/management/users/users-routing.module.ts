import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PathConstants } from '../../../shared/constants/path.constants';
import { SystemUserComponent } from './system-user/system-user.component';
import { UserGroupComponent } from './user-group/user-group.component';
import { UserTypeComponent } from './user-type/user-type.component';


const routes: Routes = [
  {
    path: PathConstants.Management.Users.User,
    component: SystemUserComponent,
    //Temporarily Comment Out for future use
    // children: [
    //   {
    //     path: PathConstants.Management.Users.User,
    //     component: SystemUserComponent,
    //   },
    //   {
    //     path: PathConstants.Management.Users.Agent,
    //     component: AgentListComponent,
    //   },
    //   {
    //     path: PathConstants.Management.Users.UserType,
    //     component: UserTypeComponent,
    //   },
    //   {
    //     path: PathConstants.Management.Users.UserGroup,
    //     component: UserGroupComponent,
    //   },
    //   { path: '', redirectTo: PathConstants.Management.Users.User, pathMatch: 'full'},
    // ]
  },
  {
    path: PathConstants.Management.Users.UserType,
    component: UserTypeComponent,
  },
  {
    path: PathConstants.Management.Users.UserGroup,
    component: UserGroupComponent,
  },
  { path: '', redirectTo: PathConstants.Management.Users.User, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule { }
