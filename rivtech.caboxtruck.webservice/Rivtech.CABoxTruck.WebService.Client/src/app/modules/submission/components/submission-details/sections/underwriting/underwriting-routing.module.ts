import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { UnderWritingComponent } from './underwriting.component';
import { BrokerGenInfoData } from '../../../../data/broker/general-information.data';

const routes: Routes = [

  {
    path: '',
    component: UnderWritingComponent,
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [BrokerGenInfoData]
})
export class UnderWritingRoutingModule { }
