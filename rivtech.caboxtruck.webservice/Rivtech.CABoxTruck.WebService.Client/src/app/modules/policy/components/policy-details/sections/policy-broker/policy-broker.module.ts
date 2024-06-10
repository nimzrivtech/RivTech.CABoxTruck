import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyBrokerRoutingModule } from './policy-broker-routing.module';
import { PolicyBrokerComponent } from './policy-broker.component';
import { GeneralInformationComponent } from './general-information/general-information.component';
import { PolicyContactsComponent } from './policy-contacts/policy-contacts.component';
import { SharedModule } from '@app/shared/shared.module';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { BrokerData } from '@app/modules/submission/data/broker.data';
import { BrokerGenInfoData } from '@app/modules/submission/data/broker/general-information.data';
import { PolicyContactSectionComponent } from './policy-contacts-section/policy-contacts-section.component';
import { PolicyContactFormComponent } from './policy-contacts-section/policy-contacts-form.component';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule } from 'ngx-bootstrap';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [
    PolicyBrokerComponent,
    GeneralInformationComponent,
    PolicyContactsComponent,
    PolicyContactSectionComponent,
    PolicyContactFormComponent],
  imports: [
    CommonModule,
    SharedModule,
    PolicyBrokerRoutingModule,
    DataTablesModule,
    ModalModule.forRoot(),
    NgxMaskModule.forRoot(maskConfig)
  ],
  providers: [
    BrokerData,
    BrokerGenInfoData
  ],
  exports: [
    PolicyContactFormComponent
  ],
  entryComponents: [
    PolicyContactFormComponent
  ]
})
export class PolicyBrokerModule { }
