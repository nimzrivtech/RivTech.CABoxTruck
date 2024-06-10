import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdditionalInterestComponent } from './additional-interest.component';
import { AdditionalInterestSectionComponent } from './additional-interest-section/additional-interest-section.component';
import { AdditionalInterestFormComponent } from './additional-interest-section/additional-interest-form.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule, TabsModule } from 'ngx-bootstrap';
import { PolicyInterestRoutingModule } from '../policy-interest-routing.module';
import { PaginationModule } from 'ngx-bootstrap';

@NgModule({
  declarations: [
    AdditionalInterestComponent,
    AdditionalInterestSectionComponent,
    AdditionalInterestFormComponent
  ],
  imports: [
    CommonModule,
    PolicyInterestRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
    DataTablesModule,
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    SharedModule,
    PaginationModule.forRoot()
  ],
  exports: [
    AdditionalInterestFormComponent,
    AdditionalInterestSectionComponent
  ],
  entryComponents: [
    AdditionalInterestFormComponent
  ]
})
export class AdditionalInterestModule { }
