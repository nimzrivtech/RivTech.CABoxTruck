import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormsPageComponent } from './forms-page/forms-page.component';

const routes: Routes = [
  {
    path: '',
    component: FormsPageComponent,
    // canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PolicyFormsRoutingModule { }
