import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { SubmissionFilingComponent } from './submission-filing.component';

const routes: Routes = [

  {
    path: '',
    component: SubmissionFilingComponent,
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SubmissionFilingRoutingModule { }
