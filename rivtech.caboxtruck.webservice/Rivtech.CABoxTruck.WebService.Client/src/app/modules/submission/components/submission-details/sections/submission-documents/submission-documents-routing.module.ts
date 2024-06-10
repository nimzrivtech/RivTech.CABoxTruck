import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../../../../../../core/guards/auth.guard';
import { SubmissionDocumentsComponent } from './submission-documents.component';


const routes: Routes = [
  {
    path: '',
    component: SubmissionDocumentsComponent,
    canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SubmissionDocumentsRoutingModule { }
