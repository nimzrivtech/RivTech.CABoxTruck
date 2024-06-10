import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PolicyPremiumsComponent } from './policy-premiums.component';
import { PolicyPremiumsRoutingModule } from './policy-premiums-routing.module';
import { PolicyPremiumsPageModule } from './premiums-page/policy-premiums-page.module';


@NgModule({
  declarations: [
    PolicyPremiumsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PolicyPremiumsRoutingModule,
    PolicyPremiumsPageModule,
  ]
})
export class PolicyPremiumsModule { }
