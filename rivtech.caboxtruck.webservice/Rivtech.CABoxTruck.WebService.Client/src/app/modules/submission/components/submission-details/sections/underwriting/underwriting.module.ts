import { NgModule } from '@angular/core';
import { UnderWritingComponent } from './underwriting.component';
import { GeneralInformationComponent } from './general-information/general-information.component';
import { UnderWritingRoutingModule } from './underwriting-routing.module';
import { SharedModule } from '../../../../../../shared/shared.module';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule, TabsModule } from 'ngx-bootstrap';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';


@NgModule({
  declarations: [GeneralInformationComponent, UnderWritingComponent],
  imports: [
    SharedModule,
    UnderWritingRoutingModule,
    DataTablesModule,
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    SubmissionDetailsSharedModule,
  ]
})
export class UnderWritingModule { }
