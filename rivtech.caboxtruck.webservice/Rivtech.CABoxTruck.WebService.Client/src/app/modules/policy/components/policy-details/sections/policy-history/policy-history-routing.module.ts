import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HistorySectionComponent } from './history-section/history-section.component';


const routes: Routes = [
  {
    path: '',
    component: HistorySectionComponent,
    // canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyHistoryRoutingModule { }
