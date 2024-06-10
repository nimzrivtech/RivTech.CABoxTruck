import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BindComponent } from './bind.component';
import { CanDeactivateGuard } from '@app/core/guards/candeactivate.guard';

const routes: Routes = [

  {
    path: '',
    component: BindComponent,
    canDeactivate: [CanDeactivateGuard]
    
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  //providers: [CanDeactivateBindComponentGuard]
})
export class BindRoutingModule { }
