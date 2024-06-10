import { NgModule } from '@angular/core';
import {Routes, RouterModule, PreloadAllModules} from '@angular/router';

import { PathConstants } from './shared/constants/path.constants';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';

import { UserLayoutComponent } from './layout/user-layout/user-layout.component';
import { MainSidebarLayoutComponent } from './layout/main-sidebar-layout/main-sidebar-layout.component';
import { AuthGuard } from './core/guards/auth.guard';
import { P404Component } from './error/404.component';
import { AuthComponent } from './auth/auth.component';
import {LoginComponent} from './login/login.component';
import { PaymentPortalComponent } from './payment-portal/pages/payment-portal/payment-portal.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: PathConstants.Auth.Index,
    component: AuthComponent,
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }
  },
  {
    path: PathConstants.PaymentManagement.Index,
    component: PaymentPortalComponent,
    data: {
      title: 'Payment Portal Page'
    }
  },
  {
    path: '',
    component: MainLayoutComponent,

    data: {
      title: 'Home'
    },
    children: [
      {
        path: PathConstants.Dashboard.Index,
        loadChildren: () => import('./modules/dashboard/dashboard.module').then(m => m.DashboardModule),
        canActivate: [AuthGuard]
      }
    ],
  },
  {
    path: '',
    component: MainSidebarLayoutComponent,
    data: {
      title: 'Home'
    },
    canActivate: [AuthGuard],
    children: [
      {
        path: PathConstants.Policy.Index,
        loadChildren: () => import('./modules/policy/policy.module').then(m => m.PolicyModule)
      },
      {
        path: PathConstants.Submission.Index,
        loadChildren: () => import('./modules/submission/submission.module').then(m => m.SubmissionModule)
      },
    ]
  },

  {
    path: '',
    component: MainSidebarLayoutComponent,
    data: {
      title: 'Home'
    },
    //canActivate: [AuthGuard, ManagementGuard]
    canActivate: [AuthGuard],
    children: [
      {
        path: PathConstants.Management.Index,
        loadChildren: () => import('./modules/management/management.module').then(m => m.ManagementModule)
      },
    ]
  },

  {
    path: '',
    component: UserLayoutComponent,
    children: [
      {
        path: PathConstants.Account.Index,
        loadChildren: () => import('./modules/account/account.module').then(m => m.AccountModule)
      }
    ]
  },

  { path: '', component: AuthComponent, pathMatch: 'full' },
  { path: '**', component: P404Component, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
