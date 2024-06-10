import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrokerComponent } from './broker.component';
import { GeneralInformationComponent } from './general-information/general-information.component';
import { BrokerRoutingModule } from './broker-routing.module';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { SharedModule } from '../../../../../../shared/shared.module';
import { PolicyContactsComponent } from './policy-contacts/policy-contacts.component';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';
import { PolicyContactSectionComponent } from './policy-contacts-section/policy-contacts-section.component';
import { DataTablesModule } from 'angular-datatables';
import { PolicyContactFormComponent } from './policy-contacts-section/policy-contacts-form.component';
import { ModalModule } from 'ngx-bootstrap';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [
    GeneralInformationComponent,
    BrokerComponent,
    PolicyContactsComponent,
    PolicyContactSectionComponent,
    PolicyContactFormComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    BrokerRoutingModule,
    DataTablesModule,
    ModalModule.forRoot(),
    NgxMaskModule.forRoot(maskConfig),
    SubmissionDetailsSharedModule,
  ],
  exports: [
    SharedModule,
    PolicyContactFormComponent
  ],
  entryComponents: [
    PolicyContactFormComponent
  ]
})
export class BrokerModule { }
