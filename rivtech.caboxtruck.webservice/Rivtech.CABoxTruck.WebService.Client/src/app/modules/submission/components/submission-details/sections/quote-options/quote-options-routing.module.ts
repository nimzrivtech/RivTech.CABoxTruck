import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../../../../../../core/guards/auth.guard';
import { CanDeactivateQuotePageComponentGuard } from './quote-page/quote-page-navigation-guard.service';
import { QuotePageComponent } from './quote-page/quote-page.component';

const routes: Routes = [

  {
    path: '',
    component: QuotePageComponent,
    canActivate: [AuthGuard],
    canDeactivate: [CanDeactivateQuotePageComponentGuard]
  },
  { path: '**', redirectTo: '/404', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    CanDeactivateQuotePageComponentGuard
  ]
})
export class QuotesRoutingModule { }
