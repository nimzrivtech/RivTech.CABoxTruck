import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NameAndAddressComponent } from './name-and-address.component';
import { NameAndAddressSectionComponent } from './name-and-address-section/name-and-address-section.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { DataTablesModule } from 'angular-datatables';
import { NgSelectModule } from '@ng-select/ng-select';
import { AddressDialogComponent } from './name-and-address-section/address-dialog/address-dialog.component';
import { PolicyApplicantRoutingModule } from '../policy-applicant-routing.module';

@NgModule({
  declarations: [
    NameAndAddressComponent,
    NameAndAddressSectionComponent,
    AddressDialogComponent
  ],
  imports: [
    CommonModule,
    PolicyApplicantRoutingModule,
    SharedModule,
    DataTablesModule,
    NgSelectModule
  ],
  entryComponents:[
    AddressDialogComponent
  ],
})
export class NameAndAddressModule { }
