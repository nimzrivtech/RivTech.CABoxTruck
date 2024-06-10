import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { BrokerComponent } from './broker.component';
import { BrokerGenInfoData } from '../../../../data/broker/general-information.data';
import { CanDeactivateBrokerComponentGuard } from './broker-navigation-guard.service';
import { CanDeactivateGuard } from '@app/core/guards/candeactivate.guard';

const routes: Routes = [

  {
    path: '',
    component: BrokerComponent,
    canDeactivate: [CanDeactivateBrokerComponentGuard, CanDeactivateGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    CanDeactivateBrokerComponentGuard
  ]
})
export class BrokerRoutingModule { }
