import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DriverComponent } from './driver.component';
import { DDriverInformationComponent } from './d-driver-information/d-driver-information.component';
import { PathConstants } from '../../../../../../shared/constants/path.constants';
import { CanDeactivateDriverComponentGuard } from './driver-navigation-guard.service';

const routes: Routes = [
  {
    path: '',
    component: DriverComponent,
    canDeactivate: [CanDeactivateDriverComponentGuard]
  },
  { path: '**', redirectTo: '/login', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [CanDeactivateDriverComponentGuard]
})
export class DriverRoutingModule { }
