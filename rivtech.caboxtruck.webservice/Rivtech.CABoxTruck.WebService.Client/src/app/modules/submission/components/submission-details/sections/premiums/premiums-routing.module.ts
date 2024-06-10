import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../../../../../../core/guards/auth.guard';
import { PremiumsPageComponent } from './premiums-page/premiums-page.component';

const routes: Routes = [

  {
    path: '',
    component: PremiumsPageComponent,
    canActivate: [AuthGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
  ]
})
export class PremiumsRoutingModule { }
