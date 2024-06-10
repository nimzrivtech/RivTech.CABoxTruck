import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PathConstants } from '../../../shared/constants/path.constants';
import { AgencyDetailsComponent } from './agency-details/agency-details.component';
import { AgencyComponent } from './agency.component';


const routes: Routes = [
  {
    path: '',
    component: AgencyComponent
  },
  {
    path: PathConstants.Management.Agency.New,
    component: AgencyDetailsComponent
  },
  {
    path: `${PathConstants.Management.Agency.Edit}/:agencyid`,
    component: AgencyDetailsComponent
  }
  // { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AgencyRoutingModule { }
