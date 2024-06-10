import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppAsideModule } from '@coreui/angular';
import { ModalModule } from 'ngx-bootstrap';
import { PathConstants } from '../../shared/constants/path.constants';
import { ManagementComponent } from './management.component';

const routes: Routes = [
  {
    path: '',
    component: ManagementComponent,
    children: [
      {
        path: PathConstants.Management.Agency.Index,
        loadChildren: () => import('./agency/agency.module').then(m => m.AgencyModule)
      },
      {
        path: PathConstants.Management.Retailer.Index,
        loadChildren: () => import('./retailer/retailer.module').then(m => m.RetailerModule)
      },
      {
        path: PathConstants.Management.Users.Index,
        loadChildren: () => import('./users/users.module').then(m => m.UsersModule)
      },
      {
        path: PathConstants.Management.Roles.Index,
        loadChildren: () => import('./role/role.module').then(m => m.RoleModule)
      },
      { path: '', redirectTo: PathConstants.Management.Agency.Index, pathMatch: 'full' },
      { path: '', redirectTo: '/404', pathMatch: 'full' },   // temporarily redirecting to suspended payments
    ]
  },
  {
    path: '',
    redirectTo: PathConstants.Management.Index,
    pathMatch: 'full'
  },
  // { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes), AppAsideModule, ModalModule.forRoot()],
  exports: [RouterModule]
})
export class ManagementRoutingModule { }
