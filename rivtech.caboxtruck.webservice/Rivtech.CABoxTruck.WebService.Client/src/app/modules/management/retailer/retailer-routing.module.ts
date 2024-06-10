import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PathConstants } from '../../../shared/constants/path.constants';
import { RetailerDetailsComponent } from './retailer-details/retailer-details.component';
//import { AgencyDetailsComponent } from './agency-details/agency-details.component';
import { RetailerComponent } from './retailer.component';


const routes: Routes = [
  {
    path: '',
    component: RetailerComponent
  },
  {
    path: PathConstants.Management.Agency.New,
    component: RetailerDetailsComponent
  },
  {
    path: `${PathConstants.Management.Agency.Edit}/:agencyid`,
    component: RetailerDetailsComponent
  }
  // { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RetailerRoutingModule { }
