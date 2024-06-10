import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';



const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

import { AppComponent } from './app.component';
import { FooterComponent } from './layout/components/footer/footer.component';
import { RightSidebarComponent } from './layout/components/right-sidebar/right-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { LoginComponent } from './login/login.component';
import { PaymentPortalComponent } from './payment-portal/pages/payment-portal/payment-portal.component';

import {

  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule,
  AppAsideModule,
  AppBreadcrumbModule,
} from '@coreui/angular';

// Import routing module
import { AppRoutingModule } from './app.routing';

// Import 3rd party components
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts';
import { HeaderComponent } from './layout/components/header/header.component';
import { LeftSidebarComponent } from './layout/components/left-sidebar/left-sidebar.component';
import { MainSidebarLayoutComponent } from './layout/main-sidebar-layout/main-sidebar-layout.component';

import { UserLayoutComponent } from './layout/user-layout/user-layout.component';

import { CoreModule } from './core/core.module';
import { StorageService } from './core/services/storage.service';
import { P404Component } from './error/404.component';

import { ToastrModule } from 'ngx-toastr';
import { AuthComponent } from './auth/auth.component';
import { SharedModule } from './shared/shared.module';
import { PagerService } from './core/services/pager.service';
import {BlacklistedDriverModule} from './modules/blacklisted-driver/blacklisted-driver.module';

import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { MatSortModule } from '@angular/material/sort';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatChipsModule } from '@angular/material/chips';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTableModule } from '@angular/material/table';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatRadioModule } from '@angular/material/radio';
import { PaymentCreditcardComponent } from 'app/payment-portal/components/payment-creditcard/payment-creditcard.component';
import { PaymentEftComponent } from 'app/payment-portal/components/payment-eft/payment-eft.component';
import { ConfirmationDialogueComponent } from 'app/payment-portal/components/confirmation-dialogue/confirmation-dialogue.component';
import { DashboardModule } from './modules/dashboard/dashboard.module';
import { CanDeactivateGuard } from './core/guards/candeactivate.guard';

import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';
import { MomentModule } from 'angular2-moment';
import { SessionTimeoutComponent } from './login/session-timeout/session-timeout.component';

import { PaginationModule } from 'ngx-bootstrap';

import { IssueEndorsementModalComponent } from '@app/modules/policy/components/policy-details/sections/policy-issuance/issue-endorsement-modal/issue-endorsement-modal.component';
import { ResetEndorsementModalComponent } from './modules/policy/components/policy-details/sections/policy-issuance/reset-process-modal/reset-process-modal.component';
@NgModule({
  imports: [

    CoreModule,

    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),

    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,

    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ChartsModule,
    ToastrModule.forRoot(),
    SharedModule,

    MatCardModule,
    MatButtonModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatIconModule,
    MatDividerModule,
    MatListModule,
    MatGridListModule,
    MatChipsModule,
    MatSlideToggleModule,
    MatTableModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatCheckboxModule,
    MatSidenavModule,
    MatToolbarModule,
    MatProgressSpinnerModule,
    MatPaginatorModule,
    MatSortModule,
    MatTooltipModule,
    MatDialogModule,
    MatRadioModule,
    BlacklistedDriverModule,
    DashboardModule,

    NgIdleKeepaliveModule.forRoot(),
    MomentModule,
    PaginationModule.forRoot()
  ],
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    LoginComponent,
    LeftSidebarComponent,
    MainLayoutComponent,
    RightSidebarComponent,
    MainSidebarLayoutComponent,
    UserLayoutComponent,
    P404Component,
    AuthComponent,
    PaymentPortalComponent,
    PaymentCreditcardComponent,
    PaymentEftComponent,
    ConfirmationDialogueComponent,
    SessionTimeoutComponent,
    IssueEndorsementModalComponent,
    ResetEndorsementModalComponent
  ],
  entryComponents: [ConfirmationDialogueComponent, SessionTimeoutComponent, IssueEndorsementModalComponent, ResetEndorsementModalComponent],
  providers: [
    StorageService,
    PagerService,
    CanDeactivateGuard,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
